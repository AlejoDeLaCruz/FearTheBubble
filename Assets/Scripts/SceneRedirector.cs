using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRedirector : MonoBehaviour
{
    public int sceneIndex;  // �ndice de la escena a cargar
    public float interactionRange = 3f;  // Distancia a la que el jugador puede interactuar con el objeto

    private Transform player;  // Referencia al jugador

    // Se llama al iniciar
    void Start()
    {
        // Encuentra al jugador por su etiqueta (aseg�rate de que el jugador tenga la etiqueta "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Se llama cada frame
    void Update()
    {
        // Verifica si el jugador est� cerca del objeto y presiona la tecla "E"
        if (Vector3.Distance(player.position, transform.position) <= interactionRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Redirecciona a la escena usando el �ndice
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }
}
