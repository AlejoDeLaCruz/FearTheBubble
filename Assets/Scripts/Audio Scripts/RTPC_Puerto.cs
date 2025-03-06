using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPC_Puerto : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform targetLocation;
    [SerializeField] private string rtpcName = "RTPC_Ambiente";
    [SerializeField] private float minDistance = 0f;
    [SerializeField] private float maxDistance = 100f;

    void Update()
    {
        if (player == null || targetLocation == null)
        {
            Debug.LogError("Falta asignar el jugador o el Puerto en el Inspector. Por favor, verifica las referencias.");
            return;
        }

        float distance = Vector3.Distance(player.position, targetLocation.position);
        Debug.Log($"Distancia al Puerto: {distance}");

        float normalizedDistance = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
        float inverseNormalizedDistance = 1f - normalizedDistance;
        float rtpcValue = inverseNormalizedDistance * 100f; // Ajusta según el rango en Wwise

        Debug.Log($"RTPC '{rtpcName}' enviado con valor: {rtpcValue}");

        // Envía el valor al RTPC en Wwise, asociado al jugador
        AkSoundEngine.SetRTPCValue(rtpcName, rtpcValue, player.gameObject);
    }
}