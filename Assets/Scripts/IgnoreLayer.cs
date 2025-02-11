using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class IgnoreLayer : MonoBehaviour
{
    // Máscara de layers a ignorar
    public LayerMask ignoreLayers;

    private Collider myCollider;

    private void Awake()
    {
        // Obtener el collider del objeto para usarlo en la comparación
        myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si la layer del objeto con el que colisionamos está dentro de la máscara definida...
        if ((ignoreLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            // Ignorar la colisión entre este objeto y el objeto con el que se colisionó
            Physics.IgnoreCollision(myCollider, collision.collider);
        }
    }
}