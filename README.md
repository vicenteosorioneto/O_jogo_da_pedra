# 🎮 O Segredo da Pedra - MVP

Um jogo de horror **2D top-down** desenvolvido em **Unity** baseado na lenda urbana de Maria Sangrenta.

---

## 📖 Sobre o Jogo

O jogador fica preso na escola durante a noite após participar de uma brincadeira envolvendo a lenda de Maria Sangrenta. Ele deve reunir **3 fragmentos do espelho original** e realizar um ritual para aprisionar a entidade e escapar da escola.

### 🎯 Objetivo
- Coleta os 3 fragmentos espalhados pela escola
- Volta ao banheiro (espelho) e completa o ritual de selamento
- Maria Sangrenta é aprisionada → **FINAL BOM ✅**

### ⚠️ Risco
- Se tentar sair sem completar o ritual → **FINAL RUIM ❌**
- Maria não será aprisionada e o perseguirá para sempre

---

## ⚡ Setup Rápido (1 Minuto)

1. **Abra o Unity** e crie uma cena vazia (File → New Scene → 2D)
2. **Delete Main Camera** (não precisa)
3. **Create Empty** (Right-click na Hierarchy)
4. **Attach SceneGenerator.cs** (Add Component → SceneGenerator)
5. **Pressione ▶️ PLAY**

Tudo é criado automaticamente! 🎉

---

## 🎮 Controles

| Ação | Tecla |
|------|-------|
| Mover | **WASD** |
| Correr | **Shift + WASD** |
| Interagir | **E** |
| Lanterna | **F** |
| Liberar mouse | **ESC** |
| Debug Info | **F1** |
| Forçar Final Bom | **F2** |
| Forçar Final Ruim | **F3** |

---

## 📦 Scripts Inclusos

| Script | Função |
|--------|--------|
| **PlayerController.cs** | Movimento 2D, sprint/stamina e interações |
| **GameManager.cs** | Hub central - gerencia fragmentos e finais |
| **FragmentCollector.cs** | Coleta os 3 fragmentos |
| **GhostAI.cs** | IA de Maria Sangrenta (perseguição + jumpscare) |
| **RitualManager.cs** | Controla o ritual de aprisionamento |
| **ExitManager.cs** | Gerencia a saída e determina final |
| **InputManager.cs** | Input global e debug |
| **SceneGenerator.cs** | **Gera a cena inteira automaticamente** ⭐ |
| **AtmosphereController.cs** | Ambiência dinâmica (hum, rangidos, heartbeat) |
| **SchoolScareDirector.cs** | Eventos de susto por área (escola) |
| **GameHUD.cs** | HUD com fragmentos, objetivo, stamina e feedback visual |
| **PlayerAudioFeedback.cs** | Passos e respiração dinâmica |
| **DocumentReader.cs** | Preparado para ler documentos (Fase 2) |

---

## 🎯 Fluxo do Jogo

```
INÍCIO
  ↓
Jogador spawna na escola
  ↓
Coleta 3 fragmentos (cubos vermelhos pequenos)
  ↓
Vai ao espelho (quadrado cinza)
  ↓
Interage com E para iniciar o ritual
  ↓
Maria Sangrenta aparece (jumpscare!)
  ↓
Espelho brilha (luz verde)
  ↓
Maria é aprisionada
  ↓
FINAL BOM ✅ - Você escapou!

---

OU (Caminho ruim):

Tenta sair sem pegar os fragmentos
  ↓
Consegue escapar da escola
  ↓
Mas no reflexo... Maria está lá
  ↓
FINAL RUIM ❌ - Ela o seguirá para sempre
```

---

## 👾 Elementos do MVP

✅ Sistema de movimento completo em 2D  
✅ Coleta de fragmentos com contador  
✅ Maria Sangrenta com IA (perseguição + jumpscare)  
✅ Aparições aleatórias (a cada 15 segundos)  
✅ Ritual de aprisionamento  
✅ 2 finais diferentes (bom e ruim)  
✅ Sistema de pausa automática ao finalizar  
✅ Debug tools (F1, F2, F3)  
✅ Interator system (interface IInteractable)  

---

## 📂 Estrutura de Pastas

```
Assets/
├── Scripts/
│   ├── PlayerController.cs
│   ├── GameManager.cs
│   ├── FragmentCollector.cs
│   ├── GhostAI.cs
│   ├── RitualManager.cs
│   ├── ExitManager.cs
│   ├── InputManager.cs
│   ├── SceneGenerator.cs
│   ├── DocumentReader.cs
│   ├── AtmosphereController.cs
│   ├── SchoolScareDirector.cs
│   ├── GameHUD.cs
│   ├── PlayerAudioFeedback.cs
│   └── IInteractable.cs
├── Prefabs/ (para futuro)
└── UI/ (para futuro)

Documentação:
├── README.md (este arquivo)
├── SETUP_INSTANTANEO.txt
├── README_MVP.txt
├── GUIA_CONFIGURACAO_MVP.txt
└── LISTAGEM_SCRIPTS.txt
```

---

## 🔧 Como Personalizar

### Mudar velocidade do player:
Abra `PlayerController.cs` e ajuste:
```csharp
[SerializeField] private float moveSpeed = 5f;      // Velocidade normal
[SerializeField] private float sprintSpeed = 8f;    // Velocidade sprint
```

### Mudar velocidade de Maria Sangrenta:
Abra `GhostAI.cs` e ajuste:
```csharp
[SerializeField] private float chaseSpeed = 6f;           // Velocidade de perseguição
[SerializeField] private float detectionRange = 20f;     // Distância de detecção
[SerializeField] private float jumpscareDistance = 2f;   // Distância do jumpscare
[SerializeField] private float appearanceInterval = 15f; // Aparição a cada X segundos
```

### Aumentar número de fragmentos:
Abra `GameManager.cs`:
```csharp
[SerializeField] private int fragmentsNeeded = 3; // Mude para 5, 10, etc
```

---

## 🐛 Solução de Problemas

### ❓ "O jogo não inicia"
→ Certifique-se de que a cena foi salva (File → Save Scene)  
→ Verifique o Console para erros (Ctrl+Shift+C)

### ❓ "Player não aparece"
→ Abra a cena gerada e procure por "Player" na Hierarchy  
→ Se não existir, rode o SceneGenerator novamente

### ❓ "Maria não persegue"
→ Verifique se Player tem tag "Player" (Tag dropdown no Inspector)  
→ Verifique se GhostAI está no mesmo nível hierárquico

### ❓ "Fragmentos não Some ao pegar"
→ Verifique o Console (deve mostrar "✓ Fragmento coletado!")  
→ Certifique-se que tem 3 fragmentos na cena

### ❓ "Ritual não funciona"
→ Coleta todos os 3 fragmentos primeiro  
→ Vá ao espelho e pressione E  
→ Veja o Console para debug

---

## 🎨 Próximas Fases (Roadmap)

### Fase 2 - Conteúdo
- [ ] Documentos para ler (jornal do desaparecimento, diário, ritual guide)
- [ ] Efeitos sonoros (footsteps, wind, scary sounds)
- [ ] Texturas de parede/chão
- [ ] Lighting aprimorado (sombras dinâmicas)
- [ ] Mais ambientes (biblioteca, corredor escuro, dormitório)

### Fase 3 - Gameplay
- [ ] Sistema de save/checkpoint
- [ ] Inventário visual
- [ ] Mais puzzles
- [ ] Múltiplos caminhos
- [ ] Inimigos adicionais

### Fase 4 - Polonimento
- [ ] Animações (Maria atacando, desaparecendo)
- [ ] Cinematics (intro, outros finais)
- [ ] Menu principal
- [ ] Settings (volume, dificuldade)
- [ ] Créditos

---

## 📊 Especificações Técnicas

- **Engine**: Unity 2022.3+ (qualquer versão recente)
- **Linguagem**: C# (scripts apenas)
- **Plataforma**: Windows (testado), Mac/Linux (compatível)
- **Requisitos**: Nenhum (só precisa do Unity)
- **Tamanho**: ~500KB (apenas scripts)
- **Performance**: Roda em qualquer PC moderno

---

## 📝 Notas de Desenvolvimento

- Todos os scripts têm comentários explicativos
- GameManager é um Singleton (existe apenas 1 instância)
- Interface IInteractable permite adicionar novos objetos interativos facilmente
- SceneGenerator usa Reflection para assignar campos automaticamente
- Sistema de fragmentos é escalável (pode mudar para mais/menos itens)

---

## 👤 Créditos

Criado como MVP para o projeto "O Segredo da Pedra"

---

## 📞 Dúvidas?

Verifique os arquivos de documentação:
- **SETUP_INSTANTANEO.txt** - Instruções rápidas
- **README_MVP.txt** - Informações do MVP
- **GUIA_CONFIGURACAO_MVP.txt** - Guia passo a passo
- **LISTAGEM_SCRIPTS.txt** - Referência técnica dos scripts

---

**Status**: ✅ MVP Completo e Funcional

**Última atualização**: 11 de Março de 2026

Divirta-se! 👻🎮
