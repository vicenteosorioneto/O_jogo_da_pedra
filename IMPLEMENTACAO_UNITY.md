# Guia de Implementação no Unity para "O Jogo da Pedra"

Este documento detalha os passos necessários para configurar o projeto Unity, integrar os scripts e assets fornecidos, e criar a estrutura básica de cenas para o jogo "O Jogo da Pedra" (Maria Sangrenta).

## 1. Estrutura de Pastas

Certifique-se de que seu projeto Unity tenha uma estrutura de pastas organizada. Os scripts e assets fornecidos foram criados com a seguinte estrutura em mente:

```
Assets/
├── Audio/
├── Materials/
├── Prefabs/
├── Scenes/
├── Scripts/
│   ├── AtmosphereController.cs
│   ├── DocumentReader.cs
│   ├── ExitManager.cs
│   ├── FragmentCollector.cs (Será substituído por MirrorFragment.cs)
│   ├── GameManager.cs
│   ├── GameHUD.cs
│   ├── GhostAI.cs
│   ├── IInteractable.cs
│   ├── InputManager.cs
│   ├── MirrorFragment.cs (NOVO)
│   ├── ObjectiveArrowGuide.cs
│   ├── PlayerAudioFeedback.cs
│   ├── PlayerController.cs
│   ├── RitualManager.cs
│   ├── SceneGenerator.cs
│   └── SchoolScareDirector.cs
└── Sprites/
    ├── Characters/
    │   ├── Player_Sprite.png
    │   └── Maria_Sangrenta_Sprite.png
    ├── Items/
    │   └── Mirror_Fragment.png
    └── UI/
        ├── Flashlight_Icon.png
        ├── Stamina_Bar_Frame.png
        └── Jumpscare_Overlay.png
```

**Ações:**
1.  Mova os scripts C# fornecidos para a pasta `Assets/Scripts/` do seu projeto Unity.
2.  Crie as subpastas `Characters`, `Items` e `UI` dentro de `Assets/Sprites/`.
3.  Importe os arquivos `.png` gerados para suas respectivas pastas em `Assets/Sprites/`.

## 2. Configuração de Sprites

Para cada arquivo de imagem `.png` importado na pasta `Assets/Sprites/`, siga estes passos para configurá-los como sprites 2D:

1.  Selecione o arquivo `.png` no painel Project do Unity.
2.  No Inspector, altere as seguintes propriedades:
    *   **Texture Type:** `Sprite (2D and UI)`
    *   **Sprite Mode:** `Single` (para sprites individuais) ou `Multiple` (se a imagem contiver múltiplos sprites em um atlas, o que não é o caso aqui).
    *   **Pixel Per Unit:** `16` (ou o valor que você usa para sua arte pixelada).
    *   **Filter Mode:** `Point (no filter)` (para manter a nitidez do pixel art).
    *   **Compression:** `None`.
3.  Clique em `Apply`.

## 3. Configuração do Player

1.  Crie um novo GameObject vazio na cena (clique com o botão direito na hierarquia -> `Create Empty`). Renomeie-o para `Player`.
2.  Adicione os seguintes componentes ao GameObject `Player`:
    *   **Sprite Renderer:** Arraste o `Player_Sprite.png` para o campo `Sprite`.
    *   **Rigidbody2D:**
        *   `Body Type`: `Dynamic`
        *   `Gravity Scale`: `0`
        *   `Collision Detection`: `Continuous`
        *   `Freeze Rotation Z`: `true`
    *   **Capsule Collider 2D:** Ajuste o `Size` e `Offset` para cobrir o sprite do jogador.
    *   **PlayerController.cs:** Anexe o script `PlayerController`.
    *   **PlayerAudioFeedback.cs:** Anexe o script `PlayerAudioFeedback` (se você tiver áudios configurados).

3.  **Configuração da Lanterna:** O script `PlayerController.cs` já cria e configura uma lanterna (`Light` component) automaticamente. Certifique-se de que a camada de iluminação esteja configurada corretamente para que a lanterna ilumine a cena.

## 4. Configuração da Maria Sangrenta (GhostAI)

1.  Crie um novo GameObject vazio na cena. Renomeie-o para `MariaSangrenta`.
2.  Adicione os seguintes componentes ao GameObject `MariaSangrenta`:
    *   **Sprite Renderer:** Arraste o `Maria_Sangrenta_Sprite.png` para o campo `Sprite`.
    *   **Rigidbody2D:**
        *   `Body Type`: `Dynamic`
        *   `Gravity Scale`: `0`
        *   `Collision Detection`: `Continuous`
        *   `Freeze Rotation Z`: `true`
    *   **Circle Collider 2D:** Marque `Is Trigger` como `true`. Ajuste o `Radius` para a área de detecção da entidade.
    *   **GhostAI.cs:** Anexe o script `GhostAI`.
        *   Arraste o GameObject `Player` para o campo `Player` no Inspector do `GhostAI`.
        *   Ajuste `Chase Speed`, `Detection Range`, `Jumpscare Distance` e `Appearance Interval` conforme desejado.

## 5. Configuração dos Fragmentos do Espelho

1.  Crie um GameObject vazio para cada fragmento do espelho na cena. Renomeie-os para `FragmentoEspelho_1`, `FragmentoEspelho_2`, `FragmentoEspelho_3`.
2.  Para cada fragmento, adicione os seguintes componentes:
    *   **Sprite Renderer:** Arraste o `Mirror_Fragment.png` para o campo `Sprite`.
    *   **Circle Collider 2D:** Marque `Is Trigger` como `true`. Ajuste o `Radius` para a área de interação.
    *   **MirrorFragment.cs:** Anexe o script `MirrorFragment`.
        *   Defina o `Fragment ID` para `1`, `2` e `3` respectivamente para cada fragmento.

## 6. Configuração dos Documentos

1.  Crie um GameObject vazio para cada documento na cena. Renomeie-os para `Documento_Jornal`, `Documento_Diario`, `Documento_Ritual`.
2.  Para cada documento, adicione os seguintes componentes:
    *   **Sprite Renderer:** (Opcional) Adicione um sprite visual para o documento, como um pedaço de papel.
    *   **Box Collider 2D:** Marque `Is Trigger` como `true`. Ajuste o `Size` e `Offset`.
    *   **DocumentReader.cs:** Anexe o script `DocumentReader`.
        *   Defina o `Document ID` para `1`, `2` e `3` respectivamente.
        *   Preencha `Document Title` e `Document Content` com os textos do seu GDD:
            *   **Documento 1 (Jornal):** Título: "Desaparecimento Misterioso na Escola", Conteúdo: "Notícias sobre o desaparecimento de Maria, a lenda local..."
            *   **Documento 2 (Diário):** Título: "Diário Antigo", Conteúdo: "Relatos sobre um espelho amaldiçoado que foi quebrado em 3 pedaços para conter uma entidade..."
            *   **Documento 3 (Ritual):** Título: "O Ritual de Selamento", Conteúdo: "Instruções detalhadas para aprisionar a Maria Sangrenta novamente usando os fragmentos do espelho e a luz..."

## 7. Configuração do Espelho do Ritual

1.  Crie um GameObject vazio na cena. Renomeie-o para `EspelhoRitual`.
2.  Posicione-o no banheiro feminino, onde o ritual ocorrerá.
3.  Adicione os seguintes componentes:
    *   **Box Collider 2D:** Marque `Is Trigger` como `true`. Ajuste o `Size` e `Offset` para a área de interação com o espelho.
    *   **RitualManager.cs:** Anexe o script `RitualManager`.
        *   Arraste o GameObject `EspelhoRitual` para o campo `Mirror Transform`.
        *   (Opcional) Adicione um componente `Light` ao `EspelhoRitual` e arraste-o para o campo `Ritual Light` no `RitualManager` para o efeito de brilho.

## 8. Configuração do Game Manager

1.  Crie um GameObject vazio na cena. Renomeie-o para `GameManager`.
2.  Anexe o script `GameManager.cs` a ele.
3.  Este GameObject será `DontDestroyOnLoad`, então ele persistirá entre as cenas.
    *   Certifique-se de que `Fragments Needed` esteja definido como `3` e `Documents Needed` como `3`.

## 9. Configuração da UI (HUD)

1.  Crie um Canvas na cena (clique com o botão direito na hierarquia -> `UI` -> `Canvas`). Renomeie-o para `GameHUD_Canvas`.
2.  No `GameHUD_Canvas`, adicione um GameObject vazio e renomeie-o para `GameHUD_Controller`.
3.  Anexe o script `GameHUD.cs` ao `GameHUD_Controller`.
4.  **Barra de Stamina:**
    *   Dentro do `GameHUD_Canvas`, crie um `Image` (clique com o botão direito -> `UI` -> `Image`). Renomeie-o para `StaminaBar_Background`.
    *   Defina a imagem `Stamina_Bar_Frame.png` como `Source Image`.
    *   Ajuste a posição e o tamanho.
    *   Dentro de `StaminaBar_Background`, crie outro `Image`. Renomeie-o para `StaminaBar_Fill`.
    *   Defina o `Image Type` como `Filled` e o `Fill Method` como `Horizontal`.
    *   Defina a cor para verde ou um tom que represente stamina.
    *   Arraste `StaminaBar_Fill` para o campo `Stamina Bar Fill` no `GameHUD_Controller`.
5.  **Painel de Jumpscare:**
    *   Dentro do `GameHUD_Canvas`, crie um `Panel` (clique com o botão direito -> `UI` -> `Panel`). Renomeie-o para `Jumpscare_Panel`.
    *   Defina a cor do painel para preto e o `Alpha` para `0` (transparente).
    *   Dentro de `Jumpscare_Panel`, crie um `Image`. Renomeie-o para `Jumpscare_Image`.
    *   Arraste o `Jumpscare_Overlay.png` para o campo `Source Image`.
    *   Ajuste o `Rect Transform` para preencher a tela (`stretch`).
    *   Arraste `Jumpscare_Panel` para o campo `Jumpscare Panel` e `Jumpscare_Image` para o campo `Jumpscare Image` no `GameHUD_Controller`.
    *   Desative o `Jumpscare_Panel` inicialmente no Inspector.

## 10. Configuração do Guia de Objetivos

1.  Crie um GameObject vazio na cena. Renomeie-o para `ObjectiveGuide`.
2.  Anexe o script `ObjectiveArrowGuide.cs` a ele.
3.  Certifique-se de que o `Player` e o `EspelhoRitual` estejam configurados corretamente, pois o script os encontrará automaticamente.

## 11. Configuração da Saída da Escola

1.  Crie um GameObject vazio na cena na localização da saída da escola. Renomeie-o para `ExitPoint`.
2.  Adicione um `Box Collider 2D` a ele e marque `Is Trigger` como `true`. Ajuste o tamanho para cobrir a área de saída.
3.  Anexe o script `ExitManager.cs` a ele.
    *   Certifique-se de que `Allow Exit` esteja marcado como `true`.

## 12. Configuração de Cenas e Tilemaps

Para criar o ambiente da escola, você pode usar Tilemaps no Unity. Como não posso criar os Tilemaps diretamente, forneço um guia:

1.  **Crie um Tilemap:**
    *   Na hierarquia, clique com o botão direito -> `2D Object` -> `Tilemap` -> `Rectangular`.
    *   Isso criará um GameObject `Grid` com um `Tilemap` e um `Tilemap Renderer` como filhos.
    *   Renomeie o `Tilemap` para `Ground_Tilemap`.
2.  **Crie Camadas de Tilemap:** Repita o passo 1 para criar mais Tilemaps para diferentes camadas (ex: `Walls_Tilemap`, `Details_Tilemap`, `Collision_Tilemap`).
3.  **Crie um Tile Palette:**
    *   Vá em `Window` -> `2D` -> `Tile Palette`.
    *   Clique em `Create New Palette` e salve-a.
    *   Arraste suas texturas de tiles (do tileset que você forneceu) para a janela `Tile Palette` para criar os Tiles.
4.  **Pinte a Cena:**
    *   Selecione o `Tilemap` que deseja editar (ex: `Ground_Tilemap`).
    *   Selecione um Tile na sua `Tile Palette`.
    *   Use as ferramentas de pintura (Brush, Fill, Erase) na janela `Tile Palette` para desenhar seu mapa da escola.
5.  **Camada de Colisão:** Para o `Collision_Tilemap`, você pode adicionar um `Tilemap Collider 2D` e um `Composite Collider 2D` para gerar colisões automaticamente com as paredes e objetos do cenário.

## Próximos Passos

Após seguir este guia, seu projeto Unity terá a estrutura básica e os scripts configurados. Você precisará:

*   Desenhar as cenas da escola usando Tilemaps.
*   Posicionar os fragmentos do espelho, documentos, o espelho do ritual e o ponto de saída na cena.
*   Testar o jogo para garantir que todas as interações e a lógica dos finais funcionem conforme o GDD.
*   Adicionar efeitos sonoros e música para imersão.

Com este guia, você terá uma base sólida para continuar o desenvolvimento do seu jogo. Boa sorte!
