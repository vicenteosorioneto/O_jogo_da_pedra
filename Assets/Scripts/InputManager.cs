using UnityEngine;

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
        // Alt+Tab para liberar mouse (debug)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        // Debug: Teclas de teste
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("=== DEBUG INFO ===");
            Debug.Log($"Fragmentos: {GameManager.Instance.GetFragmentCount()}/3");
            Debug.Log($"Game Ended: {GameManager.Instance.IsGameEnded()}");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameManager.Instance.EndGameGoodEnding();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameManager.Instance.EndGameBadEnding();
        }
    }
}
