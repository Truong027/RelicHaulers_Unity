# Unity 2D Adventure Workflow

## Contents

1. Course/version routing
2. Vertical-slice sequence
3. Suggested architecture
4. 2D correctness checklist

## 1. Course/version routing

The supplied classic course is **2D Beginner: Adventure Game** for Unity 2022.3. Unity Learn now points Unity 6.3 users to **2D Adventure Game: Robot Repair**. Preserve the curriculum's concepts while verifying UI paths and package APIs against the installed version.

The classic curriculum proceeds through player movement; tilemap environment and physics; health, collectibles, and hazards; HUD with UI Toolkit; enemies, sprite animation, projectiles, and raycast dialogue; then camera, audio, particles, hand-drawn animation, and build/share. The Unity 6.3 course also makes the game UI/game loop an explicit stage.

## 2. Vertical-slice sequence

### Stage A: player and movement

- Import/configure sprites and create a player Prefab.
- Add Rigidbody2D and an appropriate Collider2D.
- Create Move/Interact/Attack Input Actions and a PlayerInput workflow.
- Read input, normalize or clamp movement as designed, and apply physics-safe motion.
- Drive facing and movement animation from actual state.
- Test at multiple frame rates and input devices.

### Stage B: environment and physics

- Create Grid/Tilemap layers for ground, collision, decoration, and foreground as needed.
- Use TilemapCollider2D and CompositeCollider2D where they reduce collider complexity appropriately.
- Establish sorting layers, orders, and SortingGroups so character pieces do not interleave incorrectly.
- Configure 2D collision layers/matrix and verify corners, narrow gaps, slopes, and spawn points.

### Stage C: health and world interactions

- Make health an authoritative component with current/max values, clamping, and change events.
- Implement healing collectibles with triggers and reject consumption when no healing occurs if that is the design.
- Implement hazards with explicit damage/cooldown rules; avoid uncontrolled damage every physics tick.
- Test death/respawn or fail-state transitions early.

### Stage D: HUD and game loop

- Build the health HUD in the project's selected UI system and update it from health events.
- Add pause/restart/win/lose flows and ensure input focus/time scale are restored correctly.
- Test aspect ratios, safe areas, and controller/touch navigation.

### Stage E: characters and interactions

- Create enemy/NPC Prefabs with separate sensing, state, movement, health, and presentation where complexity warrants it.
- Use sprite animation states driven by data rather than duplicating logic in Animator transitions.
- Pool frequent projectiles/effects; set collision ownership and lifetime explicitly.
- Use raycasts/overlap queries with explicit layer masks for dialogue and interaction. Choose the nearest/most relevant hit deterministically.

### Stage F: polish and ship

- Add a bounded camera that respects level geometry and pixel-art rules if relevant.
- Add mixer-routed audio, pooled particles, hit feedback, and readable transitions.
- Create a development build early, then a release candidate. Verify the full game loop outside the Editor.

## 3. Suggested architecture

Keep these responsibilities separate enough to test:

- `PlayerInputReader`: converts Input Actions to intent.
- `CharacterMotor2D`: applies motion and collision behavior.
- `Health`: owns current/max health and events.
- `DamageSource` / `HealPickup`: requests state changes.
- `Interactor2D`: selects and invokes nearby/raycast interactions.
- `Projectile`: owns travel, collision filtering, hit, and lifetime.
- `CharacterView`: updates Animator/SpriteRenderer/audio from state.
- `HudController`: presents health and game-loop state.
- `GameFlow`: owns pause, restart, success, and failure transitions.

Names are illustrative; follow the existing project. Keep tunable definitions in serialized Prefabs or ScriptableObjects. Avoid turning every small value into a global singleton.

## 4. 2D correctness checklist

- Sprite import: mode, slicing, pivot, pixels per unit, filter, compression, atlas policy.
- Rendering: sorting layers/order, SortingGroup, material/shader, camera projection, pixel-perfect strategy.
- Physics: Rigidbody2D/Collider2D pairing, body type, constraints, layers, trigger/contact policy, interpolation.
- Input: action maps, device bindings, enable/disable lifecycle, rebinding/accessibility if required.
- Animation: clips, controller, transition conditions, frame rate, looping, facing, event ownership.
- Tilemap: grid scale, palettes, colliders, composite geometry, runtime modification cost.
- UI: authoritative updates, scaling, safe areas, focus/navigation, pause behavior.
- Build: scene list, platform settings, permissions, resolution/orientation, target-device test.
