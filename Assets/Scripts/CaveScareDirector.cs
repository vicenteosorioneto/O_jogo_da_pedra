using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveScareDirector : MonoBehaviour
{
    private class ScarePoint
    {
        public Vector3 position;
        public float radius;
        public ScareType type;
        public bool triggered;
    }

    private enum ScareType
    {
        Rockfall,
        Blackout,
        Apparition
    }

    [SerializeField] private float globalCooldown = 6f;

    private readonly List<ScarePoint> scarePoints = new List<ScarePoint>();
    private readonly List<Light> caveLights = new List<Light>();

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

        CacheCaveLights();
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

            if (Vector3.Distance(player.position, point.position) <= point.radius)
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
        scarePoints.Add(new ScarePoint
        {
            position = new Vector3(-2f, 1f, -2f),
            radius = 3.2f,
            type = ScareType.Rockfall,
            triggered = false
        });

        scarePoints.Add(new ScarePoint
        {
            position = new Vector3(6f, 1f, 4f),
            radius = 3.2f,
            type = ScareType.Blackout,
            triggered = false
        });

        scarePoints.Add(new ScarePoint
        {
            position = new Vector3(-12f, 1f, 12f),
            radius = 3.5f,
            type = ScareType.Apparition,
            triggered = false
        });
    }

    void CacheCaveLights()
    {
        caveLights.Clear();
        Light[] allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

        for (int i = 0; i < allLights.Length; i++)
        {
            Light current = allLights[i];
            if (current.name.StartsWith("Lamp_"))
                caveLights.Add(current);
        }
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
            case ScareType.Rockfall:
                StartCoroutine(RockfallScare());
                break;
            case ScareType.Blackout:
                StartCoroutine(BlackoutScare());
                break;
            case ScareType.Apparition:
                StartCoroutine(ApparitionScare());
                break;
        }
    }

    IEnumerator RockfallScare()
    {
        Debug.Log("⚠️ O teto da caverna treme...");
        PlayClip(CreateRockfallClip(), 1f, 0.95f);

        if (GameHUD.Instance != null)
            GameHUD.Instance.TriggerJumpscare(0.25f, 0.45f);

        yield return ShakeCamera(0.55f, 0.12f);
    }

    IEnumerator BlackoutScare()
    {
        Debug.Log("⚡ As tochas se apagam por um instante...");
        PlayClip(CreateShockHitClip(), 0.9f, 1f);

        if (GameHUD.Instance != null)
            GameHUD.Instance.TriggerJumpscare(0.5f, 0.35f);

        float[] originalIntensities = new float[caveLights.Count];
        for (int i = 0; i < caveLights.Count; i++)
        {
            originalIntensities[i] = caveLights[i] != null ? caveLights[i].intensity : 0f;
        }

        for (int blink = 0; blink < 3; blink++)
        {
            for (int i = 0; i < caveLights.Count; i++)
            {
                if (caveLights[i] != null)
                    caveLights[i].intensity = 0f;
            }
            yield return new WaitForSeconds(0.12f);

            for (int i = 0; i < caveLights.Count; i++)
            {
                if (caveLights[i] != null)
                    caveLights[i].intensity = originalIntensities[i] * Random.Range(0.5f, 0.9f);
            }
            yield return new WaitForSeconds(0.18f);
        }

        for (int i = 0; i < caveLights.Count; i++)
        {
            if (caveLights[i] != null)
                caveLights[i].intensity = originalIntensities[i];
        }
    }

    IEnumerator ApparitionScare()
    {
        Debug.Log("👻 Você viu algo no fim do túnel...");

        if (GameHUD.Instance != null)
            GameHUD.Instance.TriggerJumpscare(0.75f, 0.3f);

        PlayClip(CreateWhisperClip(), 1f, 0.88f);

        if (ghost != null && player != null)
        {
            Vector3 ahead = player.position + player.forward * 6f;
            Vector3 sideOffset = Vector3.Cross(Vector3.up, player.forward).normalized * Random.Range(-2.2f, 2.2f);
            Vector3 apparitionPosition = ahead + sideOffset;
            apparitionPosition.y = player.position.y;

            ghost.transform.position = apparitionPosition;
        }

        yield return new WaitForSeconds(0.4f);
        yield return ShakeCamera(0.28f, 0.04f);
    }

    IEnumerator ShakeCamera(float duration, float amount)
    {
        if (playerCamera == null)
            yield break;

        Transform camTransform = playerCamera.transform;
        Vector3 originalPosition = camTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float damper = 1f - Mathf.Clamp01(elapsed / duration);
            Vector3 offset = new Vector3(
                Random.Range(-amount, amount),
                Random.Range(-amount, amount),
                0f) * damper;

            camTransform.localPosition = originalPosition + offset;
            yield return null;
        }

        camTransform.localPosition = originalPosition;
    }

    void PlayClip(AudioClip clip, float volume, float pitch)
    {
        if (scareAudio == null || clip == null)
            return;

        scareAudio.pitch = pitch;
        scareAudio.PlayOneShot(clip, volume);
    }

    AudioClip CreateRockfallClip()
    {
        int sampleRate = 44100;
        float duration = 1.4f;
        int sampleCount = Mathf.RoundToInt(sampleRate * duration);
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Clamp01(1f - (t / duration));
            float lowRumble = Mathf.Sin(2f * Mathf.PI * 45f * t) * 0.22f;
            float debrisNoise = (Mathf.PerlinNoise(t * 80f, 0.24f) - 0.5f) * 0.45f;
            data[i] = (lowRumble + debrisNoise) * envelope;
        }

        AudioClip clip = AudioClip.Create("RockfallScare", sampleCount, 1, sampleRate, false);
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
