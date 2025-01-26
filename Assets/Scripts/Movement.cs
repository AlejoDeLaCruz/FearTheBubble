using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Force Settings")]
    public float maxForce = 0.05f; // Fuerza máxima que se puede acumular
    public float minForce = 0.01f; // Fuerza mínima aplicada al soltar rápidamente
    public float forceChargeRate = 2f; // Velocidad de carga de la fuerza por segundo
    public float maxSpeed = 0.05f; // Velocidad máxima de la esfera
    public float decelerationRate = 1f; // Velocidad con la que la esfera se desacelera

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

        // Si no se está acumulando fuerza, reducir gradualmente la velocidad de la esfera
        if (!isCharging)
        {
            DecelerateSphere();
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

        // Limitar la velocidad máxima
        LimitSpeed();

        // Reiniciar la fuerza acumulada
        currentForce = 0f;
    }

    private void DecelerateSphere()
    {
        // Aplicar una desaceleración proporcional a la velocidad actual
        Vector3 currentVelocity = rb.velocity;
        Vector3 deceleration = -currentVelocity.normalized * decelerationRate * Time.deltaTime;

        // No desacelerar más allá de 0 (evita invertir la dirección)
        if (currentVelocity.magnitude <= deceleration.magnitude)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity += deceleration;
        }
    }

    private void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Ignorar la componente Y para controlar la velocidad horizontal
        if (flatVelocity.magnitude > maxSpeed)
        {
            flatVelocity = flatVelocity.normalized * maxSpeed; // Limitar la velocidad
            rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z); // Aplicar la velocidad limitada
        }
    }
}
