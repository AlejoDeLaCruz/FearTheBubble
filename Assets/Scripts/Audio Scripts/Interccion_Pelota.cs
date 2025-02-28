using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    [Header("Configuración del Evento Wwise")]
    [Tooltip("Nombre del evento Wwise a disparar al interactuar.")]
    public string wwiseEventName = "Play_InteractionEvent";

    [Header("Configuración de Interacción")]
    [Tooltip("Tecla que usará el jugador para interactuar.")]
    public KeyCode interactKey = KeyCode.E;

    // Indica si el jugador se encuentra en el área de interacción
    private bool playerInRange = false;

    // Se activa cuando otro collider entra en el trigger del objeto
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que el objeto que entra tenga la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jugador en rango de interacción.");
        }
    }

    // Se activa cuando otro collider sale del trigger del objeto
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Jugador fuera de rango de interacción.");
        }
    }

    private void Update()
    {
        // Si el jugador está en rango y presiona la tecla de interacción...
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            // Se dispara el evento de Wwise
            AkSoundEngine.PostEvent(wwiseEventName, gameObject);
            Debug.Log("Evento Wwise disparado.");
        }
    }
}

