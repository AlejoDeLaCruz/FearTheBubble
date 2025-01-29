using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flashlight : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform; // Arrastrar la c�mara aqu� desde el inspector

    [Header("Ajustes")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 0, 0.2f); // Ajuste fino de posici�n
    [SerializeField] private float smoothness = 5f; // Suavidad del movimiento

    private void Start()
    {
        // Buscar autom�ticamente la c�mara principal si no est� asignada
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // Actualizar posici�n y rotaci�n suavemente
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        // Calcular posici�n objetivo con offset
        Vector3 targetPosition = cameraTransform.position +
                                cameraTransform.forward * positionOffset.z +
                                cameraTransform.up * positionOffset.y +
                                cameraTransform.right * positionOffset.x;

        // Movimiento suavizado
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        // Rotaci�n suavizada hacia la direcci�n de la c�mara
        Quaternion targetRotation = cameraTransform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
    }
}