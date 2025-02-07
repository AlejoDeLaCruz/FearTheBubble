using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class IgnoreLayer : MonoBehaviour
{
    // M�scara de layers a ignorar
    public LayerMask ignoreLayers;

    private Collider myCollider;

    private void Awake()
    {
        // Obtener el collider del objeto para usarlo en la comparaci�n
        myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si la layer del objeto con el que colisionamos est� dentro de la m�scara definida...
        if ((ignoreLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            // Ignorar la colisi�n entre este objeto y el objeto con el que se colision�
            Physics.IgnoreCollision(myCollider, collision.collider);
        }
    }
}