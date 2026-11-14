# Unity MCP

## Contents

1. Architecture and requirements
2. Setup and connection
3. Safe agent workflow
4. Custom tool registration
5. Troubleshooting

## 1. Architecture and requirements

Unity MCP lets MCP-compatible agents query and operate the Unity Editor. The documented Assistant 2.0 package requires Unity 6 (`6000.0`) or later and installs the relay under the user's `.unity/relay` directory.

The connection chain is:

1. An AI client launches the relay in MCP mode over standard input/output.
2. The relay communicates locally with Unity through named pipes on Windows or Unix sockets on macOS/Linux.
3. Unity's MCP Bridge exposes built-in and custom tools registered in `McpToolRegistry`.

The bridge starts when the Editor loads. Multiple MCP clients can connect to the same Unity instance. AI Gateway connections are automatically approved; direct external clients require user approval and approved clients are remembered.

Do not confuse these settings:

- **AI > Unity MCP:** configure the MCP server Unity exposes to external agents.
- **AI > MCP Client:** configure external MCP servers that Unity Assistant consumes.

## 2. Setup and connection

1. Install `com.unity.ai.assistant` in a Unity 6+ project.
2. Open **Edit > Project Settings > AI > Unity MCP**.
3. Confirm the Unity Bridge shows **Running**; start it if necessary.
4. Use the Integrations section to configure a supported client when possible.
5. For manual client configuration, point the MCP server command to the platform relay and add `--mcp`:

| Platform | Documented relay path |
|---|---|
| macOS Apple Silicon | `~/.unity/relay/relay_mac_arm64.app/Contents/MacOS/relay_mac_arm64` |
| macOS Intel | `~/.unity/relay/relay_mac_x64.app/Contents/MacOS/relay_mac_x64` |
| Windows | `%USERPROFILE%\.unity\relay\relay_win.exe` |
| Linux | `~/.unity/relay/relay_linux` |

6. Start Unity before or alongside the client. Review and accept a pending direct connection in Unity MCP settings.
7. Confirm the client appears under Connected Clients and dynamically lists Unity tools.
8. Test with a read-only request such as reading and summarizing Console warnings/errors.

Do not copy a relay path blindly. Confirm the installed executable and architecture in the current machine.

## 3. Safe agent workflow

- Discover the live registry; package versions can add, remove, rename, disable, or change tools.
- Query Console, project metadata, active scene, and target objects before writes.
- Perform one coherent mutation, wait for import/compilation, and query Console again.
- Use explicit scene paths and asset paths. When object names are duplicated, resolve the correct hierarchy object before editing.
- Save scenes and assets intentionally. Never assume Play Mode changes persisted.
- Maintain user control for package changes, connection approval, broad deletions, build platform switches, or scene replacement.
- Return structured evidence: object/asset paths, tool response, compilation state, tests, and runtime verification.

## 4. Custom tool registration

Unity MCP scans assemblies with `TypeCache` at Editor startup. Every tool needs a unique name, description, input parameters, and optionally an output schema.

Choose among the four documented patterns:

| Pattern | Use |
|---|---|
| Static method with typed parameters | Prefer for simple, constrained tools; schema is generated automatically |
| Static method with `JObject` | Use for genuinely dynamic/nested input and provide a custom schema |
| Class implementing `IUnityMcpTool<T>` | Use for state or helper methods |
| Runtime registration API | Use when tools must appear/disappear dynamically |

Use `[McpTool(name, description)]` to expose a method/class. Use `[McpDescription]` for parameter descriptions, required flags, enums, and defaults. Use `[McpSchema]` and `[McpOutputSchema]` for custom `JObject` schemas. Runtime registration uses `McpToolRegistry.RegisterTool` and `UnregisterTool`; registry changes can be observed with `McpToolRegistry.ToolsChanged`.

Custom tool rules:

- Put Editor-only tools in an Editor assembly/folder.
- Prefer typed parameters and enum constraints.
- Validate asset paths and restrict operations to the Unity project.
- Use `Undo.RecordObject`, `Undo.RegisterCreatedObjectUndo`, or equivalent for mutations.
- Record prefab instance modifications when needed, mark objects dirty, and save scenes/assets deliberately.
- Return machine-readable success, changed targets, warnings, and errors.
- Avoid storing important state only inside the tool instance; domain reloads can reset Editor state.
- Never treat a static dependency scan as proof that an asset is unused; `Resources`, Addressables, AssetBundles, reflection, custom loaders, and external content can hide runtime dependencies.
- For batch cleanup, split the tool into a read-only audit and an apply step that requires a reviewed, allowlisted manifest plus explicit deletion approval. Re-scan afterward and return `success: true` only when the stated verification criteria pass.

The bundled project-health template is read-only. Rename it from `.txt` to `.cs` only in a Unity 6+ project with the Assistant package installed, place it under an Editor folder, then fix any namespace/API differences against the installed package documentation before use.

## 5. Troubleshooting

### Bridge stopped

Fix all script compilation errors, verify the Assistant package, then start the bridge from Unity MCP settings. Restart the Editor only if necessary.

### Client cannot connect

Confirm Unity is running, the bridge is running, the relay exists, the client command uses the correct platform/architecture path, and `--mcp` is present. Reconfigure through Integrations when available.

### Pending but tools cannot run

Review and accept the client in Pending Connections. Do not bypass the approval mechanism.

### Tools missing

Fix custom script compilation, confirm `[McpTool]`, confirm the relevant assembly is scanned, and check whether the tool is disabled. Refresh/reload after compilation completes.

### Relay missing

Restart Unity to rerun installation; verify `Packages/com.unity.ai.assistant/RelayApp~/`; inspect Console warnings; use **Locate Server** as the documented workaround.

### Slow or timing out

Wait for imports, compilation, or builds; reduce concurrent clients; enable **Show Debug Logs** and inspect connection, discovery, execution, and error traces.
