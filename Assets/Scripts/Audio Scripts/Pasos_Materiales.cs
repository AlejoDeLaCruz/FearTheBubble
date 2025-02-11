using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonFootstep : MonoBehaviour
{
    [Header("Configuraci�n de Pasos")]
    [Tooltip("Intervalo de tiempo (segundos) entre cada sonido de paso.")]
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    [Header("Configuraci�n del Raycast")]
    [Tooltip("Distancia m�xima del raycast para detectar el suelo.")]
    public float rayLength = 1.5f;
    [Tooltip("Color del rayo de depuraci�n en la Scene View.")]
    public Color rayColor = Color.red;

    [Header("Switch por Defecto")]
    [Tooltip("Switch por defecto si no se detecta un material espec�fico.")]
    public string defaultSwitch = "Default";

    private CharacterController characterController;

    private void Start()
    {
        // Se intenta obtener el CharacterController del GameObject.
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogWarning("No se encontr� un CharacterController en " + gameObject.name);
        }
    }

    private void Update()
    {
        // Si el jugador se est� moviendo y est� en el suelo, decrementa el temporizador.
        if (IsMoving() && IsGrounded())
        {
            footstepTimer -= Time.deltaTime;

            // Si ha pasado el intervalo definido, reproduce un paso y reinicia el temporizador.
            if (footstepTimer <= 0f)
            {
                Play_Pasos();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            // Si no se est� moviendo o no est� en el suelo, reinicia el temporizador.
            footstepTimer = 0f;
        }
    }

    /// <summary>
    /// Verifica si el jugador se est� moviendo.
    /// </summary>
    /// <returns>True si la velocidad del CharacterController es mayor que un umbral.</returns>
    private bool IsMoving()
    {
        // Puedes ajustar el umbral seg�n la sensibilidad deseada.
        return characterController != null && characterController.velocity.magnitude > 0.1f;
    }

    /// <summary>
    /// Verifica si el jugador est� en el suelo utilizando un raycast.
    /// </summary>
    /// <returns>True si el raycast detecta alg�n collider debajo.</returns>
    private bool IsGrounded()
    {
        // Realiza un raycast hacia abajo para confirmar que hay suelo.
        return Physics.Raycast(transform.position, Vector3.down, rayLength);
    }

    /// <summary>
    /// Realiza el raycast para detectar el material del suelo, dibuja el rayo para depuraci�n
    /// y llama a Wwise para reproducir el sonido del paso.
    /// </summary>
    private void Play_Pasos()
    {
        string materialSwitch = defaultSwitch;
        RaycastHit hit;

        // Dibuja el raycast en la Scene View para facilitar la depuraci�n.
        Debug.DrawRay(transform.position, Vector3.down * rayLength, rayColor, 1f);

        // Realiza el raycast hacia abajo para detectar el material.
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength))
        {
            // Compara el tag del collider para asignar el switch adecuado.
            if (hit.collider.CompareTag("Madera"))
            {
                materialSwitch = "Pasos_Madera";
            }
            else if (hit.collider.CompareTag("Tierra"))
            {
                materialSwitch = "Pasos_Tierra";
            }
            else if (hit.collider.CompareTag("Pasto"))
            {
                materialSwitch = "Pasos_Pasto";
            }
            // Puedes agregar m�s condiciones seg�n tus materiales.
        }

        // Env�a el switch a Wwise.
        AkSoundEngine.SetSwitch("Pasos", materialSwitch, gameObject);

        // Publica el evento de sonido de paso.
        AkSoundEngine.PostEvent("Play_Pasos", gameObject);
    }
}
