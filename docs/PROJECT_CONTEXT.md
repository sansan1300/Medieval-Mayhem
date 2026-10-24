# Medieval Mayhem — project context

Source inspection: 2026-10-25. This is a living snapshot, not a claim that the game has passed Play Mode testing.

## Game and collaborators

Made by a parent and son, with beginner-friendly, incremental Unity learning as a priority. The intended game supports 2–4 players, each owning a village. Players spend gold on units with different costs and stats and send them against opposing villages. Upgradeable village buildings generate income, creating a choice between economic investment and immediate military strength.

The player-control style, local/online multiplayer format, unit roster, balance values, and final win rules remain open. Starter Assets being installed does not decide the final control scheme.

## Verified environment

| Item | Evidence |
| --- | --- |
| Unity 6000.4.6f1 | `ProjectSettings/ProjectVersion.txt` |
| URP 17.4.0 | `Packages/manifest.json`; Graphics settings reference `Assets/Settings/PC_RPAsset.asset` |
| AI Navigation 2.0.12 | `Packages/manifest.json` |
| Input System 1.19.0; Cinemachine 3.1.6 | `Packages/manifest.json` |
| Unity Test Framework 1.6.0 | `Packages/manifest.json`; no project-owned test suite found under `Assets/` |
| Starter Assets ThirdPerson / URP content present | `Assets/StarterAssets/ThirdPersonController/`, `Environment/`, `InputSystem/`, `Mobile/` |

## What is actually in this checkout

- `Assets/Scenes/Prototype_SpawningAndPathfinding.unity`: two village objects (`Village_Player1`, `Village_Player2`), two spawn points (`SpawnPoint_Player1`, `SpawnPoint_Player2`), `Player1Spawner`, `Player2Spawner`, ground, camera, and light.
- Both spawners serialize a `UnitSpawner` component, a spawn point, an opposing village target, a prefab reference, and `spawnInterval: 3`. These are saved fields; executable spawning is not verified.
- Ground serializes a `Unity.AI.Navigation.NavMeshSurface` and a baked NavMesh data reference. The referenced data asset could not be found under `Assets/`.
- `ProjectSettings/TagManager.asset` defines `Player1` and `Player2` tags. Village objects themselves are saved as `Untagged`.
- `Assets/Scenes/SampleScene.unity` also exists. `ProjectSettings/EditorBuildSettings.asset` enables only SampleScene, not the prototype.

## Tutorial report versus source evidence

The tutorial describes NavMesh soldier prefabs, unit spawners, and `UnitController` with health, enemy detection, and automatic combat. This checkout does **not** contain project-owned `UnitSpawner.cs`, `UnitController.cs`, or soldier prefabs, including when ignored files under `Assets/` are included in the search. Present C# files belong to Starter Assets or template tutorial support.

The prototype has unresolved asset references:

| Reference | GUID absent from local `Assets/**/*.meta` |
| --- | --- |
| UnitSpawner script | `b8271d51702ded845b74269634971216` |
| Player 1 unit prefab | `ea89e51dc545d334ebabac564ca564be` |
| Player 2 unit prefab | `f2bc4210a269a0d4e909f08f21c5ee1c` |
| Baked NavMesh data | `121e6e81f7ae6a04cb1417ecefac2f20` |

Treat spawning, movement, unit health, and auto-combat as tutorial-reported and requiring recovery/verification. Village health/destruction, purchases, gold income, building upgrades, and match completion were not found implemented. Multiplayer is not implemented in the inspected project: Multiplayer Center is declared, but that tooling package is not a playable network session or player ownership system.

## Collaboration and repository cautions

No existing AGENTS.md or project Markdown guidance was found in the inspected project, or AGENTS.md in its ancestor directories. These documents establish the initial working context.

At inspection, Git had no tracked modifications, but showed untracked generated/IDE folders and files, plus `Assets/Scenes/Prototype_SpawningAndPathfinding.unity.meta`. Preserve that scene metadata. `.gitignore` is UTF-16 little-endian with CRLF, and intended ignore rules are not taking effect for these generated files. A separate maintenance change can convert it to UTF-8 after preserving its rules; this documentation task does not modify it or clean/stage anything.

## Reopening and proving the baseline

1. Open this project folder in Unity Hub with editor 6000.4.6f1.
2. Open `Assets/Scenes/Prototype_SpawningAndPathfinding.unity` explicitly; do not assume the default/build scene is the prototype.
3. Inspect Console and both spawners for missing scripts/prefabs. Recover original scripts, prefabs, and navigation data together with their `.meta` files before testing or extending behavior. If recovery is impossible, agree on a small reconstruction task.
4. Once repaired, test both sides spawning and moving to the opposing village, then unit combat. Record observed results here.

Only filesystem, package/settings, scene serialization, and Git checks were performed for this snapshot. Unity was not opened; compilation, navigation, combat, and builds were not tested.
