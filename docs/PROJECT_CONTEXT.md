# Medieval Mayhem — project context

Source inspection and Play Mode verification: 2026-10-25. The recovered tutorial baseline spawns, moves, and fights in the tested macOS editor; future milestones are not implemented or verified by that result.

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
- Both spawners have a recovered `UnitSpawner` component, the corresponding spawn point and soldier prefab, an opposing village target, and `spawnInterval: 3`. References were checked in both Inspectors; both sides spawned in Play Mode.
- Ground serializes a `Unity.AI.Navigation.NavMeshSurface` referencing the recovered `Assets/Scenes/Prototype_SpawningAndPathfinding/NavMesh-Ground.asset`. Sampled soldiers stayed on the NavMesh during the verification run.
- Recovered gameplay source: `Assets/Scripts/Spawning/UnitSpawner.cs` and `Assets/Scripts/Units/UnitController.cs`. Soldier prefabs: `Assets/Prefabs/Units/Basic_Soldier_Player1.prefab` and `Basic_Soldier_Player2.prefab`, tagged `Player1` and `Player2` respectively.
- `ProjectSettings/TagManager.asset` defines `Player1` and `Player2` tags. Village objects themselves are saved as `Untagged`.
- `Assets/Scenes/SampleScene.unity` also exists. `ProjectSettings/EditorBuildSettings.asset` enables only SampleScene, not the prototype.

## Recovered source and remaining limits

The initial checkout was missing the tutorial gameplay files. Recovery branch `share-missing-unity-assets`, commit `390d43f`, supplied the original scripts, soldier prefabs, and baked navigation data with their metadata. The four previously unresolved references now each resolve to one local asset:

| Recovered reference | GUID present in local asset metadata |
| --- | --- |
| UnitSpawner script | `b8271d51702ded845b74269634971216` |
| Player 1 unit prefab | `ea89e51dc545d334ebabac564ca564be` |
| Player 2 unit prefab | `f2bc4210a269a0d4e909f08f21c5ee1c` |
| Baked NavMesh data | `121e6e81f7ae6a04cb1417ecefac2f20` |

Spawning, movement, unit health loss, and removal after damage were observed in Play Mode as recorded below. The current `UnitController` revision bounds enemy searches and chases to `enemyChaseRange` (default 8), resumes the village destination after combat, and gives the village priority within `villagePriorityRange` (default 8). It attacks an opposing `VillageHealth` objective when within `attackRange` and skips friendly villages by comparing `ownerTag` with the soldier's tag. These recent changes have not been verified in Unity Play Mode.

Village health/destruction, purchases, gold income, building upgrades, and match completion were not implemented in the recovered baseline. A `VillageHealth` component is now attached to both prototype villages; each has 500 starting health and a damage method that destroys the village at zero. Soldier attacks are connected to that component in source. The new village combat behavior has not been opened or verified in Unity yet. Multiplayer is not implemented in the inspected project: Multiplayer Center is declared, but that tooling package is not a playable network session or player ownership system.

One asset dependency remains unresolved in source: material GUID `31321ba15b8f8eb4c954353edc038b1d`, referenced by ground, villages, and both soldier prefabs, has no matching metadata under `Assets/`. The scene renders and gameplay ran without warnings/errors, but its original material/appearance is not verified. Recover that material and any dependencies from the working computer; do not invent a replacement GUID.

## Collaboration and repository cautions

No existing AGENTS.md or project Markdown guidance was found in the inspected project, or AGENTS.md in its ancestor directories. These documents establish the initial working context.

At initial inspection on 2026-10-25, Git showed untracked generated/IDE files and prototype scene metadata. Repository maintenance resolved these issues the same day: commit `0fcfc32` preserved `Assets/Scenes/Prototype_SpawningAndPathfinding.unity.meta`, and commit `1ba394f` converted `.gitignore` from UTF-16 to UTF-8 while preserving its rules. Git ignore checks confirmed the generated Unity/IDE files are now ignored. These changes were pushed to `origin/main`; they do not verify gameplay.

Coordinate scene and prefab editing so only one collaborator edits a given asset at a time. Pull before starting with a clean working tree, and fetch and integrate remote changes before pushing. Preserve both collaborators' work when resolving conflicts.

## Reopening and proving the baseline

1. Open this project folder in Unity Hub with editor 6000.4.6f1.
2. Open `Assets/Scenes/Prototype_SpawningAndPathfinding.unity` explicitly; do not assume the default/build scene is the prototype.
3. Inspect Console and both spawners. `Player1Spawner` should reference `Basic_Soldier_Player1`, `SpawnPoint_Player1`, and `Village_Player2`; `Player2Spawner` should reference `Basic_Soldier_Player2`, `SpawnPoint_Player2`, and `Village_Player1`. Both intervals are 3 seconds. Ground's `NavMesh Surface` should reference `NavMesh-Ground`.
4. Enter Play Mode: both sides should spawn capsule soldiers, move toward the battlefield, and exchange damage when they meet. Stop Play Mode to reset. Recheck on each collaborator's machine; Windows and builds have not been tested here.

## Initial Play Mode baseline check — 2026-10-25, before recovery

Result: failed baseline acceptance; the scene opens and enters Play Mode but does not spawn units.

- Tested `Assets/Scenes/Prototype_SpawningAndPathfinding.unity` in Unity 6000.4.6f1 on macOS.
- Selected `Player1Spawner` and `Player2Spawner`: both Inspectors show `Missing (Mono Script)` for `Assembly-CSharp::UnitSpawner`.
- Selected `Ground`: its `NavMesh Surface` component shows `Missing (Nav Mesh Data)`.
- Entered Play Mode and observed for more than 12 seconds, exceeding several saved three-second spawn intervals. Neither side added unit objects to the Hierarchy; Game view remained ground and sky with no units observed.
- Console showed two warnings, both `The referenced script (Unknown) on this Behaviour is missing!`, and zero errors. Entering Play Mode was not blocked by a compilation error; no separate clean compilation or build was run.
- Stopped Play Mode and confirmed the editor returned to edit mode. No scene, prefab, script, or navigation changes were made.

At that time, source checks confirmed the four referenced GUIDs had no matching metadata under `Assets/`, and no `UnitSpawner` or `UnitController` source files were found there. Movement and combat could not be exercised because no units spawned. This failure is superseded by the recovery check below.

## Recovery Play Mode check — 2026-10-25

- Opened Unity 6000.4.6f1 on macOS after importing the recovered assets. Both spawners' Inspector assignments resolved; soldiers from both sides appeared in the scene.
- Ran a second Play Mode session with temporary editor instrumentation for 35 simulated seconds. It observed the original gameplay without changing its scripts, prefab values, or scene wiring. The temporary code enabled background execution for that session and was removed with its generated metadata after the check.
- Initial soldiers: 0. Player 1: 11 spawned, 11 moved more than 0.5 units from their first sampled position, 10 lost health, and 8 disappeared after observed health loss. Player 2: the same counts. The recovered controller destroys soldiers at zero health.
- Two spawners had valid prefab, spawn point, and target references. Missing scene scripts: 0. Samples of soldiers without an active NavMesh placement: 0. Runtime warnings/errors/exceptions captured: 0; Console also showed zero warnings/errors.
- Stopped Play Mode and verified return to Edit Mode with 0 soldiers. Removed the temporary instrumentation and refreshed the editor; no temporary test assets remain.
- The scene metadata conflict was resolved by retaining the working computer's original scene GUID `15da031916ef0a544a75dafdd9524ade`. No source references to the Mac-generated scene GUID `4a2b7ea8831304c7598c6b8dcf692622` were found under `Assets/`, `ProjectSettings/`, or `Packages/` before replacement. No duplicate GUIDs were found under `Assets/` after recovery.

The tutorial spawning/movement/combat baseline is observed working on this Mac. Original material appearance, an unobstructed soldier reaching a village without defenders, bounded chasing and resuming the village objective, village damage, friendly-fire edge cases, Windows behavior, and builds remain untested or unimplemented. The missing material is still a recovery follow-up; milestone 1 acceptance has not been met.
