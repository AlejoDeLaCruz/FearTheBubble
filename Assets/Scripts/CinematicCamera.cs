using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class CinematicCamera : MonoBehaviour
{
    [Header("Referencias Obligatorias")]
    [SerializeField] private Transform cinematicTarget;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private PlayerCam cameraMovementScript;

    [Header("Referencias Adicionales")]
    [SerializeField] private GameObject oceanGameObject;
    [SerializeField] private GameObject floorGameObject;

    [Header("Ajustes de Transición")]
    [SerializeField] private float transitionSpeed = 2.0f;
    [SerializeField] private float rotationThreshold = 1f;

    [Header("Control de Tiempo")]
    [SerializeField] private float cinematicDuration = 0f;
    [SerializeField] private KeyCode exitKey = KeyCode.Space;

    [Header("Configuración Avanzada")]
    [SerializeField] private bool lockCursor = true;
    [SerializeField] private bool resetRotationOnExit = false;

    [Header("Efectos de Locura")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private float madnessStartDelay = 5f;
    [SerializeField] private float madnessTransitionDuration = 30f; // Aumentado para mayor gradualidad

    float saturation = 1.0f;
    float value = 1.0f;

    private float _originalSensX;
    private float _originalSensY;
    private Quaternion _originalCameraRotation;
    private Quaternion _targetRotation;
    private Movement _playerMovement;
    private Rigidbody _playerRigidbody;
    private bool _isCinematicActive;
    private float _cinematicTimer;

    private Vignette _vignette;
    private ChromaticAberration _chromatic;
    private ColorAdjustments _colorAdjustments;
    private float _madnessTimer;
    private bool _isMadnessActive;
    private Color _originalVignetteColor;
    private float _originalVignetteIntensity;
    private float _originalChromaticIntensity;
    private float _originalHueShift;

    [Header("Subtítulos")]
    [SerializeField] private ActiveSubtitles subtitleController;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<Movement>();
        _playerRigidbody = player.GetComponent<Rigidbody>();
        _originalCameraRotation = playerCamera.transform.rotation;

        _originalSensX = cameraMovementScript.sensX;
        _originalSensY = cameraMovementScript.sensY;

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out _vignette);
            postProcessVolume.profile.TryGet(out _chromatic);
            postProcessVolume.profile.TryGet(out _colorAdjustments);

            if (_vignette != null)
            {
                _originalVignetteColor = _vignette.color.value;
                _originalVignetteIntensity = _vignette.intensity.value;
            }
            if (_chromatic != null) _originalChromaticIntensity = _chromatic.intensity.value;
            if (_colorAdjustments != null) _originalHueShift = _colorAdjustments.hueShift.value;

            postProcessVolume.enabled = false;
            postProcessVolume.weight = 0f; // Inicializar peso en 0
        }
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
        _madnessTimer = 0f;
        _isMadnessActive = false;
        ResetMadnessEffects();

        if (subtitleController != null)
        {
            subtitleController.gameObject.SetActive(true);
            subtitleController.StartDialogue();
        }

        cameraMovementScript.sensX = 0;
        cameraMovementScript.sensY = 0;

        _playerMovement.enabled = false;
        _playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX |
                                         RigidbodyConstraints.FreezePositionZ |
                                         RigidbodyConstraints.FreezeRotation;

        Vector3 direction = cinematicTarget.position - playerCamera.transform.position;
        _targetRotation = Quaternion.LookRotation(direction.normalized);

        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    private void Update()
    {
        if (!_isCinematicActive) return;

        ApplyCameraTransition();
        MaintainCameraLock();
        HandleCinematicExit();
        HandleMadnessEffects();
    }

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

    private void HandleMadnessEffects()
    {
        if (postProcessVolume == null) return;

        _madnessTimer += Time.deltaTime;

        if (_madnessTimer >= madnessStartDelay && !_isMadnessActive)
        {
            _isMadnessActive = true;
            _madnessTimer = 0f;
            postProcessVolume.enabled = true;
        }

        if (_isMadnessActive)
        {
            // Suavizado extremo usando curva cuadrática
            float rawProgress = Mathf.Clamp01(_madnessTimer / madnessTransitionDuration);
            float progress = Mathf.Pow(rawProgress, 3); // Curva cúbica para inicio más lento
            float hue = Mathf.PingPong(Time.time * 0.1f, 1); // Oscilación más lenta

            // Control de peso del Volume
            postProcessVolume.weight = Mathf.SmoothStep(0f, 1f, progress);

            if (_vignette != null)
            {
                _vignette.color.value = Color.Lerp(_originalVignetteColor,
                    Color.HSVToRGB(hue, saturation, value),
                    progress * 0.8f); // Reducir intensidad del cambio

                _vignette.intensity.value = Mathf.Lerp(_originalVignetteIntensity,
                    0.6f,
                    progress * 0.5f); // Transición más suave
            }

            if (_chromatic != null)
            {
                _chromatic.intensity.value = Mathf.Lerp(_originalChromaticIntensity,
                    1f,
                    progress * 0.6f); // Aumento más gradual
            }

            if (_colorAdjustments != null)
            {
                _colorAdjustments.hueShift.value = Mathf.Lerp(_originalHueShift,
                    360f,
                    progress * 0.4f); // Rotación más lenta
            }
        }
    }

    private void ResetMadnessEffects()
    {
        if (_vignette != null)
        {
            _vignette.color.value = _originalVignetteColor;
            _vignette.intensity.value = _originalVignetteIntensity;
        }
        if (_chromatic != null) _chromatic.intensity.value = _originalChromaticIntensity;
        if (_colorAdjustments != null) _colorAdjustments.hueShift.value = _originalHueShift;
    }

    public void EndCinematic()
    {
        if (!_isCinematicActive) return;

        if (subtitleController != null)
        {
            subtitleController.gameObject.SetActive(false);
        }

        cameraMovementScript.sensX = _originalSensX;
        cameraMovementScript.sensY = _originalSensY;

        if (resetRotationOnExit)
            playerCamera.transform.rotation = _originalCameraRotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Follow followCam = playerCamera.GetComponent<Follow>();
        if (followCam != null) followCam.enabled = false;

        if (oceanGameObject != null)
        {
            Follow followOcean = oceanGameObject.GetComponent<Follow>();
            if (followOcean != null) followOcean.enabled = false;
        }

        StartCoroutine(DisableFloorAfterDelay(0.5f));

        SlowFall slowFall = _playerRigidbody.GetComponent<SlowFall>();
        if (slowFall != null) slowFall.enabled = true;

        ResetMadnessEffects();
        if (postProcessVolume != null)
        {
            postProcessVolume.weight = 0f;
            postProcessVolume.enabled = false;
        }
        _isMadnessActive = false;
        _isCinematicActive = false;
    }

    private IEnumerator DisableFloorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (floorGameObject != null) floorGameObject.SetActive(false);
    }
}