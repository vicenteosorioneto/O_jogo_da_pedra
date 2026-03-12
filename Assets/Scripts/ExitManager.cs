using UnityEngine;

public class ExitManager : MonoBehaviour, IInteractable
{
    [SerializeField] private bool allowExit = true;

    void Start()
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
            triggerCollider.isTrigger = true;
    }

    public void Interact()
    {
        if (!allowExit)
        {
            Debug.Log("✗ A porta está trancada! Você precisa completar o ritual ou encontrar uma forma de sair.");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.IsRitualCompleted())
        {
            Debug.Log("✅ Você escapou da escola após aprisionar Maria Sangrenta!");
            // O final bom já foi acionado pelo RitualManager, então aqui apenas confirmamos a saída.
            // Poderíamos adicionar uma transição de cena aqui.
        }
        else
        {
            Debug.Log("⚠️ Você consegue sair, mas Maria Sangrenta não foi aprisionada...");
            GameManager.Instance.EndGameBadEnding();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && allowExit)
        {
            Debug.Log("Pressione E para sair da escola.");
        }
    }
}
