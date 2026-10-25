# Medieval Mayhem — gradual roadmap

This is an ordered plan, not an implementation checklist already completed. Keep each step small enough for a parent and son to understand and test together. See [current evidence](PROJECT_CONTEXT.md) and [decisions](DECISIONS.md).

## Parallel prototype — world exploration

User-authorized on 2026-10-25, branch `world-exploration` based on `main`. One team-owned third-person observer now walks among AI in the existing battlefield scene. Team selection is an Inspector dropdown; F5 toggles the original camera. Both team spawns, movement, camera rotation, mode toggles, and ongoing AI combat passed the recorded macOS Play Mode checks. See [setup and verification](WORLD_EXPLORATION.md). Rebased onto main `04d0e53`, including village-destruction and health-bar source changes. Initial exploration checks predate that integration. Later combined macOS checks confirmed team movement, AI combat, both sides damaging undefended villages, and health bars following camera switches; full match-end acceptance remains open. This does not complete village combat or commit to a final multiplayer/control design.

## 0. Recover and prove the tutorial baseline

Status, 2026-10-25: original scripts, prefabs, and baked NavMesh data recovered from `share-missing-unity-assets`. Both sides spawned, moved, lost health, and had damaged units removed during a 35-second macOS Play Mode check with no warnings/errors. See [verification and limits](PROJECT_CONTEXT.md#recovery-play-mode-check--2026-10-25). Recover the still-unresolved shared material from the working computer; original appearance and unobstructed travel to the opposing village remain unverified.

Keep the recovered original assets and metadata. The actual controller has been inspected: it searches globally for the closest enemy every frame, has no chase bound, and does not explicitly resume the village destination after disengagement. Preserve the baseline while implementing milestone 1 incrementally.

Acceptance: in the prototype scene, no missing gameplay components or Console errors; both sides spawn on navigable ground and travel toward opposing villages; opposing units can fight and die if the tutorial implementation is recovered. Record any remaining gap instead of claiming completion. Do not add economy or networking yet.

## 1. Next gameplay milestone: village health and destruction

Deliver in small steps, checking each before continuing:

1. **In progress:** `Assets/Scripts/VillageHealth.cs` is attached to `Village_Player1` and `Village_Player2` in the prototype scene. Each has 500 starting health; `TakeDamage` reduces health and destroys its village at zero. Soldiers and villages create world-space health bars. Runtime behavior still needs Unity verification. Verify health decreases and destruction happens once at zero, and check that each bar stays visible above its object. Use a temporary test method only if necessary and remove temporary test wiring before handoff.
2. **Implemented in source; needs Play Mode verification:** soldiers attack their assigned opposing village within `villageAttackRange` of its collider surface and skip a village whose `ownerTag` matches their own tag. Unit-versus-unit combat remains active outside the village priority range.
3. **Implemented in source; needs Play Mode verification:** soldiers retain a valid enemy target within `enemyChaseRange`, drop it outside that range, and resume the village objective. Within `villagePriorityRange` they focus on the village, avoiding repeated diversion to defenders near its spawn point. Both ranges default to 8 Unity units and are Inspector-editable.
4. **Implemented in source; needs Play Mode verification:** when a village reaches zero health, show a winner message and Restart button, stop active soldiers and spawners, and reload the prototype scene when Restart is clicked. The prototype was added to Build Settings to support scene reload. See `Assets/Scripts/VictorySystem.cs`.

Acceptance checks in Play Mode:

- With no nearby defenders, soldiers attack the opposing village at the intended interval and reduce its health.
- A nearby enemy triggers combat; dead enemies are released safely and the village objective resumes.
- A retreating enemy cannot pull a soldier into an endless chase or instant reacquisition loop; after the chosen bound, village progress resumes.
- Friendly units/villages take no damage from their own soldiers.
- At zero health, defeat/destruction and the winner signal occur once; no repeated damage, missing-reference errors, or continued match spawning.
- Both player sides work; stopping/restarting Play Mode restores the starting state; Console stays clear.

Use focused health-rule tests when practical; navigation, prefab assignments, and destruction interactions still need Play Mode checks. This milestone is not complete until those observations are recorded.

## 2. One gold counter and one paid unit

Start with one unit type and explicit per-village ownership. Buying deducts the correct cost and spawns one unit; insufficient gold leaves the balance unchanged and gives understandable feedback. Verify both sides, exact-cost purchase, repeated purchase, and rejection without negative balances.

## 3. One income building and one upgrade

Add a basic periodic gold source, then one paid upgrade that increases income. Show cost and resulting benefit. Verify income belongs to the correct village, upgrade costs are paid once, and spending on an upgrade visibly delays buying soldiers.

## 4. Prove the economy-versus-army choice

Play short two-village rounds with a small unit roster and readable costs/stats. Adjust one balance value at a time. Record whether saving, upgrading, and attacking each provide useful choices before expanding content.

## 5. Decide player controls and multiplayer format, then expand to 2–4 players

Choose shared-device/local or online play, camera/input model, ownership, and win rules together. If online is chosen, plan authority and synchronization before implementation. Prove a minimal two-player session before adding four villages. Multiplayer is a future feature, not present behavior.

## Later, only when needed

Art polish, animation, audio, richer buildings/units, menus, saving, matchmaking, and production deployment. Avoid adding them before the basic round is fun and understandable.
