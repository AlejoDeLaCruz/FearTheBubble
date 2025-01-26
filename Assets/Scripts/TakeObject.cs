using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con Image
using System.Collections;

public class TakeObject : MonoBehaviour
{
    [Header("Configuración de Imágenes")]
    public Image primaryImage;              // Imagen principal que se muestra inicialmente
    public Image secondaryImage;            // Imagen secundaria que se muestra por X segundos
    public float secondaryImageDuration = 3f; // Duración en segundos de la imagen secundaria

    [Header("Movimiento del Jugador")]
    public MonoBehaviour playerMovement;    // Referencia al script de movimiento del jugador (habilitar/deshabilitar)

    private bool isPlayerStopped = false;   // Indica si el movimiento del jugador está desactivado

    private void Start()
    {
        // Asegúrate de que las imágenes estén inicialmente desactivadas
        if (primaryImage != null)
        {
            primaryImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Imagen principal no asignada en el Inspector.");
        }

        if (secondaryImage != null)
        {
            secondaryImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Imagen secundaria no asignada en el Inspector.");
        }

        // Verifica que el script de movimiento del jugador esté asignado
        if (playerMovement == null)
        {
            Debug.LogError("El script de movimiento del jugador no está asignado en el Inspector.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisionó tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            if (primaryImage != null && playerMovement != null)
            {
                // Activa la imagen principal
                primaryImage.gameObject.SetActive(true);

                // Desactiva el movimiento del jugador
                playerMovement.enabled = false;
                isPlayerStopped = true;
            }
        }
    }

    private void Update()
    {
        // Verifica si el jugador está detenido y presiona la tecla E
        if (isPlayerStopped && Input.GetKeyDown(KeyCode.E))
        {
            // Desactiva la imagen principal
            if (primaryImage != null)
            {
                primaryImage.gameObject.SetActive(false);
            }

            // Reactiva el movimiento del jugador
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
                isPlayerStopped = false;
            }

            // Inicia la rutina para mostrar la imagen secundaria
            if (secondaryImage != null)
            {
                StartCoroutine(ShowSecondaryImage());
            }
        }
    }

    private IEnumerator ShowSecondaryImage()
    {
        // Activa la imagen secundaria
        secondaryImage.gameObject.SetActive(true);

        // Espera durante la duración especificada
        yield return new WaitForSeconds(secondaryImageDuration);

        // Desactiva la imagen secundaria
        secondaryImage.gameObject.SetActive(false);
    }
}
