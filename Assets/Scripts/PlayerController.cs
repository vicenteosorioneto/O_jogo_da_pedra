using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float maxStamina = 5f;
    [SerializeField] private float staminaDrainPerSecond = 1.2f;
    [SerializeField] private float staminaRecoveryPerSecond = 0.9f;
    [SerializeField] private float exhaustionRecoveryThreshold = 0.35f;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Light flashlight;
    [SerializeField] private float flashlightIntensity = 4f;

    private float currentSpeed;
    private float currentStamina;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 lastInteractionDirection = Vector2.up;
    private bool canMove = true;
    private bool isExhausted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;

        SetupFlashlight();
        
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

        rb.linearVelocity = moveDirection * currentSpeed;
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

        moveDirection = new Vector2(horizontal, vertical).normalized;
        if (moveDirection.sqrMagnitude > 0.001f)
            lastInteractionDirection = moveDirection;

        // Sprint
        bool wantsToSprint;
    #if ENABLE_INPUT_SYSTEM
        wantsToSprint = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
    #else
        wantsToSprint = Input.GetKey(KeyCode.LeftShift);
    #endif

        bool isMoving = moveDirection.sqrMagnitude > 0.01f;
        bool canSprint = wantsToSprint && isMoving && !isExhausted && currentStamina > 0f;

        if (canSprint)
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isExhausted = true;
            }
        }
        else
        {
            currentStamina = Mathf.MoveTowards(currentStamina, maxStamina, staminaRecoveryPerSecond * Time.deltaTime);
            if (isExhausted && maxStamina > 0f && currentStamina >= maxStamina * exhaustionRecoveryThreshold)
                isExhausted = false;
        }

        currentSpeed = canSprint ? sprintSpeed : moveSpeed;

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
            Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, interactionDistance);
            IInteractable nearestInteractable = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < nearby.Length; i++)
            {
                Collider2D candidateCollider = nearby[i];
                if (candidateCollider == null)
                    continue;

                IInteractable candidateInteractable = candidateCollider.GetComponent<IInteractable>();
                if (candidateInteractable == null)
                    continue;

                float candidateDistance = Vector2.Distance(transform.position, candidateCollider.transform.position);
                if (candidateDistance < nearestDistance)
                {
                    nearestDistance = candidateDistance;
                    nearestInteractable = candidateInteractable;
                }
            }

            if (nearestInteractable != null)
            {
                nearestInteractable.Interact();
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, lastInteractionDirection, interactionDistance);
                if (hit.collider != null)
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                        interactable.Interact();
                }
            }
        }

        bool flashlightTogglePressed;
#if ENABLE_INPUT_SYSTEM
        flashlightTogglePressed = Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame;
#else
        flashlightTogglePressed = Input.GetKeyDown(KeyCode.F);
#endif

        if (flashlightTogglePressed && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
            Debug.Log(flashlight.enabled ? "🔦 Lanterna ligada" : "🔦 Lanterna desligada");
        }
    }

    void SetupFlashlight()
    {
        if (flashlight == null)
        {
            GameObject flashlightObject = new GameObject("Flashlight");
            flashlightObject.transform.SetParent(transform);
            flashlightObject.transform.localPosition = Vector3.zero;
            flashlightObject.transform.localRotation = Quaternion.identity;

            flashlight = flashlightObject.AddComponent<Light>();
            flashlight.type = LightType.Spot;
            flashlight.spotAngle = 60f;
            flashlight.range = 18f;
            flashlight.intensity = flashlightIntensity;
            flashlight.color = new Color(1f, 0.95f, 0.85f);
            flashlight.shadows = LightShadows.Soft;
        }

        flashlight.enabled = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public float GetStaminaNormalized()
    {
        if (maxStamina <= 0f)
            return 1f;

        return Mathf.Clamp01(currentStamina / maxStamina);
    }

    public bool IsExhausted()
    {
        return isExhausted;
    }
}
