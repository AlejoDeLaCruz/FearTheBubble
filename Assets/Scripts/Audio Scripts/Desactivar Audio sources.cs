using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUnityAudio : MonoBehaviour
{
    void Start()
    {
        // Encuentra todos los AudioSources en la escena
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        // Desactiva o silencia cada AudioSource
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop(); // Detiene la reproducción
            audioSource.enabled = false; // Desactiva el componente
            Debug.Log($"AudioSource desactivado: {audioSource.name}");
        }
    }
}