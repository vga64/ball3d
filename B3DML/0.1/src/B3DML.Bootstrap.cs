using System.IO;
using UnityEngine;

namespace B3DML
{
    public class B3DMLBootstrap
    {
        private static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnGameStart() => Init();

        public static void Init()
        {
            if (initialized) return;
            initialized = true;

            try
            {
                string gameRoot = Path.Combine(Application.dataPath, "..");
                string logsDir = Path.Combine(gameRoot, "logs");
                if (!System.IO.Directory.Exists(logsDir))
                    System.IO.Directory.CreateDirectory(logsDir);
                string logPath = Path.Combine(logsDir, "b3dml_bootstrap_touch.txt");
                File.AppendAllText(logPath, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + " bootstrap start" + System.Environment.NewLine);
            }
            catch { }

            Debug.Log("[B3DML] === BOOTSTRAP STARTING ===");
            GameObject b3dmlObject = new GameObject("B3DML_Core");
            b3dmlObject.AddComponent<Core.B3DMLCore>();
            Debug.Log("[B3DML] B3DMLCore component added");
            DLLInjector.DLLInjector.Initialize();
            Debug.Log("[B3DML] DLL Injector initialized");
            Debug.Log("[B3DML] === BOOTSTRAP COMPLETED ===");
        }
    }
}

namespace Doorstop
{
    public static class Entrypoint
    {
        private static bool hooked;
        
        public static void Start()
        {
            try
            {
                if (!hooked)
                {
                    hooked = true;
                    UnityEngine.Application.logMessageReceived += OnLogOnce;
                }
            }
            catch { }
        }

        private static void OnLogOnce(string condition, string stackTrace, UnityEngine.LogType type)
        {
            try
            {
                UnityEngine.Application.logMessageReceived -= OnLogOnce;
                B3DML.B3DMLBootstrap.Init();
            }
            catch { }
        }
    }
}

