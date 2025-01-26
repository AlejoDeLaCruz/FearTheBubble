using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Configuración de la Luz")]
    [Tooltip("El tiempo que tarda la luz en parpadear (suavizado).")]
    public float flickerDuration = 0.5f;

    [Tooltip("Variación aleatoria en el tiempo de parpadeo.")]
    public float randomVariance = 0.2f;

    [Tooltip("Referencia a la luz que se controlará.")]
    public Light targetLight;

    private float targetIntensity;
    private float originalIntensity;

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }

        if (targetLight == null)
        {
            Debug.LogError("No se encontró ninguna luz. Asigna una luz al script.");
        }

        originalIntensity = targetLight.intensity;
        StartCoroutine(FlickerLight());
    }

    private System.Collections.IEnumerator FlickerLight()
    {
        while (true)
        {
            targetIntensity = Random.Range(0f, originalIntensity); // Intensidad aleatoria
            float duration = flickerDuration + Random.Range(-randomVariance, randomVariance);

            float elapsedTime = 0f;
            float startIntensity = targetLight.intensity;

            while (elapsedTime < duration)
            {
                targetLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetLight.intensity = targetIntensity;

            // Espera antes de cambiar de nuevo
            yield return new WaitForSeconds(duration);
        }
    }
}
