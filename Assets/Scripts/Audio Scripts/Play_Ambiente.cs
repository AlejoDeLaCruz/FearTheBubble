using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Ambiente : MonoBehaviour
{
    // Nombre del evento de Wwise que reproduce el ambiente (aseg�rate de que coincida con Wwise)
    [SerializeField] private string eventName = "Play_Ambientes";

    // Indica si el sonido debe reproducirse una sola vez (para evitar bucles)
    [SerializeField] private bool playOnce = true;
    private bool hasPlayed = false; // Controla si el sonido ya se reprodujo

    // ID del sonido en reproducci�n para controlar su detenci�n
    private uint playingId = AkSoundEngine.AK_INVALID_PLAYING_ID;

    void Start()
    {
        // Verificar si ya se reprodujo el sonido (si playOnce est� activado)
        if (playOnce && hasPlayed)
        {
            return;
        }

        // Buscar el GameObject con el componente AkAudioListener
        AkAudioListener listener = FindObjectOfType<AkAudioListener>();

        if (listener != null)
        {
            // Publicar el evento de Wwise asociado al listener y guardar el playingID
            playingId = AkSoundEngine.PostEvent(eventName, listener.gameObject);

            // Marcar que el sonido se reprodujo si playOnce est� activado
            if (playOnce)
            {
                hasPlayed = true;
            }

            Debug.Log($"Evento '{eventName}' reproducido con �xito en el listener. PlayingID: {playingId}");
        }
        else
        {
            Debug.LogError("No se encontr� un AkAudioListener en la escena. Aseg�rate de tener uno configurado.");
        }
    }

    // M�todo para detener el sonido manualmente si es necesario (por ejemplo, al cambiar de escena)
    public void StopAmbientSound()
    {
        if (playingId != AkSoundEngine.AK_INVALID_PLAYING_ID && !string.IsNullOrEmpty(eventName))
        {
            AkSoundEngine.StopPlayingID(playingId);
            Debug.Log($"Evento '{eventName}' detenido manualmente. PlayingID: {playingId}");
            playingId = AkSoundEngine.AK_INVALID_PLAYING_ID; // Resetear el ID
        }
    }

    // M�todo para reiniciar el sonido si es necesario
    public void ReplayAmbientSound()
    {
        if (!playOnce || !hasPlayed)
        {
            AkAudioListener listener = FindObjectOfType<AkAudioListener>();
            if (listener != null)
            {
                playingId = AkSoundEngine.PostEvent(eventName, listener.gameObject);
                if (playOnce)
                {
                    hasPlayed = true;
                }
                Debug.Log($"Evento '{eventName}' reproducido nuevamente en el listener. PlayingID: {playingId}");
            }
        }
        else
        {
            Debug.LogWarning($"El evento '{eventName}' no se puede reproducir de nuevo porque playOnce est� activado y ya se reprodujo.");
        }
    }

    // Aseg�rate de detener el sonido al destruir el objeto (por ejemplo, al cambiar de escena)
    void OnDestroy()
    {
        StopAmbientSound();
    }
}