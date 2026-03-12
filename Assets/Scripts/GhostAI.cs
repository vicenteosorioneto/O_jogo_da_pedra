using UnityEngine;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float jumpscareDistance = 2f;
    [SerializeField] private float appearanceInterval = 15f;
    [SerializeField] private float jumpscareCooldown = 1.5f;

    private Rigidbody2D rb;
    private bool isChasing = false;
    private float timeSinceLastAppearance = 0f;
    private float lastJumpscareTime = -999f;
    private bool isRitualAttacking = false;
    private Vector3 ritualMirrorPosition;
    [SerializeField] private float ritualAttackSpeed = 4f;
    [SerializeField] private float repelForce = 2f;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        gameManager = GameManager.Instance;
        StartCoroutine(RandomAppearances());
    }

    void FixedUpdate()
    {
        if (player == null || gameManager == null || gameManager.IsGameEnded())
            return;

        if (isRitualAttacking)
        {
            RitualAttackBehavior();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            isChasing = true;
            ChasePlayer();

            if (distanceToPlayer < jumpscareDistance)
            {
                Jumpscare();
            }
        }
        else
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void ChasePlayer()
    {
        Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = directionToPlayer * chaseSpeed;
    }

    void RitualAttackBehavior()
    {
        Vector2 directionToMirror = ((Vector2)ritualMirrorPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = directionToMirror * ritualAttackSpeed;
    }

    void Jumpscare()
    {
        if (Time.time < lastJumpscareTime + jumpscareCooldown)
            return;

        lastJumpscareTime = Time.time;

        Debug.Log("👻 MARIA SANGRENTA!!!");
        Debug.Log("Pressione movimento para correr!");

        if (GameHUD.Instance != null)
        {
            GameHUD.Instance.TriggerJumpscare(jumpscareCooldown, 0.25f);
        }
        
        // Efeito visual (piscar)
        StartCoroutine(FlashEffect());
    }

    IEnumerator FlashEffect()
    {
        // Simular piscar de luzes
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator RandomAppearances()
    {
        while (gameManager != null && !gameManager.IsGameEnded() && !gameManager.IsRitualInProgress())
        {
            yield return new WaitForSeconds(appearanceInterval);

            if (!isChasing && player != null)
            {
                Vector2 randomOffset = Random.insideUnitCircle * 10f;
                Vector3 randomPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
                transform.position = randomPosition;
                Debug.Log("👻 Você sente uma presença ao seu redor...");
            }
        }
    }

    public bool IsChasing()
    {
        return isChasing;
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void StartRitualAttack(Vector3 mirrorPosition)
    {
        isRitualAttacking = true;
        ritualMirrorPosition = mirrorPosition;
        rb.linearVelocity = Vector2.zero; // Parar qualquer movimento anterior
        Debug.Log("Maria Sangrenta está focada no espelho!");
    }

    public void RepelGhost()
    {
        if (isRitualAttacking)
        {
            Vector2 directionFromMirror = ((Vector2)transform.position - (Vector2)ritualMirrorPosition).normalized;
            rb.AddForce(directionFromMirror * repelForce, ForceMode2D.Impulse);
            Debug.Log("Maria Sangrenta repelida pela luz!");
        }
    }

    public void StopRitualAttack()
    {
        isRitualAttacking = false;
        Debug.Log("Maria Sangrenta parou o ataque ritualístico.");
    }
}
