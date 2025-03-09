# BALL 3D

---

## Library

| Project Name       | Features                            | Compatibility  | Download                  |
|-----------------|--------------------------------------|----------------|----------------------------|
| enhancedFps | Helps to arrange maxFps and vSync options. (eliminating input lag) | slow_2020       | [0.1](https://github.com/yoareh/ball3d/raw/main/enhancedFps/0.1/0.1.zip) |
| enhancedFps + Corrected Shift (CS) | enhancedFps features and corrected shift. If you use mouse rotation, it helps to control your shift (vertical kick) curve more smoothly. | slow_2020       | [0.1](https://github.com/yoareh/ball3d/raw/main/enhancedFps/0.1[shiftcorrected]/0.1[shiftcorrected].zip) |
|slow_2020+|[Features](https://github.com/yoareh/ball3d/blob/main/slow2020plus.md)|slow_2020|[0.1](https://github.com/yoareh/ball3d/raw/main/slow2020+/0.1/Assembly-UnityScript.dll)|

## Installation
**How To Install:**

Just click to chosen version of any patch on "Download" section.

After installation, extract zip to "...\Steam\steamapps\common\Ball 3D\Ball 3D_Data" and replace "Assembly-UnityScript.dll". That's it!

**Reliability**

Do you think about safety? For sure it is safe, you can scan all files with any anti-virus client. e.g. virustotal

## Trust Issue
If you don't trust .dll files, you can build your one!

First of all, you have to install **.NET decompiler**, to open and edit your **.dll** file. I recommend **dnSpyEx**, but you can also use **dnSpy** which is outdated. (**dotPeek** and **ILSpy** are alternatives)

1) https://github.com/dnSpyEx/dnSpy/releases
2) https://github.com/dnSpy/dnSpy/releases

(Advanced Users can also use **ghidra**.)

Secondly, open **dnSpy** and load **Assembly-UnityScript.dll** file from your game directory. (And dive into dnSpy. Try to learn! It will be helpful)

Thirdly, you have to change some of classes. Look inside of the **Assemblies** folder in the repository. There are **.cs** files in **Assemblies** folder. Install which you need.

Now you have to find all classes of the **Assembly-UnityScript.dll** file from dnSpy (generally classes are in: **{}**). Replace your chosen assemblies (.cs files) with **Assembly-UnityScript.dll** classes.

When you finish replacements, only thing to do is **saving all modules**. Good luck!

### Definitions
**enhancedFps**

(.dll) FPSDisplayer.cs -> has to be changed with Assemblies/FPSDisplayer.cs

**enhancedFps[CS]**

(.dll) FPSDisplayer.cs -> has to be changed with Assemblies/FPSDisplayer.cs

(.dll) HeroInput.cs -> has to be changed with Assemblies/HeroInput.cs
