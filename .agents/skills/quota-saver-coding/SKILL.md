---
name: quota-saver-coding
description: >-
  Token and quota optimization rules for coding RELIC HAULERS in Unity C#, Ink, and Blender.
  Use on every coding task to avoid re-reading docs, scanning Unity Library/meta/YAML bloat,
  burning turns on fragmented edits, or flooding context with verbose logs.
---

# Quota-Saver Coding Discipline

> Every extra turn replays the full conversation history. Minimize turns, bytes read, and uncached tool noise.

## 1. Memory-First, Slice-Only Reading
1. **Use `project-memory` first**: Read `memory-snapshot.md` and `sprint-tracker.md` (~4 KB total) instead of opening `kich-ban/00..15` (`~160 KB`).
2. **Search before reading**: Use `rg -n` or PowerShell `Select-String` against `Assets/_Project/Scripts` or `kich-ban/` to find exact line numbers, then `read_file` a tight slice (`StartLine..EndLine`, max `120` lines).
3. **Never re-read**: Do not re-read files already inspected in the same session unless another tool modified them.

## 2. Batch Tool Calls & Surgical Edits (Fewer Turns = Huge Quota Savings)
- **Parallelize independent actions**: Issue multiple independent `read_file` or `edit_file` calls in a single turn instead of serializing 1 file per turn.
- **Prefer `edit_file` over full `write_file`**: Patch only changed methods/blocks; never reprint a 300-line C# class for a 10-line fix.
- **Complete a vertical slice before compiling**: Write all cohesive classes of a slice (e.g., `RelicDataSO` + `RelicItem` + `CarrySystem` hook) cleanly, then run **one** Unity compile / Console check / PlayMode verification pass rather than testing after every single edit.

## 3. Unity & Blender Token Traps (HARD EXCLUSIONS)
Never feed raw Unity overhead into context. See [references/unity-ignore-and-slice-guide.md](./references/unity-ignore-and-slice-guide.md) for exact patterns:
- **NEVER read or grep inside**: `Library/`, `Temp/`, `Logs/`, `obj/`, `Build/`, `.vs/`, `.git/`, `Packages/*/Documentation~`.
- **NEVER read `.meta`, `.fbx`, `.blend`, `.png`, `.wav` as text**, and **NEVER read raw `.unity` / `.prefab` / `.mat` YAML files in full** (use Unity MCP tools `Unity_ManageScene` / `Unity_ReadConsole` or targeted `rg` for GUIDs).
- **Truncate noisy command output**: Pipe CLI outputs through `Select-Object -First 40` or filter `Error|Warning` lines so compiler/build spam does not inflate input tokens.

## 4. Lean Output & Subagent Restraint
- Keep explanations concise; skip restating code you just wrote.
- **Do not spawn subagents** for trivial `1–3` file reads/edits (parent context handles those cheaper). Delegate to `research` / `flash` subagents only when sweeping `>= 5` files or filtering massive error logs.
- Update `project-memory/references/sprint-tracker.md` once at the end of a milestone—not after every micro-edit.
