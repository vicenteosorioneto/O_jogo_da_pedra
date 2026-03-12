using UnityEngine;
using System.Collections;

public class RitualManager : MonoBehaviour, IInteractable
{
    [SerializeField] private float ritualDuration = 5f;
    [SerializeField] private Light ritualLight;
    [SerializeField] private Transform mirrorTransform; // Posição do espelho para o ritual
    [SerializeField] private float flashlightRitualRange = 3f; // Distância para a lanterna ser considerada "apontada"
    [SerializeField] private float flashlightAngleThreshold = 30f; // Ângulo máximo para a lanterna ser considerada "apontada"

    private bool ritualActive = false;
    private PlayerController playerController;
    private GhostAI mariaSangrenta;
    private GameManager gameManager;

    void Start()
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
            triggerCollider.isTrigger = true;
        if (ritualLight != null)
            ritualLight.enabled = false;

        playerController = FindObjectOfType<PlayerController>();
        mariaSangrenta = FindObjectOfType<GhostAI>();
        gameManager = GameManager.Instance;

        if (mirrorTransform == null)
        {
            Debug.LogError("Mirror Transform não atribuído no RitualManager!");
        }
    }

    public void Interact()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager não encontrado!");
            return;
        }

        if (!gameManager.HasAllFragments())
        {
            Debug.Log("✗ Você precisa coletar os 3 fragmentos primeiro.");
            return;
        }

        if (!ritualActive)
        {
            StartRitual();
        }
    }

    void StartRitual()
    {
        ritualActive = true;
        gameManager.StartRitual();
        
        if (ritualLight != null)
            ritualLight.enabled = true;

        // Desativar o movimento do jogador durante o ritual
        if (playerController != null) playerController.DisableMovement();
        // Ativar comportamento de ataque da Maria Sangrenta durante o ritual
        if (mariaSangrenta != null) mariaSangrenta.StartRitualAttack(mirrorTransform.position);

        StartCoroutine(PerformRitual());
    }

    IEnumerator PerformRitual()
    {
        float ritualTimer = ritualDuration;
        Debug.Log("✨ O espelho brilha intensamente... Maria Sangrenta se aproxima!");
        Debug.Log("Mantenha a lanterna apontada para o espelho!");

        while (ritualTimer > 0f)
        {
            if (IsFlashlightPointingAtMirror())
            {
                ritualTimer -= Time.deltaTime;
                // Repelir Maria Sangrenta com a luz
                if (mariaSangrenta != null) mariaSangrenta.RepelGhost();
            }
            else
            {
                // Se a lanterna não estiver apontada, Maria Sangrenta pode atacar mais agressivamente
                // Ou o ritual pode ser interrompido/atrasado
                Debug.Log("⚠️ A lanterna não está apontada para o espelho! Maria Sangrenta avança!");
                // Opcional: Aumentar o timer ou fazer Maria Sangrenta causar um jumpscare/dano
            }
            yield return null;
        }

        Debug.Log("📍 Maria Sangrenta é puxada para dentro do espelho!");
        yield return new WaitForSeconds(2f);
        
        Debug.Log("💥 O espelho se quebra novamente!");
        yield return new WaitForSeconds(1f);

        if (playerController != null) playerController.EnableMovement();
        if (mariaSangrenta != null) mariaSangrenta.StopRitualAttack();
        gameManager.CompleteRitual();
    }

    bool IsFlashlightPointingAtMirror()
    {
        if (playerController == null || !playerController.IsFlashlightOn() || mirrorTransform == null)
        {
            return false;
        }

        Light flashlight = playerController.GetFlashlight();
        if (flashlight == null) return false;

        Vector2 playerToMirror = (mirrorTransform.position - playerController.transform.position).normalized;
        Vector2 flashlightDirection = playerController.GetFlashlightDirection();

        float distance = Vector2.Distance(playerController.transform.position, mirrorTransform.position);
        float angle = Vector2.Angle(flashlightDirection, playerToMirror);

        // Verifica se o jogador está perto o suficiente e a lanterna está apontada na direção geral do espelho
        return distance <= flashlightRitualRange && angle < flashlightAngleThreshold;
    }
}
