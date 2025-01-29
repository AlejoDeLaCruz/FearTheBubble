using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flashlight : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform; // Arrastrar la cámara aquí desde el inspector

    [Header("Ajustes")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 0, 0.2f); // Ajuste fino de posición
    [SerializeField] private float smoothness = 5f; // Suavidad del movimiento

    private void Start()
    {
        // Buscar automáticamente la cámara principal si no está asignada
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // Actualizar posición y rotación suavemente
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        // Calcular posición objetivo con offset
        Vector3 targetPosition = cameraTransform.position +
                                cameraTransform.forward * positionOffset.z +
                                cameraTransform.up * positionOffset.y +
                                cameraTransform.right * positionOffset.x;

        // Movimiento suavizado
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        // Rotación suavizada hacia la dirección de la cámara
        Quaternion targetRotation = cameraTransform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
    }
}