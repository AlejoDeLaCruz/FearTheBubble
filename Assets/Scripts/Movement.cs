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

    [Header("Tormenta Settings")]
    public float maxForceTormenta = 1f;  // 🔥 Mucho más alto que el normal
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
        // 🔥 Si está en modo tormenta, usa los valores aumentados
        float maxFuerzaActual = modoTormenta ? maxForceTormenta : maxForce;
        float minFuerzaActual = modoTormenta ? minForceTormenta : minForce;

        currentForce += forceChargeRate * Time.deltaTime;
        currentForce = Mathf.Clamp(currentForce, minFuerzaActual, maxFuerzaActual);
    }

    private void ApplyForce()
    {
        Vector3 forceDirection;

        // 🔥 Si está en modo tormenta, SIEMPRE usa una dirección aleatoria
        if (modoTormenta && currentForce < 1.4f)
        {
            float randomAngle = Random.Range(0f, 360f);
            forceDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        }
        else
        {
            forceDirection = Camera.main.transform.forward;
        }

        rb.AddForce(forceDirection * currentForce, ForceMode.Impulse);
        LimitSpeed();
        currentForce = 0f;
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