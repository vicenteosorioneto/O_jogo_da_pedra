using System.Collections;
using UnityEngine;

public class AtmosphereController : MonoBehaviour
{
    [SerializeField] private float ambientVolume = 0.22f;
    [SerializeField] private float eventVolume = 0.35f;
    [SerializeField] private float heartbeatMaxVolume = 0.55f;

    private AudioSource ambientSource;
    private AudioSource eventSource;
    private AudioSource heartbeatSource;

    private Transform player;
    private GhostAI ghost;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        ghost = Object.FindFirstObjectByType<GhostAI>();

        SetupAudio();
        StartCoroutine(AmbientEventsRoutine());
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (ghost == null)
            ghost = Object.FindFirstObjectByType<GhostAI>();

        UpdateHeartbeatByDistance();
    }

    void SetupAudio()
    {
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.loop = true;
        ambientSource.spatialBlend = 0f;
        ambientSource.playOnAwake = false;
        ambientSource.volume = ambientVolume;
        ambientSource.clip = CreateAmbientHumClip();
        ambientSource.Play();

        eventSource = gameObject.AddComponent<AudioSource>();
        eventSource.loop = false;
        eventSource.spatialBlend = 0f;
        eventSource.playOnAwake = false;
        eventSource.volume = eventVolume;

        heartbeatSource = gameObject.AddComponent<AudioSource>();
        heartbeatSource.loop = true;
        heartbeatSource.spatialBlend = 0f;
        heartbeatSource.playOnAwake = false;
        heartbeatSource.volume = 0f;
        heartbeatSource.clip = CreateHeartbeatClip();
        heartbeatSource.Play();
    }

    IEnumerator AmbientEventsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 15f));

            if (eventSource == null)
                continue;

            eventSource.pitch = Random.Range(0.88f, 1.08f);
            eventSource.PlayOneShot(CreateCreakClip(), Random.Range(0.65f, 1f));
        }
    }

    void UpdateHeartbeatByDistance()
    {
        if (heartbeatSource == null || player == null || ghost == null)
            return;

        float distance = Vector2.Distance(player.position, ghost.transform.position);
        float tension = Mathf.InverseLerp(10f, 1.25f, distance);

        float targetVolume = tension * heartbeatMaxVolume;
        heartbeatSource.volume = Mathf.MoveTowards(heartbeatSource.volume, targetVolume, Time.deltaTime * 0.85f);
        heartbeatSource.pitch = Mathf.Lerp(0.9f, 1.25f, tension);
    }

    AudioClip CreateAmbientHumClip()
    {
        int sampleRate = 44100;
        float duration = 4f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float toneA = Mathf.Sin(2f * Mathf.PI * 43f * t) * 0.08f;
            float toneB = Mathf.Sin(2f * Mathf.PI * 58f * t) * 0.05f;
            float noise = (Mathf.PerlinNoise(t * 1.5f, 0.31f) - 0.5f) * 0.03f;
            data[i] = toneA + toneB + noise;
        }

        AudioClip clip = AudioClip.Create("AmbientHum", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    AudioClip CreateCreakClip()
    {
        int sampleRate = 44100;
        float duration = 1.1f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Clamp01(1f - (t / duration));
            float baseFreq = Mathf.Lerp(460f, 170f, t / duration);
            float squeal = Mathf.Sin(2f * Mathf.PI * baseFreq * t) * envelope * 0.18f;
            float grit = (Mathf.PerlinNoise(t * 35f, 0.7f) - 0.5f) * envelope * 0.12f;
            data[i] = squeal + grit;
        }

        AudioClip clip = AudioClip.Create("AmbientCreak", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    AudioClip CreateHeartbeatClip()
    {
        int sampleRate = 44100;
        float duration = 0.42f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float thumpA = Mathf.Exp(-40f * Mathf.Pow(t - 0.06f, 2f)) * Mathf.Sin(2f * Mathf.PI * 72f * t);
            float thumpB = Mathf.Exp(-70f * Mathf.Pow(t - 0.16f, 2f)) * Mathf.Sin(2f * Mathf.PI * 88f * t);
            data[i] = (thumpA + thumpB) * 0.65f;
        }

        AudioClip clip = AudioClip.Create("Heartbeat", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
}
