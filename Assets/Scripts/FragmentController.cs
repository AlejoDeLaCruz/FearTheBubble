using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentController : MonoBehaviour
{
    private Rigidbody rb;

    // Par�metros para la fuerza que se aplicar� cuando el fragmento sea golpeado
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 5f;

    private bool yaDerribado = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el fragmento colisiona con el jugador y a�n no ha sido derribado
        if (!yaDerribado && collision.gameObject.CompareTag("Player"))
        {
            // Utilizamos collision.relativeVelocity para determinar la fuerza del impacto
            float impactSpeed = collision.relativeVelocity.magnitude;
            Debug.Log("Impact Speed: " + impactSpeed);

            // Solo activamos la f�sica si la velocidad del impacto es mayor o igual a 0.6
            if (impactSpeed >= 5f)
            {
                // Activar la f�sica: ya no es kinematic
                rb.isKinematic = false;

                // Aplicar una fuerza para simular el derribo usando el punto de contacto de la colisi�n
                if (collision.contacts.Length > 0)
                {
                    rb.AddExplosionForce(explosionForce, collision.contacts[0].point, explosionRadius);
                }

                yaDerribado = true;
            }
        }
    }
}