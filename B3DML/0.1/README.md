# B3DML - Ball 3D Mod Loader

Version-agnostic mod loader. A mod built for 4.21 also runs on 4.x/slow even in slow_2020.

## Features

- + Version independent (4.21+)
- + Asset override (textures, models, audio, etc.)
- + Runtime C# script compilation (zero build)
- + Harmony patching (method hooks)
- + AssetBundle support
- + Auto-load on game start

## Install

### 1) Place B3DML in the game
- Copy everything inside `root/` into the Ball 3D folder (winhttp.dll, doorstop_config.ini, BepInEx/).

### 2) Add a mod
- Keep `root/mods/ExampleMod/` as-is or drop your own mod there.

### 3) Launch the game
- B3DML loads automatically.

## How it works

Unity loads DLLs from `Managed/`. B3DML:
- Starts via `[RuntimeInitializeOnLoadMethod]`
- Discovers mods (`mods/*/mod.json`)
- Compiles C# scripts at runtime (CSharpCodeProvider)
- Applies Harmony patches
- Loads AssetBundles

## Mod format

```
MyMod/
├── mod.json
├── scripts/   (C# scripts - auto compile)
├── bundles/   (AssetBundles)
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

### Sample mod.json
```json
{
	"name": "MyMod",
	"version": "1.0.0",
	"author": "YourName",
	"description": "What the mod does",
	"targetGameVersion": "4.21+",
	"priority": 100,
	"dlls": ["dlls/MyMod.dll"],
	"assets": {
		"textures": ["assets/textures/mytex.png"],
		"sprites": ["assets/sprites/mysprite.png"],
		"meshes": ["assets/meshes/mymesh.obj"],
		"audioClips": ["assets/audio/mysound.wav"],
		"fonts": ["assets/fonts/myfont.ttf"]
	},
	"bundles": ["bundles/myobjects.unity3d"]
}
```

**Use the ExampleMod folder as your template.**
