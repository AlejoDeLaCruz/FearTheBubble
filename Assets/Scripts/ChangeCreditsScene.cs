using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class ChangeCreditsScene : MonoBehaviour
{
    [Header("Configuración de la Escena")]
    public int sceneIndex; // Índice de la escena a cargar

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisionó tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Carga la escena correspondiente al índice
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
