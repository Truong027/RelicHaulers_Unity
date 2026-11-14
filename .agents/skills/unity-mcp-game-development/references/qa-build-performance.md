# Unity QA, Build, and Performance

## Contents

1. Source control and asset safety
2. Testing strategy
3. Console and runtime QA
4. Profiling
5. Build gate

## 1. Source control and asset safety

- Version `Assets`, `Packages`, and `ProjectSettings` with `.meta` files.
- Exclude generated `Library`, `Temp`, `Logs`, object/build caches, and local IDE state according to Unity's current recommendations.
- Commit coherent changes and inspect scene/prefab diffs. Avoid having multiple people or agents rewrite the same serialized scene.
- Move/rename assets through Unity or APIs that preserve GUIDs.
- Back up or checkpoint before package upgrades, render-pipeline conversion, mass prefab changes, or automated deletes.
- Do not classify assets as unused from Asset Database references alone when runtime loading, Addressables, `Resources`, AssetBundles, reflection, or external content may reference them.

## 2. Testing strategy

Use Edit Mode tests for pure logic, asset/editor utilities, and code that does not require a running player. Use Play Mode tests for MonoBehaviour lifecycle, scenes, physics, coroutines, UI, and runtime integration. Run critical Play Mode tests in a Player/target platform where feasible.

Prioritize tests for deterministic state transitions, damage/healing rules, inventory/progression, save data, scene/game flow, input mapping contracts, and regressions found during playtesting. Use arrange-act-assert and make tests independent of ordering and prior scene state.

## 3. Console and runtime QA

- Treat compile errors as blockers and unexpected exceptions/assertions as failures.
- Distinguish import warnings, deprecations, asset issues, and expected test logs.
- Test cold start, scene transitions, pause/resume, death/restart, resolution changes, lost focus, device reconnect, and repeated enter/exit of gameplay.
- Confirm there are no missing scripts, missing references, unsaved intended scenes, or required assets absent from the build.
- Test a built Player because Editor behavior, stripping, platform APIs, file paths, shaders, and performance can differ.

## 4. Profiling

Profile representative gameplay on target hardware. Use a Development Build and connect the Unity Profiler. Inspect CPU, GPU, rendering, memory, physics, audio, and network modules relevant to the project. Use deep profiling briefly because it adds overhead.

Measure before changing architecture. Look for recurring allocations and garbage collection, expensive per-frame searches, excessive physics contacts, overdraw, draw calls/batches, shader variants, texture memory, audio decompression, animation cost, and loading spikes. Add `ProfilerMarker` around custom systems when built-in samples are insufficient.

Editor FPS is not a shipping performance measurement.

## 5. Build gate

1. Select the intended Build Profile/platform and scene list.
2. Confirm scenes are enabled and ordered correctly.
3. Confirm scripting backend, architecture, API compatibility, stripping, graphics APIs, input handling, orientation/resolution, icons, identifiers, and required permissions.
4. Build a Development Player and run smoke tests on target.
5. Profile representative content and fix blockers.
6. Build the release candidate without development-only settings.
7. Record Editor/package versions, build profile, commit/source revision if available, target device, test result, and known issues.
