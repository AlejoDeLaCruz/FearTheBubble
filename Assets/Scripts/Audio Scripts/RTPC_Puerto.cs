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

    [Header("Parámetros RTPC")]
    // Nombre del RTPC en Wwise (asegúrate de que coincida exactamente)
    public string rtpcName = "RTPC_Ambiente";
    // Distancia mínima y máxima para normalizar el valor
    public float minDistance = 0f;
    public float maxDistance = 100f;

    void Update()
    {
        // Verifica que las referencias estén asignadas
        if (player == null || targetLocation == null)
            return;

        // Calcula la distancia entre el jugador y el punto de referencia
        float distance = Vector3.Distance(player.position, targetLocation.position);

        // Normaliza el valor: si la distancia es menor que minDistance, el valor es mínimo;
        // si es mayor que maxDistance, el valor es máximo.
        // Aquí se asume que el RTPC espera valores entre 0 y 100.
        float rtpcValue = Mathf.Clamp((distance - minDistance) / (maxDistance - minDistance) * 100f, 0f, 100f);

        // Envía el valor actualizado al RTPC en Wwise.
        // Se asocia al objeto del jugador, lo que es útil si el audio tiene características 3D.
        AkSoundEngine.SetRTPCValue(rtpcName, rtpcValue, player.gameObject);
    }
}
