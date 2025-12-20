## Disclaimer

This repository contains multiple fan-made mods for the game *Ball 3D: Soccer Online*. These projects are unofficial and are not affiliated with, endorsed by, or sponsored by the original developers or publishers of the game. All trademarks, copyrighted materials, and proprietary content related to the original game remain the property of their respective owners.

Some parts of the code in these mods may reference or depend on elements of the original game's code or behavior. Such references are included solely for compatibility, interoperability, and educational purposes. No unauthorized redistribution of proprietary assets is intended.

These mods are provided in good faith under the assumption that modding and community-created content are allowed within the boundaries of the game's terms of service and license agreements. If you are a copyright holder and have concerns about any content in this repository, please contact me (vga64@outlook.com.tr) directly, and I will take immediate action to address the issue.

## Library

| Project Name       | Features                            | Compatibility  | Download                  |
|-----------------|--------------------------------------|----------------|----------------------------|
| enhancedFps | Helps to arrange maxFps and vSync options. (eliminating input lag) | slow_2020       | [0.1](https://github.com/vga64/ball3d/raw/main/enhancedFps/0.1/0.1.zip) |
| enhancedFps + Corrected Shift (CS) | enhancedFps features and corrected shift. If you use mouse rotation, it helps to control your shift (vertical kick) curve more smoothly. | slow_2020       | [0.1](https://github.com/yoareh/vga64/raw/main/enhancedFps/0.1[shiftcorrected]/0.1[shiftcorrected].zip) |
|slow_2020+|[Features](https://github.com/yoareh/ball3d/blob/main/slow2020plus.md)|slow_2020|[0.1](https://github.com/vga64/ball3d/raw/main/slow2020+/0.1/Assembly-UnityScript.dll)|
|sp+|-|sprint (latest)|[0.1]|
|B3DML| The first ever universal modloader for Ball 3D!|sp/slow/slow2020 (all of them)|[0.1](https://github.com/vga64/ball3d/tree/main/B3DML/0.1)|

## Installation
**How To Install**

Just click to chosen version of any patch on "Download" section.

After installation, extract zip to "...\Steam\steamapps\common\Ball 3D\Ball 3D_Data" and replace "Assembly-UnityScript.dll". That's it!

**Reliability**

Do you think about safety? For sure it is safe, you can scan all files with any anti-virus client. e.g. virustotal

## Building Custom Modules
If you don't trust .dll files or if you want to make some other mods with custom classes - you can build your one!

First of all, you have to install **.NET decompiler**, to open and edit your **.dll** file. I recommend **dnSpyEx**, but you can also use **dnSpy** which is outdated. (**dotPeek** and **ILSpy** are alternatives)

1) https://github.com/dnSpyEx/dnSpy/releases
2) https://github.com/dnSpy/dnSpy/releases

(Advanced Users can also use **ghidra**.)

Secondly, open **dnSpy** and load **Assembly-UnityScript.dll** file from your game directory. (And dive into dnSpy. Try to learn! It will be helpful)

Thirdly, you have to change some of classes. Look inside of the **Assemblies** folder in the repository. There are **.cs** files in **Assemblies** folder. Install which you need.

Now you have to find all classes of the **Assembly-UnityScript.dll** file from dnSpy (generally classes are in: **{}**). Replace your chosen assemblies (.cs files) with **Assembly-UnityScript.dll** classes.

When you finish replacements, only thing to do is **saving all modules**. Good luck!

**Warning!** Some of classes have strong restrictions to recompile, as an example: **GlobalManager**. If you want to build your .dll, then you have to use some techniques for replacing chosen assembly with game .dll's class. You can contact with me from discord: **viyuv**. I am here for your all questions.

## Using Custom Classes/Modules

Do you want to make your own mod customized with my custom classes/modules? Sure, you can use all of the custom classes/modules. But please give credit to me. Nothing more I want :)
