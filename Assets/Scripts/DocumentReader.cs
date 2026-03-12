using UnityEngine;

public class DocumentReader : MonoBehaviour, IInteractable
{
    [SerializeField] private string documentTitle = "Documento";
    [TextArea(5, 10)]
    [SerializeField] private string documentContent = "Conteúdo do documento aqui...";
    [SerializeField] private int documentID = 1;

    void Start()
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
            triggerCollider.isTrigger = true;
    }

    public void Interact()
    {
        DisplayDocument();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReadDocument(documentID);
        }
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
