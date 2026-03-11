using UnityEngine;
using System.Collections;

public class RitualManager : MonoBehaviour, IInteractable
{
    [SerializeField] private float ritualDuration = 5f;
    [SerializeField] private Light ritualLight;
    private bool ritualActive = false;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        if (ritualLight != null)
            ritualLight.enabled = false;
    }

    public void Interact()
    {
        if (!GameManager.Instance.HasAllFragments())
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
        GameManager.Instance.StartRitual();
        
        if (ritualLight != null)
            ritualLight.enabled = true;

        StartCoroutine(PerformRitual());
    }

    IEnumerator PerformRitual()
    {
        Debug.Log("✨ O espelho brilha intensamente...");
        yield return new WaitForSeconds(1f);
        
        Debug.Log("📍 Maria Sangrenta é puxada para dentro do espelho!");
        yield return new WaitForSeconds(2f);
        
        Debug.Log("💥 O espelho se quebra novamente!");
        yield return new WaitForSeconds(1f);

        GameManager.Instance.CompleteRitual();
    }
}
