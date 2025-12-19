using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace B3DML.DLLInjector
{
    public class DLLInjector
    {
        internal static Harmony harmony;
        
        public static void Initialize()
        {
            harmony = new Harmony("B3DML.ModLoader");
            Debug.Log("[B3DML] Harmony initialized");
        }
        
        public static void LoadModDLLs(B3DML.Core.ModInfo mod)
        {
            string logPath = Path.Combine(Path.Combine(Application.dataPath, ".."), Path.Combine("logs", "dllinjector.log"));
            if (mod.DLLs == null || mod.DLLs.Count == 0) return;
            
            for (int di = 0; di < mod.DLLs.Count; di++)
            {
                string fullPath = Path.Combine(mod.ModPath, mod.DLLs[di]);
                
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning($"[B3DML] DLL not found: {fullPath}");
                    continue;
                }
                
                try
                {
                    Assembly modAssembly = Assembly.LoadFrom(fullPath);
                    Type[] types = modAssembly.GetTypes();
                    
                    for (int ti = 0; ti < types.Length; ti++)
                    {
                        try
                        {
                            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(types[ti].TypeHandle);
                        }
                        catch { }
                    }
                    
                    harmony.PatchAll(modAssembly);
                    Debug.Log($"[B3DML] Loaded and patched: {mod.DLLs[di]}");
                    
                    try { File.AppendAllText(logPath, $"SUCCESS: {mod.DLLs[di]}\n"); }
                    catch { }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[B3DML] Failed to load {mod.DLLs[di]}: {e.Message}");
                    try { File.AppendAllText(logPath, $"ERROR: {e.Message}\n"); }
                    catch { }
                }
            }
        }
        
        public static void PatchType(Type type) => harmony.CreateClassProcessor(type).Patch();
        
        public static void PatchMethod(MethodBase original, HarmonyMethod prefix = null, HarmonyMethod postfix = null) =>
            harmony.Patch(original, prefix, postfix);
    }
}
