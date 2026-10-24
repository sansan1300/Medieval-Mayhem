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

## Open decisions

- Player-control style and shared-device/local versus online multiplayer.
- Final 2–4-player elimination/win conditions, including how eliminated villages behave.
- Unit roster, stats, purchase costs, income tick rate, upgrade costs, and balance.
- Recovery source for missing tutorial assets; reconstruct only as a separately scoped task if originals cannot be recovered.
