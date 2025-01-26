using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("El tag del jugador para identificarlo al entrar en el collider.")]
    public string playerTag = "Player";

    [Tooltip("El AudioSource que reproducirá el sonido.")]
    public AudioSource audioSource;

    [Tooltip("El clip de audio que se reproducirá.")]
    public AudioClip soundClip;

    [Tooltip("Define si este sonido debe reproducirse en loop.")]
    public bool shouldLoop;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al collider tiene el tag del jugador
        if (other.CompareTag(playerTag))
        {
            // Configura y reproduce el sonido si no está ya sonando
            if (audioSource != null && soundClip != null && !audioSource.isPlaying)
            {
                audioSource.clip = soundClip;
                audioSource.loop = shouldLoop; // Establece si el clip debe loopearse
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que salió del collider es el jugador
        if (other.CompareTag(playerTag))
        {
            // Detiene el sonido cuando el jugador sale del área, pero solo si no debe loopearse
            if (!shouldLoop && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}