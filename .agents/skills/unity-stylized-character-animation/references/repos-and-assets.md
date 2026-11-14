# Curated Repositories & Asset Sources

## Group A: Stylized Unity Animation & Character Reference Projects

| Repository | Branch | License / Notes | Key Techniques to Reference |
| :--- | :--- | :--- | :--- |
| `https://github.com/fierc3/UnityCharacterAnimations` | `master` | Check repo | Combines Mixamo keyframe animations, Mecanim, Unity Animation Rigging (foot IK on slopes, hand IK, head aim), procedural animation, and ragdoll physics (`_Showcase` scene). |
| `https://github.com/joebinns/stylised-character-controller` | `main` | Check repo | Unity URP floating-capsule physics character controller (inspired by Toyful Games' *Very Very Valet*), Squash & Stretch, occlusion fade shader, blob shadows, and movement dust VFX. |
| `https://github.com/murenne/3DLowPolyRPG_` | `main` | Check repo | Full Unity 2022.3 URP 14 Low-Poly RPG project: Character Customization / Bone Retargeting for armor & gear, Blend Tree locomotion, Root Motion, Input System, Cinemachine, Shader Graph, NavMesh, ScriptableObject inventory. |
| `https://github.com/teamgravitydev/gamedev-free-resources` | `main` | Mixed (check each linked asset) | Curated index of free 2D/3D game art, audio, and prototyping resources. |
| `https://github.com/bewilderbug/lowpolycoop` | `master` | **CC BY 2.5** (attribution required) | Heritage collection of lightweight `.blend` low-poly models with 512x512 diffuse textures and 1024x1024 UV templates for rapid prototyping. |

### External CC0 Stylized Asset Packs (Humanoid / Mixamo Compatible)

- **KayKit** (CC0): Stylized low-poly characters, weapons, dungeons, and props sharing consistent bone naming and Unity Humanoid / Mixamo rig compatibility.
- **Quaternius** (CC0): Free low-poly character, monster, weapon, and environment packs compatible with Unity Humanoid and Mixamo animation libraries.

---

## Group B: AI Agent Skills, MCP & Animation Tools

| Tool / Repository | Role | Location / Usage |
| :--- | :--- | :--- |
| `https://github.com/harveyxiacn/blender-mcp-ai` | Blender 4.x/5.x MCP Server + Addon (380 tools, 56 modules, Toon/Stylized presets, 67 procedural materials, character/animation/batch_assets) | See skill `blender-mcp-ai`. |
| `https://github.com/lovecatisgood-sudo/3d-asset-generation-blender-unity-game-development-skills` | Blender -> Mesh/UV/Rig audit -> FBX/GLB -> Unity URP Prefab & Unity MCP skills | Installed locally in `.agents/skills/blender-3d-asset-generation/` and `.agents/skills/unity-mcp-game-development/`. |
| `https://github.com/tjboudreaux/cc-plugin-unity-gamedev` | `tools-unity-animation` skill (Animancer, Mecanim State Machines, Playables performance) | Installed locally in `.agents/skills/tools-unity-animation/`. |
| `https://github.com/claude-dev-suite/claude-dev-suite` | `unity-physics-anim` skill (3D Rigidbody, Animator State Machines, Blend Trees, Animation Rigging, Humanoid retargeting) | Installed locally in `.agents/skills/unity-physics-anim/`. |
| `https://github.com/Roboticela/Animator` | Open-source 3D model animation studio (React + Tauri + Three.js, AGPL-3.0). Import rigged models, inspect bone hierarchies, author keyframes, apply premade humanoid clips, export GLB. | Run in browser or build desktop app from source: `git clone https://github.com/Roboticela/Animator.git`. |

## Quick Clone Reference

To inspect any Group A reference project locally (outside your Unity project's `Assets/` folder, e.g. in a `Refs/` folder ignored by Unity or a separate directory):

```bash
git clone --depth 1 https://github.com/fierc3/UnityCharacterAnimations.git
git clone --depth 1 https://github.com/joebinns/stylised-character-controller.git
git clone --depth 1 https://github.com/murenne/3DLowPolyRPG_.git
```
