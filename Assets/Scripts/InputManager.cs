using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    void Start()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        // Lock mouse ao iniciar
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        bool escPressed = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        bool f1Pressed = Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame;
        bool f2Pressed = Keyboard.current != null && Keyboard.current.f2Key.wasPressedThisFrame;
        bool f3Pressed = Keyboard.current != null && Keyboard.current.f3Key.wasPressedThisFrame;
#else
        bool escPressed = Input.GetKeyDown(KeyCode.Escape);
        bool f1Pressed = Input.GetKeyDown(KeyCode.F1);
        bool f2Pressed = Input.GetKeyDown(KeyCode.F2);
        bool f3Pressed = Input.GetKeyDown(KeyCode.F3);
#endif

        // Alt+Tab para liberar mouse (debug)
        if (escPressed)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        // Debug: Teclas de teste
        if (f1Pressed)
        {
            Debug.Log("=== DEBUG INFO ===");
            Debug.Log($"Fragmentos: {GameManager.Instance.GetFragmentCount()}/3");
            Debug.Log($"Game Ended: {GameManager.Instance.IsGameEnded()}");
        }

        if (f2Pressed)
        {
            GameManager.Instance.EndGameGoodEnding();
        }

        if (f3Pressed)
        {
            GameManager.Instance.EndGameBadEnding();
        }
    }
}
