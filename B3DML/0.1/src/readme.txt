B3DML - MODLOADER CORE MODULES

This folder contains all B3DML source code.

=====================================
STRUCTURE
=====================================

Each module is a separate Visual Studio project:
- B3DML.sln - main solution (build all)

Core modules (net35 target):
- B3DML.Bootstrap.cs / .csproj
- B3DML.Core.cs / .csproj
- B3DML.DLLInjector.cs / .csproj
- B3DML.ScriptLoader.cs / .csproj
- B3DML.BundleLoader.cs / .csproj
- B3DML.AssetLoader.cs / .csproj
- B3DML.BepInExPlugin.cs / .csproj

=====================================
BUILD INSTRUCTIONS
=====================================

1. Open with Visual Studio 2019+:
	File -> Open -> B3DML.sln

2. Build (Release):
	Build -> Build Solution
	Output: DLLs go to bin/Release/

3. Copy DLLs:
	cp bin/Release/*.dll /path/to/Ball3D/Managed/

4. Check sample mod:
	../root/mods/ExampleMod/ - shows all asset types

=====================================
MAKE A NEW MOD
=====================================

1. Use the template:
	cp -r ../root/mods/ExampleMod/ ../root/mods/NewMod/

2. Folder layout:
	NewMod/
	├── mod.json
	├── scripts/
	├── bundles/
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

3. mod.json example:
	{
	  "name": "NewMod",
	  "version": "1.0.0",
	  "author": "YourName",
	  "description": "What it does",
	  "targetGameVersion": "4.21+",
	  "priority": 100,
	  "dlls": [],
	  "assets": {
		 "textures": ["assets/textures/file.png"]
	  },
	  "bundles": ["bundles/file.unity3d"]
	}

=====================================
COMPATIBILITY
=====================================

Target Framework: .NET 3.5 (Mono 2.0)
Game Version: Ball 3D 4.21+
C# Version: 3.0

Mono 2.0 notes:
- No async/await, no dynamic
- System.CodeDom missing (runtime compile limited)
- Use for instead of foreach + reflection; prefer ReferenceEquals

=====================================
DEBUG LOGS
=====================================

When the game starts:
- BepInEx/LogOutput.log - main log
- logs/b3dml_debug.log - loader events
- logs/scriptloader.log - script/dll compile

If something breaks, check LogOutput.log first.

=====================================
MORE INFO
=====================================

- ../MOD_CREATION_GUIDE.md - Mod guide
- ../README.md - Overview
- ../root/mods/ExampleMod/ - Template
