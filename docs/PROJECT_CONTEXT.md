# Medieval Mayhem — project context

Source inspection and Play Mode verification: 2026-10-26. The recovered tutorial baseline spawns, moves, and fights in the tested macOS editor; future milestones are not implemented or verified by that result.

## Game and collaborators

Made by a parent and son, with beginner-friendly, incremental Unity learning as a priority. The intended game supports 2–4 players, each owning a village. Players spend gold on units with different costs and stats and send them against opposing villages. Upgradeable village buildings generate income, creating a choice between economic investment and immediate military strength.

Third-person exploration is now a tested local prototype on `world-exploration`; it does not settle the final player-control or multiplayer format. Unit roster, balance values, and final win rules remain open.

## Verified environment

| Item | Evidence |
| --- | --- |
| Unity 6000.4.6f1 | `ProjectSettings/ProjectVersion.txt` |
| URP 17.4.0 | `Packages/manifest.json`; Graphics settings reference `Assets/Settings/PC_RPAsset.asset` |
| AI Navigation 2.0.12 | `Packages/manifest.json` |
| Input System 1.19.0; Cinemachine 3.1.6 | `Packages/manifest.json` |
| Unity Test Framework 1.6.0 | `Packages/manifest.json`; nine gold-rule Edit Mode cases under `Assets/Tests/EditMode/` |
| Starter Assets ThirdPerson / URP content present | `Assets/StarterAssets/ThirdPersonController/`, `Environment/`, `InputSystem/`, `Mobile/` |

## What is actually in this checkout

- `Assets/Scenes/Prototype_SpawningAndPathfinding.unity`: two village objects (`Village_Player1`, `Village_Player2`), two spawn points (`SpawnPoint_Player1`, `SpawnPoint_Player2`), `Player1Spawner`, `Player2Spawner`, ground, camera, and light.
- Both spawners have a recovered `UnitSpawner` component, the corresponding spawn point and soldier prefab, an opposing village target, and `spawnInterval: 3`. References were checked in both Inspectors; both sides spawned in Play Mode.
- Ground serializes a `Unity.AI.Navigation.NavMeshSurface` referencing the recovered `Assets/Scenes/Prototype_SpawningAndPathfinding/NavMesh-Ground.asset`. Sampled soldiers stayed on the NavMesh during the verification run.
- Recovered gameplay source: `Assets/Scripts/Spawning/UnitSpawner.cs` and `Assets/Scripts/Units/UnitController.cs`. Soldier prefabs: `Assets/Prefabs/Units/Basic_Soldier_Player1.prefab` and `Basic_Soldier_Player2.prefab`, tagged `Player1` and `Player2` respectively.
- `ProjectSettings/TagManager.asset` defines `Player1` and `Player2` tags. Village objects themselves are saved as `Untagged`.
- `Assets/Scenes/SampleScene.unity` also exists. `ProjectSettings/EditorBuildSettings.asset` enables SampleScene and the prototype; victory Restart reloads the active scene.
- The prototype now has three `Box_100x100x100_Prefab` obstacles in the center lane (`MapObstacle_Crate_Left`, `MapObstacle_Crate_Center`, and `MapObstacle_Crate_Right`). Each has a box `NavMeshObstacle` with carving enabled; Play Mode routing around them still needs to be checked.
- The same prototype scene contains a `WorldExploration` prefab instance. It adds a team-owned third-person observer and separate follow camera, with F5 switching back to the original camera. The player is a variant of Starter Assets `PlayerArmature`, retains the `Player` tag, and is not a soldier combat target. See [controls, assignments, and verification](WORLD_EXPLORATION.md).

## Recovered source and remaining limits

The initial checkout was missing the tutorial gameplay files. Recovery branch `share-missing-unity-assets`, commit `390d43f`, supplied the original scripts, soldier prefabs, and baked navigation data with their metadata. The four previously unresolved references now each resolve to one local asset:

| Recovered reference | GUID present in local asset metadata |
| --- | --- |
| UnitSpawner script | `b8271d51702ded845b74269634971216` |
| Player 1 unit prefab | `ea89e51dc545d334ebabac564ca564be` |
| Player 2 unit prefab | `f2bc4210a269a0d4e909f08f21c5ee1c` |
| Baked NavMesh data | `121e6e81f7ae6a04cb1417ecefac2f20` |

Spawning, movement, unit health loss, and removal after damage were observed in Play Mode as recorded below. The current `UnitController` revision bounds enemy searches and chases to `enemyChaseRange` (default 8), resumes the village destination after combat, gives the village priority within `villagePriorityRange` (default 8), and sets `NavMeshAgent.speed` from Inspector-editable `movementSpeed` (default 5.335) with auto-braking disabled. It attacks an opposing `VillageHealth` objective within `villageAttackRange` (default 1.5) of the village collider surface and skips friendly villages by comparing `ownerTag` with the soldier's tag. Basic unit combat and unobstructed village damage were observed in the combined Play Mode check below; the new movement speed and chase/reacquisition edge cases remain unverified.

Village health/destruction, purchases, gold income, building upgrades, and match completion were not implemented in the recovered baseline. A `VillageHealth` component is now attached to both prototype villages; each has 500 starting health and a damage method that destroys the village at zero. Soldier attacks are connected to that component in source. Soldiers and villages now create world-space health bars that follow them and update when damaged. Both sides damaged opposing villages in controlled Play Mode checks, and health bars switched between the exploration and overview cameras. A runtime `VictorySystem` now displays the winner and a Restart button when a village reaches zero health, and stops active soldiers and spawners; the prototype scene was added to Build Settings so Restart can reload it. The combined purchasing/victory check below verifies both winners, match stopping, and restart on macOS. Multiplayer is not implemented in the inspected project: Multiplayer Center is declared, but that tooling package is not a playable network session or player ownership system.

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

## World exploration check — 2026-10-25

On branch `world-exploration`, the shared scene passed 29 temporary runtime checks over approximately 43 simulated seconds in Unity 6000.4.6f1 on macOS. These covered both team spawns/colours, keyboard walking/sprinting/jumping/landing, mouse-input camera rotation, cursor release, F5 and Inspector camera toggles, component disable/re-enable, soldier collision exclusion, and existing spawning/movement/damage/removal for both AI sides. Zero runtime warnings/errors were captured; stopping Play Mode left zero soldiers. The imported assets, existing battlefield objects, AI scripts, physics settings, and packages were not changed. Temporary tools were removed; see [the handoff](WORLD_EXPLORATION.md#verification--2026-10-25) for the method and remaining limits.

## Integration with latest main — 2026-10-25

Rebased `world-exploration` onto main `04d0e53`, which includes village health/destruction, revised soldier objective combat, and world-space health bars. Preserved the uncommitted exploration scripts/prefabs and restored the shared scene by retaining both the upstream village components and the exploration prefab instance. The initial exploration Play Mode evidence above predates these incoming changes. A later combined check is recorded below; post-destruction behavior remains unverified.

## Combined exploration/village verification — 2026-10-25

In Unity 6000.4.6f1 on macOS, a 38-second smoke run confirmed both teams' exploration spawns/movement, AI spawning/unit damage, NavMesh placement, and active-camera health-bar switching. Neither village lost health in that balanced battle. A subsequent 46-second run passed 16 checks, including separate unobstructed attacks by each team's soldier after temporarily stopping spawners/removing defenders in Play Mode. Each soldier reached and damaged the opposing village within 12 seconds. No runtime warnings/errors were captured, and stopping Play Mode restored zero soldiers and the saved scene. Temporary test assets were removed. `WorldHealthBar` now reacquires the active camera when its previous camera is inactive, including its Canvas camera reference. See [the integration notes](WORLD_EXPLORATION.md#combined-integration-check--2026-10-25). Full village-destruction/match-end acceptance and Windows/build checks remain open.

## Unit purchasing verification — 2026-10-26

On `unit-purchasing`, based on main `900f365`, the shared prototype now has a `UnitPurchasing` prefab instance with `UnitPurchaseMenu`. B opens the shop anywhere; each team starts with 100 gold and buys its own Basic Soldier for 25 at its existing spawn point. Both teams retain free automatic spawning with unchanged timers. Shopping releases the cursor and blocks character movement/look while the battle continues. Restarting Play Mode resets both budgets. There is no income or persistence.

Unity 6000.4.6f1 on macOS compiled the change and passed all nine gold-rule Edit Mode cases. A temporary Play Mode probe passed 49 checks over 32.3 simulated seconds, covering both teams' paid spawns, navigation/objectives, balance isolation, exact-cost spending, rejection without charging, automatic timers, input blocking/restoration, camera switching, ongoing automatic spawning and unit combat. It captured zero runtime warnings/errors. A separate manual run verified the visible Buy and Close buttons, 100-to-75 gold spending, continued shop cursor ownership, and restart resetting both budgets to 100. Additional runtime probes confirmed destroyed own/opposing villages reject purchases without charge. Inspector references were checked; Console remained clear and Play Mode was stopped. Temporary authoring/verification scripts were removed, keeping the reusable wallet tests.

See [setup, controls, and repeatable acceptance checks](UNIT_PURCHASING.md). This verifies the local purchase prototype; Windows, builds, networking, income, and full match-end behavior remain untested or outside this change. The pre-existing missing shared material remains unresolved.

## Purchasing and victory integration — 2026-10-26

Integrated incoming main `03f8156` without replacing its victory, village, unit, or build-settings changes. Kept both spawner paths and all decision records; renumbered the upstream victory decision to D014 to avoid duplicating purchasing D009. Purchases now reject ended matches, the shop closes/hides on victory, and exploration leaves the cursor free and player input blocked for Restart.

A temporary combined Play Mode probe checked both teams buying before victory, both automatic spawners, NavMesh placement, village defeat and correct winner, closed/blocked shop and paid spawns after defeat, stable soldier count and health with no navigation paths/movement after defeat, and blocked walking/B/click recapture. Native mouse clicks on each winner's Restart button reloaded the prototype, restored both villages to 500 health and both budgets to 100, and reset victory state. The first stop assertion incorrectly expected NavMeshAgent.isStopped to remain true after ResetPath; the final check measures empty paths, zero velocity, stable counts, and stable health directly. Final result: 31 checks passed, zero failures and zero runtime warnings/errors; temporary probes were removed and the editor returned to Edit Mode. See the purchasing handoff for details. Full natural-round balance and remaining targeting edge cases are still separate; Windows/builds remain untested.
