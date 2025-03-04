using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeWwiseEvent : MonoBehaviour
{
    [Header("Configuración del Evento Wwise")]
    [Tooltip("Nombre del evento Wwise a disparar al cambiar de escena.")]
    public string wwiseEventName = "Play_SceneChangeEvent";

    private void OnEnable()
    {
        // Se suscribe al evento que se dispara cuando se carga una escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Se cancela la suscripción al evento para evitar errores o llamadas innecesarias
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Método que se ejecuta cada vez que se carga una nueva escena.
    /// </summary>
    /// <param name="scene">La escena que se ha cargado.</param>
    /// <param name="mode">El modo de carga de la escena.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Se dispara el evento de Wwise
        AkSoundEngine.PostEvent(wwiseEventName, gameObject);
        Debug.Log($"Escena cargada: {scene.name}. Evento '{wwiseEventName}' disparado.");
    }
}
