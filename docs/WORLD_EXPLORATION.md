# World exploration prototype

Branch: `world-exploration`, originally based on `main` at `615dc56`, rebased onto `04d0e53` on 2026-10-25. It now includes main's village health/destruction and world-space health bars. The checks below predate that integration; combined Play Mode behavior has not been retested.

## Try it in Unity

1. Open the project with Unity **6000.4.6f1**.
2. In the Project panel, open `Assets/Scenes/Prototype_SpawningAndPathfinding.unity`. There is one shared battlefield scene; no exploration scene copy was created.
3. In the Hierarchy, select `WorldExploration`. On its `World Exploration` component, leave **Exploration Enabled** checked. Choose **Player 1** or **Player 2** in **Team** before pressing Play.
4. Press Play, then click inside the Game view. You should see a third-person character beside the selected team's village, a blue marker for Player 1 or red marker for Player 2, and a controls panel.
5. Walk toward the soldiers. They should continue fighting each other while you move through them.

| Control | Action |
| --- | --- |
| WASD | Walk relative to the camera |
| Mouse | Look around |
| Left Shift + WASD | Run |
| Space | Jump |
| Escape | Release the cursor and pause player movement/look |
| Click inside Game view | Capture the cursor and resume controls |
| F5 | Toggle exploration and the existing camera; on some Mac keyboards hold Fn too |

F5 keeps the player's position when returning to exploration. Changing **Team** during Play Mode replaces the avatar at the other team's starting position. If the player falls below Y = -10, they respawn beside their selected village. Stopping Play Mode restores the saved scene and default team.

To start in the original camera view, uncheck **Exploration Enabled** before Play. The AI battle still runs; press F5 to enter exploration. Disabling the `World Exploration` component during Play also restores the original camera.

## Inspector references and tuning

The scene contains one instance of `Assets/Prefabs/Exploration/WorldExploration.prefab`. Its scene-specific references are:

| Field on `WorldExploration` | Assignment |
| --- | --- |
| Overview Camera | `Main Camera` → Camera |
| Player 1 Spawn Point | `SpawnPoint_Player1` → Transform |
| Player 2 Spawn Point | `SpawnPoint_Player2` → Transform |
| Player Prefab | `Assets/Prefabs/Exploration/ExplorationPlayer.prefab` |
| Exploration Camera | `WorldExploration/ExplorationCamera` → Camera |
| Follow Camera | `WorldExploration/ExplorationCamera` → Exploration Follow Camera |

**Spawn Offset** defaults to `(0, 0.05, 4)`, placing the avatar beside the soldier spawn lane. On `ExplorationCamera`, **Distance** and **Collision Radius** tune the follow camera. The player variant inherits movement, animation, input actions, and camera-target rotation from Starter Assets `PlayerArmature`. Its `Third Person Controller` retains Inspector speed/jump tuning; its ground mask is Default for this battlefield. The added `TeamMarker` sphere has no collider.

`ExplorationPlayer` stores team membership separately from AI tags. The avatar retains the built-in `Player` tag, has no `UnitController`, and is outside combat. It ignores collisions with nearby soldier colliders without changing the physics collision matrix, soldier prefabs, or ground/building collisions. The player variant uses the built-in Ignore Raycast layer so it cannot obstruct its own camera.

The imported follow-camera prefab uses an older rig. This prototype uses `ExplorationFollowCamera`, a small project-owned follower of the existing controller's camera target, with sphere casts against scenery. No imported camera, controller, input asset, package, or global setting was changed. The original scene camera is temporarily deactivated at runtime and restored when exploration is off.

## Verification — 2026-10-25

Tested the explicit prototype scene in Unity 6000.4.6f1 on macOS. Visually inspected the character, blue team marker, HUD, soldiers, and F5 camera switching. Temporary editor instrumentation then ran 29 checks over approximately 43 simulated seconds, including queued keyboard/mouse events through the existing Input System and `PlayerInput`:

- Correct Player 1 and Player 2 spawn, team metadata, blue/red materials, and replacement camera target on team change.
- Walk, faster sprint, jump and landing, mouse-driven camera rotation, and matching follow-camera rotation.
- Escape releases cursor/look and prevents walking; F5 restores the original camera and resumes exploration at the same position.
- One active audio listener; player excluded from soldier targeting tags/combat components.
- Each AI side spawned at least 10 soldiers, moved, lost health, and had damaged units removed while the explorer stood in the battle lane. Nearby soldier collision pairs were ignored; sampled soldiers remained on the NavMesh.
- Inspector mode toggle and component disable/re-enable restored the appropriate camera and avatar visibility.
- Zero captured runtime warnings/errors/exceptions. Stopping Play Mode returned to Edit Mode with zero soldiers.

Unity generated the prefabs and metadata and wired the scene through editor APIs. The scene diff adds only the new prefab instance, its three scene references, and its root entry. Temporary authoring/verification tools were removed.

Still untested: Windows, builds, gamepad, physical mouse feel over a longer session, click/focus behavior across all editor layouts, camera collision at every obstacle, and falling off the battlefield. The missing original battlefield material remains as documented in [project context](PROJECT_CONTEXT.md). Main now supplies updated AI targeting, village combat, and health bars; their interaction with exploration and camera switching needs a fresh Play Mode check. Player combat, a team-selection menu, and multiplayer remain outside this prototype.
