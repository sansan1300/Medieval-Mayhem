# Medieval Mayhem — decisions

Append significant decisions as the project evolves. Include date, status, reason, and consequence. Update a superseded entry with a link to its replacement; preserve the history. Observed source facts belong in [PROJECT_CONTEXT.md](PROJECT_CONTEXT.md).

## D001 — Learn through small playable changes

- Date: 2026-10-25
- Status: Accepted — user direction
- Decision: Build as a parent-and-son project with beginner-friendly explanations and one manageable behavior at a time.
- Reason: The parent is rusty with Unity; changes should be easy to understand and verify together.
- Consequence: Prefer simple components and explicit Inspector setup; defer broad architecture and polish.

## D002 — Complete village combat before the economy

- Date: 2026-10-25
- Status: Accepted milestone — user direction
- Decision: Next gameplay milestone is village health/destruction plus targeting that avoids endless chase.
- Reason: Soldiers need a meaningful objective and a visible end to a round before adding economic choices.
- Consequence: First recover/verify missing tutorial assets. Then follow the incremental acceptance checks in the roadmap. Exact targeting distances/times remain to be chosen after controller inspection; the roadmap rule is a proposal.

## D003 — Separate the game vision from implemented behavior

- Date: 2026-10-25
- Status: Accepted — documentation convention
- Decision: Track verified files and observed runtime behavior separately from tutorial reports and future plans.
- Reason: The scene references missing scripts/prefabs/navigation data; tutorial progress cannot be assumed to exist in this checkout.
- Consequence: Do not mark spawning/combat verified until the baseline is repaired and tested. Do not treat installed Multiplayer Center or Starter Assets as implemented multiplayer.

## D004 — Keep engine and imported content stable

- Date: 2026-10-25
- Status: Accepted — working baseline
- Decision: Use the recorded Unity 6000.4.6f1 / URP setup and preserve Starter Assets and existing asset metadata during incremental work.
- Reason: Unrelated upgrades or GUID changes would make a learning prototype harder to debug.
- Consequence: No upgrades, generated-folder edits, or broad scene rewrites as incidental cleanup. Documentation stays outside Assets.

## D005 — Coordinate scene and prefab editing

- Date: 2026-10-25
- Status: Accepted — user direction
- Decision: Agree who edits each scene or prefab and avoid simultaneous edits to the same asset. Pull before starting with a clean working tree; fetch and integrate remote changes before pushing.
- Reason: Unity scene and prefab conflicts are difficult to resolve safely in a learning project.
- Consequence: Preserve both collaborators' work during integration; do not overwrite another person's changes to resolve a conflict.

## Resolved repository maintenance

- 2026-10-25: Prototype scene metadata committed in `0fcfc32`; `.gitignore` converted to UTF-8 in `1ba394f` with its rules preserved. Generated Unity/IDE files were verified ignored, and both changes were pushed to `origin/main`.

## D006 — Preserve the recovered original gameplay assets and scene identity

- Date: 2026-10-25
- Status: Accepted — asset recovery
- Decision: Integrate `share-missing-unity-assets` without rewriting its gameplay. Resolve the scene metadata conflict using the original working computer's GUID `15da031916ef0a544a75dafdd9524ade`.
- Reason: The branch restores the exact gameplay GUIDs referenced by the prototype. The Mac-generated scene metadata had no source references in the inspected asset/settings/package files; retaining the original preserves the working project's scene identity.
- Consequence: Existing targeting limitations remain for milestone 1. The shared material still needs recovery; record Play Mode evidence and remaining gaps in project context.

## D007 — Prototype exploration inside the shared battlefield

- Date: 2026-10-25
- Status: Accepted — user direction
- Decision: Use one shared prototype scene and a project-owned exploration rig plus Starter Assets player prefab variant. Offer a team dropdown, a matching team marker/start position, and exploration/original-camera switching. Keep the avatar outside combat initially.
- Reason: A duplicated scene would drift as collaborators update the battlefield. The installed third-person controller already supplies movement/input/animation; team tags used by soldiers cannot safely identify a non-soldier player.
- Consequence: Scene changes are limited to one rig instance and its references. Team data is separate from AI tags. A small camera follower avoids modifying the legacy imported camera rig or upgrading packages. Final player combat and multiplayer remain open. Initially based on main before village-destruction; rebased onto main `04d0e53` on 2026-10-25 to include the village and health-bar changes. Basic combined runtime behavior was subsequently checked as recorded in project context; full match-end acceptance remains open.

## D008 — Health bars follow the active exploration or overview camera

- Date: 2026-10-25
- Status: Accepted — integration fix
- Decision: Reacquire `Camera.main` and update the world-space Canvas reference when a health bar's cached camera becomes inactive.
- Reason: Exploration deactivates the original camera; retaining that camera would orient bars toward a view the player is no longer using.
- Consequence: Existing bars follow both camera modes without changing health, combat, or targeting rules. Verified in the combined Play Mode checks.

## D009 — Open the unit purchase menu with B

- Date: 2026-10-26
- Status: Accepted — user direction
- Decision: On the `unit-purchasing` prototype, B opens and closes the unit purchase menu from anywhere on the battlefield; village proximity is not required.
- Reason: Keep the first purchasing prototype easy to access and test while exploring.
- Consequence: Implement in the shared battlefield scene. Economy values are settled in D010, automatic spawning in D011, and menu input/pause behavior in D012.

## D010 — Start with a fixed budget and one paid soldier

- Date: 2026-10-26
- Status: Accepted — user direction
- Decision: Each team starts with 100 gold. One Basic Soldier costs 25 gold. There is no income in the first prototype; restarting Play Mode resets the balances.
- Reason: Four affordable purchases make spending and insufficient-gold feedback easy to test before adding income buildings.
- Consequence: Keep starting gold and soldier cost Inspector-editable. Verify team balance isolation, exact-cost purchases, repeated purchases, and rejection without negative balances. These values are prototype tuning, not final balance.

## D011 — Purchases supplement automatic spawning for the player's team

- Date: 2026-10-26
- Status: Accepted — user direction
- Decision: Both teams continue spawning soldiers automatically. The player can spend gold to spawn additional soldiers for their own selected team at that team's existing spawn point.
- Reason: Preserve the ongoing AI battle while testing purchases as extra reinforcements.
- Consequence: Keep existing automatic spawn intervals and behavior. Automatic spawns remain free and do not deduct from either team's purchase budget; paid purchases must not reset or delay the automatic spawn timer. The purchase menu does not permit buying for the opposing team.

## D012 — Shopping pauses player controls while the battle continues

- Date: 2026-10-26
- Status: Accepted — user direction
- Decision: Opening the purchase menu releases the cursor and stops player movement and camera look. The battle, including automatic spawning, movement, and combat, continues running. Closing the menu returns control to the character.
- Reason: Let the player click purchase buttons without moving or rotating the camera, while preserving the ongoing battlefield.
- Consequence: Do not pause game time when shopping. Coordinate cursor and control ownership with the existing exploration mode, cursor-release behavior, and camera switching.

## D013 — Keep purchases in a separate prefab on the shared battlefield

- Date: 2026-10-26
- Status: Implemented and verified on macOS
- Decision: Add a project-owned `UnitPurchasing` prefab instance to the existing prototype scene. Its scene overrides reference exploration, both existing spawners, and both villages. Use a small IMGUI menu and a pure `GoldWallet` class with focused Edit Mode tests.
- Reason: Preserve shared battlefield updates and keep the first economy step easy to inspect and tune without duplicating the scene or changing vendor assets.
- Consequence: Scene references belong to the instance; starting gold and cost live on the prefab. The automatic spawn loop is unchanged. Paid spawns validate team, village, prefab, and navigable spawn placement before charging. Menu inputs coordinate with exploration. D009-D012 passed the recorded local checks; Windows and builds remain unverified.
## D014 — End the prototype match when a village falls

- Date: 2026-10-25
- Status: Implemented; both winner/restart paths verified on macOS, 2026-10-26
- Integration note: Renumbered upstream D009 to D014 to preserve unique decision IDs alongside purchasing D009-D013.
- Decision: Show the opposing player as the winner when a village reaches zero health, stop AI soldiers and spawners, and offer a Restart button that reloads the prototype scene.
- Reason: A clear round ending makes village attacks meaningful and provides a simple way to play again.
- Consequence: The prototype scene is enabled in Build Settings. Combined purchasing/victory checks cover both winners and native mouse restart; the shop and paid spawns close at match end, and exploration preserves the victory cursor. Windows/build behavior remains unverified.

## Open decisions

- Final player-control style and shared-device/local versus online multiplayer; third-person exploration is a provisional local prototype.
- Final 2–4-player elimination/win conditions, including how eliminated villages behave.
- Unit roster, stats, purchase costs, income tick rate, upgrade costs, and balance.
- Recovery of the shared material and its dependencies from the original working project; scripts, prefabs, and navigation data have been recovered.
