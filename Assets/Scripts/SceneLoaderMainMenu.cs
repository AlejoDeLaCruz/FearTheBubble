using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Importante para trabajar con los botones

public class SceneLoaderMainMenu : MonoBehaviour
{
    // Botones que serán asignados desde el Inspector
    public Button buttonChangeScene;
    public Button buttonQuitGame;

    void Start()
    {
        // Asigna las funciones a los botones al iniciar
        if (buttonChangeScene != null)
        {
            buttonChangeScene.onClick.AddListener(ChangeSceneToIndex1);
        }

        if (buttonQuitGame != null)
        {
            buttonQuitGame.onClick.AddListener(QuitGame);
        }
    }

    // Método para cambiar a la escena con índice 1
    public void ChangeSceneToIndex1()
    {
        SceneManager.LoadScene(1); // Usando el índice 1
    }

    // Método para cerrar el juego
    public void QuitGame()
    {
        // Si estamos en el editor de Unity
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // Cierra el juego cuando está compilado
        #endif

        Debug.Log("El juego ha sido cerrado.");
    }
}
