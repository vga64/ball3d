using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace B3DML.Core
{
    public class B3DMLCore : MonoBehaviour
    {
        public static B3DMLCore Instance { get; private set; }
        public List<ModInfo> loadedMods = new List<ModInfo>();
        private string modsDirectory;
        private string debugLogPath;
        
        void Awake()
        {
            if (Instance != null) { Destroy(this); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            try
            {
                var dllInjectorType = System.Type.GetType("B3DML.DLLInjector.DLLInjector, B3DML.DLLInjector");
                var initMethod = dllInjectorType?.GetMethod("Initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                initMethod?.Invoke(null, null);
            }
            catch { }
            
            string gameRoot = Path.Combine(Application.dataPath, "..");
            string logsDir = Path.Combine(gameRoot, "logs");
            if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);
            debugLogPath = Path.Combine(logsDir, "b3dml_debug.log");
            
            FileLog("=== B3DML Core Awake Start ===");
            
            modsDirectory = Path.Combine(gameRoot, "mods");
            if (!Directory.Exists(modsDirectory)) Directory.CreateDirectory(modsDirectory);
            
            FileLog($"Mods directory: {modsDirectory}");
            Initialize();
        }
        
        void Initialize()
        {
            try
            {
                DiscoverMods();
                loadedMods = loadedMods.OrderBy(m => m.Priority).ToList();
                LoadMods();
                Debug.Log($"[B3DML] Loaded {loadedMods.Count} mod(s)");
                FileLog($"Loaded {loadedMods.Count} mod(s) - Initialization complete");
            }
            catch (Exception ex)
            {
                FileLog($"FATAL ERROR: {ex.Message}\n{ex.StackTrace}");
                Debug.LogError($"[B3DML] Fatal error: {ex}");
            }
        }
        
        void DiscoverMods()
        {
            if (!Directory.Exists(modsDirectory)) return;
            string[] modDirs = Directory.GetDirectories(modsDirectory);
            
            foreach (string modDir in modDirs)
            {
                string modJsonPath = Path.Combine(modDir, "mod.json");
                string modTxtPath = Path.Combine(modDir, "mod.txt");
                ModInfo mod = null;
                
                if (File.Exists(modJsonPath))
                    mod = ParseModJson(File.ReadAllText(modJsonPath));
                else if (File.Exists(modTxtPath))
                    mod = ParseModTxt(File.ReadAllText(modTxtPath));
                
                if (mod != null && IsVersionCompatible(mod.TargetGameVersion))
                {
                    mod.ModPath = modDir;
                    loadedMods.Add(mod);
                    Debug.Log($"[B3DML] Discovered: {mod.Name} v{mod.Version}");
                }
            }
        }
        
        ModInfo ParseModJson(string json)
        {
            var mod = new ModInfo();
            try
            {
                var dict = new System.Collections.Generic.Dictionary<string, string>();
                var matches = System.Text.RegularExpressions.Regex.Matches(json, @"""([^""]+)""\s*:\s*""([^""]+)""");
                foreach (System.Text.RegularExpressions.Match m in matches)
                    dict[m.Groups[1].Value] = m.Groups[2].Value;
                
                if (dict.ContainsKey("name")) mod.Name = dict["name"];
                if (dict.ContainsKey("version")) mod.Version = dict["version"];
                if (dict.ContainsKey("author")) mod.Author = dict["author"];
                if (dict.ContainsKey("description")) mod.Description = dict["description"];
                if (dict.ContainsKey("targetGameVersion")) mod.TargetGameVersion = dict["targetGameVersion"];
                
                ParseArray(json, "scripts", mod.Scripts);
                ParseArray(json, "bundles", mod.Bundles);
                ParseArray(json, "dlls", mod.DLLs);
            }
            catch (Exception ex) { FileLog($"ParseModJson error: {ex.Message}"); }
            return mod;
        }
        
        void ParseArray(string json, string key, List<string> list)
        {
            var match = System.Text.RegularExpressions.Regex.Match(json, $@"""{key}""\s*:\s*\[([\s\S]*?)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var itemMatches = System.Text.RegularExpressions.Regex.Matches(match.Groups[1].Value, @"""([^""]+)""");
                foreach (System.Text.RegularExpressions.Match m in itemMatches)
                    list.Add(m.Groups[1].Value);
            }
        }
        
        ModInfo ParseModTxt(string txt)
        {
            var mod = new ModInfo();
            foreach (string line in txt.Split('\n'))
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) continue;
                
                int eq = trimmed.IndexOf('=');
                if (eq < 0) continue;
                
                string key = trimmed.Substring(0, eq).Trim().ToLower();
                string value = trimmed.Substring(eq + 1).Trim();
                
                switch (key)
                {
                    case "name": mod.Name = value; break;
                    case "version": mod.Version = value; break;
                    case "author": mod.Author = value; break;
                    case "description": mod.Description = value; break;
                    case "targetgameversion": mod.TargetGameVersion = value; break;
                    case "priority": int.TryParse(value, out int p); mod.Priority = p; break;
                    case "scripts": AddListItems(value, mod.Scripts); break;
                    case "bundles": AddListItems(value, mod.Bundles); break;
                    case "dlls": AddListItems(value, mod.DLLs); break;
                }
            }
            return mod;
        }
        
        void AddListItems(string value, List<string> list)
        {
            foreach (string item in value.Split(','))
                if (!string.IsNullOrEmpty(item.Trim()))
                    list.Add(item.Trim());
        }
        
        void LoadMods()
        {
            foreach (ModInfo mod in loadedMods)
            {
                try
                {
                    LoadModDLLs(mod);
                    LoadModScripts(mod);
                    Debug.Log($"[B3DML] Loaded mod: {mod.Name}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[B3DML] Failed to load {mod.Name}: {e.Message}");
                }
            }
        }
        
        void LoadModScripts(ModInfo mod)
        {
            try
            {
                var type = System.Type.GetType("B3DML.ScriptLoader.ScriptLoader, B3DML.ScriptLoader");
                if (type != null)
                {
                    var method = type.GetMethod("LoadModScripts", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (method != null)
                    {
                        var harmonyType = System.Type.GetType("B3DML.DLLInjector.DLLInjector, B3DML.DLLInjector");
                        var harmonyField = harmonyType?.GetField("harmony", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                        var harmonyInstance = harmonyField?.GetValue(null);
                        method.Invoke(null, new object[] { mod, harmonyInstance });
                    }
                }
            }
            catch (Exception e) { Debug.LogError($"[B3DML] Script load error: {e.Message}"); }
        }

        void LoadModDLLs(ModInfo mod)
        {
            try
            {
                var type = System.Type.GetType("B3DML.DLLInjector.DLLInjector, B3DML.DLLInjector");
                if (type != null)
                {
                    var method = type.GetMethod("LoadModDLLs", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (method != null)
                        method.Invoke(null, new object[] { mod });
                }
            }
            catch (Exception e) { Debug.LogError($"[B3DML] DLL load error: {e.Message}"); }
        }
        
        bool IsVersionCompatible(string targetVersion) =>
            string.IsNullOrEmpty(targetVersion) || targetVersion == "4.21+" || targetVersion.Contains("4.21");
        
        public static string ResolveAssetPath(string assetPath)
        {
            if (Instance == null) return assetPath;
            foreach (ModInfo mod in Instance.loadedMods)
            {
                string candidate = Path.Combine(mod.ModPath, assetPath);
                if (File.Exists(candidate)) return candidate;
            }
            return assetPath;
        }
        
        void FileLog(string message)
        {
            try { File.AppendAllText(debugLogPath, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + message + System.Environment.NewLine); }
            catch { }
        }
    }
    
    public class ModInfo
    {
        public string Name { get; set; } = "";
        public string Version { get; set; } = "1.0.0";
        public string Author { get; set; } = "";
        public string Description { get; set; } = "";
        public string TargetGameVersion { get; set; } = "4.21+";
        public int Priority { get; set; } = 100;
        public string ModPath { get; set; } = "";
        public List<string> Scripts { get; set; } = new List<string>();
        public List<string> Bundles { get; set; } = new List<string>();
        public List<string> DLLs { get; set; } = new List<string>();
    }
}
