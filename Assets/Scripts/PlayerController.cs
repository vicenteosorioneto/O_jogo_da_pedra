using UnityEngine;

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
        
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.z * currentSpeed);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Sprint
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Rotação com mouse
        float mouseX = Input.GetAxis("Mouse X") * 2f;
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        rb.velocity = Vector3.zero;
    }

    public void EnableMovement()
    {
        canMove = true;
    }
}
