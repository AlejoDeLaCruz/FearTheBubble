using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Force Settings")]
    public float maxForce = 0.05f;
    public float minForce = 0.01f;
    public float forceChargeRate = 2f;
    public float maxSpeed = 0.05f;
    public float decelerationRate = 1f;

    [Header("Dirección Opuesta Settings")]
    public float resistenciaMaxima = 0.3f; // Fuerza mínima permitida al cambiar de dirección
    public float multiplicadorOposicion = 2f; // Intensidad del efecto de resistencia

    [Header("Tormenta Settings")]
    public float maxForceTormenta = 1f;
    public float minForceTormenta = 0.5f;
    public float maxSpeedTormenta = 1f;
    public bool modoTormenta = false;

    private float currentForce = 0f;
    private bool isCharging = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isCharging = true;
            AccumulateForce();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            ApplyForce();
        }

        if (!isCharging)
        {
            DecelerateSphere();
        }
    }

    private void AccumulateForce()
    {
        float maxFuerzaActual = modoTormenta ? maxForceTormenta : maxForce;
        float minFuerzaActual = modoTormenta ? minForceTormenta : minForce;

        currentForce += forceChargeRate * Time.deltaTime;
        currentForce = Mathf.Clamp(currentForce, minFuerzaActual, maxFuerzaActual);
    }

    private void ApplyForce()
    {
        Vector3 forceDirection = CalculateMovementDirection();

        if (forceDirection != Vector3.zero)
        {
            // Calcular resistencia por dirección opuesta
            float resistencia = CalcularResistenciaDireccion(forceDirection);
            currentForce *= resistencia;

            rb.AddForce(forceDirection * currentForce, ForceMode.Impulse);
            LimitSpeed();
        }

        currentForce = 0f;
    }

    private float CalcularResistenciaDireccion(Vector3 nuevaDireccion)
    {
        Vector3 velocidadActual = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (velocidadActual.magnitude < 0.1f) return 1f; // No aplicar resistencia si no hay movimiento

        Vector3 direccionActual = velocidadActual.normalized;
        float oposicion = Vector3.Dot(nuevaDireccion, -direccionActual);

        if (oposicion > 0)
        {
            float velocidadNormalizada = velocidadActual.magnitude / (modoTormenta ? maxSpeedTormenta : maxSpeed);
            float resistencia = 1f - (oposicion * velocidadNormalizada * multiplicadorOposicion);
            return Mathf.Clamp(resistencia, resistenciaMaxima, 1f);
        }

        return 1f;
    }

    private Vector3 CalculateMovementDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        return (cameraForward * vertical + cameraRight * horizontal).normalized;
    }

    private void DecelerateSphere()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 deceleration = -currentVelocity.normalized * decelerationRate * Time.deltaTime;

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
        float velocidadMaxima = modoTormenta ? maxSpeedTormenta : maxSpeed;
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > velocidadMaxima)
        {
            flatVelocity = flatVelocity.normalized * velocidadMaxima;
            rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
        }
    }

    public void ActivarModoTormenta()
    {
        modoTormenta = true;
        Debug.Log("¡Modo tormenta activado!");
    }

    public void DesactivarModoTormenta()
    {
        modoTormenta = false;
        Debug.Log("Modo tormenta desactivado.");
    }
}