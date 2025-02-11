using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientEventTrigger : MonoBehaviour
{
    // Arrastra y suelta el evento de Wwise desde el Inspector
    public AK.Wwise.Event ambientEvent;

    void Start()
    {
        // Publica el evento usando el objeto asignado
        ambientEvent.Post(gameObject);
    }
}