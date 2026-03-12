using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    [SerializeField] private Image staminaBarFill;
    [SerializeField] private GameObject jumpscarePanel;
    [SerializeField] private Image jumpscareImage;
    [SerializeField] private float jumpscareFadeInDuration = 0.1f;
    [SerializeField] private float jumpscareStayDuration = 0.75f;
    [SerializeField] private float jumpscareFadeOutDuration = 0.25f;

    private PlayerController playerController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (jumpscarePanel != null) jumpscarePanel.SetActive(false);
    }

    void Update()
    {
        if (playerController != null && staminaBarFill != null)
        {
            staminaBarFill.fillAmount = playerController.GetStaminaNormalized();
        }
    }

    public void TriggerJumpscare(float stayDuration, float fadeOutDuration)
    {
        if (jumpscarePanel != null && jumpscareImage != null)
        {
            StartCoroutine(PerformJumpscare(stayDuration, fadeOutDuration));
        }
    }

    private IEnumerator PerformJumpscare(float stayDuration, float fadeOutDuration)
    {
        // Fade In
        jumpscarePanel.SetActive(true);
        Color startColor = jumpscareImage.color;
        startColor.a = 0f;
        jumpscareImage.color = startColor;

        float timer = 0f;
        while (timer < jumpscareFadeInDuration)
        {
            timer += Time.deltaTime;
            startColor.a = Mathf.Lerp(0f, 1f, timer / jumpscareFadeInDuration);
            jumpscareImage.color = startColor;
            yield return null;
        }
        startColor.a = 1f;
        jumpscareImage.color = startColor;

        // Stay visible
        yield return new WaitForSeconds(stayDuration);

        // Fade Out
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            startColor.a = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
            jumpscareImage.color = startColor;
            yield return null;
        }
        startColor.a = 0f;
        jumpscareImage.color = startColor;
        jumpscarePanel.SetActive(false);
    }
}
