using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRain : MonoBehaviour
{
    public GameObject sistemaDeLluvia; // Referencia al GameObject del sistema de part�culas de lluvia

    private bool lluviaActivada = false; // Variable para controlar si la lluvia ya se activ�

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al collider es el jugador
        if (other.CompareTag("Player") && !lluviaActivada)
        {
            // Activa el sistema de part�culas de lluvia
            if (sistemaDeLluvia != null)
            {
                sistemaDeLluvia.SetActive(true);
                lluviaActivada = true; // Marca que la lluvia ya se activ�
                Debug.Log("�Lluvia activada!");
            }
            else
            {
                Debug.LogWarning("No se ha asignado un sistema de lluvia en el inspector.");
            }
        }
    }
}