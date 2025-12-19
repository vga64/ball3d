using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Microsoft.CSharp;
using UnityEngine;

namespace B3DML.ScriptLoader
{
    public class ScriptLoader
    {
        private static Dictionary<string, Assembly> compiledScripts = new Dictionary<string, Assembly>();
        private static readonly string LogPath = Path.Combine(Path.Combine(Path.Combine(Application.dataPath, ".."), "logs"), "scriptloader.log");
        private static bool resolveHooked;
        private static string cachedCSharpPath;

        static void EnsureCSharpAssembly()
        {
            if (!resolveHooked)
            {
                resolveHooked = true;
                try
                {
                    AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                    {
                        try
                        {
                            var an = new AssemblyName(args.Name);
                            if (!an.Name.Equals("Microsoft.CSharp", StringComparison.OrdinalIgnoreCase))
                                return null;
                            foreach (var p in GetCSharpCandidates())
                            {
                                if (!string.IsNullOrEmpty(p) && File.Exists(p))
                                {
                                    cachedCSharpPath = p;
                                    return Assembly.LoadFrom(p);
                                }
                            }
                        }
                        catch { }
                        return null;
                    };
                }
                catch { }
            }
        }

        static IEnumerable<string> GetCSharpCandidates()
        {
            string gameRoot = Path.Combine(Application.dataPath, "..");
            string managedDir = Path.Combine(Application.dataPath, "Managed");
            string bepinCoreDir = Path.Combine(Path.Combine(gameRoot, "BepInEx"), "core");
            var candidates = new List<string>();
            candidates.Add(Path.Combine(managedDir, "Microsoft.CSharp.dll"));
            candidates.Add(Path.Combine(bepinCoreDir, "Microsoft.CSharp.dll"));
            candidates.Add("/usr/lib/mono/2.0/Microsoft.CSharp.dll");
            candidates.Add("/usr/lib/mono/3.5/Microsoft.CSharp.dll");
            candidates.Add("/usr/lib/mono/4.0-api/Microsoft.CSharp.dll");
            return candidates;
        }
        
        public static void LoadModScripts(B3DML.Core.ModInfo mod, Harmony harmony)
        {
            if (mod.Scripts == null || mod.Scripts.Count == 0) return;

            EnsureCSharpAssembly();
            if (harmony == null)
            {
                try { harmony = new Harmony("B3DML.ScriptLoader.Fallback"); }
                catch { return; }
            }
                
            foreach (string scriptPath in mod.Scripts)
            {
                string fullPath = Path.Combine(mod.ModPath, scriptPath);
                if (!File.Exists(fullPath)) continue;
                
                try
                {
                    Assembly scriptAssembly = CompileScript(fullPath);
                    if (scriptAssembly != null)
                    {
                        harmony.PatchAll(scriptAssembly);
                        compiledScripts[scriptPath] = scriptAssembly;
                        Debug.Log($"[B3DML] Compiled and patched: {scriptPath}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[B3DML] Failed to load {scriptPath}: {e.Message}");
                }
            }
        }
        
        static Assembly CompileScript(string scriptPath)
        {
            try
            {
                string scriptCode = File.ReadAllText(scriptPath);
                string managedDir = Path.Combine(Application.dataPath, "Managed");
                string bepinCoreDir = Path.Combine(Path.Combine(Path.Combine(Application.dataPath, ".."), "BepInEx"), "core");
                
                var csharpType = Type.GetType("Microsoft.CSharp.CSharpCodeProvider, Microsoft.CSharp, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                if (csharpType == null)
                    csharpType = Type.GetType("Microsoft.CSharp.CSharpCodeProvider, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                
                if (csharpType == null) return null;
                
                CodeDomProvider codeProvider = null;
                try { codeProvider = (CodeDomProvider)Activator.CreateInstance(csharpType); }
                catch { return null; }
                
                var parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                parameters.GenerateExecutable = false;
                parameters.TreatWarningsAsErrors = false;
                
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                parameters.ReferencedAssemblies.Add("System.Data.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.dll");
                
                string mcsPath = cachedCSharpPath;
                if (string.IsNullOrEmpty(mcsPath) || !File.Exists(mcsPath))
                {
                    foreach (var p in GetCSharpCandidates())
                        if (File.Exists(p)) { mcsPath = p; break; }
                }
                if (File.Exists(mcsPath))
                    parameters.ReferencedAssemblies.Add(mcsPath);
                
                parameters.ReferencedAssemblies.Add(typeof(UnityEngine.MonoBehaviour).Assembly.Location);
                
                string harmonyPath = Path.Combine(managedDir, "0Harmony.dll");
                if (!File.Exists(harmonyPath))
                    harmonyPath = Path.Combine(bepinCoreDir, "0Harmony.dll");
                if (File.Exists(harmonyPath))
                    parameters.ReferencedAssemblies.Add(harmonyPath);
                
                string[] gameAssemblies = { "Assembly-CSharp.dll", "Assembly-CSharp-firstpass.dll", "Assembly-UnityScript.dll", "Assembly-UnityScript-firstpass.dll" };
                foreach (string asmName in gameAssemblies)
                {
                    string asmPath = Path.Combine(managedDir, asmName);
                    if (File.Exists(asmPath))
                        parameters.ReferencedAssemblies.Add(asmPath);
                }
                
                CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, scriptCode);
                
                if (results.Errors.HasErrors)
                {
                    Debug.LogError($"[B3DML] Compilation errors in {scriptPath}:");
                    foreach (CompilerError error in results.Errors)
                        Debug.LogError($"  Line {error.Line}: {error.ErrorText}");
                    return null;
                }
                
                Debug.Log($"[B3DML] Compiled {Path.GetFileName(scriptPath)}");
                return results.CompiledAssembly;
            }
            catch (Exception e)
            {
                Debug.LogError($"[B3DML] Script compilation failed: {e.Message}");
                return null;
            }
        }

        private static void FileLog(string message)
        {
            try
            {
                var dir = Path.GetDirectoryName(LogPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.AppendAllText(LogPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + message + Environment.NewLine);
            }
            catch { }
        }
    }
}
