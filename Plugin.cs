using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using ProjectM;
using ShardExtraLife.Configs;
using ShardExtraLife.Hooks;
using ShardExtraLife.Utils;
using System.Reflection;
using VampireCommandFramework;
namespace ShardExtraLife
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
   // [BepInDependency("gg.deca.VampireCommandFramework")]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Logger;
        internal static Plugin Instance;

        Harmony _harmony;

        public override void Load()
        {
            Instance = this;
            Logger = Log;

            // Plugin startup logic
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
            MainConfig.SettingsInit();
            // Harmony patching
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            LoadPersistenceSystem_Patch.GamedataInit += GameDataInit;
            CommandRegistry.RegisterAll(Assembly.GetExecutingAssembly());
        }
        public static void GameDataInit()
        {         
            ShardExtraLife.InitData();
            ShardExtraLife.ChangeLifeTime();
            ShardExtraLife.UpdateShardslist();
        }

        public override bool Unload()
        {
            _harmony?.UnpatchSelf();
            return true;
        }

    }
}
