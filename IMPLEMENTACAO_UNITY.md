# IMPLEMENTAÇÃO UNITY (MVP 2D)

## Estado atual
- Projeto em Unity com gameplay 2D top-down.
- Cenário principal: escola abandonada.
- Loop principal: coletar 3 fragmentos -> ritual no banheiro feminino -> saída no corredor sul.

## Scripts principais
- Assets/Scripts/SceneGenerator.cs
- Assets/Scripts/PlayerController.cs
- Assets/Scripts/GameManager.cs
- Assets/Scripts/GhostAI.cs
- Assets/Scripts/GameHUD.cs
- Assets/Scripts/ObjectiveArrowGuide.cs
- Assets/Scripts/SchoolScareDirector.cs
- Assets/Scripts/AtmosphereController.cs

## Geração automática da cena
- O SceneGenerator cria:
  - Mapa da escola 2D
  - Jogador
  - Fragmentos
  - Espelho do ritual
  - Saída
  - Maria Sangrenta
  - Atmosfera e sustos
  - Guia de setas de objetivo

## Jogabilidade (resumo)
1. Seguir as setas no chão para coletar os 3 fragmentos.
2. Após coletar tudo, seguir as setas até o espelho do ritual.
3. Interagir com E para completar ritual.
4. Seguir para a saída.

## Controles
- WASD: mover
- Shift: correr
- E: interagir
- F: lanterna
- ESC: alternar cursor
- F1/F2/F3: debug

## Como testar rápido
1. Abrir uma cena vazia 2D.
2. Criar um Empty GameObject.
3. Adicionar SceneGenerator.
4. Dar Play.

## Observações
- Arquivo criado para centralizar a implementação atual.
- Caso queira, este documento pode virar checklist de release (arte, áudio, menu, build).