using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LowPolyWater; // Incluir el namespace donde se encuentra LowPolyWaterClass

public class FloatingObjectBarrier : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody rb; // El Rigidbody del objeto

    [Header("Puntos de flotación")]
    [Tooltip("Puntos que determinarán la flotación del objeto. Colócalos en posiciones estratégicas (ej. esquinas o base).")]
    public Transform[] floaterPoints;

    [Header("Parámetros de flotación")]
    [Tooltip("Fuerza base de flotación aplicada en cada punto.")]
    public float buoyancyForce = 0.3f;
    [Tooltip("Profundidad en la que el punto se considera totalmente sumergido (0 a 1).")]
    public float depthBeforeSubmerged = 0.11f;
    [Tooltip("Amortiguación lineal en el agua.")]
    public float waterDrag = 4f;
    [Tooltip("Amortiguación angular en el agua.")]
    public float waterAngularDrag = 4f;

    [Header("Referencia al agua")]
    [Tooltip("Referencia al script que devuelve la altura de las olas (LowPolyWaterClass).")]
    public LowPolyWaterClass water;

    private void FixedUpdate()
    {
        float totalSubmerged = 0f;

        // Recorre cada punto de flotación para aplicar la fuerza
        foreach (Transform floater in floaterPoints)
        {
            // Obtiene la altura de la ola en la posición del punto de flotación
            float waveHeight = water.GetWaveHeightAtPosition(floater.position);

            // Calcula la fracción de inmersión (0 = no sumergido, 1 = totalmente sumergido)
            float submergedFraction = Mathf.Clamp01((waveHeight - floater.position.y) / depthBeforeSubmerged);

            // Si el punto está sumergido, aplica la fuerza de flotación en ese punto
            if (submergedFraction > 0f)
            {
                // La fuerza de flotación contrarresta la gravedad
                Vector3 buoyantForce = -Physics.gravity * buoyancyForce * submergedFraction;
                rb.AddForceAtPosition(buoyantForce, floater.position, ForceMode.Force);
            }

            totalSubmerged += submergedFraction;
        }

        // Calcula el promedio de inmersión de todos los puntos
        float avgSubmerged = totalSubmerged / floaterPoints.Length;

        // Si el objeto está al menos parcialmente sumergido, aplica amortiguación
        if (avgSubmerged > 0f)
        {
            // Reducir la velocidad (drag) proporcional al grado de inmersión
            rb.velocity *= (1f - waterDrag * avgSubmerged * Time.fixedDeltaTime);
            rb.angularVelocity *= (1f - waterAngularDrag * avgSubmerged * Time.fixedDeltaTime);
        }
    }
}
