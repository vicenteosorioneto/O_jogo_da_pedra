using UnityEngine;

public class PlayerAudioFeedback : MonoBehaviour
{
    [SerializeField] private float walkStepInterval = 0.52f;
    [SerializeField] private float sprintStepInterval = 0.34f;

    private PlayerController playerController;
    private Rigidbody2D rb;

    private AudioSource footstepSource;
    private AudioSource breathingSource;

    private AudioClip[] footstepClips;
    private AudioClip breathingClip;

    private float stepTimer;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.spatialBlend = 0f;
        footstepSource.playOnAwake = false;
        footstepSource.volume = 0.3f;

        breathingSource = gameObject.AddComponent<AudioSource>();
        breathingSource.spatialBlend = 0f;
        breathingSource.loop = true;
        breathingSource.playOnAwake = false;
        breathingSource.volume = 0f;

        footstepClips = new AudioClip[]
        {
            CreateFootstepClip(135f),
            CreateFootstepClip(160f),
            CreateFootstepClip(190f)
        };

        breathingClip = CreateBreathingLoopClip();
        breathingSource.clip = breathingClip;
        breathingSource.Play();

        stepTimer = walkStepInterval;
    }

    void Update()
    {
        if (playerController == null || rb == null)
            return;

        float horizontalSpeed = rb.linearVelocity.magnitude;
        bool moving = horizontalSpeed > 0.25f;

        float stamina = playerController.GetStaminaNormalized();
        bool exhausted = playerController.IsExhausted();

        float interval = exhausted ? sprintStepInterval : walkStepInterval;
        stepTimer -= Time.deltaTime;

        if (moving && stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = interval;
        }

        if (!moving)
        {
            stepTimer = Mathf.Min(stepTimer, walkStepInterval * 0.6f);
        }

        float targetBreathVolume = Mathf.Lerp(0.03f, 0.42f, 1f - stamina);
        if (exhausted)
            targetBreathVolume = Mathf.Max(targetBreathVolume, 0.52f);

        breathingSource.volume = Mathf.MoveTowards(breathingSource.volume, targetBreathVolume, Time.deltaTime * 0.7f);
        breathingSource.pitch = Mathf.Lerp(0.9f, 1.22f, 1f - stamina);
    }

    void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0)
            return;

        AudioClip selectedClip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepSource.pitch = Random.Range(0.92f, 1.08f);
        footstepSource.PlayOneShot(selectedClip, Random.Range(0.75f, 1f));
    }

    AudioClip CreateFootstepClip(float baseFrequency)
    {
        int sampleRate = 44100;
        float duration = 0.11f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Exp(-22f * t);
            float tone = Mathf.Sin(2f * Mathf.PI * baseFrequency * t) * 0.25f;
            float noise = (Mathf.PerlinNoise(t * 120f, baseFrequency * 0.01f) - 0.5f) * 0.35f;
            data[i] = (tone + noise) * envelope;
        }

        AudioClip clip = AudioClip.Create("Footstep", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    AudioClip CreateBreathingLoopClip()
    {
        int sampleRate = 44100;
        float duration = 1.6f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float cycle = Mathf.Sin(2f * Mathf.PI * (t / duration));
            float envelope = Mathf.Clamp01((cycle + 1f) * 0.5f);
            float breathNoise = (Mathf.PerlinNoise(t * 26f, 0.11f) - 0.5f) * 0.1f;
            data[i] = breathNoise * envelope;
        }

        AudioClip clip = AudioClip.Create("BreathingLoop", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
}
