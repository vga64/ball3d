# Mod Creation Guide

Build every new mod from the ExampleMod template. Follow these steps.

## Quick start (3 steps)
1) Copy `mods/ExampleMod/` → `mods/NewMod/`
2) Edit `mod.json` (name, version, targetGameVersion, priority, assets)
3) Drop textures/models/sounds into `assets/`, launch the game

## Folder layout

```
mods/
└── NewMod/
    ├── mod.json
    ├── scripts/      (optional - C# code, auto-compiled)
    ├── bundles/      (optional - Unity AssetBundle)
    └── assets/
        ├── textures/
        ├── sprites/
        ├── meshes/
        ├── audio/
        ├── fonts/
        ├── animators/
        ├── shaders/
        ├── text/
        └── monobehaviours/
```

## mod.json example

```json
{
  "name": "NewMod",
  "version": "1.0.0",
  "author": "YourName",
  "description": "Short description",
  "targetGameVersion": "4.21+",
  "priority": 100,
  "dlls": ["dlls/NewMod.dll"],
  "assets": {
    "textures": ["assets/textures/texture.png"],
    "sprites": ["assets/sprites/ui.png"],
    "meshes": ["assets/meshes/model.obj"],
    "audioClips": ["assets/audio/sound.wav"],
    "fonts": ["assets/fonts/font.ttf"]
  },
  "bundles": ["bundles/objects.unity3d"]
}
```

Required fields: `name`, `version`. Others are optional.

## DLL or script?
- Prefer DLL: build with mcs or Visual Studio, place in `dlls/`, list in mod.json.
- Scripts: place `.cs` files in `scripts/`, add to mod.json; remember Mono 2.0 limits.

### Quick mcs build example
```bash
mcs -target:library -out:dlls/NewMod.dll \
  -r:UnityEngine.dll -r:Assembly-CSharp.dll \
  scripts/YourScript.cs
```

## AssetBundle note
- Tag assets in Unity Editor → build → place under `bundles/` → list paths in mod.json `bundles`.

## Debugging
- Logs: `logs/b3dml_debug.log`, `logs/scriptloader.log`, `BepInEx/LogOutput.log`
- If it fails: validate mod.json, verify file paths, check logs for errors.

## Mono 2.0 tips
- No async/await, no `dynamic`.
- `System.CodeDom` missing; runtime compile is limited, DLL preferred.
- Use classic `for` instead of `foreach` + reflection; prefer `ReferenceEquals`.

## Reference
- Everything in `mods/ExampleMod/` shows a working example for every asset type.

