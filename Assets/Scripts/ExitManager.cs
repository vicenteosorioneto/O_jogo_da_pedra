using UnityEngine;

public class ExitManager : MonoBehaviour, IInteractable
{
    [SerializeField] private bool allowExit = true;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void Interact()
    {
        if (!allowExit)
        {
            Debug.Log("✗ A porta está trancada! Você precisa completar o ritual.");
            return;
        }

        if (GameManager.Instance.HasAllFragments())
        {
            // Não deveria sair sem fazer o ritual
            Debug.Log("⚠️  Você consegue sair, mas Maria Sangrenta não foi aprisionada...");
            GameManager.Instance.EndGameBadEnding();
        }
        else
        {
            Debug.Log("⚠️  Você consegue sair, mas Maria Sangrenta não foi aprisionada...");
            GameManager.Instance.EndGameBadEnding();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && allowExit)
        {
            Debug.Log("Press E para sair da escola.");
        }
    }
}
