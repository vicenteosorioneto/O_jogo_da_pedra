using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolScareDirector : MonoBehaviour
{
    private class ScarePoint
    {
        public Vector2 position;
        public float radius;
        public ScareType type;
        public bool triggered;
    }

    private enum ScareType
    {
        DistantBang,
        HallBlackout,
        Apparition
    }

    [SerializeField] private float globalCooldown = 6f;

    private readonly List<ScarePoint> scarePoints = new List<ScarePoint>();

    private Transform player;
    private GhostAI ghost;
    private Camera playerCamera;
    private AudioSource scareAudio;

    private float nextAllowedScareTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        ghost = Object.FindFirstObjectByType<GhostAI>();
        playerCamera = Camera.main;

        SetupAudio();
        BuildScarePoints();

        nextAllowedScareTime = Time.time + 8f;
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (ghost == null)
            ghost = Object.FindFirstObjectByType<GhostAI>();
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (player == null || Time.time < nextAllowedScareTime)
            return;

        for (int i = 0; i < scarePoints.Count; i++)
        {
            ScarePoint point = scarePoints[i];
            if (point.triggered)
                continue;

            if (Vector2.Distance(player.position, point.position) <= point.radius)
            {
                point.triggered = true;
                TriggerScare(point.type);
                nextAllowedScareTime = Time.time + globalCooldown;
                break;
            }
        }
    }

    void BuildScarePoints()
    {
        scarePoints.Add(new ScarePoint { position = new Vector2(11.3f, 1.1f), radius = 2.2f, type = ScareType.DistantBang, triggered = false });
        scarePoints.Add(new ScarePoint { position = new Vector2(8.5f, -2.1f), radius = 2.3f, type = ScareType.HallBlackout, triggered = false });
        scarePoints.Add(new ScarePoint { position = new Vector2(11.2f, -5.2f), radius = 2.4f, type = ScareType.Apparition, triggered = false });
    }

    void SetupAudio()
    {
        scareAudio = gameObject.AddComponent<AudioSource>();
        scareAudio.playOnAwake = false;
        scareAudio.loop = false;
        scareAudio.spatialBlend = 0f;
        scareAudio.volume = 0.8f;
    }

    void TriggerScare(ScareType scareType)
    {
        switch (scareType)
        {
            case ScareType.DistantBang:
                StartCoroutine(DistantBangScare());
                break;
            case ScareType.HallBlackout:
                StartCoroutine(HallBlackoutScare());
                break;
            case ScareType.Apparition:
                StartCoroutine(ApparitionScare());
                break;
        }
    }

    IEnumerator DistantBangScare()
    {
        Debug.Log("⚠️ Um estrondo ecoa pelos corredores...");
        PlayClip(CreateBangClip(), 1f, 0.95f);

        if (GameHUD.Instance != null)
            GameHUD.Instance.TriggerJumpscare(0.25f, 0.35f);

        yield return ShakeCamera(0.45f, 0.18f);
    }

    IEnumerator HallBlackoutScare()
    {
        Debug.Log("⚡ As luzes falham por um instante...");
        PlayClip(CreateShockHitClip(), 0.9f, 1f);

        if (GameHUD.Instance != null)
            GameHUD.Instance.TriggerJumpscare(0.55f, 0.35f);

        yield return ShakeCamera(0.35f, 0.09f);
    }

    IEnumerator ApparitionScare()
    {
        Debug.Log("👻 Algo cruza o corredor ao longe...");

        if (GameHUD.Instance != null)
            GameHUD.Instance.TriggerJumpscare(0.75f, 0.3f);

        PlayClip(CreateWhisperClip(), 1f, 0.88f);

        if (ghost != null && player != null)
        {
            Vector2 ahead = (Vector2)player.position + Vector2.up * 4f;
            Vector2 sideOffset = Random.insideUnitCircle.normalized * Random.Range(1.2f, 2.2f);
            Vector2 apparitionPosition = ahead + sideOffset;

            ghost.transform.position = new Vector3(apparitionPosition.x, apparitionPosition.y, 0f);
        }

        yield return new WaitForSeconds(0.35f);
        yield return ShakeCamera(0.25f, 0.06f);
    }

    IEnumerator ShakeCamera(float duration, float amount)
    {
        if (playerCamera == null)
            yield break;

        Transform camTransform = playerCamera.transform;
        Vector3 originalPosition = camTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float damper = 1f - Mathf.Clamp01(elapsed / duration);
            Vector2 offset2D = Random.insideUnitCircle * amount * damper;
            camTransform.position = originalPosition + new Vector3(offset2D.x, offset2D.y, 0f);
            yield return null;
        }

        camTransform.position = originalPosition;
    }

    void PlayClip(AudioClip clip, float volume, float pitch)
    {
        if (scareAudio == null || clip == null)
            return;

        scareAudio.pitch = pitch;
        scareAudio.PlayOneShot(clip, volume);
    }

    AudioClip CreateBangClip()
    {
        int sampleRate = 44100;
        float duration = 0.9f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Exp(-6f * t);
            float rumble = Mathf.Sin(2f * Mathf.PI * 52f * t) * 0.3f;
            float debrisNoise = (Mathf.PerlinNoise(t * 85f, 0.24f) - 0.5f) * 0.36f;
            data[i] = (rumble + debrisNoise) * envelope;
        }

        AudioClip clip = AudioClip.Create("DistantBang", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    AudioClip CreateShockHitClip()
    {
        int sampleRate = 44100;
        float duration = 0.5f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float burst = Mathf.Exp(-18f * t) * Mathf.Sin(2f * Mathf.PI * 320f * t);
            float staticNoise = (Mathf.PerlinNoise(t * 120f, 0.73f) - 0.5f) * 0.35f;
            data[i] = burst * 0.6f + staticNoise * Mathf.Exp(-10f * t);
        }

        AudioClip clip = AudioClip.Create("BlackoutShock", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    AudioClip CreateWhisperClip()
    {
        int sampleRate = 44100;
        float duration = 1.1f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Sin(Mathf.Clamp01(t / duration) * Mathf.PI);
            float whisperNoise = (Mathf.PerlinNoise(t * 70f, 0.12f) - 0.5f) * 0.23f;
            float undertone = Mathf.Sin(2f * Mathf.PI * 130f * t) * 0.05f;
            data[i] = (whisperNoise + undertone) * envelope;
        }

        AudioClip clip = AudioClip.Create("WhisperScare", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
}
