using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Sensibilidad del Mouse")]
    public float sensX;
    public float sensY;

    [Header("Referencia a la orientación (solo se actualiza el yaw)")]
    public Transform orientation;

    [Header("Inclinación por dirección (WASD)")]
    [Tooltip("Intensidad de la inclinación en pitch (vertical). Al presionar W se inclina hacia adelante.")]
    public float tiltPitch = 3f;
    [Tooltip("Intensidad de la inclinación en roll (horizontal). Al presionar D se inclina hacia la derecha.")]
    public float tiltRoll = 3f;

    [Header("Movimiento aleatorio (efecto 'mareo')")]
    [Tooltip("Amplitud del movimiento aleatorio en el eje X (pitch).")]
    public float randomJitterAmplitudeX = 1f;
    [Tooltip("Amplitud del movimiento aleatorio en el eje Y (yaw).")]
    public float randomJitterAmplitudeY = 1f;
    [Tooltip("Amplitud del movimiento aleatorio en el eje Z (roll).")]
    public float randomJitterAmplitudeZ = 1f;
    [Tooltip("Frecuencia del movimiento aleatorio.")]
    public float randomJitterFrequency = 1f;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Lectura del mouse para la rotación básica.
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // --- Inclinación según entrada WASD ---
        // Se utiliza la misma entrada que en el script de movimiento.
        float inputHorizontal = Input.GetAxis("Horizontal"); // A y D
        float inputVertical = Input.GetAxis("Vertical");         // W y S

        // Se invierten los signos para corregir las direcciones:
        // Al presionar W (inputVertical positivo) se inclina hacia adelante.
        float tiltOffsetX = inputVertical * tiltPitch;
        // Al presionar D (inputHorizontal positivo) se inclina hacia la derecha.
        float tiltOffsetZ = -inputHorizontal * tiltRoll;

        // --- Movimiento aleatorio (jitter) ---
        // Usamos Perlin noise para obtener un valor suave aleatorio en cada eje.
        float jitterX = (Mathf.PerlinNoise(Time.time * randomJitterFrequency, 0f) - 0.5f) * 2f * randomJitterAmplitudeX;
        float jitterY = (Mathf.PerlinNoise(Time.time * randomJitterFrequency, 1f) - 0.5f) * 2f * randomJitterAmplitudeY;
        float jitterZ = (Mathf.PerlinNoise(Time.time * randomJitterFrequency, 2f) - 0.5f) * 2f * randomJitterAmplitudeZ;

        // --- Combinamos todas las rotaciones ---
        // Se suman al ángulo base obtenido del mouse los offsets de inclinación y jitter.
        float finalX = xRotation + tiltOffsetX + jitterX;
        float finalY = yRotation + jitterY;
        float finalZ = tiltOffsetZ + jitterZ;

        transform.rotation = Quaternion.Euler(finalX, finalY, finalZ);

        // La orientación se mantiene con solo el yaw (sin inclinaciones ni jitter) para fines de movimiento.
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
