using UnityEngine;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float jumpscareDistance = 2f;
    [SerializeField] private float appearanceInterval = 15f;

    private Rigidbody rb;
    private bool isChasing = false;
    private float timeSinceLastAppearance = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        StartCoroutine(RandomAppearances());
    }

    void FixedUpdate()
    {
        if (player == null || GameManager.Instance.IsGameEnded())
            return;

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
            rb.velocity = Vector3.zero;
        }
    }

    void ChasePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        rb.velocity = directionToPlayer * chaseSpeed;
    }

    void Jumpscare()
    {
        Debug.Log("👻 MARIA SANGRENTA!!!");
        Debug.Log("Pressione movimento para correr!");
        
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
        while (!GameManager.Instance.IsGameEnded())
        {
            yield return new WaitForSeconds(appearanceInterval);

            if (!isChasing)
            {
                Vector3 randomPosition = player.position + Random.insideUnitSphere * 10f;
                randomPosition.y = player.position.y;
                transform.position = randomPosition;
                Debug.Log("👻 Você sente uma presença ao seu redor...");
            }
        }
    }

    public bool IsChasing()
    {
        return isChasing;
    }
}
