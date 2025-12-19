using System;
using BepInEx;

namespace B3DML
{
    [BepInPlugin("com.cerenomy.b3dml", "B3DML Loader", "0.1.0")]
    public class B3DMLBepInExPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            try
            {
                // Bridge into our existing bootstrap
                B3DMLBootstrap.Init();
                Logger.LogInfo("B3DML Bootstrap.Init() invoked via BepInEx.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"B3DML init failed: {ex}");
            }
        }
    }
}
