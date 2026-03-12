using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;

    private float currentSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null)
            playerCamera = Camera.main;
        
        Debug.Log("Jogo iniciado! Você está preso na escola...");
    }

    void Update()
    {
        if (!canMove) return;

        HandleMovement();
        HandleInteraction();
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        
        rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);
    }

    void HandleMovement()
    {
        float horizontal;
        float vertical;

    #if ENABLE_INPUT_SYSTEM
        horizontal = 0f;
        vertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
            if (Keyboard.current.dKey.isPressed) horizontal += 1f;
            if (Keyboard.current.sKey.isPressed) vertical -= 1f;
            if (Keyboard.current.wKey.isPressed) vertical += 1f;
        }
    #else
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    #endif

        moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Sprint
        bool isSprinting;
    #if ENABLE_INPUT_SYSTEM
        isSprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
    #else
        isSprinting = Input.GetKey(KeyCode.LeftShift);
    #endif
        currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Rotação com mouse
        float mouseX;
    #if ENABLE_INPUT_SYSTEM
        mouseX = Mouse.current != null ? Mouse.current.delta.ReadValue().x * 0.05f : 0f;
    #else
        mouseX = Input.GetAxis("Mouse X") * 2f;
    #endif
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleInteraction()
    {
        bool interactPressed;
    #if ENABLE_INPUT_SYSTEM
        interactPressed = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
    #else
        interactPressed = Input.GetKeyDown(KeyCode.E);
    #endif

        if (interactPressed)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector3.zero;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void SetCamera(Camera cam)
    {
        playerCamera = cam;
    }
}
