using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int fragmentsNeeded = 3;
    private int fragmentsCollected = 0;
    private List<int> collectedFragmentIDs = new List<int>();
    private bool ritualInProgress = false;
    private bool gameEnded = false;
    private List<int> readDocumentIDs = new List<int>();
    [SerializeField] private int documentsNeeded = 3;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("=== MARIA SANGRENTA ACORDOU ===");
        Debug.Log("Procure pelos 3 fragmentos do espelho para aprisionar a entidade.");
        Debug.Log("Use E para interagir com objetos.");
    }

    public void CollectFragment(int fragmentID)
    {
        if (!collectedFragmentIDs.Contains(fragmentID))
        {
            collectedFragmentIDs.Add(fragmentID);
            fragmentsCollected++;

            if (fragmentsCollected >= fragmentsNeeded)
            {
                OnAllFragmentsCollected();
            }
        }
    }

    void OnAllFragmentsCollected()
    {
        Debug.Log("╔════════════════════════════════════╗");
        Debug.Log("║  Todos os fragmentos coletados!   ║");
        Debug.Log("║  Vá ao banheiro feminino.         ║");
        Debug.Log("╚════════════════════════════════════╝");
    }

    public int GetFragmentCount()
    {
        return fragmentsCollected;
    }

    public bool HasAllFragments()
    {
        return fragmentsCollected >= fragmentsNeeded;
    }

    public void StartRitual()
    {
        if (!HasAllFragments())
        {
            Debug.Log("✗ Você precisa de todos os 3 fragmentos!");
            return;
        }

        ritualInProgress = true;
        Debug.Log("🔴 O ritual começou! Maria Sangrenta aparece...");
        Debug.Log("Segure a lanterna apontada para o espelho até o final!");
    }

    public void CompleteRitual()
    {
        if (!ritualInProgress) return;

        ritualInProgress = false;
        EndGameGoodEnding();
        // Considerar adicionar uma flag específica para ritual concluído se 'gameEnded' não for suficiente
        // Por exemplo: ritualSuccessfullyCompleted = true;
    }

    public void EndGameGoodEnding()
    {
        gameEnded = true;
        Debug.Log("\n╔════════════════════════════════════╗");
        Debug.Log("║      FINAL BOM - Ritual Completo   ║");
        Debug.Log("╠════════════════════════════════════╣");
        Debug.Log("║  Maria Sangrenta foi aprisionada!  ║");
        Debug.Log("║  Você conseguiu escapar...         ║");
        Debug.Log("║                                    ║");
        Debug.Log("║  Mas ao passar por uma loja,      ║");
        Debug.Log("║  vê um pequeno pedaço de espelho. ║");
        Debug.Log("║  E no reflexo... ela ainda        ║");
        Debug.Log("║  está lá, observando...           ║");
        Debug.Log("╚════════════════════════════════════╝\n");

        Time.timeScale = 0f; // Pausa o jogo
    }

    public void EndGameBadEnding()
    {
        gameEnded = true;
        Debug.Log("\n╔════════════════════════════════════╗");
        Debug.Log("║    FINAL RUIM - Sem os Fragmentos  ║");
        Debug.Log("╠════════════════════════════════════╣");
        Debug.Log("║  Você conseguiu escapar da escola. ║");
        Debug.Log("║  Respira aliviado...               ║");
        Debug.Log("║                                    ║");
        Debug.Log("║  Mas no reflexo de uma poça,      ║");
        Debug.Log("║  um carro, ou do seu celular...   ║");
        Debug.Log("║                                    ║");
        Debug.Log("║  Maria Sangrenta está atrás de    ║");
        Debug.Log("║  você. Ela não foi aprisionada.   ║");
        Debug.Log("║  E agora... ela o seguirá para    ║");
        Debug.Log("║  sempre.                          ║");
        Debug.Log("║                                    ║");
        Debug.Log("║  'Algumas lendas não terminam.'   ║");
        Debug.Log("╚════════════════════════════════════╝\n");

        Time.timeScale = 0f; // Pausa o jogo
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public bool IsRitualInProgress()
    {
        return ritualInProgress;
    }

    public bool IsRitualCompleted()
    {
        return gameEnded && !ritualInProgress; // Ritual completed implies game ended and not in progress
    }

    public void ReadDocument(int documentID)
    {
        if (!readDocumentIDs.Contains(documentID))
        {
            readDocumentIDs.Add(documentID);
            Debug.Log($"Documento {documentID} lido. Total: {readDocumentIDs.Count}/{documentsNeeded}");
            if (readDocumentIDs.Count >= documentsNeeded)
            {
                Debug.Log("Todos os documentos informativos foram lidos! Você sabe o que fazer agora.");
            }
        }
    }

    public bool HasReadDocument(int documentID)
    {
        return readDocumentIDs.Contains(documentID);
    }

    public bool HasReadAllDocuments()
    {
        return readDocumentIDs.Count >= documentsNeeded;
    }
}
