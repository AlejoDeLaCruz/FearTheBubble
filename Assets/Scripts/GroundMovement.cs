using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroundMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Buttons")]
    public GameObject activationButton; // El botón para activar el movimiento
    public GameObject exitButton; // El botón para salir del juego

    private bool isActivated = false; // Indica si el movimiento está activado

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        // Desactivamos el movimiento inicialmente
        isActivated = false;
    }

    private void Update()
    {
        // El script no funcionará hasta que esté activado
        if (!isActivated) return;

        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // Handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (isActivated)
        {
            MovePlayer();
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);
        }
        // In air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 1f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void ActivateMovement()
    {
        // Este método es llamado por el botón
        isActivated = true;

        // Destruye el botón una vez activado
        if (activationButton != null)
        {
            Destroy(activationButton);
        }
    }

    public void ExitGame()
    {
        // Este método es llamado por el botón de salida
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Nota: En el editor de Unity, Application.Quit no cierra el editor.
        // Para probar este comportamiento, puedes agregar un mensaje en el log como el de arriba.
    }
}