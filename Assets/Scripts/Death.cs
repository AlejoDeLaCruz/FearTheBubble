using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Death : MonoBehaviour
{
    public Transform respawnPoint;  // El GameObject vacío que define el punto de respawn
    public Canvas canvas;           // El Canvas que contiene la pantalla negra
    public Image blackScreen;       // La Image negra que cubrirá la pantalla
    public AudioSource audioSource; // El componente AudioSource que reproducirá el sonido
    public AudioClip deathSound;    // El clip de sonido que se reproducirá al morir
    public float transitionDuration = 2f;  // Duración de la transición en segundos

    // Esta función se llama cuando un objeto entra en contacto con este objeto
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisionó es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reproduce el sonido
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            // Inicia la secuencia de muerte (transición)
            StartCoroutine(DeathSequence(collision.gameObject));
        }
    }

    private IEnumerator DeathSequence(GameObject player)
    {
        // Activa la pantalla negra (si no está activada ya)
        blackScreen.gameObject.SetActive(true);

        // Hace la pantalla negra opaca (de 0 a 1) durante la duración de la transición
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            blackScreen.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, elapsedTime / transitionDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0f, 0f, 0f, 1f); // Asegura que esté completamente negro

        // Teletransporta al jugador a la posición del punto de respawn
        player.transform.position = respawnPoint.position;

        // Espera unos segundos en la pantalla negra antes de desaparecer
        yield return new WaitForSeconds(1f);

        // Hace que la pantalla negra se desvanezca (de 1 a 0)
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            blackScreen.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0f, 0f, 0f, 0f); // Asegura que esté completamente transparente

        // Desactiva la pantalla negra
        blackScreen.gameObject.SetActive(false);
    }
}
