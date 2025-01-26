using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class ChangeCreditsScene : MonoBehaviour
{
    [Header("Configuraci�n de la Escena")]
    public int sceneIndex; // �ndice de la escena a cargar

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colision� tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Carga la escena correspondiente al �ndice
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
