using UnityEngine;

public class SoundTrigger: MonoBehaviour
{
    [Header("Configuraci�n")]
    [Tooltip("El tag del jugador para identificarlo al entrar en el collider.")]
    public string playerTag = "Player";

    [Tooltip("El AudioSource que reproducir� el sonido.")]
    public AudioSource audioSource;

    [Tooltip("El clip de audio que se reproducir� en loop.")]
    public AudioClip soundClip;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al collider tiene el tag del jugador
        if (other.CompareTag(playerTag))
        {
            // Reproduce el sonido en loop si no est� ya sonando
            if (audioSource != null && soundClip != null && !audioSource.isPlaying)
            {
                audioSource.clip = soundClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que sali� del collider es el jugador
        if (other.CompareTag(playerTag))
        {
            // Detiene el sonido cuando el jugador sale del �rea
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
