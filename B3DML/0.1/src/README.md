# B3DML Source

## Build

Requirements:
- .NET SDK / MSBuild
- UnityEngine.dll and Assembly-CSharp.dll (from Ball 3D_Data/Managed/)

Steps:
1. Build `B3DML.sln` in Release via Visual Studio or msbuild.
2. Output goes to `bin/Release/net35/`.
3. Copy DLLs into your test install `Ball 3D_Data/Managed/`.

Quick commands:
```bash
msbuild B3DML.sln /p:Configuration=Release
# or
dotnet build B3DML.sln -c Release
```

Single-file (mcs):
```bash
mcs -target:library -out:B3DML.Core.dll \
   -r:UnityEngine.dll -r:Assembly-CSharp.dll \
   B3DML.Core.cs
```

## Modules
- `B3DML.Bootstrap`: entry; RuntimeInitializeOnLoadMethod; starts loader.
- `B3DML.Core`: mod discovery, mod.json parsing, load order.
- `B3DML.DLLInjector`: DLL loading, triggers Harmony patches.
- `B3DML.ScriptLoader`: script compile (Mono 2.0 limits; DLL preferred).
- `B3DML.BundleLoader`: AssetBundle load and cache.
- `B3DML.AssetLoader`: Texture/Sprite/Font/Mesh loading with fallback.
- `B3DML.BepInExPlugin`: Doorstop/BepInEx stub.

All target net35, Unity 5.5.3 compatible.

## Mono 2.0 notes
- No async/await, no `dynamic`.
- `System.CodeDom` missing; runtime compile is limited.
- Use `for` instead of `foreach` + reflection; prefer `ReferenceEquals`.
- Path.Combine handles two args; chain as needed.

## Logs
- `logs/b3dml_debug.log`: loader flow
- `logs/scriptloader.log`: script/dll compile
- `BepInEx/LogOutput.log`: general log

## Mods directory
- Mods live in `root/mods/`
- Template: `root/mods/ExampleMod/`
- Format and mod.json reference: [../MOD_CREATION_GUIDE.md](../MOD_CREATION_GUIDE.md)
