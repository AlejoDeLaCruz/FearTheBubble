using UnityEngine;
using LowPolyWater;
using System.Collections;
using System.Collections.Generic;

public class ActivarModoTormenta : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [SerializeField] private float duracionTransicion = 5f; // Tiempo en segundos para llegar a los valores máximos

    private LowPolyWaterClass water;
    private Coroutine transicionCoroutine;
    private bool transicionEnCurso = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activar modo tormenta en jugador
            Movement movimientoJugador = other.GetComponent<Movement>();
            if (movimientoJugador != null) movimientoJugador.ActivarModoTormenta();

            // Buscar agua si no está asignada
            if (water == null)
            {
                water = FindObjectOfType<LowPolyWaterClass>();
            }

            if (water != null && !transicionEnCurso)
            {
                // Guardar valores iniciales
                ValoresAgua valoresIniciales = new ValoresAgua(
                    water.waveHeight,
                    water.waveFrequency,
                    water.waveLength
                );

                // Valores objetivo
                ValoresAgua valoresObjetivo = new ValoresAgua(0.48f, 0.35f, 0.27f);

                // Iniciar transición
                transicionCoroutine = StartCoroutine(TransicionarAgua(valoresIniciales, valoresObjetivo));
            }
        }
    }

    IEnumerator TransicionarAgua(ValoresAgua inicio, ValoresAgua objetivo)
    {
        transicionEnCurso = true;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicion)
        {
            float porcentaje = tiempoTranscurrido / duracionTransicion;
            porcentaje = Mathf.SmoothStep(0f, 1f, porcentaje); // Suavizar la transición

            // Interpolar cada valor
            water.waveHeight = Mathf.Lerp(inicio.waveHeight, objetivo.waveHeight, porcentaje);
            water.waveFrequency = Mathf.Lerp(inicio.waveFrequency, objetivo.waveFrequency, porcentaje);
            water.waveLength = Mathf.Lerp(inicio.waveLength, objetivo.waveLength, porcentaje);

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurar valores finales exactos
        water.waveHeight = objetivo.waveHeight;
        water.waveFrequency = objetivo.waveFrequency;
        water.waveLength = objetivo.waveLength;

        transicionEnCurso = false;
    }

    // Clase auxiliar para almacenar valores
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