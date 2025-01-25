using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Force Settings")]
    public float maxForce = 20f; // Fuerza máxima que se puede acumular
    public float minForce = 5f;  // Fuerza mínima aplicada al soltar rápidamente
    public float forceChargeRate = 10f; // Velocidad de carga de la fuerza por segundo

    private float currentForce = 0f; // Fuerza acumulada mientras se mantiene el clic
    private bool isCharging = false; // Indica si el jugador está acumulando fuerza

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Detectar si el clic izquierdo del mouse está presionado
        if (Input.GetMouseButton(0))
        {
            isCharging = true;
            AccumulateForce();
        }

        // Detectar si el clic izquierdo del mouse se suelta
        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            ApplyForce();
        }
    }

    private void AccumulateForce()
    {
        // Aumentar la fuerza acumulada hasta el límite máximo
        currentForce += forceChargeRate * Time.deltaTime;
        currentForce = Mathf.Clamp(currentForce, minForce, maxForce);
    }

    private void ApplyForce()
    {
        // Aplicar la fuerza acumulada en la dirección hacia adelante
        Vector3 forceDirection = Camera.main.transform.forward; // La dirección puede ajustarse según tus necesidades
        rb.AddForce(forceDirection * currentForce, ForceMode.Impulse);

        // Reiniciar la fuerza acumulada
        currentForce = 0f;
    }
}