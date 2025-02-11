using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFall : MonoBehaviour
{
    // Define la gravedad personalizada (valor negativo para caer hacia abajo).
    // Puedes ajustar este valor para que la caída sea más lenta (por ejemplo, -4 en lugar de -9.81).
    public float customGravity = -4.0f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Desactivar la gravedad predeterminada solo cuando el script esté activo.
        rb.useGravity = false;
    }

    private void OnDisable()
    {
        // Restaurar la gravedad cuando el script esté desactivado.
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        // Aplica una fuerza constante en la dirección Y (hacia abajo).
        // Se usa ForceMode.Acceleration para que la fuerza sea independiente de la masa.
        rb.AddForce(new Vector3(0, customGravity, 0), ForceMode.Acceleration);
    }
}