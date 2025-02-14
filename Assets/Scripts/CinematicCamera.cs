using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CinematicCamera : MonoBehaviour
{
    [Header("Referencias Obligatorias")]
    [SerializeField] private Transform cinematicTarget;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private PlayerCam cameraMovementScript;

    [Header("Referencias Adicionales")]
    [SerializeField] private GameObject oceanGameObject;    // Asigna desde el Inspector el GameObject del Océano
    [SerializeField] private GameObject floorGameObject;      // Asigna desde el Inspector el GameObject del Piso

    [Header("Ajustes de Transición")]
    [SerializeField] private float transitionSpeed = 2.0f; // Ajusta este valor para modificar la velocidad de transición
    [SerializeField] private float rotationThreshold = 1f;

    [Header("Control de Tiempo")]
    [SerializeField] private float cinematicDuration = 0f;
    [SerializeField] private KeyCode exitKey = KeyCode.Space;

    [Header("Configuración Avanzada")]
    [SerializeField] private bool lockCursor = true;
    [SerializeField] private bool resetRotationOnExit = false;

    // Variables para guardar el estado original
    private float _originalSensX;
    private float _originalSensY;
    private Quaternion _originalCameraRotation;
    private Quaternion _targetRotation;
    private Movement _playerMovement;
    private Rigidbody _playerRigidbody;
    private bool _isCinematicActive;
    private float _cinematicTimer;

    [Header("Subtítulos")]
    [SerializeField] private ActiveSubtitles subtitleController; // Asigna desde el Inspector

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<Movement>();
        _playerRigidbody = player.GetComponent<Rigidbody>();
        _originalCameraRotation = playerCamera.transform.rotation;

        // Guardar configuración original de sensibilidad
        _originalSensX = cameraMovementScript.sensX;
        _originalSensY = cameraMovementScript.sensY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isCinematicActive)
        {
            StartCinematic();
        }
    }

    private void StartCinematic()
    {
        _isCinematicActive = true;
        _cinematicTimer = cinematicDuration;

        // Activar subtítulos
        if (subtitleController != null)
        {
            subtitleController.gameObject.SetActive(true);
            subtitleController.StartDialogue();
        }

        // Bloquear el control del mouse (manteniendo otros efectos)
        // Se anula la sensibilidad de la cámara para impedir su movimiento durante la cinemática.
        cameraMovementScript.sensX = 0;
        cameraMovementScript.sensY = 0;

        // Congelar movimiento del jugador, excepto en el eje Y
        _playerMovement.enabled = false;
        _playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX |
                                         RigidbodyConstraints.FreezePositionZ |
                                         RigidbodyConstraints.FreezeRotation;

        // Calcular la rotación objetivo para la cámara
        Vector3 direction = cinematicTarget.position - playerCamera.transform.position;
        _targetRotation = Quaternion.LookRotation(direction.normalized);

        // Configurar cursor
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    private void Update()
    {
        if (!_isCinematicActive) return;

        ApplyCameraTransition();
        MaintainCameraLock();
        HandleCinematicExit();
    }

    // Interpolación suave de la rotación de la cámara
    private void ApplyCameraTransition()
    {
        playerCamera.transform.rotation = Quaternion.Slerp(
            playerCamera.transform.rotation,
            _targetRotation,
            transitionSpeed * Time.deltaTime
        );
    }

    private void MaintainCameraLock()
    {
        // Si la diferencia de ángulo es mínima, forzamos la rotación final y reafirmamos las restricciones
        if (Quaternion.Angle(playerCamera.transform.rotation, _targetRotation) <= rotationThreshold)
        {
            playerCamera.transform.rotation = _targetRotation;
            _playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX |
                                             RigidbodyConstraints.FreezePositionZ |
                                             RigidbodyConstraints.FreezeRotation;
        }
    }

    private void HandleCinematicExit()
    {
        if (cinematicDuration > 0 && (_cinematicTimer -= Time.deltaTime) <= 0) EndCinematic();
        if (Input.GetKeyDown(exitKey)) EndCinematic();
    }

    public void EndCinematic()
    {
        if (!_isCinematicActive) return;

        // Desactivar subtítulos
        if (subtitleController != null)
        {
            subtitleController.gameObject.SetActive(false);
        }

        // Restaurar la sensibilidad de la cámara para permitir el movimiento libre de ésta
        cameraMovementScript.sensX = _originalSensX;
        cameraMovementScript.sensY = _originalSensY;

        if (resetRotationOnExit)
            playerCamera.transform.rotation = _originalCameraRotation;

        // Configuración del cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Desactivar el script Follow en la cámara principal
        Follow followCam = playerCamera.GetComponent<Follow>();
        if (followCam != null)
        {
            followCam.enabled = false;
        }

        // Desactivar el script Follow en el Océano
        if (oceanGameObject != null)
        {
            Follow followOcean = oceanGameObject.GetComponent<Follow>();
            if (followOcean != null)
            {
                followOcean.enabled = false;
            }
        }

        // Esperar un breve lapso para la transición y luego desactivar el piso, permitiendo que el jugador caiga
        StartCoroutine(DisableFloorAfterDelay(0.5f));

        // ACTIVAR: Se habilita el script SlowFall para aplicar la gravedad personalizada de caída lenta
        SlowFall slowFall = _playerRigidbody.GetComponent<SlowFall>();
        if (slowFall != null)
        {
            slowFall.enabled = true;
        }

        // Importante: No reactivamos el movimiento del jugador (_playerMovement)
        // ni liberamos las restricciones del Rigidbody, para que el jugador
        // permanezca inmóvil en X y Z y en rotación.
        _isCinematicActive = false;
    }

    private IEnumerator DisableFloorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (floorGameObject != null)
        {
            floorGameObject.SetActive(false);
        }
    }
}
