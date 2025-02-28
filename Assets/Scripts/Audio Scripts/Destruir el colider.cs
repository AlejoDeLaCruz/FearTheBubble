using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruirelcolider : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private bool destroyCollider = true; // Destruir o desactivar
    [SerializeField] private string playerTag = "Player"; // Tag del jugador

    private BoxCollider boxCollider;

    void Start()
    {
        // Obtener el componente BoxCollider
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogError("No se encontró un BoxCollider en este GameObject.");
            enabled = false; // Desactiva el script si no hay BoxCollider
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra tiene el tag del jugador
        if (other.CompareTag(playerTag))
        {
            if (destroyCollider)
            {
                Destroy(boxCollider); // Destruye el BoxCollider
                Debug.Log("BoxCollider destruido.");
            }
            else
            {
                boxCollider.enabled = false; // Desactiva el BoxCollider
                Debug.Log("BoxCollider desactivado.");
            }
        }
    }
}