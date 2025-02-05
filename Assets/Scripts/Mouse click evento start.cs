using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    // Nombre del evento tal como lo configuraste en Wwise.
    public string startButtonEvent = "Play_Start";

    // Este m�todo se llamar� al hacer click en el bot�n.
    public void OnStartButtonClick()
    {
        // Se dispara el evento de Wwise. El 'gameObject' pasado se usa como referencia espacial (si aplica).
        AkSoundEngine.PostEvent(startButtonEvent, gameObject);
    }
}