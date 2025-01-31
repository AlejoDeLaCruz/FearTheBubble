using UnityEngine;
using LowPolyWater;
using System.Collections;

public class ActivarModoTormenta : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [SerializeField] private float duracionTransicion = 5f;

    private LowPolyWaterClass water;
    private Coroutine transicionCoroutine;
    private bool transicionEnCurso = false;

    // Almacena los valores iniciales del agua (bajos)
    private ValoresAgua valoresIniciales;

    // Nuevos valores objetivo (altos)
    private ValoresAgua valoresObjetivo = new ValoresAgua(0.48f, 0.35f, 0.27f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement movimientoJugador = other.GetComponent<Movement>();
            if (movimientoJugador != null) movimientoJugador.ActivarModoTormenta();

            if (water == null) water = FindObjectOfType<LowPolyWaterClass>();

            if (water != null && !transicionEnCurso)
            {
                // Captura los valores iniciales actuales del agua
                valoresIniciales = new ValoresAgua(
                    water.waveHeight,
                    water.waveFrequency,
                    water.waveLength
                );

                transicionCoroutine = StartCoroutine(TransicionarAgua());
            }
        }
    }

    IEnumerator TransicionarAgua()
    {
        transicionEnCurso = true;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicion)
        {
            float porcentaje = tiempoTranscurrido / duracionTransicion;

            // Usar una curva de aceleración para empezar lento y aumentar progresivamente
            porcentaje = Mathf.Pow(porcentaje, 2); // Cambia el exponente para ajustar la velocidad

            // Interpolar desde los valores iniciales (bajos) hasta los objetivos (altos)
            water.waveHeight = Mathf.Lerp(valoresIniciales.waveHeight, valoresObjetivo.waveHeight, porcentaje);
            water.waveFrequency = Mathf.Lerp(valoresIniciales.waveFrequency, valoresObjetivo.waveFrequency, porcentaje);
            water.waveLength = Mathf.Lerp(valoresIniciales.waveLength, valoresObjetivo.waveLength, porcentaje);

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurar valores finales
        water.waveHeight = valoresObjetivo.waveHeight;
        water.waveFrequency = valoresObjetivo.waveFrequency;
        water.waveLength = valoresObjetivo.waveLength;

        transicionEnCurso = false;
    }

    // Clase auxiliar y métodos restantes igual...

    private class ValoresAgua
    {
        public readonly float waveHeight;
        public readonly float waveFrequency;
        public readonly float waveLength;

        public ValoresAgua(float h, float f, float l)
        {
            waveHeight = h;
            waveFrequency = f;
            waveLength = l;
        }
    }

    private void OnDisable()
    {
        if (transicionCoroutine != null)
        {
            StopCoroutine(transicionCoroutine);
            transicionEnCurso = false;
        }
    }
}