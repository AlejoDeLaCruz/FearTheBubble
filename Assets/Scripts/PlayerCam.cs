using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Sensibilidad del Mouse")]
    public float sensX;
    public float sensY;

    [Header("Referencia a la orientaci�n (solo se actualiza el yaw)")]
    public Transform orientation;

    [Header("Inclinaci�n por direcci�n (WASD)")]
    [Tooltip("Intensidad de la inclinaci�n en pitch (vertical). Al presionar W se inclina hacia adelante.")]
    public float tiltPitch = 3f;
    [Tooltip("Intensidad de la inclinaci�n en roll (horizontal). Al presionar D se inclina hacia la derecha.")]
    public float tiltRoll = 3f;

    [Header("Movimiento aleatorio (efecto 'mareo')")]
    [Tooltip("Amplitud del movimiento aleatorio en el eje X (pitch).")]
    public float randomJitterAmplitudeX = 1f;
    [Tooltip("Amplitud del movimiento aleatorio en el eje Y (yaw).")]
    public float randomJitterAmplitudeY = 1f;
    [Tooltip("Amplitud del movimiento aleatorio en el eje Z (roll).")]
    public float randomJitterAmplitudeZ = 1f;
    [Tooltip("Frecuencia base del movimiento aleatorio.")]
    public float randomJitterFrequency = 1f;
    [Tooltip("Factor para ralentizar el movimiento aleatorio. Valores menores a 1 lo har�n m�s lento.")]
    public float randomJitterSpeedFactor = 0.3f;

    [Header("Simulaci�n de pasos")]
    [Tooltip("Si el jugador est� en el suelo, se simula el movimiento de pasos en la c�mara.")]
    public bool isOnGround = true;
    [Tooltip("Velocidad del efecto de pasos (head bobbing).")]
    public float footstepBobbingSpeed = 8f;
    [Tooltip("Intensidad del efecto de pasos (head bobbing).")]
    public float footstepBobbingAmount = 0.05f;

    float xRotation;
    float yRotation;

    // Se guarda la posici�n inicial para aplicar el offset de head bobbing
    private Vector3 initialCamPos;

    // Acumulador para el head bobbing (se actualiza solo cuando hay movimiento)
    private float bobbingTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialCamPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Lectura del mouse para la rotaci�n b�sica.
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // --- Inclinaci�n seg�n entrada WASD ---
        float inputHorizontal = Input.GetAxis("Horizontal"); // A y D
        float inputVertical = Input.GetAxis("Vertical");         // W y S

        // Ajuste de inclinaci�n:
        float tiltOffsetX = inputVertical * tiltPitch;
        float tiltOffsetZ = -inputHorizontal * tiltRoll;

        // --- Movimiento aleatorio (jitter) ---
        float jitterX = (Mathf.PerlinNoise(Time.time * randomJitterFrequency * randomJitterSpeedFactor, 0f) - 0.5f) * 2f * randomJitterAmplitudeX;
        float jitterY = (Mathf.PerlinNoise(Time.time * randomJitterFrequency * randomJitterSpeedFactor, 1f) - 0.5f) * 2f * randomJitterAmplitudeY;
        float jitterZ = (Mathf.PerlinNoise(Time.time * randomJitterFrequency * randomJitterSpeedFactor, 2f) - 0.5f) * 2f * randomJitterAmplitudeZ;

        // --- Combinamos todas las rotaciones ---
        float finalX = xRotation + tiltOffsetX + jitterX;
        float finalY = yRotation + jitterY;
        float finalZ = tiltOffsetZ + jitterZ;

        transform.rotation = Quaternion.Euler(finalX, finalY, finalZ);

        // La orientaci�n se mantiene solo con el yaw (sin inclinaci�n ni jitter) para fines de movimiento.
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Simulaci�n de pasos (head bobbing) en la posici�n de la c�mara:
        // Se activar� solo si isOnGround es true y el jugador se est� moviendo.
        if (isOnGround && (Mathf.Abs(inputHorizontal) > 0.01f || Mathf.Abs(inputVertical) > 0.01f))
        {
            bobbingTimer += Time.deltaTime * footstepBobbingSpeed;
            float bobbingOffset = Mathf.Sin(bobbingTimer) * footstepBobbingAmount;
            Vector3 newPos = initialCamPos;
            newPos.y += bobbingOffset;
            transform.localPosition = newPos;
        }
        else
        {
            bobbingTimer = 0f;
            transform.localPosition = initialCamPos;
        }
    }
}
