# Medieval Mayhem — gradual roadmap

This is an ordered plan, not an implementation checklist already completed. Keep each step small enough for a parent and son to understand and test together. See [current evidence](PROJECT_CONTEXT.md) and [decisions](DECISIONS.md).

## 0. Recover and prove the tutorial baseline

Resolve the missing UnitSpawner script, soldier prefabs/controllers, and baked NavMesh data noted in project context. Prefer recovering original assets and their metadata. Confirm the actual controller behavior before changing it.

Acceptance: in the prototype scene, no missing gameplay components or Console errors; both sides spawn on navigable ground and travel toward opposing villages; opposing units can fight and die if the tutorial implementation is recovered. Record any remaining gap instead of claiming completion. Do not add economy or networking yet.

## 1. Next gameplay milestone: village health and destruction

Deliver in small steps, checking each before continuing:

1. Add a simple village health component with Inspector-set starting health and a damage method. Verify health decreases and destruction happens once at zero. Use a temporary test method only if necessary and remove temporary test wiring before handoff.
2. Connect soldier attacks to the opposing village when within reach. Preserve unit-versus-unit combat; prevent damage to friendly villages. An unobstructed soldier should reach and damage the opposing village.
3. Improve targeting so units make progress toward villages. Proposed initial rule: consider only nearby opposing units, keep the current valid target to reduce switching, disengage beyond a bounded distance or chase duration, and resume the assigned village objective after disengagement/death. Prevent immediate reacquisition of an abandoned target from defeating the chase limit. Choose the simplest rule after inspecting recovered code; tune values in the Inspector.
4. Add minimal defeat/winner feedback for the two-village prototype and stop further attacks/spawning for the ended match. Keep any required village objective reference safe after destruction. Restarting Play Mode is sufficient for the first reset.

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
