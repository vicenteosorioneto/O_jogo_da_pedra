using UnityEngine;

public class FragmentCollector : MonoBehaviour, IInteractable
{
    [SerializeField] private int fragmentID = 1;
    [SerializeField] private string fragmentName = "Fragmento do Espelho";
    private bool collected = false;

    void Start()
    {
        // Adicionar trigger ao collider
        GetComponent<Collider>().isTrigger = true;
    }

    public void Interact()
    {
        if (!collected)
        {
            Collect();
        }
    }

    public void Collect()
    {
        collected = true;
        GameManager.Instance.CollectFragment(fragmentID);
        Debug.Log($"✓ {fragmentName} coletado! ({GameManager.Instance.GetFragmentCount()}/3)");
        Destroy(gameObject);
    }

    public int GetFragmentID()
    {
        return fragmentID;
    }

    public void SetFragmentID(int id)
    {
        fragmentID = id;
    }
}
