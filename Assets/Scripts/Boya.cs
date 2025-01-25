using UnityEngine;

public class Boya : MonoBehaviour
{
    public Animator animator; // El Animator asignado al GameObject
    public string animationTriggerName = "Play"; // Nombre del Trigger en el Animator

    private void Start()
    {
        // Verifica que el Animator esté asignado
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("No se encontró un componente Animator. Asigna uno en el Inspector.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisionó tiene la etiqueta "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Dispara la animación usando el Trigger
            if (animator != null)
            {
                animator.SetTrigger(animationTriggerName);
            }
        }
    }
}
