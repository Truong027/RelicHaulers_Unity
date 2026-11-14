---
name: blender-mcp-ai
description: >-
  Workflow and integration guide for harveyxiacn/blender-mcp-ai (380 tools across
  56 modules for Blender 4.x/5.x). Use when setting up Blender MCP AI, connecting
  a Blender addon to an AI IDE, generating stylized/toon assets or procedural
  materials in Blender, batch-creating props, rigging characters, or exporting
  Unity URP-ready FBX models with zeroed transforms.
---

# Blender MCP AI (harveyxiacn/blender-mcp-ai)

Repository: `https://github.com/harveyxiacn/blender-mcp-ai`

> Note: Do not install `blender-mcp` from PyPI (that belongs to a different project). Install by cloning `harveyxiacn/blender-mcp-ai` from source.

## When to Use

- Connecting Antigravity / Cursor / Windsurf / Claude Desktop to Blender 4.0+ / 5.x over MCP.
- Using Blender MCP's on-demand `skill` profile (32 startup tools + 12 on-demand skill groups) to avoid context bloat.
- Creating stylized/toon characters, animations, procedural materials, or batch props inside Blender and exporting them cleanly to Unity URP.

## Setup & Installation

1. **Prerequisites**: Blender 4.0+, Python 3.10+, `uv`.
2. **Clone & install dependencies**:
   ```bash
   git clone https://github.com/harveyxiacn/blender-mcp-ai.git
   cd blender-mcp-ai
   uv sync
   ```
3. **Build & install the Blender Addon**:
   ```bash
   python build_addon.py
   ```
   In Blender: `Edit -> Preferences -> Add-ons -> Install...` -> select `dist/blender_mcp_addon.zip` -> enable `Blender MCP` -> open the `MCP` panel in the 3D View sidebar.
4. **Configure `mcp_config.json` / `mcp.json`**:
   ```json
   {
     "mcpServers": {
       "blender": {
         "command": "uv",
         "args": ["run", "--directory", "/path/to/blender-mcp-ai", "blender-mcp-ai"]
       }
     }
   }
   ```

## On-Demand Skill Groups

Under the default `skill` profile, activate only the skill groups needed for the current step:

- `style`: Style presets, outlines, baking (Toons / Stylized presets).
- `materials`: Standard + procedural materials (wood, stone, metal, toon shaders, etc.).
- `modeling`: Mesh editing, modifiers, curves, UV (~38 tools).
- `character`: Character templates, rigging, auto-rig (~23 tools).
- `animation`: Keyframes and animation presets (~17 tools).
- `batch_assets`: Batch operations and prop/asset generation helpers (~11 tools).
- `scene_setup`, `physics`, `advanced_3d`, `sport_character`, `training`, `automation` (note: `pipeline` and `quality_audit` in `automation` require addon handler parity verification).

## Unity URP Export Checklist (Zeroed Transform)

Follow the repo's Unity export workflow (`docs/UNITY_QUEST_EXPORT.md`):

1. **Blender units**: `scene.unit_settings.system = 'METRIC'`, `scale_length = 1.0`.
2. **Apply transforms**: Scale `(1, 1, 1)`, Rotation `(0, 0, 0)`, Front faces `-Y` (maps to `+Z` forward in Unity).
3. **Origin placement**:
   - Floor-standing props: bottom-centre (`Z=0`) via `blender_set_origin(..., origin_type="BOTTOM")`.
   - Wall props / orbs / targets: geometric centre via `origin_type="GEOMETRY"`.
4. **Non-rigged props / environments**:
   ```python
   blender_export_fbx(
       filepath="<UnityProject>/Assets/Models/.../PROP_Name.fbx",
       selected_only=True,
       unity_static_preset=True,  # axis_forward=-Z, axis_up=Y, bake_space_transform=True
   )
   ```
   On the Unity model importer, enable **Bake Axis Conversion** and set **Scale Factor = 1**.
5. **Rigged characters** (do NOT bake space transform):
   ```python
   blender_export_fbx(
       filepath="<UnityProject>/Assets/Models/Characters/CHAR_Name.fbx",
       selected_only=True,
       unity_static_preset=False,
       primary_bone_axis="Y",
       secondary_bone_axis="X",
       add_leaf_bones=False,
   )
   ```
