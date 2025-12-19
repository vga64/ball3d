using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace B3DML.BundleLoader
{
    public class BundleLoader
    {
        private static Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();
        
        public static void LoadModBundles(B3DML.Core.ModInfo mod)
        {
            if (mod.Bundles == null || mod.Bundles.Count == 0) return;
                
            foreach (string bundlePath in mod.Bundles)
            {
                string fullPath = Path.Combine(mod.ModPath, bundlePath);
                
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning($"[B3DML] Bundle not found: {fullPath}");
                    continue;
                }
                
                try
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(fullPath);
                    
                    if (bundle != null)
                    {
                        loadedBundles[bundlePath] = bundle;
                        string bundleName = Path.GetFileName(bundlePath);
                        if (bundleName != bundlePath)
                            loadedBundles[bundleName] = bundle;
                        
                        Debug.Log($"[B3DML] Loaded bundle: {bundlePath}");
                        
                        string[] assetNames = bundle.GetAllAssetNames();
                        for (int i = 0; i < assetNames.Length; i++)
                            Debug.Log($"[B3DML]   - Asset: {assetNames[i]}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[B3DML] Failed to load bundle {bundlePath}: {e.Message}");
                }
            }
        }
        
        public static AssetBundle GetBundle(string bundleName) =>
            loadedBundles.ContainsKey(bundleName) ? loadedBundles[bundleName] : null;
        
        public static T LoadAsset<T>(string bundlePath, string assetName) where T : UnityEngine.Object
        {
            if (!loadedBundles.ContainsKey(bundlePath))
            {
                Debug.LogWarning($"[B3DML] Bundle not loaded: {bundlePath}");
                return null;
            }
            return loadedBundles[bundlePath].LoadAsset<T>(assetName);
        }
        
        public static GameObject InstantiateFromBundle(string bundlePath, string assetName, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = LoadAsset<GameObject>(bundlePath, assetName);
            if (prefab != null)
                return UnityEngine.Object.Instantiate(prefab, position, rotation);
            return null;
        }
        
        public static void UnloadAllBundles()
        {
            foreach (AssetBundle bundle in loadedBundles.Values)
                bundle.Unload(true);
            loadedBundles.Clear();
        }
    }
}
