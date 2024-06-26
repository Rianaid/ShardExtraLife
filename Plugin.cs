﻿using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using ShardExtraLife.Configs;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using System.Reflection;
using VampireCommandFramework;
namespace ShardExtraLife
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("gg.deca.VampireCommandFramework")]
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
            MainConfig.ConfigInit();
            // Harmony patching
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            CommandRegistry.RegisterAll(Assembly.GetExecutingAssembly());
        }
        public static void GameDataInit()
        {
            BaseLoop.Initialize();
            ShardDropper.ClearDropTable();
            ShardUtils.ChangeLifeTime();
            DB.ShardSpellPrefabs.Clear();
            DB.ShardSpellPrefabs.Add(Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary["AB_Blood_BloodStorm_AbilityGroup"]);
            DB.ShardSpellPrefabs.Add(Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary["AB_Storm_EyeOfTheStorm_AbilityGroup"]);
            DB.ShardSpellPrefabs.Add(Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary["AB_Chaos_Voidquake_AbilityGroup"]);
            DB.ShardSpellPrefabs.Add(Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary["AB_Unholy_SummonFallenAngel_AbilityGroup"]);
        }
        public override bool Unload()
        {
            _harmony?.UnpatchSelf();
            return true;
        }
    }
}
