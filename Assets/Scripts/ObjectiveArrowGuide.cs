using UnityEngine;

public class ObjectiveArrowGuide : MonoBehaviour
{
    enum ObjectiveState
    {
        Documents,
        Fragment,
        Ritual,
        None
    }

    [SerializeField] private int arrowCount = 4;
    [SerializeField] private float firstArrowDistance = 1.2f;
    [SerializeField] private float arrowSpacing = 1.2f;
    [SerializeField] private float arrowSize = 0.32f;

    private Transform player;
    private Transform mirror;
    private Transform[] arrowTransforms;
    private SpriteRenderer[] arrowRenderers;
    private AudioSource guideAudio;
    private AudioClip fragmentPingClip;
    private AudioClip ritualPingClip;
    private AudioClip documentPingClip;

    private ObjectiveState currentState = ObjectiveState.None;
    private string currentObjectiveText = "Explore a escola";
    private float currentObjectiveDistance = 0f;
    private ObjectiveState lastAnnouncedState = ObjectiveState.None;

    private readonly Color documentColor = new Color(0.8f, 0.8f, 0.8f, 0.9f); // Cor para documentos
    private readonly Color fragmentColor = new Color(1f, 0.78f, 0.22f, 0.9f);
    private readonly Color ritualColor = new Color(0.3f, 1f, 0.55f, 0.92f);

    public string GetCurrentObjectiveText()
    {
        return currentObjectiveText;
    }

    public float GetCurrentObjectiveDistance()
    {
        return currentObjectiveDistance;
    }

    public bool HasActiveTarget()
    {
        return currentState != ObjectiveState.None;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        mirror = GameObject.Find("EspelhoRitual")?.transform;

        BuildArrows();
        SetupAudio();
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (mirror == null)
            mirror = GameObject.Find("EspelhoRitual")?.transform;

        if (GameManager.Instance == null || player == null || GameManager.Instance.IsGameEnded())
        {
            currentState = ObjectiveState.None;
            currentObjectiveText = "Jogo encerrado";
            currentObjectiveDistance = 0f;
            SetArrowsActive(false);
            return;
        }

        Transform target = ResolveCurrentTarget(out ObjectiveState resolvedState);
        currentState = resolvedState;

        if (target == null)
        {
            currentObjectiveText = "Explore a escola";
            currentObjectiveDistance = 0f;
            SetArrowsActive(false);
            return;
        }

        if (currentState != lastAnnouncedState)
        {
            PlayObjectivePing(currentState);
            lastAnnouncedState = currentState;
        }

        Vector2 direction = (Vector2)target.position - (Vector2)player.position;
        float distance = direction.magnitude;
        currentObjectiveDistance = distance;

        if (currentState == ObjectiveState.Documents)
            currentObjectiveText = "Encontre os documentos para entender a lenda";
        else if (currentState == ObjectiveState.Fragment)
            currentObjectiveText = "Colete o fragmento mais próximo";
        else if (currentState == ObjectiveState.Ritual)
            currentObjectiveText = "Vá ao Espelho do Ritual";
        else
            currentObjectiveText = "Explore a escola";

        if (distance < 0.8f)
        {
            SetArrowsActive(false);
            return;
        }

        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Color arrowColor = documentColor;
        if (currentState == ObjectiveState.Fragment) arrowColor = fragmentColor;
        else if (currentState == ObjectiveState.Ritual) arrowColor = ritualColor;

        SetArrowColor(arrowColor);
        SetArrowsActive(true);

        for (int i = 0; i < arrowTransforms.Length; i++)
        {
            float step = firstArrowDistance + (i * arrowSpacing);
            Vector2 arrowPos = (Vector2)player.position + direction * step;
            arrowTransforms[i].position = new Vector3(arrowPos.x, arrowPos.y, 0.15f);
            arrowTransforms[i].rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    Transform ResolveCurrentTarget(out ObjectiveState state)
    {
        // Primeiro, verificar se todos os documentos foram lidos
        if (GameManager.Instance != null && !GameManager.Instance.HasReadAllDocuments())
        {
            DocumentReader[] documents = FindObjectsOfType<DocumentReader>();
            Transform nearestDocument = null;
            float bestDistance = float.MaxValue;

            foreach (DocumentReader doc in documents)
            {
                if (doc != null && !GameManager.Instance.HasReadDocument(doc.documentID))
                {
                    float distance = Vector2.Distance(player.position, doc.transform.position);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        nearestDocument = doc.transform;
                    }
                }
            }
            state = nearestDocument != null ? ObjectiveState.Documents : ObjectiveState.None;
            return nearestDocument;
        }

        // Se os documentos foram lidos, verificar os fragmentos
        if (GameManager.Instance != null && !GameManager.Instance.HasAllFragments())
        {
            MirrorFragment[] fragments = FindObjectsOfType<MirrorFragment>(); // Usar MirrorFragment
            Transform nearest = null;
            float bestDistance = float.MaxValue;

            foreach (MirrorFragment fragment in fragments)
            {
                if (fragment != null)
                {
                    float distance = Vector2.Distance(player.position, fragment.transform.position);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        nearest = fragment.transform;
                    }
                }
            }
            state = nearest != null ? ObjectiveState.Fragment : ObjectiveState.None;
            return nearest;
        }

        // Se todos os fragmentos foram coletados, guiar para o ritual
        if (GameManager.Instance != null && GameManager.Instance.HasAllFragments())
        {
            state = ObjectiveState.Ritual;
            return mirror;
        }

        state = ObjectiveState.None;
        return null;
    }

    void BuildArrows()
    {
        arrowTransforms = new Transform[Mathf.Max(1, arrowCount)];
        arrowRenderers = new SpriteRenderer[arrowTransforms.Length];

        Sprite arrowSprite = GetArrowSprite();
        Material arrowMaterial = new Material(Shader.Find("Sprites/Default"));

        for (int i = 0; i < arrowTransforms.Length; i++)
        {
            GameObject arrow = new GameObject($"SetaObjetivo_{i + 1}");
            arrow.transform.SetParent(transform);

            SpriteRenderer renderer = arrow.AddComponent<SpriteRenderer>();
            renderer.sprite = arrowSprite;
            renderer.material = arrowMaterial;
            renderer.sortingOrder = 7;

            arrow.transform.localScale = new Vector3(arrowSize, arrowSize, 1f);
            arrowTransforms[i] = arrow.transform;
            arrowRenderers[i] = renderer;
        }

        SetArrowColor(documentColor); // Cor inicial para documentos
    }

    void SetupAudio()
    {
        guideAudio = gameObject.AddComponent<AudioSource>();
        guideAudio.playOnAwake = false;
        guideAudio.loop = false;
        guideAudio.spatialBlend = 0f;
        guideAudio.volume = 0.65f;

        fragmentPingClip = CreatePingClip(760f, 0.12f);
        ritualPingClip = CreatePingClip(520f, 0.2f);
        documentPingClip = CreatePingClip(900f, 0.1f); // Novo som para documentos
    }

    void PlayObjectivePing(ObjectiveState state)
    {
        if (guideAudio == null)
            return;

        AudioClip clip = null;
        float pitch = 1f;

        switch (state)
        {
            case ObjectiveState.Documents:
                clip = documentPingClip;
                pitch = 1.2f;
                break;
            case ObjectiveState.Fragment:
                clip = fragmentPingClip;
                pitch = 1.08f;
                break;
            case ObjectiveState.Ritual:
                clip = ritualPingClip;
                pitch = 0.9f;
                break;
        }

        if (clip == null)
            return;

        guideAudio.pitch = pitch;
        guideAudio.PlayOneShot(clip, 1f);
    }

    AudioClip CreatePingClip(float baseFrequency, float duration)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Exp(-20f * t);
            float toneA = Mathf.Sin(2f * Mathf.PI * baseFrequency * t) * 0.42f;
            float toneB = Mathf.Sin(2f * Mathf.PI * (baseFrequency * 1.5f) * t) * 0.2f;
            data[i] = (toneA + toneB) * envelope;
        }

        AudioClip clip = AudioClip.Create("ObjectivePing", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    Sprite GetArrowSprite()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;

        Color clear = new Color(0f, 0f, 0f, 0f);
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                texture.SetPixel(x, y, clear);
            }
        }

        Color arrowColor = Color.white;
        for (int y = 6; y <= 25; y++)
        {
            int halfWidth = Mathf.RoundToInt((y - 6) * 0.6f);
            int minX = Mathf.Clamp(16 - halfWidth, 0, 31);
            int maxX = Mathf.Clamp(16 + halfWidth, 0, 31);
            for (int x = minX; x <= maxX; x++)
                texture.SetPixel(x, y, arrowColor);
        }

        for (int y = 0; y < 8; y++)
        {
            for (int x = 12; x <= 20; x++)
                texture.SetPixel(x, y, arrowColor);
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, 32f, 32f), new Vector2(0.5f, 0.5f), 32f);
    }

    void SetArrowColor(Color color)
    {
        if (arrowRenderers == null)
            return;

        for (int i = 0; i < arrowRenderers.Length; i++)
        {
            if (arrowRenderers[i] != null)
                arrowRenderers[i].color = color;
        }
    }

    void SetArrowsActive(bool active)
    {
        if (arrowTransforms == null)
            return;

        for (int i = 0; i < arrowTransforms.Length; i++)
        {
            if (arrowTransforms[i] != null)
                arrowTransforms[i].gameObject.SetActive(active);
        }
    }
}
