using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarModoTormenta : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement movimientoJugador = other.GetComponent<Movement>();

            if (movimientoJugador != null)
            {
                movimientoJugador.ActivarModoTormenta();
            }
        }
    }
}
