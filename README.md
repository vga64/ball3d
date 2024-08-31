# BALL 3D

---

## Library

| Project Name       | Explanation                             | Compatibility  | Download                  |
|-----------------|--------------------------------------|----------------|----------------------------|
| enhancedFps | Helps to arrange maxFps and vSync options. (eliminating input lag) | slow_2020       | [0.1](https://github.com/yoareh/ball3d/raw/main/enhancedFps/0.1/0.1.zip) |
| enhancedFps + Corrected Shift (CS) | enhancedFps features and corrected shift. If you use mouse rotation, it helps to control your shift (vertical kick) curve more smoothly. | slow_2020       | [0.1](https://github.com/yoareh/ball3d/raw/main/enhancedFps/0.1[shiftcorrected]/0.1[shiftcorrected].zip) |

## enhancedFps / CS
**How To Install:**

Just click to chosen version of enhancedFps on "Download" section.

After installation, extract "0.x.zip" to "...\Steam\steamapps\common\Ball 3D\Ball 3D_Data" and replace "Assembly-UnityScript.dll". That's it!

**Is This Safe:**

Sure! It is safe, you can scan all files (config.txt | Assembly-UnityScript.dll) with any anti-virus client. e.g. virustotal

## Trust Issue
If you don't trust .dll files, you can build your one!

First of all, you have to install **.NET decompiler**, to open and edit your **.dll** file. I recommend **dnSpyEx**, but you can also use **dnSpy** which is outdated. (**dotPeek** and **ILSpy** are alternatives)

1) https://github.com/dnSpyEx/dnSpy/releases
2) https://github.com/dnSpy/dnSpy/releases

(Advanced Users can also use **ghidra**.)

Secondly, open **dnSpy** and load **Assembly-UnityScript.dll** from your game directory. (And dive into dnSpy. Try to learn! It will be helpful)

Thirdly, you have to change some of classes. Look inside of the **Assemblies** folder in the repository. There are **.cs** files in this **Assemblies** folder. Install them. You have to find all classes of the **Assembly-UnityScript.dll** from dnSpy (generally classes are in: **{}**). Replace defined **Assembly-UnityScript.dll** classes with these (**Assemblies**) **.cs** files.

When you finish replacements, only thing to do is **saving All Modules**. Good luck!

### Definitions
**enhancedFps**

(.dll) FPSDisplayer.cs -> has to be changed with Assemblies/FPSDisplayer.cs

**enhancedFps[CS]**

(.dll) FPSDisplayer.cs -> has to be changed with Assemblies/FPSDisplayer.cs

(.dll) HeroInput.cs -> has to be changed with Assemblies/HeroInput.cs
