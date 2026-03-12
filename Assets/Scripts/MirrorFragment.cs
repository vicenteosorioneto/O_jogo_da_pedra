using UnityEngine;

public class MirrorFragment : MonoBehaviour, IInteractable
{
    [SerializeField] private int fragmentID;

    public void Interact()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectFragment(fragmentID);
            Debug.Log($"Fragmento {fragmentID} coletado!");
            Destroy(gameObject); // Destrói o fragmento após a coleta
        }
    }
}
