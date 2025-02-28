using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasos : MonoBehaviour
{
    // Configuración de Wwise
    [Header("Configuración de Audio (Wwise)")]
    public string switchGroup = "Pasos";       // Nombre del grupo de switches en Wwise
    public string defaultMaterial = "Pasos_Pasto";     // Material por defecto si no se detecta superficie

    // Configuración de pasos
    [Header("Configuración de Pasos")]
    public float raycastDistance = 1.0f;          // Distancia máxima del raycast
    public float stepInterval = 0.5f;             // Tiempo entre cada paso (en segundos)
    private string currentMaterial;               // Material actual detectado
    private float stepTimer = 0f;                 // Temporizador para los pasos

    void Start()
    {
        // Asegura que el objeto tenga el componente AkGameObj para Wwise
        if (!gameObject.GetComponent<AkGameObj>())
        {
            gameObject.AddComponent<AkGameObj>();
        }
        // Inicializa el material actual con el valor por defecto
        currentMaterial = defaultMaterial;
        AkSoundEngine.SetSwitch(switchGroup, currentMaterial, gameObject);
    }

    void Update()
    {
        // Detectar el material de la superficie
        UpdateSurfaceMaterial();

        // Controlar la reproducción de sonidos de pasos
        HandleFootsteps();
    }

    void UpdateSurfaceMaterial()
    {
        // Lanzar un raycast hacia abajo para detectar la superficie
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // Obtener el tag del objeto impactado
            string detectedMaterial = hit.collider.gameObject.tag;

            // Verificar si el tag corresponde a uno de los materiales permitidos
            if (detectedMaterial == "Pasos_Madera" || detectedMaterial == "Pasos_Tierra" || detectedMaterial == "Pasos_Pasto")
            {
                // Actualizar el switch de Wwise solo si el material ha cambiado
                if (detectedMaterial != currentMaterial)
                {
                    currentMaterial = detectedMaterial;
                    AkSoundEngine.SetSwitch(switchGroup, currentMaterial, gameObject);
                }
            }
        }
        else
        {
            // Si no se detecta superficie, usar el material por defecto
            if (currentMaterial != defaultMaterial)
            {
                currentMaterial = defaultMaterial;
                AkSoundEngine.SetSwitch(switchGroup, currentMaterial, gameObject);
            }
        }
    }

    void HandleFootsteps()
    {
        // Verificar si se presiona cualquiera de las teclas de movimiento (W, A, S, D)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // Incrementar el temporizador
            stepTimer += Time.deltaTime;

            // Si el temporizador supera el intervalo, disparar un paso
            if (stepTimer >= stepInterval)
            {
                AkSoundEngine.PostEvent("Play_Pasos", gameObject);
                stepTimer = 0f; // Reiniciar el temporizador
            }
        }
        else
        {
            // Reiniciar el temporizador cuando no se está caminando
            stepTimer = 0f;
        }
    }
}