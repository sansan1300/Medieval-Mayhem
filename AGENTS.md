# Medieval Mayhem — working agreement

A parent-and-son Unity learning project. Keep changes small, understandable, and playable. The parent is returning to Unity; explain editor steps without assuming recent Unity knowledge.

## Start here

1. Read `docs/PROJECT_CONTEXT.md`, `docs/ROADMAP.md`, and `docs/DECISIONS.md`.
2. Inspect current files and Git status before editing. Merge existing guidance; preserve unrelated work.
3. Distinguish the intended game, tutorial-reported progress, verified source, and verified Play Mode behavior. Never call a feature implemented just because a conversation or scene name mentions it.
4. Work on the requested milestone only. Documentation tasks do not authorize gameplay changes.

## Change conventions

- Prefer one visible behavior per change, simple C#, descriptive names, and Inspector-editable tuning values. Avoid frameworks, broad refactors, package upgrades, or speculative multiplayer architecture.
- Preserve Starter Assets, input, camera, rendering, and unrelated scene settings. Do not edit imported/vendor assets unless the task requires it; prefer project-owned scripts and prefab variants.
- Before changing targeting, inspect the actual controller. After disengaging from a unit, soldiers should resume their village objective; proposed targeting rules are recorded in the roadmap.
- Never hand-edit generated folders: `Library/`, `Temp/`, `Obj/` or `obj/`, `Logs/`, `UserSettings/`, build output, IDE caches, generated `.csproj` or `.sln` files. Never use cached/generated files as the source of truth or delete them as a routine fix.
- Preserve `.meta` files and GUIDs. Move/rename assets with their metadata, preferably in Unity. Include Unity-generated metadata with new assets under `Assets/`; do not regenerate IDs to conceal missing references.
- Prefer Unity for scene/prefab wiring. If serialized text must change, make a narrow edit and verify references in the editor. Avoid reserializing unrelated assets.
- Coordinate who is editing each scene or prefab before starting; avoid simultaneous edits to the same asset. Pull before starting when the working tree is clean. Before pushing, fetch and integrate remote changes, resolving any conflicts without overwriting another collaborator's work.
- Keep handoff notes in root `docs/` outside `Assets/`. Do not add documentation metadata there.
- Check for missing scripts, prefabs, and baked navigation before extending this checkout. Recover original files with their metadata where possible; do not silently recreate tutorial code or invent matching GUIDs.

## Verification and handoff

- For documentation, check paths, source claims, links, and the final change list; no Unity build is needed.
- For gameplay, open Unity 6000.4.6f1 and the explicit prototype scene. Check compilation/Console, Inspector references, then Play Mode. Test both sides and existing spawning/movement/combat alongside the changed behavior.
- Add focused Edit Mode tests for meaningful pure rules (health, gold spending, upgrades) when those rules exist. Use Play Mode/manual checks for navigation and scene wiring. Do not install a new test framework; Unity Test Framework is already declared.
- State exactly what was tested and what remains untested. Source inspection is not proof of runtime behavior. Use the milestone acceptance checks in the roadmap.
- Provide exact scene/object/component names, Inspector assignments, steps to reproduce, and expected results for editor work. Explain one gradual step at a time.
- Update context after verified progress and append significant decisions with date, reason, and status. Report files changed and discrepancies. Do not commit/push or include unrelated files without a request; never stage everything blindly.
