using UnityEngine;

/// <summary>
/// DocumentReader permite que o jogador leia documentos espalhados pela escola.
/// Implementar nos próximos passos.
/// </summary>
public class DocumentReader : MonoBehaviour, IInteractable
{
    [SerializeField] private string documentTitle = "Documento";
    [SerializeField] private string documentContent = "Conteúdo do documento aqui...";
    [SerializeField] private int documentID = 1;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void Interact()
    {
        DisplayDocument();
    }

    void DisplayDocument()
    {
        Debug.Log($"\n═══════════════════════════════════");
        Debug.Log($"  {documentTitle}");
        Debug.Log($"═══════════════════════════════════");
        Debug.Log(documentContent);
        Debug.Log($"═══════════════════════════════════\n");
    }
}
