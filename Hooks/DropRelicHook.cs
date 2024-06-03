using HarmonyLib;
using ProjectM;
using ProjectM.Shared;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using Unity.Collections;
namespace ShardExtraLife.Hooks
{
    [HarmonyPatch(typeof(DropItemThrowSystem), nameof(DropItemThrowSystem.OnUpdate))]
    public class DropItemSystem_patch
    {
        [HarmonyPrefix]
        public static void postfix(DropItemThrowSystem __instance)
        {
            var RelicsQuery = __instance.__query_2070481711_0.ToEntityArray(Allocator.Temp);
            foreach (var item in RelicsQuery)
            {
                if (Helper.EntityManager.HasComponent<DropItemAroundPosition>(item))
                {
                    var dropItemAroundPosition = Helper.EntityManager.GetComponentData<DropItemAroundPosition>(item);
                    var prefab = dropItemAroundPosition.ItemHash;
                    var name = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary[prefab];
                    if (DB.NewShardNames.Contains(name) || DB.OldShardName.Contains(name))
                    {
                        BaseLoop.NeedUpdateShardList = true;
                    }
                }
            }
            RelicsQuery.Dispose();
        }
    }
    /*[HarmonyPatch(typeof(ItemPickupSystem), nameof(ItemPickupSystem.OnUpdate))]
    public class ItemPickupSystem_patch
    {
        [HarmonyPostfix]
        public static void postfix(ItemPickupSystem __instance)
        {
           // ShardUtils.UpdateShardslist();
        }
    }
    */
    [HarmonyPatch(typeof(RelicDestroySystem), nameof(RelicDestroySystem.OnUpdate))]
    public class RelicDestroySystem_patch
    {
        [HarmonyPostfix]
        public static void postfix(RelicDestroySystem __instance)
        {
            var RelicsQuery = __instance._RelicsQuery.ToEntityArray(Allocator.Temp);
            foreach (var item in RelicsQuery)
            {
                if (Helper.EntityManager.HasComponent<Relic>(item))
                {
                    BaseLoop.NeedUpdateShardList = true;
                }
            }
            RelicsQuery.Dispose();
        }
    }
}
