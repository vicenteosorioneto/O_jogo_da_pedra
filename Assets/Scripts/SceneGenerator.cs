using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    [SerializeField] private bool gerarAoIniciar = true;
    [SerializeField] private bool mostrarRotulosMapa = false;

    private Material materialPiso;
    private Material materialParede;
    private Material materialProp;
    private Material materialPerigo;
    private Sprite squareSprite;

    void Start()
    {
        if (gerarAoIniciar)
            GerarCena();
    }

    void GerarCena()
    {
        Debug.Log("🎮 Gerando mapa 2D da escola abandonada...");

        LimparCena();
        ConfigurarCamera();
        ConfigurarIluminacao();
        CriarMateriais();

        CriarBaseMapa();
        CriarSalasPrincipais();
        CriarParedesColisao();
        CriarDecoracaoEscola();
        CriarPortasVisuais();
        if (mostrarRotulosMapa)
            CriarRotulosMapa();

        CriarGameManager();
        CriarInputManager();
        CriarHUD();
        CriarJogador();

        CriarFragmentos();
        CriarDocumentos();
        CriarEspelhoRitual();
        CriarSaida();
        CriarMariaSangrenta();
        CriarGuiaSetasObjetivo();

        CriarAtmosfera();
        CriarDiretorSustos();

        Debug.Log("✅ Mapa da escola criado com sucesso.");
    }

    void LimparCena()
    {
        Camera cameraPrincipal = Camera.main;
        if (cameraPrincipal != null)
            DestroyImmediate(cameraPrincipal.gameObject);
    }

    void ConfigurarCamera()
    {
        GameObject cameraObject = new GameObject("CameraPrincipal");
        Camera cam = cameraObject.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 10.4f;
        cam.nearClipPlane = -10f;
        cam.farClipPlane = 60f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.12f, 0.12f, 0.14f);

        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0f, 0f, -10f);
        cameraObject.AddComponent<AudioListener>();
    }

    void ConfigurarIluminacao()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.28f, 0.28f, 0.3f);
        RenderSettings.fog = false;
    }

    void CriarMateriais()
    {
        materialPiso = CriarMaterial(new Color(0.32f, 0.32f, 0.34f));
        materialParede = CriarMaterial(new Color(0.18f, 0.18f, 0.2f));
        materialProp = CriarMaterial(new Color(0.44f, 0.44f, 0.46f));
        materialPerigo = CriarMaterial(new Color(0.7f, 0.2f, 0.2f));
    }

    void CriarBaseMapa()
    {
        GameObject mapa = new GameObject("MapaDaEscola");

        CriarRetangulo("BaseEscola", Vector2.zero, new Vector2(28f, 19f), materialPiso, false).transform.SetParent(mapa.transform);
        CriarRetangulo("MolduraInterna", Vector2.zero, new Vector2(26.6f, 17.4f), CriarMaterial(new Color(0.14f, 0.14f, 0.15f)), false).transform.SetParent(mapa.transform);

        CriarRetangulo("CorredorNorte", new Vector2(0f, 7f), new Vector2(21.6f, 1.6f), CriarMaterial(new Color(0.22f, 0.22f, 0.24f)), false).transform.SetParent(mapa.transform);
        CriarRetangulo("CorredorCentral", new Vector2(0f, 0.3f), new Vector2(21.8f, 1.6f), CriarMaterial(new Color(0.2f, 0.2f, 0.22f)), false).transform.SetParent(mapa.transform);
        CriarRetangulo("CorredorSul", new Vector2(0f, -6.8f), new Vector2(12.6f, 1.6f), CriarMaterial(new Color(0.22f, 0.22f, 0.24f)), false).transform.SetParent(mapa.transform);

        CriarRetangulo("PassagemEsquerda", new Vector2(-6.2f, -4.5f), new Vector2(1.6f, 3.8f), CriarMaterial(new Color(0.2f, 0.2f, 0.22f)), false).transform.SetParent(mapa.transform);
        CriarRetangulo("PassagemDireita", new Vector2(6.2f, -1.5f), new Vector2(1.5f, 2.9f), CriarMaterial(new Color(0.2f, 0.2f, 0.22f)), false).transform.SetParent(mapa.transform);

        CriarRetangulo("AlaDireita", new Vector2(9.6f, -2.2f), new Vector2(6.8f, 8.6f), CriarMaterial(new Color(0.19f, 0.19f, 0.21f)), false).transform.SetParent(mapa.transform);
    }

    void CriarSalasPrincipais()
    {
        GameObject salas = new GameObject("SalasPrincipais");

        CriarSala("Diretoria", new Vector2(0f, 6.9f), new Vector2(4.8f, 1.4f), new Color(0.2f, 0.19f, 0.22f), salas.transform);
        CriarSala("BanheiroFeminino", new Vector2(-9.3f, 2.2f), new Vector2(5.2f, 2.7f), new Color(0.22f, 0.18f, 0.18f), salas.transform);
        CriarSala("Vestiario", new Vector2(-9.4f, -1.9f), new Vector2(5f, 2.1f), new Color(0.18f, 0.2f, 0.2f), salas.transform);

        CriarSala("Biblioteca", new Vector2(0f, 1.7f), new Vector2(7.2f, 2.4f), new Color(0.21f, 0.2f, 0.18f), salas.transform);
        CriarSala("SalaDeAula", new Vector2(0f, -0.8f), new Vector2(7.2f, 2.0f), new Color(0.2f, 0.2f, 0.22f), salas.transform);
        CriarSala("SalaDosProfessores", new Vector2(0f, -3.8f), new Vector2(7.2f, 2.0f), new Color(0.21f, 0.2f, 0.18f), salas.transform);

        CriarSala("Laboratorio", new Vector2(9.6f, 2.5f), new Vector2(5.6f, 2.7f), new Color(0.2f, 0.22f, 0.2f), salas.transform);
        CriarSala("Ginasio", new Vector2(9.6f, -3.2f), new Vector2(5.6f, 4.1f), new Color(0.18f, 0.2f, 0.24f), salas.transform);
    }

    void CriarSala(string nome, Vector2 posicao, Vector2 tamanho, Color cor, Transform parent)
    {
        GameObject sala = CriarRetangulo(nome, posicao, tamanho, CriarMaterial(cor), false);
        sala.transform.SetParent(parent);
    }

    void CriarParedesColisao()
    {
        GameObject paredes = new GameObject("ParedesColisao");

        CriarParede("ParedeNorte", new Vector2(0f, 9.5f), new Vector2(28f, 1f), paredes.transform);
        CriarParede("ParedeSul", new Vector2(0f, -9.5f), new Vector2(28f, 1f), paredes.transform);
        CriarParede("ParedeOeste", new Vector2(-14f, 0f), new Vector2(1f, 19f), paredes.transform);
        CriarParede("ParedeLeste", new Vector2(14f, 0f), new Vector2(1f, 19f), paredes.transform);

        // Colunas entre bloco central e corredores
        CriarParedeVerticalComAbertura("ColunaNorteEsq", -6.8f, -4.2f, 8.2f, 0.8f, 1.4f, paredes.transform);
        CriarParedeVerticalComAbertura("ColunaNorteDir", 6.8f, -4.2f, 8.2f, 0.8f, 1.4f, paredes.transform);

        // Separadores horizontais do bloco central com portas
        CriarParedeHorizontalComAbertura("DivisoriaBiblioteca", -6.8f, 6.8f, 3.2f, 0f, 1.8f, paredes.transform);
        CriarParedeHorizontalComAbertura("DivisoriaSalaAula", -6.8f, 6.8f, 0.0f, 4.8f, 1.4f, paredes.transform);
        CriarParedeHorizontalComAbertura("DivisoriaProfessores", -6.8f, 6.8f, -2.6f, -4.9f, 1.4f, paredes.transform);

        // Ala esquerda (banheiro/vestiário) com acesso pelo corredor central
        CriarParedeVerticalComAbertura("ParedeAlaEsquerdaInterna", -11.8f, -4.2f, 4.2f, 0.6f, 1.5f, paredes.transform);
        CriarParedeHorizontalComAbertura("DivisoriaBanheiroVestiario", -13f, -6.8f, 0f, -8.8f, 1.4f, paredes.transform);

        // Ala direita (laboratório/ginásio)
        CriarParedeVerticalComAbertura("ParedeAlaDireitaInterna", 11.8f, -5.8f, 4.8f, 0.8f, 1.5f, paredes.transform);
        CriarParedeHorizontalComAbertura("DivisoriaLaboratorioGinasio", 6.8f, 13f, -0.1f, 8.5f, 1.4f, paredes.transform);

        // Corredor sul reduzido como no mapa
        CriarParede("FechamentoSulEsq", new Vector2(-7.6f, -7f), new Vector2(2.2f, 1f), paredes.transform);
        CriarParede("FechamentoSulDir", new Vector2(7.6f, -7f), new Vector2(2.2f, 1f), paredes.transform);

        // Guias de escada/passagem (formato em L no lado esquerdo)
        CriarParede("EscadaPassagemTopo", new Vector2(-5.2f, -3.7f), new Vector2(2.4f, 0.7f), paredes.transform);
        CriarParede("EscadaPassagemLateral", new Vector2(-6.5f, -5.1f), new Vector2(0.7f, 2.1f), paredes.transform);
    }

    void CriarDecoracaoEscola()
    {
        GameObject decoracao = new GameObject("DecoracaoEscola");

        CriarRetangulo("MesaBiblioteca_1", new Vector2(-1.8f, 1.7f), new Vector2(1.2f, 0.6f), materialProp, true).transform.SetParent(decoracao.transform);
        CriarRetangulo("MesaBiblioteca_2", new Vector2(1.8f, 1.7f), new Vector2(1.2f, 0.6f), materialProp, true).transform.SetParent(decoracao.transform);
        CriarRetangulo("QuadroSalaAula", new Vector2(0f, -0.1f), new Vector2(2.2f, 0.35f), materialProp, false).transform.SetParent(decoracao.transform);
        CriarRetangulo("ArmarioLaboratorio", new Vector2(11.1f, 2.4f), new Vector2(1f, 1.8f), materialProp, true).transform.SetParent(decoracao.transform);

        CriarRetangulo("PerigoLaboratorio", new Vector2(11.3f, 1.1f), new Vector2(0.55f, 0.55f), materialPerigo, false).transform.SetParent(decoracao.transform);
        CriarRetangulo("PerigoGinasio_1", new Vector2(8.1f, -2f), new Vector2(0.55f, 0.55f), materialPerigo, false).transform.SetParent(decoracao.transform);
        CriarRetangulo("PerigoGinasio_2", new Vector2(11.2f, -5.2f), new Vector2(0.55f, 0.55f), materialPerigo, false).transform.SetParent(decoracao.transform);

        CriarRetangulo("SinalEscadaEsquerda", new Vector2(-6.2f, -4.9f), new Vector2(0.7f, 0.4f), CriarMaterial(new Color(0.78f, 0.78f, 0.68f)), false).transform.SetParent(decoracao.transform);
        CriarRetangulo("SinalEscadaDireita", new Vector2(6.2f, -1.3f), new Vector2(0.7f, 0.4f), CriarMaterial(new Color(0.78f, 0.78f, 0.68f)), false).transform.SetParent(decoracao.transform);
    }

    void CriarPortasVisuais()
    {
        GameObject portas = new GameObject("PortasVisuais");
        Material materialPorta = CriarMaterial(new Color(0.64f, 0.62f, 0.58f));

        CriarRetangulo("PortaDiretoria", new Vector2(0f, 6.1f), new Vector2(1.2f, 0.25f), materialPorta, false).transform.SetParent(portas.transform);
        CriarRetangulo("PortaBiblioteca", new Vector2(0f, 3.2f), new Vector2(1.2f, 0.25f), materialPorta, false).transform.SetParent(portas.transform);
        CriarRetangulo("PortaSalaAula", new Vector2(4.8f, 0f), new Vector2(0.25f, 1.1f), materialPorta, false).transform.SetParent(portas.transform);
        CriarRetangulo("PortaProfessores", new Vector2(-4.9f, -2.6f), new Vector2(0.25f, 1.1f), materialPorta, false).transform.SetParent(portas.transform);

        CriarRetangulo("PortaBanheiro", new Vector2(-8.8f, 0.6f), new Vector2(1.1f, 0.25f), materialPorta, false).transform.SetParent(portas.transform);
        CriarRetangulo("PortaVestiario", new Vector2(-8.9f, -0.7f), new Vector2(1.1f, 0.25f), materialPorta, false).transform.SetParent(portas.transform);

        CriarRetangulo("PortaLaboratorio", new Vector2(8.5f, 1.0f), new Vector2(0.25f, 1.1f), materialPorta, false).transform.SetParent(portas.transform);
        CriarRetangulo("PortaGinasio", new Vector2(8.5f, -3.5f), new Vector2(0.25f, 1.1f), materialPorta, false).transform.SetParent(portas.transform);
    }

    void CriarGameManager()
    {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
    }

    void CriarInputManager()
    {
        GameObject im = new GameObject("InputManager");
        im.AddComponent<InputManager>();
    }

    void CriarHUD()
    {
        GameObject hud = new GameObject("GameHUD");
        hud.AddComponent<GameHUD>();
    }

    void CriarJogador()
    {
        GameObject jogador = CriarRetangulo("Jogador", new Vector2(-9.8f, -2.8f), new Vector2(0.9f, 0.9f), CriarMaterial(new Color(0.72f, 0.82f, 0.95f)), true);
        jogador.tag = "Player";

        Rigidbody2D rb = jogador.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        jogador.AddComponent<PlayerController>();
        jogador.AddComponent<PlayerAudioFeedback>();
    }

    void CriarFragmentos()
    {
        Vector2[] posicoes =
        {
            new Vector2(-8.6f, 2.1f),
            new Vector2(10.8f, 2.7f),
            new Vector2(8.2f, -2.4f)
        };

        Material materialFragmento = CriarMaterial(new Color(0.88f, 0.2f, 0.2f));

        for (int i = 0; i < posicoes.Length; i++)
        {
            GameObject fragmento = CriarRetangulo($"Fragmento_{i + 1}", posicoes[i], new Vector2(0.6f, 0.6f), materialFragmento, true);

            FragmentCollector coletor = fragmento.AddComponent<FragmentCollector>();
            var idField = typeof(FragmentCollector).GetField("fragmentID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (idField != null)
                idField.SetValue(coletor, i + 1);
        }
    }

    void CriarDocumentos()
    {
        Material materialDocumento = CriarMaterial(new Color(0.84f, 0.8f, 0.66f));

        CriarDocumento("Documento_Biblioteca", new Vector2(2.4f, 1.7f), "Anotações da Biblioteca", "Páginas arrancadas contam sobre o ritual do espelho.", 1, materialDocumento);
        CriarDocumento("Documento_SalaAula", new Vector2(2.2f, -0.8f), "Bilhete da Sala", "'Não olhe no espelho depois da meia-noite.'", 2, materialDocumento);
        CriarDocumento("Documento_Professores", new Vector2(2.8f, -3.8f), "Relatório da Diretoria", "Há relatos de sussurros vindos do banheiro feminino.", 3, materialDocumento);
    }

    void CriarDocumento(string nome, Vector2 posicao, string titulo, string conteudo, int id, Material material)
    {
        GameObject documento = CriarRetangulo(nome, posicao, new Vector2(0.55f, 0.55f), material, true);
        DocumentReader leitor = documento.AddComponent<DocumentReader>();

        var tituloField = typeof(DocumentReader).GetField("documentTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var conteudoField = typeof(DocumentReader).GetField("documentContent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var idField = typeof(DocumentReader).GetField("documentID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (tituloField != null) tituloField.SetValue(leitor, titulo);
        if (conteudoField != null) conteudoField.SetValue(leitor, conteudo);
        if (idField != null) idField.SetValue(leitor, id);
    }

    void CriarEspelhoRitual()
    {
        GameObject espelho = CriarRetangulo("EspelhoRitual", new Vector2(-10.8f, 3.1f), new Vector2(1.1f, 1.7f), CriarMaterial(new Color(0.64f, 0.64f, 0.72f)), true);
        espelho.AddComponent<RitualManager>();

        GameObject luzRitualObj = new GameObject("LuzRitual");
        luzRitualObj.transform.SetParent(espelho.transform);
        luzRitualObj.transform.localPosition = Vector3.zero;

        Light luzRitual = luzRitualObj.AddComponent<Light>();
        luzRitual.type = LightType.Point;
        luzRitual.range = 6f;
        luzRitual.intensity = 1.2f;
        luzRitual.color = new Color(0.2f, 1f, 0.5f);
        luzRitual.enabled = false;
    }

    void CriarSaida()
    {
        GameObject saida = CriarRetangulo("SaidaEscola", new Vector2(4.8f, -6.8f), new Vector2(1.2f, 0.9f), CriarMaterial(new Color(0.16f, 0.45f, 0.2f)), true);
        saida.AddComponent<ExitManager>();
    }

    void CriarMariaSangrenta()
    {
        GameObject maria = CriarRetangulo("MariaSangrenta", new Vector2(9.6f, -3.4f), new Vector2(0.95f, 1.15f), CriarMaterial(new Color(0.2f, 0.04f, 0.05f)), true);

        Rigidbody2D rb = maria.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        maria.AddComponent<GhostAI>();
    }

    void CriarAtmosfera()
    {
        GameObject atmosfera = new GameObject("AtmosferaController");
        atmosfera.AddComponent<AtmosphereController>();
    }

    void CriarDiretorSustos()
    {
        GameObject diretorSustos = new GameObject("DiretorSustosEscola");
        System.Type scareType = System.Type.GetType("SchoolScareDirector")
            ?? System.Type.GetType("SchoolScareDirector, Assembly-CSharp");

        if (scareType != null)
            diretorSustos.AddComponent(scareType);
    }

    void CriarGuiaSetasObjetivo()
    {
        GameObject guiaObjetivo = new GameObject("GuiaSetasObjetivo");
        guiaObjetivo.AddComponent<ObjectiveArrowGuide>();
    }

    void CriarRotulosMapa()
    {
        GameObject rotulos = new GameObject("RotulosMapa");

        CriarRotulo("CORREDOR NORTE", new Vector2(0f, 7.7f), 0.09f, rotulos.transform);
        CriarRotulo("DIRETORIA", new Vector2(0f, 6.9f), 0.1f, rotulos.transform);
        CriarRotulo("BANHEIRO FEMININO", new Vector2(-9.3f, 3.2f), 0.075f, rotulos.transform);
        CriarRotulo("VESTIÁRIO", new Vector2(-9.4f, -1.9f), 0.08f, rotulos.transform);
        CriarRotulo("BIBLIOTECA", new Vector2(0f, 2.2f), 0.095f, rotulos.transform);
        CriarRotulo("SALA DE AULA", new Vector2(0f, -0.8f), 0.09f, rotulos.transform);
        CriarRotulo("SALA DOS PROFESSORES", new Vector2(0f, -3.8f), 0.07f, rotulos.transform);
        CriarRotulo("LABORATÓRIO", new Vector2(9.6f, 3.1f), 0.085f, rotulos.transform);
        CriarRotulo("GINÁSIO", new Vector2(9.6f, -3.3f), 0.1f, rotulos.transform);
        CriarRotulo("CORREDOR SUL", new Vector2(0f, -6.8f), 0.09f, rotulos.transform);
    }

    void CriarRotulo(string texto, Vector2 posicao, float tamanhoFonte, Transform parent)
    {
        GameObject rotulo = new GameObject($"Rotulo_{texto}");
        rotulo.transform.SetParent(parent);
        rotulo.transform.position = new Vector3(posicao.x, posicao.y, 0.4f);

        TextMesh textMesh = rotulo.AddComponent<TextMesh>();
        textMesh.text = texto;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.characterSize = tamanhoFonte;
        textMesh.fontSize = 34;
        textMesh.color = new Color(0.9f, 0.88f, 0.78f, 0.75f);
        textMesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        MeshRenderer renderer = rotulo.GetComponent<MeshRenderer>();
        renderer.sortingOrder = 1;
    }

    void CriarParede(string nome, Vector2 posicao, Vector2 tamanho, Transform parent)
    {
        GameObject parede = CriarRetangulo(nome, posicao, tamanho, materialParede, true);
        parede.transform.SetParent(parent);
    }

    void CriarParedeHorizontalComAbertura(string nomeBase, float xInicio, float xFim, float y, float centroAbertura, float larguraAbertura, Transform parent)
    {
        float esquerdaFim = centroAbertura - (larguraAbertura * 0.5f);
        float direitaInicio = centroAbertura + (larguraAbertura * 0.5f);

        float larguraEsquerda = Mathf.Max(0f, esquerdaFim - xInicio);
        float larguraDireita = Mathf.Max(0f, xFim - direitaInicio);

        if (larguraEsquerda > 0.01f)
            CriarParede($"{nomeBase}_Esq", new Vector2(xInicio + (larguraEsquerda * 0.5f), y), new Vector2(larguraEsquerda, 0.7f), parent);

        if (larguraDireita > 0.01f)
            CriarParede($"{nomeBase}_Dir", new Vector2(direitaInicio + (larguraDireita * 0.5f), y), new Vector2(larguraDireita, 0.7f), parent);
    }

    void CriarParedeVerticalComAbertura(string nomeBase, float x, float yInicio, float yFim, float centroAbertura, float alturaAbertura, Transform parent)
    {
        float baixoFim = centroAbertura - (alturaAbertura * 0.5f);
        float cimaInicio = centroAbertura + (alturaAbertura * 0.5f);

        float alturaBaixa = Mathf.Max(0f, baixoFim - yInicio);
        float alturaAlta = Mathf.Max(0f, yFim - cimaInicio);

        if (alturaBaixa > 0.01f)
            CriarParede($"{nomeBase}_Baixo", new Vector2(x, yInicio + (alturaBaixa * 0.5f)), new Vector2(0.7f, alturaBaixa), parent);

        if (alturaAlta > 0.01f)
            CriarParede($"{nomeBase}_Cima", new Vector2(x, cimaInicio + (alturaAlta * 0.5f)), new Vector2(0.7f, alturaAlta), parent);
    }

    GameObject CriarRetangulo(string nome, Vector2 posicao, Vector2 tamanho, Material material, bool comColisor)
    {
        GameObject retangulo = new GameObject(nome);

        SpriteRenderer renderer = retangulo.AddComponent<SpriteRenderer>();
        renderer.sprite = GetSquareSprite();
        renderer.material = material;

        retangulo.transform.position = new Vector3(posicao.x, posicao.y, 0f);
        retangulo.transform.localScale = new Vector3(tamanho.x, tamanho.y, 1f);

        if (comColisor)
        {
            BoxCollider2D collider = retangulo.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
        }

        return retangulo;
    }

    Material CriarMaterial(Color cor)
    {
        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
            shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");

        Material material = new Material(shader);
        material.color = cor;
        return material;
    }

    Sprite GetSquareSprite()
    {
        if (squareSprite != null)
            return squareSprite;

        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        squareSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        return squareSprite;
    }
}
