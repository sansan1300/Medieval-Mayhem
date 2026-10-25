# Unit purchasing prototype

On branch `unit-purchasing`, based on main `900f365`. Uses the existing `Assets/Scenes/Prototype_SpawningAndPathfinding.unity`; there is no duplicate battlefield. Decisions: [D009-D013](DECISIONS.md#d009--open-the-unit-purchase-menu-with-b).

## Try one purchase

1. Open the project with Unity **6000.4.6f1** and open `Assets/Scenes/Prototype_SpawningAndPathfinding.unity` in the Project panel.
2. Enter Play Mode, click the Game view, and press **B**. The Reinforcements panel should show your selected team and **100 gold**. The cursor is free; walking and camera look stop while soldiers continue spawning, moving, and fighting.
3. Click **Buy Basic Soldier**. Your balance becomes **75**, and one extra soldier appears at your team's spawn point and targets the opposing village. The shop remains open.
4. Click **Close**, or press **B / Esc**, to return to character controls. **F5** still switches between exploration and the original overview camera, including while shopping.

There is no proximity requirement. Both teams spawn free reinforcements every three seconds. Purchases do not reset or delay that timer. There is no income, saving, or networking; stop and restart Play Mode to reset both balances.

## Inspector assignments and tuning

Select scene root **UnitPurchasing** and its **Unit Purchase Menu** component. Its source prefab is `Assets/Prefabs/Purchasing/UnitPurchasing.prefab`.

| Field | Assignment |
| --- | --- |
| Starting Gold | 100 for each team, applied when Play Mode starts |
| Soldier Cost | 25 per Basic Soldier |
| Exploration | `WorldExploration` component on root `WorldExploration` |
| Player 1 Spawner | `UnitSpawner` on `Player1Spawner` |
| Player 2 Spawner | `UnitSpawner` on `Player2Spawner` |
| Player 1 Village | `VillageHealth` on `Village_Player1` |
| Player 2 Village | `VillageHealth` on `Village_Player2` |

The prefab stores tuning; the scene instance stores battlefield references. Its references are intentionally empty when viewed as a standalone prefab. Existing spawners supply the soldier prefabs, spawn transforms, and opposing village objectives.

To try the other side, select **WorldExploration → World Exploration → Team → Player2**. This can be done during Play Mode: the avatar respawns on that side and the shop switches to that team's independent balance. This Inspector switch is a test aid, not a multiplayer player-selection interface. Runtime Inspector changes reset on stopping Play Mode.

## Acceptance checks

- Make four purchases: 100 → 75 → 50 → 25 → 0. Each adds exactly one own-team soldier. Buy then becomes disabled and explains insufficient gold; repeated attempts cannot create soldiers or negative gold.
- Switch to Player2: it still has 100. Buy there: Player2 becomes 75 while Player1 remains 0. Each paid soldier uses its team's spawn point and opposing village objective.
- Keep the shop open for more than three seconds: both automatic spawners and the battle continue without charging either wallet. Hold movement and move the mouse: the character and camera remain still. Close and verify walking/look resume.
- Try B, Esc, the Close button, and F5 camera switching. Clicking Buy must keep the shop open with a free cursor.
- An unavailable/destroyed own or opposing village, invalid soldier assignment, missing spawn point, or ground outside the soldier's NavMesh must reject a purchase without spending gold. Repair scene references before testing automatic spawning: its existing free spawn path assumes valid scene wiring.
- Stop/restart Play Mode: both budgets return to 100, the saved villages return, and no runtime soldiers remain after stopping. Check Console throughout.

## Verification — 2026-10-26

Unity 6000.4.6f1 on macOS compiled the change. All **9 Edit Mode gold-rule cases** passed: exact funds, insufficient funds, repeated spending, invalid costs, separate wallets, invalid initial balance, and affordability without spending. Run them from **Window → General → Test Runner → EditMode**, selecting `MedievalMayhem.Economy.Tests`.

A temporary editor/runtime probe passed **49 checks** across **32.3 simulated seconds** using the production purchase method and queued Input System keyboard/mouse events. It checked scene references, both wallets and soldier placement/team/objective/navigation, unchanged automatic timers, failed purchase paths, movement/look blocking, restoration after closing, team/camera switches, disabling the shop, continued automatic spawning, surviving soldiers on the NavMesh, and existing unit damage. **Zero runtime warnings/errors** were captured.

A second Play Mode run confirmed both balances reset to 100. Native mouse clicks on Buy and Close showed 100 → 75 and correct feedback; a runtime snapshot confirmed buying kept look blocked and closing restored it. Additional probes destroyed the own village and then checked buying as the opposing team: both cases rejected without charge. Inspector assignments and Console were visually checked. The editor was returned to Edit Mode and temporary setup/probe assets removed; wallet tests remain in the repository.

Windows, standalone builds, touch/gamepad controls, unusual/small display sizes, extended balance play, income, persistence, and multiplayer were not tested. Full winner/match shutdown remains a separate unfinished milestone: paid purchases reject destroyed villages, but this change does not stop existing automatic spawning after defeat. The pre-existing shared material recovery gap remains.
