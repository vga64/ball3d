using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace B3DML.AssetLoader
{
    public class AssetLoader
    {
        public static Texture2D LoadTexture2D(string path)
        {
            string modPath = B3DML.Core.B3DMLCore.ResolveAssetPath(path);
            
            if (modPath != path && File.Exists(modPath))
            {
                byte[] data = File.ReadAllBytes(modPath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(data))
                {
                    Debug.Log($"[B3DML] Loaded Texture2D: {modPath}");
                    return texture;
                }
            }
            
            return Resources.Load<Texture2D>(path);
        }
        
        public static Sprite LoadSprite(string path)
        {
            string modPath = B3DML.Core.B3DMLCore.ResolveAssetPath(path);
            
            if (modPath != path && File.Exists(modPath))
            {
                Texture2D texture = LoadTexture2D(path);
                if (texture != null)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    return sprite;
                }
            }
            
            return Resources.Load<Sprite>(path);
        }
        
        public static IEnumerator LoadAudioClipAsync(string path, Action<AudioClip> callback)
        {
            string modPath = B3DML.Core.B3DMLCore.ResolveAssetPath(path);
            
            if (modPath != path && File.Exists(modPath))
            {
                string fileUrl = "file://" + modPath;
                using (WWW www = new WWW(fileUrl))
                {
                    yield return www;
                    if (string.IsNullOrEmpty(www.error))
                    {
                        AudioClip clip = www.GetAudioClip(false, false);
                        callback?.Invoke(clip);
                        yield break;
                    }
                }
            }
            
            callback?.Invoke(Resources.Load<AudioClip>(path));
        }
        
        public static Font LoadFont(string path)
        {
            string modPath = B3DML.Core.B3DMLCore.ResolveAssetPath(path);
            if (modPath != path && File.Exists(modPath))
            {
                Font font = new Font();
                font.fontNames = new string[] { Path.GetFileNameWithoutExtension(modPath) };
                return font;
            }
            return Resources.Load<Font>(path);
        }
        
        public static Shader LoadShader(string path)
        {
            string modPath = B3DML.Core.B3DMLCore.ResolveAssetPath(path);
            if (modPath != path && File.Exists(modPath))
                return Shader.Find(Path.GetFileNameWithoutExtension(modPath));
            return Resources.Load<Shader>(path);
        }
        
        public static TextAsset LoadTextAsset(string path) =>
            Resources.Load<TextAsset>(B3DML.Core.B3DMLCore.ResolveAssetPath(path));
        
        public static Mesh LoadMesh(string path) =>
            Resources.Load<Mesh>(B3DML.Core.B3DMLCore.ResolveAssetPath(path));
        
        public static T LoadMonoBehaviourConfig<T>(string path) where T : class
        {
            string modPath = B3DML.Core.B3DMLCore.ResolveAssetPath(path);
            if (File.Exists(modPath))
                return JsonUtility.FromJson<T>(File.ReadAllText(modPath));
            
            TextAsset asset = Resources.Load<TextAsset>(path);
            return asset != null ? JsonUtility.FromJson<T>(asset.text) : null;
        }
    }
}
