using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BreakableObject : MonoBehaviour
{
    [SerializeField] private GameObject fracturedPrefab; // Prefab del objeto fracturado
    [SerializeField] private float cleanupDelay = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si colisionó con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            Debug.Log(playerRb.velocity.magnitude + "Magnitud");

            // Romper solo si está en modo tormenta, usó fuerza máxima y se mueve a una velocidad mayor o igual a 5
            if (playerMovement != null && playerMovement.modoTormenta && playerMovement.isUsingMaxStormForce &&
                playerRb != null && playerRb.velocity.magnitude >= 0.6f)
            {
                BreakObject();
            }
        }
    }

    public void BreakObject()
    {
        // Instanciar el objeto fracturado en la misma posición y rotación
        GameObject fractured = Instantiate(fracturedPrefab, transform.position, transform.rotation);

        // Configurar los fragmentos para que sean kinematic inicialmente
        foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true; // Se mantendrán quietos (no afectados por la física)

            // Agregar el script que permitirá activarlos al ser golpeados por el jugador
            if (rb.gameObject.GetComponent<FragmentController>() == null)
            {
                rb.gameObject.AddComponent<FragmentController>();
            }
        }

        // Destruir el objeto original y limpiar los fragmentos después de un tiempo
        Destroy(gameObject);
        Destroy(fractured, cleanupDelay);
    }
}
