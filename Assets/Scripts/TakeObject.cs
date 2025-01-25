using UnityEngine;
using System.Collections;

public class TakeObject : MonoBehaviour
{
    public Canvas canvas;                 // El Canvas inicial que contiene la imagen a mostrar
    public Canvas secondaryCanvas;       // El Canvas secundario que se muestra por X segundos
    public float secondaryCanvasDuration = 3f; // Duración en segundos del Canvas secundario
    public MonoBehaviour playerMovement; // Referencia al script de movimiento del jugador (habilitar/deshabilitar)

    private bool isPlayerStopped = false; // Indica si el movimiento del jugador está desactivado

    private void Start()
    {
        // Asegúrate de que los Canvas estén inicialmente desactivados
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Canvas inicial no asignado en el Inspector.");
        }

        if (secondaryCanvas != null)
        {
            secondaryCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Canvas secundario no asignado en el Inspector.");
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
            if (canvas != null && playerMovement != null)
            {
                // Activa el Canvas inicial
                canvas.gameObject.SetActive(true);

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
            // Desactiva el Canvas inicial
            canvas.gameObject.SetActive(false);

            // Reactiva el movimiento del jugador
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
                isPlayerStopped = false;
            }

            // Inicia la rutina para mostrar el Canvas secundario
            if (secondaryCanvas != null)
            {
                StartCoroutine(ShowSecondaryCanvas());
            }
        }
    }

    private IEnumerator ShowSecondaryCanvas()
    {
        // Activa el Canvas secundario
        secondaryCanvas.gameObject.SetActive(true);

        // Espera durante la duración especificada
        yield return new WaitForSeconds(secondaryCanvasDuration);

        // Desactiva el Canvas secundario
        secondaryCanvas.gameObject.SetActive(false);
    }
}