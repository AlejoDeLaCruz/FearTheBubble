using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class FootstepSystem : MonoBehaviour
{
    [System.Serializable]
    public class MaterialSound
    {
        public SurfaceMaterial material;
        public string wwiseSwitch;
    }

    [Header("Configuración de Pasos")]
    [SerializeField] private float stepInterval = 0.4f;
    [SerializeField] private float rayDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector3 rayOffset = new Vector3(0, 0.1f, 0);

    [Header("Asignación de Sonidos")]
    [SerializeField] private MaterialSound[] materialSounds;
    [SerializeField] private SurfaceMaterial defaultMaterial = SurfaceMaterial.Tierra;

    private float timer;
    private SurfaceMaterial currentMaterial;

    void Update()
    {
        CheckGroundMaterial();
        HandleFootsteps();
    }

    void CheckGroundMaterial()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayOffset;

        if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance, groundMask))
        {
            MaterialIdentifier identifier = hit.collider.GetComponent<MaterialIdentifier>();
            currentMaterial = identifier ? identifier.surfaceMaterial : defaultMaterial;
        }
        else
        {
            currentMaterial = defaultMaterial;
        }
    }

    void HandleFootsteps()
    {
        if (ShouldPlayStep())
        {
            PlayStepSound();
            timer = 0f;
        }
        timer += Time.deltaTime;
    }

    bool ShouldPlayStep()
    {
        return timer >= stepInterval &&
               GetComponent<CharacterController>().velocity.magnitude > 0.1f &&
               currentMaterial != defaultMaterial;
    }

    void PlayStepSound()
    {
        foreach (MaterialSound sound in materialSounds)
        {
            if (sound.material == currentMaterial)
            {
                AkSoundEngine.SetSwitch("Pasos", sound.wwiseSwitch, gameObject);
                AkSoundEngine.PostEvent("Play_Pasos", gameObject);
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = transform.position + rayOffset;
        Gizmos.DrawLine(origin, origin + Vector3.down * rayDistance);
    }
}