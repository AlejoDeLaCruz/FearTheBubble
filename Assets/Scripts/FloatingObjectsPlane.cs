using UnityEngine;
using LowPolyWater; // Incluir el namespace donde se encuentra LowPolyWaterClass

public class FloatingObjectsPlane : MonoBehaviour
{
    public Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 1f;
    public int floaterCount = 1;
    public float waterAngularDrag = 0.5f;
    public float waterDrag = 0.99f;

    // Referencia al script LowPolyWaterClass del objeto de agua
    public LowPolyWaterClass water;

    private void FixedUpdate()
    {
        // Aplicar la gravedad
        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        // Obtener la altura de la ola en la posición del objeto usando LowPolyWaterClass
        float waveHeight = water.GetWaveHeightAtPosition(transform.position);

        // Si el objeto se encuentra por debajo de la altura de la ola, aplica fuerza de flotación
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}