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

        // Criar HUD
        CreateHUD();

        // Criar Fragmentos (3x)
        CreateFragments();

        // Criar Maria Sangrenta
        CreateGhost();

        // Criar Atmosfera dinâmica
        CreateAtmosphereController();

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
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.35f, 0.35f, 0.4f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.08f, 0.08f, 0.1f);
        RenderSettings.fogDensity = 0.015f;

        // Directional Light (sol/lua)
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 0.9f;
        light.color = new Color(0.7f, 0.7f, 0.9f); // Azul assustador
        lightObj.transform.rotation = Quaternion.Euler(45, 0, 0);

        GameObject fillLightObj = new GameObject("Fill Light");
        Light fillLight = fillLightObj.AddComponent<Light>();
        fillLight.type = LightType.Point;
        fillLight.intensity = 1.5f;
        fillLight.range = 30f;
        fillLight.color = new Color(0.75f, 0.75f, 0.85f);
        fillLightObj.transform.position = new Vector3(0, 3, 0);

        Debug.Log("✓ Iluminação criada");
    }

    void CreateEnvironment()
    {
        // Chão
        GameObject ground = new GameObject("Ground");
        ground.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Plane.fbx");
        ground.AddComponent<MeshRenderer>().material = CreateDefaultMaterial(new Color(0.2f, 0.2f, 0.2f));
        ground.AddComponent<BoxCollider>();
        ground.transform.localScale = new Vector3(24, 1, 24);

        // Paredes simples
        CreateWall("WallN", new Vector3(0, 2, 12), new Vector3(24, 4, 1));
        CreateWall("WallS", new Vector3(0, 2, -12), new Vector3(24, 4, 1));
        CreateWall("WallE", new Vector3(12, 2, 0), new Vector3(1, 4, 24));
        CreateWall("WallW", new Vector3(-12, 2, 0), new Vector3(1, 4, 24));
        CreateWall("Ceiling", new Vector3(0, 4, 0), new Vector3(24, 1, 24));

        CreateFloorTiles();
        CreateCorridorSections();
        CreateInteriorLayout();
        CreateBathroomZone();
        CreateClassroomProps();
        CreateWallDetails();
        CreateCeilingLamps();
        CreateDebrisAndStains();

        Debug.Log("✓ Ambiente criado");
    }

    void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = new GameObject(name);
        wall.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        wall.AddComponent<MeshRenderer>().material = CreateDefaultMaterial(new Color(0.3f, 0.3f, 0.3f));
        wall.AddComponent<BoxCollider>();
        wall.transform.position = position;
        wall.transform.localScale = scale;
    }

    void CreateFloorTiles()
    {
        GameObject floorDetails = new GameObject("FloorTiles");

        for (int x = -10; x <= 10; x += 2)
        {
            for (int z = -10; z <= 10; z += 2)
            {
                Color tileColor = ((x + z) % 4 == 0) ? new Color(0.23f, 0.23f, 0.25f) : new Color(0.16f, 0.16f, 0.18f);
                GameObject tile = CreatePropCube(
                    $"Tile_{x}_{z}",
                    new Vector3(x, 0.01f, z),
                    new Vector3(2f, 0.02f, 2f),
                    tileColor,
                    false);

                tile.transform.SetParent(floorDetails.transform);
            }
        }

        GameObject corridorStrip = CreatePropCube(
            "CorridorStrip",
            new Vector3(0f, 0.02f, 0f),
            new Vector3(1.8f, 0.03f, 22f),
            new Color(0.12f, 0.12f, 0.14f),
            false);
        corridorStrip.transform.SetParent(floorDetails.transform);

        GameObject crossStrip = CreatePropCube(
            "CrossStrip",
            new Vector3(0f, 0.02f, 0f),
            new Vector3(22f, 0.03f, 1.8f),
            new Color(0.12f, 0.12f, 0.14f),
            false);
        crossStrip.transform.SetParent(floorDetails.transform);
    }

    void CreateCorridorSections()
    {
        GameObject corridors = new GameObject("CorridorSections");

        CreatePropCube("NorthMidWall", new Vector3(0f, 1.6f, 6f), new Vector3(12f, 3.2f, 0.25f), new Color(0.24f, 0.24f, 0.26f)).transform.SetParent(corridors.transform);
        CreatePropCube("SouthMidWall", new Vector3(0f, 1.6f, -6f), new Vector3(12f, 3.2f, 0.25f), new Color(0.24f, 0.24f, 0.26f)).transform.SetParent(corridors.transform);
        CreatePropCube("EastMidWall", new Vector3(6f, 1.6f, 0f), new Vector3(0.25f, 3.2f, 12f), new Color(0.24f, 0.24f, 0.26f)).transform.SetParent(corridors.transform);
        CreatePropCube("WestMidWall", new Vector3(-6f, 1.6f, 0f), new Vector3(0.25f, 3.2f, 12f), new Color(0.24f, 0.24f, 0.26f)).transform.SetParent(corridors.transform);

        // Aberturas/caminhos
        CreatePropCube("DoorFrameN", new Vector3(0f, 2.85f, 6f), new Vector3(2.4f, 0.35f, 0.3f), new Color(0.14f, 0.14f, 0.16f)).transform.SetParent(corridors.transform);
        CreatePropCube("DoorFrameS", new Vector3(0f, 2.85f, -6f), new Vector3(2.4f, 0.35f, 0.3f), new Color(0.14f, 0.14f, 0.16f)).transform.SetParent(corridors.transform);
        CreatePropCube("DoorFrameE", new Vector3(6f, 2.85f, 0f), new Vector3(0.3f, 0.35f, 2.4f), new Color(0.14f, 0.14f, 0.16f)).transform.SetParent(corridors.transform);
        CreatePropCube("DoorFrameW", new Vector3(-6f, 2.85f, 0f), new Vector3(0.3f, 0.35f, 2.4f), new Color(0.14f, 0.14f, 0.16f)).transform.SetParent(corridors.transform);

        // Blocos extras para quebrar linha reta
        CreatePropCube("BlockerA", new Vector3(3.5f, 1.2f, 3.5f), new Vector3(1.8f, 2.4f, 1f), new Color(0.2f, 0.2f, 0.22f)).transform.SetParent(corridors.transform);
        CreatePropCube("BlockerB", new Vector3(-3.5f, 1.2f, -3.5f), new Vector3(1.8f, 2.4f, 1f), new Color(0.2f, 0.2f, 0.22f)).transform.SetParent(corridors.transform);
    }

    void CreateInteriorLayout()
    {
        GameObject layout = new GameObject("InteriorLayout");

        GameObject partitionLeft = CreatePropCube("PartitionLeft", new Vector3(-3f, 1.6f, 1f), new Vector3(4f, 3.2f, 0.28f), new Color(0.26f, 0.26f, 0.28f));
        partitionLeft.transform.SetParent(layout.transform);

        GameObject partitionRight = CreatePropCube("PartitionRight", new Vector3(3f, 1.6f, 1f), new Vector3(4f, 3.2f, 0.28f), new Color(0.26f, 0.26f, 0.28f));
        partitionRight.transform.SetParent(layout.transform);

        GameObject doorTop = CreatePropCube("DoorFrameTop", new Vector3(0f, 2.85f, 1f), new Vector3(2.2f, 0.35f, 0.35f), new Color(0.14f, 0.14f, 0.16f));
        doorTop.transform.SetParent(layout.transform);

        GameObject leftFrame = CreatePropCube("DoorFrameLeft", new Vector3(-1.1f, 1.4f, 1f), new Vector3(0.22f, 2.5f, 0.3f), new Color(0.14f, 0.14f, 0.16f));
        leftFrame.transform.SetParent(layout.transform);

        GameObject rightFrame = CreatePropCube("DoorFrameRight", new Vector3(1.1f, 1.4f, 1f), new Vector3(0.22f, 2.5f, 0.3f), new Color(0.14f, 0.14f, 0.16f));
        rightFrame.transform.SetParent(layout.transform);
    }

    void CreateBathroomZone()
    {
        GameObject bathroom = new GameObject("BathroomZone");

        GameObject tileWall = CreatePropCube("BathroomBackWall", new Vector3(0f, 1.6f, 3.8f), new Vector3(7f, 3.2f, 0.18f), new Color(0.62f, 0.62f, 0.65f));
        tileWall.transform.SetParent(bathroom.transform);

        for (int i = 0; i < 3; i++)
        {
            float x = -2.2f + (i * 2.2f);
            GameObject sink = CreatePropCube($"Sink_{i}", new Vector3(x, 0.95f, 3.25f), new Vector3(1.4f, 0.35f, 0.6f), new Color(0.78f, 0.78f, 0.8f));
            sink.transform.SetParent(bathroom.transform);

            GameObject faucet = CreatePropCube($"Faucet_{i}", new Vector3(x, 1.2f, 3.45f), new Vector3(0.18f, 0.2f, 0.15f), new Color(0.45f, 0.45f, 0.48f), false);
            faucet.transform.SetParent(bathroom.transform);
        }

        GameObject stallLeft = CreatePropCube("StallWallLeft", new Vector3(-3.8f, 1.3f, 2.2f), new Vector3(0.16f, 2.6f, 2.4f), new Color(0.68f, 0.68f, 0.7f));
        stallLeft.transform.SetParent(bathroom.transform);
        GameObject stallRight = CreatePropCube("StallWallRight", new Vector3(-2.4f, 1.3f, 2.2f), new Vector3(0.16f, 2.6f, 2.4f), new Color(0.68f, 0.68f, 0.7f));
        stallRight.transform.SetParent(bathroom.transform);
        GameObject stallDoor = CreatePropCube("StallDoorBroken", new Vector3(-3.1f, 1.1f, 1.05f), new Vector3(1.2f, 2.2f, 0.08f), new Color(0.58f, 0.58f, 0.6f));
        stallDoor.transform.rotation = Quaternion.Euler(0, 22f, 0);
        stallDoor.transform.SetParent(bathroom.transform);
    }

    void CreateDebrisAndStains()
    {
        GameObject debris = new GameObject("DebrisAndStains");

        for (int i = 0; i < 16; i++)
        {
            float x = Random.Range(-4.3f, 4.3f);
            float z = Random.Range(-4.3f, 4.3f);
            float scale = Random.Range(0.08f, 0.22f);

            GameObject paper = CreatePropCube($"Paper_{i}", new Vector3(x, 0.03f, z), new Vector3(scale * 1.5f, 0.01f, scale), new Color(0.6f, 0.6f, 0.55f), false);
            paper.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), Random.Range(-6f, 6f));
            paper.transform.SetParent(debris.transform);
        }

        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(-4.2f, 4.2f);
            float z = Random.Range(-4.2f, 4.2f);
            float size = Random.Range(0.18f, 0.45f);

            GameObject stain = CreatePropCube($"Stain_{i}", new Vector3(x, 0.015f, z), new Vector3(size, 0.01f, size), new Color(0.08f, 0.05f, 0.04f), false);
            stain.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            stain.transform.SetParent(debris.transform);
        }
    }

    void CreateClassroomProps()
    {
        GameObject classroomProps = new GameObject("ClassroomProps");
        Color deskColor = new Color(0.32f, 0.24f, 0.16f);
        Color chairColor = new Color(0.22f, 0.22f, 0.24f);

        for (int row = 0; row < 2; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                float x = -2.8f + (col * 2.8f);
                float z = 2.5f - (row * 3f);

                GameObject desk = CreatePropCube($"Desk_{row}_{col}", new Vector3(x, 0.55f, z), new Vector3(1.4f, 0.7f, 0.9f), deskColor);
                desk.transform.SetParent(classroomProps.transform);

                GameObject chair = CreatePropCube($"Chair_{row}_{col}", new Vector3(x, 0.3f, z - 0.9f), new Vector3(0.7f, 0.4f, 0.7f), chairColor);
                chair.transform.SetParent(classroomProps.transform);
            }
        }

        GameObject teacherDesk = CreatePropCube("TeacherDesk", new Vector3(0, 0.65f, 3.9f), new Vector3(2.4f, 0.9f, 1f), new Color(0.36f, 0.28f, 0.2f));
        teacherDesk.transform.SetParent(classroomProps.transform);

        GameObject board = CreatePropCube("Blackboard", new Vector3(0, 2.2f, 4.45f), new Vector3(4.6f, 1.8f, 0.08f), new Color(0.08f, 0.15f, 0.12f));
        board.transform.SetParent(classroomProps.transform);

        GameObject lockerA = CreatePropCube("LockerA", new Vector3(4.2f, 1.3f, 2.2f), new Vector3(0.7f, 2.6f, 1f), new Color(0.2f, 0.23f, 0.27f));
        lockerA.transform.SetParent(classroomProps.transform);
        GameObject lockerB = CreatePropCube("LockerB", new Vector3(4.2f, 1.3f, 0.9f), new Vector3(0.7f, 2.6f, 1f), new Color(0.18f, 0.2f, 0.24f));
        lockerB.transform.SetParent(classroomProps.transform);
    }

    void CreateWallDetails()
    {
        GameObject wallDetails = new GameObject("WallDetails");

        GameObject windowN1 = CreatePropCube("WindowN1", new Vector3(-2.4f, 2.1f, 4.48f), new Vector3(1.8f, 1.2f, 0.08f), new Color(0.3f, 0.45f, 0.55f));
        windowN1.transform.SetParent(wallDetails.transform);
        GameObject windowN2 = CreatePropCube("WindowN2", new Vector3(2.4f, 2.1f, 4.48f), new Vector3(1.8f, 1.2f, 0.08f), new Color(0.3f, 0.45f, 0.55f));
        windowN2.transform.SetParent(wallDetails.transform);

        GameObject posterA = CreatePropCube("PosterA", new Vector3(-4.45f, 1.8f, -1.5f), new Vector3(0.08f, 1.1f, 0.9f), new Color(0.52f, 0.2f, 0.2f));
        posterA.transform.SetParent(wallDetails.transform);
        GameObject posterB = CreatePropCube("PosterB", new Vector3(4.45f, 1.7f, -2.2f), new Vector3(0.08f, 1f, 0.8f), new Color(0.22f, 0.22f, 0.52f));
        posterB.transform.SetParent(wallDetails.transform);

        GameObject bloodMark = CreatePropCube("BloodMark", new Vector3(0f, 1.4f, -4.45f), new Vector3(1.6f, 0.7f, 0.08f), new Color(0.35f, 0.05f, 0.05f));
        bloodMark.transform.SetParent(wallDetails.transform);
    }

    void CreateCeilingLamps()
    {
        GameObject lamps = new GameObject("CeilingLamps");

        CreateLamp("Lamp_0", new Vector3(0f, 3.4f, 0f), 2.3f, new Color(0.9f, 0.9f, 0.8f), lamps.transform);
        CreateLamp("Lamp_1", new Vector3(0f, 3.4f, 2.8f), 1.8f, new Color(0.75f, 0.75f, 0.9f), lamps.transform);
        CreateLamp("Lamp_2", new Vector3(0f, 3.4f, -2.8f), 1.6f, new Color(0.8f, 0.75f, 0.65f), lamps.transform);
    }

    void CreateLamp(string name, Vector3 position, float intensity, Color color, Transform parent)
    {
        GameObject lamp = CreatePropCube(name, position, new Vector3(0.6f, 0.1f, 0.6f), new Color(0.8f, 0.8f, 0.82f), false);
        lamp.transform.SetParent(parent);

        Light light = lamp.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 10f;
        light.intensity = intensity;
        light.color = color;
    }

    GameObject CreatePropCube(string name, Vector3 position, Vector3 scale, Color color, bool withCollider = true)
    {
        GameObject prop = new GameObject(name);
        prop.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        prop.AddComponent<MeshRenderer>().material = CreateDefaultMaterial(color);
        if (withCollider)
            prop.AddComponent<BoxCollider>();

        prop.transform.position = position;
        prop.transform.localScale = scale;
        return prop;
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
        player.AddComponent<PlayerAudioFeedback>();

        // Camera filho
        GameObject camera = new GameObject("PlayerCamera");
        camera.transform.parent = player.transform;
        camera.transform.localPosition = new Vector3(0, 0.85f, 0);
        camera.transform.localRotation = Quaternion.identity;

        Camera cam = camera.AddComponent<Camera>();
        cam.fieldOfView = 75f;
        cam.nearClipPlane = 0.03f;
        camera.AddComponent<AudioListener>();

        // Assignar no PlayerController
        PlayerController pc = player.GetComponent<PlayerController>();
        var playerCameraField = typeof(PlayerController).GetField("playerCamera",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (playerCameraField != null)
            playerCameraField.SetValue(pc, cam);

        player.transform.position = new Vector3(-9, 1, -9);
        player.transform.rotation = Quaternion.Euler(0, 180, 0);

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

    void CreateHUD()
    {
        GameObject hud = new GameObject("GameHUD");
        hud.AddComponent<GameHUD>();

        Debug.Log("✓ HUD criado");
    }

    void CreateFragments()
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-9f, 0.8f, 8f),
            new Vector3(9f, 0.8f, -8f),
            new Vector3(8f, 0.8f, 9f)
        };

        Material fragmentMat = CreateDefaultMaterial(new Color(0.8f, 0.2f, 0.2f)); // Vermelho

        for (int i = 0; i < 3; i++)
        {
            GameObject fragment = new GameObject($"Fragment_{i + 1}");
            
            fragment.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
            fragment.AddComponent<MeshRenderer>().material = fragmentMat;
            
            SphereCollider collider = fragment.AddComponent<SphereCollider>();
            collider.isTrigger = false;
            collider.radius = 0.3f;

            FragmentCollector fc = fragment.AddComponent<FragmentCollector>();
            var fragmentIdField = typeof(FragmentCollector).GetField("fragmentID",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fragmentIdField != null)
                fragmentIdField.SetValue(fc, i + 1);

            fragment.transform.position = positions[i];
            fragment.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }

        Debug.Log("✓ 3 Fragmentos criados");
    }

    void CreateGhost()
    {
        GameObject ghost = new GameObject("MariasSangrenta");
        
        ghost.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        ghost.AddComponent<MeshRenderer>().material = CreateDefaultMaterial(new Color(0.5f, 0, 0)); // Vermelho escuro

        CapsuleCollider collider = ghost.AddComponent<CapsuleCollider>();
        collider.height = 2;

        Rigidbody rb = ghost.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        GhostAI ghostAI = ghost.AddComponent<GhostAI>();
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        var playerField = typeof(GhostAI).GetField("player",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (playerField != null)
            playerField.SetValue(ghostAI, playerTransform);

        ghost.transform.position = new Vector3(10, 1, 10);
        ghost.transform.localScale = new Vector3(0.8f, 2, 0.8f);

        Debug.Log("✓ Maria Sangrenta criada");
    }

    void CreateAtmosphereController()
    {
        GameObject atmosphere = new GameObject("AtmosphereController");
        atmosphere.AddComponent<AtmosphereController>();
        Debug.Log("✓ Atmosfera dinâmica criada");
    }

    void CreateMirror()
    {
        GameObject mirror = new GameObject("Mirror");
        
        mirror.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
        mirror.AddComponent<MeshRenderer>().material = CreateDefaultMaterial(new Color(0.7f, 0.7f, 0.7f)); // Cinza espelho

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

        mirror.transform.position = new Vector3(-10, 1.5f, 10);
        mirror.transform.rotation = Quaternion.identity;

        Debug.Log("✓ Espelho (Ritual) criado");
    }

    void CreateExit()
    {
        GameObject exit = new GameObject("SchoolExit");
        
        exit.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        exit.AddComponent<MeshRenderer>().material = CreateDefaultMaterial(new Color(0, 0.5f, 0)); // Verde

        BoxCollider collider = exit.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        exit.AddComponent<ExitManager>();

        exit.transform.position = new Vector3(10, 1, -10);
        exit.transform.localScale = new Vector3(1, 2, 1);

        Debug.Log("✓ Saída criada");
    }

    Material CreateDefaultMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");
        if (shader == null)
            shader = Shader.Find("Sprites/Default");

        Material material = new Material(shader);
        material.color = color;
        return material;
    }
}
