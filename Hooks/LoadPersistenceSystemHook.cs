using HarmonyLib;
using ProjectM;

namespace ShardExtraLife.Hooks
{
    [HarmonyPatch(typeof(LoadPersistenceSystemV2), nameof(LoadPersistenceSystemV2.SetLoadState))]
    public class LoadPersistenceSystem_Patch
    {
        public static void Prefix(ServerStartupState.State loadState, LoadPersistenceSystemV2 __instance)
        {
            if (loadState == ServerStartupState.State.SuccessfulStartup)
            {
                Plugin.GameDataInit();
            }
        }
    }
}
