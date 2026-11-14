# Unity Editor and Game Development Core

## Contents

1. Project mental model
2. Project setup
3. Scenes, prefabs, and assets
4. C# and lifecycle
5. Input, physics, animation, UI, and audio
6. Feature workflow

## 1. Project mental model

- A **GameObject** is a scene entity and Transform hierarchy node.
- A **Component** supplies data or behavior; compose focused components rather than building one giant controller.
- A **Scene** stores an authored world/state and object references.
- A **Prefab** stores a reusable configured GameObject hierarchy; instances stay linked and can have overrides.
- A **ScriptableObject** stores reusable serialized data as an asset, useful for definitions and configuration rather than per-instance scene behavior.
- An asset's `.meta` file contains its stable GUID. Losing or regenerating it can break serialized references.

The authoring source is the project, not the generated `Library` cache. Treat assets, packages, and project settings as versioned inputs; derived caches can be regenerated.

## 2. Project setup

1. Choose an Editor version and install it through Unity Hub with target platform modules.
2. Prefer an LTS or project-mandated version. Record exact Editor and package versions.
3. Choose the correct template/render pipeline before importing large content.
4. Establish folders by responsibility, for example `Art`, `Audio`, `Prefabs`, `Scenes`, `Scripts`, `Settings`, `Tests`, and `UI`; follow an existing convention rather than forcing this one.
5. Use visible meta files and text serialization for collaboration. Ignore generated caches such as `Library`, `Temp`, `Logs`, and build outputs, but keep `Assets`, `Packages`, and `ProjectSettings`.
6. Create a small vertical slice and target build early.

## 3. Scenes, prefabs, and assets

- Keep scene responsibilities clear: bootstrap/loading, menus, gameplay areas, persistent services, or additive layers.
- Put repeated objects and characters in Prefabs. Apply/revert overrides deliberately; avoid unpacking only to make an edit easier.
- Use prefab variants for controlled families, not long chains of fragile overrides.
- Configure import settings intentionally: sprite mode/pixels-per-unit/filtering/compression, texture color space, audio load type, model scale/rig/animation, and platform overrides.
- Keep runtime references serialized or resolved through explicit systems. Avoid repeated global searches in gameplay loops.
- Use Addressables only when the project needs asynchronous/on-demand content management; do not introduce it to a small prototype without a reason.

## 4. C# and lifecycle

- Use serialized private fields for Inspector wiring while preserving encapsulation.
- Initialize references in `Awake`; use `OnEnable`/`OnDisable` for paired subscriptions; use `Start` for initialization that depends on other objects being awake.
- Use `Update` for per-frame input and non-physics behavior. Use `FixedUpdate`/Rigidbody APIs for physics changes according to the chosen input buffering strategy.
- Scale rates by elapsed time. Do not multiply direct displacements, velocities, and forces indiscriminately; understand the API's units.
- Avoid allocations, scene searches, reflection, and logging in hot paths unless measured and justified.
- Keep data/state separate from presentation. Use C# events or explicit orchestration to notify UI/animation/audio without creating circular dependencies.
- Expect domain reloads and serialization. Do not rely on nonserialized Editor state persisting across compilation.

Use assembly definitions when they improve compile boundaries and dependency control. Keep Editor code separate from runtime assemblies.

## 5. Input, physics, animation, UI, and audio

### Input

Use Input Actions to separate actions such as Move, Interact, Pause, and Fire from keyboard/gamepad/touch bindings. Choose a PlayerInput callback workflow deliberately and unsubscribe callbacks when disabled.

### Physics

Do not mix 2D and 3D physics components. Configure Rigidbody body type, colliders, layers, collision matrix, triggers, interpolation, and continuous detection according to behavior. Move dynamic bodies through physics APIs; do not fight them by rewriting Transforms every frame.

### Animation

Keep Animator parameters derived from gameplay state. Verify transitions, exit time, interruption, root motion, and SpriteRenderer sorting. Use Animation Events sparingly for presentation timing, not critical game-state authority.

### UI

Choose UI Toolkit or uGUI based on the existing project and platform needs. Keep UI declarative and bind/update it from authoritative state. Test multiple resolutions, safe areas, localization growth, keyboard/gamepad navigation, and touch targets.

### Audio/VFX

Separate mixers/groups for music, SFX, UI, and ambience when useful. Avoid spawning unbounded AudioSources or particle systems; pool repeated effects. Drive polish from real events and verify that disabled/pooled objects reset correctly.

## 6. Feature workflow

For every feature:

1. Write an observable acceptance case.
2. Identify data, behavior, presentation, and scene wiring.
3. Implement the smallest vertical path.
4. Compile and clear Console errors.
5. Test in isolation and then in representative gameplay.
6. Convert reusable configurations to Prefabs/assets.
7. Add tests around deterministic logic and critical integration.
8. Profile/build on target before polishing or scaling content.
