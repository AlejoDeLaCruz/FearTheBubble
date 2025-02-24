using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTriggerDisparandoEnOtroObjeto : MonoBehaviour
{
    public GameObject targetObject; // Objeto donde se ejecutar� el sonido
    public string wwiseEventName = "Play_Sound"; // Nombre del evento en Wwise

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que el Player tenga este tag
        {
            if (targetObject != null)
            {
                AkSoundEngine.PostEvent(wwiseEventName, targetObject);
                Debug.Log("Evento de Wwise disparado en: " + targetObject.name);
            }
            else
            {
                Debug.LogError("No se asign� un GameObject para reproducir el sonido.");
            }

            // Destruye el objeto despu�s de activar el evento
            Destroy(gameObject);
            Debug.Log("Trigger destruido.");
        }
    }
}
