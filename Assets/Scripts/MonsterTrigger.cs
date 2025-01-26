using UnityEngine;

public class MonsterTrigger : MonoBehaviour
{
    [Header("Configuraci�n")]
    [Tooltip("El tag del jugador para identificarlo al entrar en el collider.")]
    public string playerTag = "Player";

    [Tooltip("El Animator que controla las animaciones.")]
    public Animator targetAnimator;

    [Tooltip("El nombre del trigger de animaci�n que deseas activar.")]
    public string animationTriggerName;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al collider tiene el tag del jugador
        if (other.CompareTag(playerTag))
        {
            // Activa el trigger de la animaci�n
            if (targetAnimator != null && !string.IsNullOrEmpty(animationTriggerName))
            {
                targetAnimator.SetTrigger(animationTriggerName);
            }
            else
            {
                Debug.LogWarning("Animator o nombre del trigger no configurados correctamente.");
            }
        }
    }
}
