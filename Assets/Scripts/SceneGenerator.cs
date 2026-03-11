using UnityEngine;

/// <summary>
/// SceneGenerator - Cria a cena inteira automaticamente!
/// Basta criar um GameObject vazio, attachar este script e dar PLAY.
/// </summary>
public class SceneGenerator : MonoBehaviour
{
    [SerializeField] private bool generateOnStart = true;

    void Start()
    {
        if (generateOnStart)
        {
            GenerateScene();
        }
    }

    void GenerateScene()
    {
        Debug.Log("🎮 Gerando cena automaticamente...");

        // Limpar cena anterior se necessário
        ClearScene();

        // Criar iluminação
        CreateLighting();

        // Criar ambiente
        CreateEnvironment();

        // Criar Player
        CreatePlayer();

        // Criar GameManager
        CreateGameManager();

        // Criar InputManager
        CreateInputManager();

        // Criar Fragmentos (3x)
        CreateFragments();

        // Criar Maria Sangrenta
        CreateGhost();

        // Criar Espelho (Ritual)
        CreateMirror();

        // Criar Saída
        CreateExit();

        Debug.Log("✅ Cena gerada com sucesso! Pressione PLAY ▶️");
    }

    void ClearScene()
    {
        // Remover Main Camera padrão
        Camera mainCam = Camera.main;
        if (mainCam != null && mainCam.name == "Main Camera")
        {
            DestroyImmediate(mainCam.gameObject);
        }
    }

    void CreateLighting()
    {
        // Directional Light (sol/lua)
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 0.4f;
        light.color = new Color(0.7f, 0.7f, 0.9f); // Azul assustador
        lightObj.transform.rotation = Quaternion.Euler(45, 0, 0);

        Debug.Log("✓ Iluminação criada");
    }

    void CreateEnvironment()
    {
        // Chão
        GameObject ground = new GameObject("Ground");
        ground.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Plane.fbx");
        ground.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        ground.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
        ground.AddComponent<BoxCollider>();
        ground.transform.localScale = new Vector3(10, 1, 10);

        // Paredes simples
        CreateWall("WallN", new Vector3(0, 2, 5), new Vector3(10, 4, 1));
        CreateWall("WallS", new Vector3(0, 2, -5), new Vector3(10, 4, 1));
        CreateWall("WallE", new Vector3(5, 2, 0), new Vector3(1, 4, 10));
        CreateWall("WallW", new Vector3(-5, 2, 0), new Vector3(1, 4, 10));

        Debug.Log("✓ Ambiente criado");
    }

    void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = new GameObject(name);
        wall.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        wall.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        wall.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f);
        wall.AddComponent<BoxCollider>();
        wall.transform.position = position;
        wall.transform.localScale = scale;
    }

    void CreatePlayer()
    {
        // Player Capsule
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        CapsuleCollider collider = player.AddComponent<CapsuleCollider>();
        collider.height = 2;
        collider.radius = 0.4f;

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;

        player.AddComponent<PlayerController>();

        // Camera filho
        GameObject camera = new GameObject("PlayerCamera");
        camera.transform.parent = player.transform;
        camera.transform.localPosition = new Vector3(0, 0.6f, 0);
        camera.transform.localRotation = Quaternion.identity;

        Camera cam = camera.AddComponent<Camera>();
        camera.AddComponent<AudioListener>();

        // Assignar no PlayerController
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.GetComponent<PlayerController>().SetCamera(cam);

        player.transform.position = new Vector3(0, 1, -3);

        Debug.Log("✓ Player criado");
    }

    void CreateGameManager()
    {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();

        Debug.Log("✓ GameManager criado");
    }

    void CreateInputManager()
    {
        GameObject im = new GameObject("InputManager");
        im.AddComponent<InputManager>();

        Debug.Log("✓ InputManager criado");
    }

    void CreateFragments()
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-3, 0.5f, 2),
            new Vector3(3, 0.5f, 2),
            new Vector3(0, 0.5f, -4)
        };

        Material fragmentMat = new Material(Shader.Find("Standard"));
        fragmentMat.color = new Color(0.8f, 0.2f, 0.2f); // Vermelho

        for (int i = 0; i < 3; i++)
        {
            GameObject fragment = new GameObject($"Fragment_{i + 1}");
            
            fragment.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            fragment.AddComponent<MeshRenderer>().material = fragmentMat;
            
            SphereCollider collider = fragment.AddComponent<SphereCollider>();
            collider.isTrigger = false;
            collider.radius = 0.3f;

            FragmentCollector fc = fragment.AddComponent<FragmentCollector>();
            fc.SetFragmentID(i + 1);

            fragment.transform.position = positions[i];
            fragment.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        Debug.Log("✓ 3 Fragmentos criados");
    }

    void CreateGhost()
    {
        GameObject ghost = new GameObject("MariasSangrenta");
        
        ghost.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        ghost.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        ghost.GetComponent<Renderer>().material.color = new Color(0.5f, 0, 0); // Vermelho escuro

        CapsuleCollider collider = ghost.AddComponent<CapsuleCollider>();
        collider.height = 2;

        Rigidbody rb = ghost.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        GhostAI ghostAI = ghost.AddComponent<GhostAI>();
        ghostAI.SetPlayer(GameObject.FindGameObjectWithTag("Player").transform);

        ghost.transform.position = new Vector3(5, 1, 5);
        ghost.transform.localScale = new Vector3(0.8f, 2, 0.8f);

        Debug.Log("✓ Maria Sangrenta criada");
    }

    void CreateMirror()
    {
        GameObject mirror = new GameObject("Mirror");
        
        mirror.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
        mirror.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        mirror.GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.7f); // Cinza espelho

        BoxCollider collider = mirror.AddComponent<BoxCollider>();
        collider.size = new Vector3(1, 1.5f, 0.1f);

        mirror.AddComponent<RitualManager>();

        // Light para ritual
        GameObject lightObj = new GameObject("RitualLight");
        lightObj.transform.parent = mirror.transform;
        lightObj.transform.localPosition = Vector3.zero;
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Point;
        light.intensity = 2;
        light.range = 10;
        light.color = new Color(0.2f, 1, 0.5f); // Verde fantasmagórico
        light.enabled = false;

        mirror.transform.position = new Vector3(0, 1.5f, 4);
        mirror.transform.rotation = Quaternion.identity;

        Debug.Log("✓ Espelho (Ritual) criado");
    }

    void CreateExit()
    {
        GameObject exit = new GameObject("SchoolExit");
        
        exit.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        exit.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        exit.GetComponent<Renderer>().material.color = new Color(0, 0.5f, 0); // Verde

        BoxCollider collider = exit.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        exit.AddComponent<ExitManager>();

        exit.transform.position = new Vector3(-4, 1, -4);
        exit.transform.localScale = new Vector3(1, 2, 1);

        Debug.Log("✓ Saída criada");
    }
}

// Extensão para PlayerController
public partial class PlayerController : MonoBehaviour
{
    private Camera playerCamera;

    public void SetCamera(Camera cam)
    {
        playerCamera = cam;
        // Usar reflection para assignar o campo private
        System.Reflection.FieldInfo field = typeof(PlayerController).GetField("playerCamera", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(this, cam);
    }
}

// Extensão para FragmentCollector
public partial class FragmentCollector : MonoBehaviour
{
    public void SetFragmentID(int id)
    {
        System.Reflection.FieldInfo field = typeof(FragmentCollector).GetField("fragmentID", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(this, id);
    }
}

// Extensão para GhostAI
public partial class GhostAI : MonoBehaviour
{
    public void SetPlayer(Transform playerTransform)
    {
        System.Reflection.FieldInfo field = typeof(GhostAI).GetField("player", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(this, playerTransform);
    }
}
