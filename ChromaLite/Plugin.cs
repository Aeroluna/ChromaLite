using Harmony;
using IPA;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace ChromaLite {
    public class Plugin : IBeatSaberPlugin {

        HarmonyInstance harmony = HarmonyInstance.Create("net.binaryelement.chromalite");

        public void OnApplicationStart() {

            if (IsModInstalled("ChromaToggle") || IsModInstalled("Chroma")) {
                ChromaLogger.Log("ChromaToggle/Chroma Detected, Disabling ChromaLite.");
                ChromaLogger.Log("ChromaToggle/Chroma contains all features (and many more) of ChromaLite.");
                return;
            }

            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            //ChromaLogger.Log("Harmonized");

            // Register capabilities for songcore
            if (IsModInstalled("SongCore")) RegisterCapabilities();
        }

        public void RegisterCapabilities()
        {
            SongCore.Collections.RegisterCapability("ChromaLite");
            SongCore.Collections.RegisterCapability("Chroma Lighting Events");
            SongCore.Collections.RegisterCapability("Chroma Special Events");
        }

        public void OnActiveSceneChanged(Scene current, Scene next) {
            if (current.name == "GameCore") {
                if (next.name != "GameCore") {
                    //ChromaLogger.Log("Transitioning out of GameCore");
                    return;
                }
            } else {
                if (next.name == "GameCore") {
                    //ChromaLogger.Log("Transitioning into GameCore");
                    new GameObject("ChromaLiteReader").AddComponent<ChromaLiteBehaviour>();
                    return;
                }
            }
        }

        public void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {

        }
        public void OnSceneUnloaded(Scene scene)
        {

        }

        public void OnApplicationQuit() {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnLevelWasLoaded(int level) {

        }

        public void OnLevelWasInitialized(int level) {

        }

        public void OnUpdate() {

        }

        public void OnFixedUpdate() {

        }
        public void Init(object thisIsNull, IPALogger pluginLogger)
        {

            ChromaLogger.logger = pluginLogger;
        }

        public static bool IsModInstalled(string ModName)
        {
            foreach (var mod in IPA.Loader.PluginManager.Plugins)
            {
                if (mod.Name == ModName)
                    return true;
            }
            foreach (var mod in IPA.Loader.PluginManager.AllPlugins)
            {
                if (mod.Metadata.Id == ModName)
                    return true;
            }
            return false;
        }
    }
}
