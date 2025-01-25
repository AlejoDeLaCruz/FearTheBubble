using UnityEngine;

public class Boya : MonoBehaviour
{
    public Animator animator; // El Animator asignado al GameObject
    public string animationTriggerName = "Play"; // Nombre del Trigger en el Animator

    private void Start()
    {
        // Verifica que el Animator est� asignado
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("No se encontr� un componente Animator. Asigna uno en el Inspector.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colision� tiene la etiqueta "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Dispara la animaci�n usando el Trigger
            if (animator != null)
            {
                animator.SetTrigger(animationTriggerName);
            }
        }
    }
}
