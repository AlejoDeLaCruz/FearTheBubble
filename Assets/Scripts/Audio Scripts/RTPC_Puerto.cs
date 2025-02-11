using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPCUpdater : MonoBehaviour
{
    [Header("Referencias")]
    // Transform del jugador
    public Transform player;
    // Transform del objeto o punto de referencia (por ejemplo, el puerto)
    public Transform targetLocation;

    [Header("Par�metros RTPC")]
    // Nombre del RTPC en Wwise (aseg�rate de que coincida exactamente)
    public string rtpcName = "RTPC_Ambiente";
    // Distancia m�nima y m�xima para normalizar el valor
    public float minDistance = 0f;
    public float maxDistance = 100f;

    void Update()
    {
        // Verifica que las referencias est�n asignadas
        if (player == null || targetLocation == null)
            return;

        // Calcula la distancia entre el jugador y el punto de referencia
        float distance = Vector3.Distance(player.position, targetLocation.position);

        // Normaliza el valor: si la distancia es menor que minDistance, el valor es m�nimo;
        // si es mayor que maxDistance, el valor es m�ximo.
        // Aqu� se asume que el RTPC espera valores entre 0 y 100.
        float rtpcValue = Mathf.Clamp((distance - minDistance) / (maxDistance - minDistance) * 100f, 0f, 100f);

        // Env�a el valor actualizado al RTPC en Wwise.
        // Se asocia al objeto del jugador, lo que es �til si el audio tiene caracter�sticas 3D.
        AkSoundEngine.SetRTPCValue(rtpcName, rtpcValue, player.gameObject);
    }
}
