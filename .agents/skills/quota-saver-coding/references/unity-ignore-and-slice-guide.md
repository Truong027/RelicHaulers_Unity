# Unity Ignore List & Fast Search Patterns

## Directories & Extensions to Exclude From `rg` / File Scans
Always filter out Unity generated & binary bloat when searching:
```powershell
# PowerShell fast C# search inside project scripts only:
Get-ChildItem -Path 'Assets/_Project/Scripts' -Filter '*.cs' -Recurse |
  Select-String -Pattern 'PlayerReviveSystem|LastSafeGround' |
  Select-Object -First 30 Filename, LineNumber, Line
```
If `rg` (ripgrep) is installed:
```powershell
rg -n --glob '*.cs' 'QuotaManager|OnTriggerEnter' Assets/_Project/Scripts
```

## Hard Do-Not-Read Paths (Zero Value, Massive Token Cost)
| Path / Extension | Reason | Alternative |
| :--- | :--- | :--- |
| `Library/**`, `Temp/**`, `obj/**`, `Logs/**` | Auto-generated cache & megabyte logs | `Unity_ReadConsole` or tail last 30 error lines |
| `*.meta`, `*.asset` (large meshes/navmesh) | Binary/hex blobs & GUID noise | `Unity_FindAssets` / Inspector via Unity MCP |
| `*.unity`, `*.prefab` (> 150 lines) | Verbose serialization YAML | `Unity_ManageScene` / `Unity_ManageGameObject` |
| `kich-ban/00..15` (all at once) | ~160 KB repeated design text | Read `project-memory` (~4 KB) + 1 targeted slice |

## Safe Verification Loop (Compile-Once Pattern)
1. Author cohesive C# slice + ScriptableObject fields.
2. Refresh/Check Unity Console via Unity MCP (`Unity_ReadConsole` errors only).
3. Fix all compile errors in **one batched edit turn**, then re-check once.
4. Record created filenames & invariants in `.agents/skills/project-memory/references/sprint-tracker.md`.
