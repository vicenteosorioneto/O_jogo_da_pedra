╔═══════════════════════════════════════════════════════════════════╗
║                 O SEGREDO DA PEDRA - MVP 2D                      ║
╚═══════════════════════════════════════════════════════════════════╝

✅ STATUS: MVP FUNCIONAL (BASE 2D)

Direção atual do projeto:
- Jogo 2D top-down
- Cenário: escola abandonada
- Entidade: Maria Sangrenta
- Loop principal: fragmentos → ritual → finais

═══════════════════════════════════════════════════════════════════

📦 O QUE ESTÁ IMPLEMENTADO

✓ Movimento 2D com sprint e stamina
✓ Interação por tecla E (objetos interativos)
✓ Lanterna no F
✓ Coleta de 3 fragmentos
✓ Ritual final no espelho
✓ Final bom e final ruim
✓ HUD com objetivo e barra de stamina
✓ IA da entidade com perseguição/jumpscare
✓ Atmosfera sonora dinâmica
✓ Eventos de susto por área da escola

═══════════════════════════════════════════════════════════════════

📂 SCRIPTS PRINCIPAIS

Assets/Scripts/
├── PlayerController.cs
├── GameManager.cs
├── InputManager.cs
├── GameHUD.cs
├── GhostAI.cs
├── FragmentCollector.cs
├── RitualManager.cs
├── ExitManager.cs
├── DocumentReader.cs
├── AtmosphereController.cs
├── SchoolScareDirector.cs
├── PlayerAudioFeedback.cs
├── SceneGenerator.cs
└── IInteractable.cs

═══════════════════════════════════════════════════════════════════

🎮 CONTROLES

WASD            → Mover
Shift + WASD    → Correr
E               → Interagir
F               → Lanterna
ESC             → Liberar/Prender cursor
F1              → Debug info
F2              → Forçar final bom
F3              → Forçar final ruim

═══════════════════════════════════════════════════════════════════

🚀 SETUP RÁPIDO

1. Abra o Unity e use uma cena 2D vazia.
2. Crie um GameObject vazio.
3. Adicione SceneGenerator.cs.
4. Pressione PLAY.

O SceneGenerator monta automaticamente a escola, player, inimiga,
fragmentos, espelho, saída, HUD e sistemas de susto/atmosfera.

═══════════════════════════════════════════════════════════════════

🧭 PRÓXIMOS PASSOS SUGERIDOS

- Refinar arte 2D (sprites/tiles da escola)
- Adicionar documentos narrativos jogáveis
- Integrar áudio real (SFX/música)
- Criar menu inicial e tela de créditos
