using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] public GameObject fracturedPrefab; // Arrastra aquí el prefab fracturado
    [SerializeField] float breakForce = 10f;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float cleanupDelay = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si colisionó con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();

            // Romper solo si está en modo tormenta y usó fuerza máxima
            if (playerMovement != null && playerMovement.modoTormenta && playerMovement.isUsingMaxStormForce)
            {
                BreakObject();
            }
        }
    }

    public void BreakObject()
    {
        // Instanciar el objeto fracturado
        GameObject fractured = Instantiate(fracturedPrefab, transform.position, transform.rotation);

        // Aplicar fuerza explosiva a los fragmentos
        foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(500f, transform.position, explosionRadius);
        }

        // Destruir el objeto original y limpiar fragmentos después de un tiempo
        Destroy(gameObject);
        Destroy(fractured, cleanupDelay);
    }
}