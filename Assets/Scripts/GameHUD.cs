using UnityEngine;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    private GUIStyle labelStyle;
    private PlayerController playerController;
    private float jumpscareAlpha;
    private float jumpscareTimer;

    const float JumpscareFadeSpeed = 3.5f;

    void OnEnable()
    {
        Instance = this;
    }

    void OnDisable()
    {
        if (Instance == this)
            Instance = null;
    }

    void Awake()
    {
        labelStyle = new GUIStyle
        {
            fontSize = 18,
            normal = { textColor = Color.white }
        };

        playerController = Object.FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (playerController == null)
            playerController = Object.FindFirstObjectByType<PlayerController>();

        if (jumpscareTimer > 0f)
        {
            jumpscareTimer -= Time.deltaTime;
        }
        else if (jumpscareAlpha > 0f)
        {
            jumpscareAlpha = Mathf.MoveTowards(jumpscareAlpha, 0f, JumpscareFadeSpeed * Time.deltaTime);
        }
    }

    void OnGUI()
    {
        int fragments = GameManager.Instance != null ? GameManager.Instance.GetFragmentCount() : 0;

        GUI.Label(new Rect(20, 20, 500, 30), $"Fragmentos: {fragments}/3", labelStyle);
        GUI.Label(new Rect(20, 50, 700, 30), "Objetivo: Colete os 3 fragmentos e faça o ritual no espelho", labelStyle);
        GUI.Label(new Rect(20, 80, 700, 30), "Controles: WASD mover | Shift correr | E interagir | F lanterna | Esc mouse", labelStyle);

        DrawStaminaBar();

        float centerX = Screen.width * 0.5f;
        float centerY = Screen.height * 0.5f;
        GUI.Label(new Rect(centerX - 5, centerY - 10, 20, 20), "+", labelStyle);

        if (jumpscareAlpha > 0.001f)
        {
            Color previousColor = GUI.color;
            GUI.color = new Color(0.8f, 0.05f, 0.05f, jumpscareAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = previousColor;
        }
    }

    void DrawStaminaBar()
    {
        if (playerController == null)
            return;

        Rect backgroundRect = new Rect(20, Screen.height - 50, 220, 20);
        float stamina = playerController.GetStaminaNormalized();
        Rect fillRect = new Rect(backgroundRect.x + 2, backgroundRect.y + 2, (backgroundRect.width - 4) * stamina, backgroundRect.height - 4);

        Color previousColor = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.7f);
        GUI.DrawTexture(backgroundRect, Texture2D.whiteTexture);

        GUI.color = playerController.IsExhausted() ? new Color(0.95f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.85f, 0.35f, 1f);
        GUI.DrawTexture(fillRect, Texture2D.whiteTexture);

        GUI.color = Color.white;
        GUI.Label(new Rect(backgroundRect.x, backgroundRect.y - 24, 220, 20), "Stamina", labelStyle);
        GUI.color = previousColor;
    }

    public void TriggerJumpscare(float alpha = 0.75f, float duration = 0.2f)
    {
        jumpscareAlpha = Mathf.Max(jumpscareAlpha, alpha);
        jumpscareTimer = Mathf.Max(jumpscareTimer, duration);
    }
}
