using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TakeObject : MonoBehaviour
{
    [Header("Configuración de Imágenes")]
    public Image primaryImage;              // Imagen principal que se muestra inicialmente
    public Image secondaryImage;            // Imagen secundaria que se muestra por X segundos
    public float secondaryImageDuration = 3f; // Duración en segundos de la imagen secundaria

    [Header("Movimiento del Jugador")]
    public MonoBehaviour playerMovement;    // Referencia al script de movimiento del jugador (ej: FirstPersonController)
    public Rigidbody playerRigidbody;       // Rigidbody del jugador (¡Asignar en el Inspector!)

    private bool isPlayerInTrigger = false; // Indica si el jugador está dentro del trigger
    private bool isPlayerStopped = false;   // Indica si el movimiento está desactivado
    private Vector3 originalVelocity;       // Guarda la velocidad inicial del jugador
    private float originalAngularDrag;      // Guarda la fricción angular original

    private void Start()
    {
        // Asegurar que las imágenes estén desactivadas al inicio
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

        // Verificaciones de componentes
        if (playerMovement == null)
        {
            Debug.LogError("El script de movimiento del jugador no está asignado en el Inspector.");
        }

        if (playerRigidbody == null)
        {
            Debug.LogError("El Rigidbody del jugador no está asignado en el Inspector.");
        }
        else
        {
            originalAngularDrag = playerRigidbody.angularDrag;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            // Congelar movimiento BRUSCAMENTE
            originalVelocity = playerRigidbody.velocity;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

            // Desactivar script de movimiento
            playerMovement.enabled = false;
            isPlayerStopped = true;

            // Mostrar imagen principal
            if (primaryImage != null)
            {
                primaryImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            // Restaurar movimiento si el jugador sale sin presionar E
            if (isPlayerStopped)
            {
                RestoreMovement();
                if (primaryImage != null) primaryImage.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && isPlayerStopped && Input.GetKeyDown(KeyCode.E))
        {
            // Restaurar movimiento
            RestoreMovement();

            // Ocultar imagen principal
            if (primaryImage != null)
            {
                primaryImage.gameObject.SetActive(false);
            }

            // Mostrar imagen secundaria temporalmente
            if (secondaryImage != null)
            {
                StartCoroutine(ShowSecondaryImage());
            }
        }
    }

    private void RestoreMovement()
    {
        // Reactivar componentes
        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.angularDrag = originalAngularDrag;
        playerRigidbody.velocity = originalVelocity;

        playerMovement.enabled = true;
        isPlayerStopped = false;
    }

    private IEnumerator ShowSecondaryImage()
    {
        secondaryImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(secondaryImageDuration);
        secondaryImage.gameObject.SetActive(false);
    }
}