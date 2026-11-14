---
name: unity-stylized-character-animation
description: >-
  Reference architecture, animation pipelines, rigging workflows, and curated
  open-source Unity/3D repositories for stylized & low-poly Unity games (URP).
  Use when designing stylized character controllers, combining Mixamo keyframe
  animations with procedural animation/IK/ragdoll, setting up bone retargeting
  and Blend Trees, inspecting/applying Humanoid clips with Roboticela/Animator,
  or sourcing free stylized/low-poly 3D assets (KayKit, Quaternius, GitHub).
---

# Unity Stylized Character, Animation & Asset Pipeline

This skill integrates the curated reference projects, animation tooling, and free stylized 3D asset sources for Unity URP game development.

For repository URLs, branches, licenses, and clone/setup details, read [references/repos-and-assets.md](references/repos-and-assets.md).

## End-to-End Stylized Character & Animation Workflow

1. **Source or Create Stylized 3D Models**
   - **Pre-rigged CC0 Humanoid/Mixamo compatible packs**: Prefer **KayKit** or **Quaternius** CC0 packs when prototyping so characters, weapons, chests, and props share Unity Humanoid / Mixamo bone conventions and reuse animations directly.
   - **GitHub Low-Poly Asset Collections**: Use `bewilderbug/lowpolycoop` (`.blend`, 512x512 diffuse, CC BY 2.5 — requires attribution) and `teamgravitydev/gamedev-free-resources` for rapid prototyping assets.
   - **Custom Blender Modeling & Rigging**: Use `blender-mcp-ai` and `blender-3d-asset-generation` skills to model, apply stylized/toon materials, rig, audit topology/UVs, and export FBX/GLB.

2. **Preview, Retarget & Author Clips Before Unity Import**
   - Use **Roboticela/Animator** (`https://github.com/Roboticela/Animator`, AGPL-3.0, browser or Tauri desktop) to import rigged models, inspect bone hierarchies, hand-author keyframe animations, apply premade humanoid clips, and export lightweight GLB (or verify rig compatibility before FBX import).

3. **Configure Unity URP Character & Animation Systems**
   - **Hybrid Keyframe + Procedural Animation + IK + Ragdoll**: Reference `fierc3/UnityCharacterAnimations` (`master` branch) for combining Mixamo keyframe clips, Mecanim, Unity Animation Rigging (foot IK on slopes, hand grab/hold, head look-at), procedural animation, and ragdoll physics.
   - **Stylized Physics Character Controller (URP)**: Reference `joebinns/stylised-character-controller` (`main` branch) for the floating-capsule physics controller (Toyful Games *Very Very Valet* style), Squash & Stretch effects on jump/land, occlusion-fade shader when obstacles block the character, and movement dust VFX.
   - **Low-Poly RPG Systems (URP)**: Reference `murenne/3DLowPolyRPG_` (`main` branch, Unity 2022.3 + URP 14) for Bone Retargeting (swapping armor/equipment cleanly across characters), Blend Tree locomotion, Root Motion, Cinemachine, Input System, Shader Graph, and ScriptableObject-driven inventory.
   - **C# Animation & Physics Best Practices**: Pair with the companion skills `tools-unity-animation` (Animancer, Mecanim, Playables), `unity-physics-anim` (3D Rigidbody, Animator state machines, Animation Rigging, Humanoid retargeting), and `unity-mcp-game-development`.

## Checklist When Implementing a Stylized Character in Unity

- [ ] Set Rig **Animation Type = Humanoid** (or Generic if non-humanoid) and verify Avatar bone mapping before writing Animator code.
- [ ] For modular armor/equipment swaps (`murenne/3DLowPolyRPG_` pattern), ensure skinned meshes share the exact skeleton hierarchy and rebind `SkinnedMeshRenderer.bones` / `rootBone` to the character's active rig.
- [ ] Keep Rigidbody movement in `FixedUpdate` with `RigidbodyInterpolation.Interpolate`, and drive visual Squash & Stretch on a child visual transform rather than scaling the physics collider.
- [ ] Put foot/hand/head IK in an **Animation Rigging** Rig Layer above the base locomotion Animator layer so keyframe locomotion and procedural IK compose cleanly.
