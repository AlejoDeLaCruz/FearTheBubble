using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Bouncer : MonoBehaviour
{
    [Header("Configuración de Rebote")]
    [SerializeField] private float bounceForce = 15f;
    [SerializeField] private float velocityMultiplier = 1.2f;
    [SerializeField] private bool preserveVerticalVelocity = true;

    private void Start()
    {
        // Configurar componentes requeridos
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.rigidbody;
            if (playerRb != null)
            {
                // Calcular dirección de rebote
                Vector3 bounceDirection = CalculateBounceDirection(collision);

                // Aplicar fuerza de rebote
                ApplyBounceForce(playerRb, bounceDirection);
            }
        }
    }

    private Vector3 CalculateBounceDirection(Collision collision)
    {
        // Obtener el primer punto de contacto y su normal
        ContactPoint contact = collision.contacts[0];
        Vector3 surfaceNormal = contact.normal;

        // Calcular dirección de entrada del jugador
        Vector3 incomingDirection = collision.relativeVelocity.normalized;

        // Calcular dirección de rebote usando reflexión vectorial
        Vector3 bounceDirection = Vector3.Reflect(incomingDirection, surfaceNormal).normalized;

        // Ajustar dirección vertical si es necesario
        if (preserveVerticalVelocity)
        {
            bounceDirection.y = Mathf.Clamp(bounceDirection.y, 0.1f, 1f);
        }

        return bounceDirection;
    }

    private void ApplyBounceForce(Rigidbody playerRb, Vector3 direction)
    {
        // Calcular fuerza final manteniendo la magnitud de velocidad actual
        float currentSpeed = playerRb.velocity.magnitude;
        Vector3 force = direction * (currentSpeed * velocityMultiplier + bounceForce);

        // Aplicar fuerza como impulso
        playerRb.velocity = Vector3.zero; // Resetear velocidad para mejor control
        playerRb.AddForce(force, ForceMode.Impulse);
    }
}