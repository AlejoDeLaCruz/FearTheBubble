using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    // Nombre del evento configurado en Wwise (debe coincidir exactamente)
    public string wwiseEventName = "Play_Ranas";

    // Bandera para asegurarse de que se active solo una vez
    private bool hasTriggered = false;

    // Este método se llama cuando otro collider entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter detectado con objeto: " + other.name);

        // Verifica que el objeto que ingresa tenga el tag "Player"
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Trigger activado por el Player: " + other.name);

            // Dispara el evento de sonido en Wwise
            AkSoundEngine.PostEvent(wwiseEventName, gameObject);

            // Destruye el BoxCollider del trigger
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                Destroy(col); // Esto elimina solo el collider, pero deja el GameObject intacto
                Debug.Log("Collider destruido.");
            }
        }
    }
}


