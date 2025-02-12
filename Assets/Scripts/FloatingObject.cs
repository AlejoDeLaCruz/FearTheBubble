using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LowPolyWater; // Incluir el namespace donde se encuentra LowPolyWaterClass

public class FloatingObjectBarrier : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody rb; // El Rigidbody del objeto

    [Header("Puntos de flotaci�n")]
    [Tooltip("Puntos que determinar�n la flotaci�n del objeto. Col�calos en posiciones estrat�gicas (ej. esquinas o base).")]
    public Transform[] floaterPoints;

    [Header("Par�metros de flotaci�n")]
    [Tooltip("Fuerza base de flotaci�n aplicada en cada punto.")]
    public float buoyancyForce = 0.3f;
    [Tooltip("Profundidad en la que el punto se considera totalmente sumergido (0 a 1).")]
    public float depthBeforeSubmerged = 0.11f;
    [Tooltip("Amortiguaci�n lineal en el agua.")]
    public float waterDrag = 4f;
    [Tooltip("Amortiguaci�n angular en el agua.")]
    public float waterAngularDrag = 4f;

    [Header("Referencia al agua")]
    [Tooltip("Referencia al script que devuelve la altura de las olas (LowPolyWaterClass).")]
    public LowPolyWaterClass water;

    private void FixedUpdate()
    {
        float totalSubmerged = 0f;

        // Recorre cada punto de flotaci�n para aplicar la fuerza
        foreach (Transform floater in floaterPoints)
        {
            // Obtiene la altura de la ola en la posici�n del punto de flotaci�n
            float waveHeight = water.GetWaveHeightAtPosition(floater.position);

            // Calcula la fracci�n de inmersi�n (0 = no sumergido, 1 = totalmente sumergido)
            float submergedFraction = Mathf.Clamp01((waveHeight - floater.position.y) / depthBeforeSubmerged);

            // Si el punto est� sumergido, aplica la fuerza de flotaci�n en ese punto
            if (submergedFraction > 0f)
            {
                // La fuerza de flotaci�n contrarresta la gravedad
                Vector3 buoyantForce = -Physics.gravity * buoyancyForce * submergedFraction;
                rb.AddForceAtPosition(buoyantForce, floater.position, ForceMode.Force);
            }

            totalSubmerged += submergedFraction;
        }

        // Calcula el promedio de inmersi�n de todos los puntos
        float avgSubmerged = totalSubmerged / floaterPoints.Length;

        // Si el objeto est� al menos parcialmente sumergido, aplica amortiguaci�n
        if (avgSubmerged > 0f)
        {
            // Reducir la velocidad (drag) proporcional al grado de inmersi�n
            rb.velocity *= (1f - waterDrag * avgSubmerged * Time.fixedDeltaTime);
            rb.angularVelocity *= (1f - waterAngularDrag * avgSubmerged * Time.fixedDeltaTime);
        }
    }
}
