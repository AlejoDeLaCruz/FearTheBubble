using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public AK.Wwise.Event wwiseEvent; // Arrastra el evento desde Wwise

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        {
            // Reproduce el evento (se repetirá si está configurado en Wwise)
            wwiseEvent.Post(gameObject);

            // Evita futuros triggers
            GetComponent<Collider>().enabled = false;
            hasTriggered = true;
        }
    }
}


