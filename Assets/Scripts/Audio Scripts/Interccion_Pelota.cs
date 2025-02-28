using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    [Header("Configuraci�n del Evento Wwise")]
    [Tooltip("Nombre del evento Wwise a disparar al interactuar.")]
    public string wwiseEventName = "Play_InteractionEvent";

    [Header("Configuraci�n de Interacci�n")]
    [Tooltip("Tecla que usar� el jugador para interactuar.")]
    public KeyCode interactKey = KeyCode.E;

    // Indica si el jugador se encuentra en el �rea de interacci�n
    private bool playerInRange = false;

    // Se activa cuando otro collider entra en el trigger del objeto
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que el objeto que entra tenga la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jugador en rango de interacci�n.");
        }
    }

    // Se activa cuando otro collider sale del trigger del objeto
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Jugador fuera de rango de interacci�n.");
        }
    }

    private void Update()
    {
        // Si el jugador est� en rango y presiona la tecla de interacci�n...
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            // Se dispara el evento de Wwise
            AkSoundEngine.PostEvent(wwiseEventName, gameObject);
            Debug.Log("Evento Wwise disparado.");
        }
    }
}

