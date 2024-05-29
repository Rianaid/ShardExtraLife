using HarmonyLib;
using ProjectM;
using ProjectM.Shared;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using System.IO;
using Unity.Collections;
using Unity.Entities;

namespace ShardExtraLife.Hooks
{
    [HarmonyPatch(typeof(DropItemThrowSystem), nameof(DropItemThrowSystem.OnUpdate))]
    public class DropItemSystem_patch
    {
        [HarmonyPostfix]
        public static void postfix(DropItemThrowSystem __instance)
        {
            ShardExtraLife.UpdateShardslist();
        }
    }
    [HarmonyPatch(typeof(ItemPickupSystem), nameof(ItemPickupSystem.OnUpdate))]
    public class ItemPickupSystem_patch
    {
        [HarmonyPostfix]
        public static void postfix(ItemPickupSystem __instance)
        {
            ShardExtraLife.UpdateShardslist();
        }
    }

    [HarmonyPatch(typeof(RelicDestroySystem), nameof(RelicDestroySystem.OnUpdate))]
    public class RelicDestroySystem_patch
    {
        [HarmonyPostfix]
        public static void postfix(RelicDestroySystem __instance)
        {
            var entities = __instance.__query_1425231924_0.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                if (Helper.EntityManager.HasComponent<Relic>(entity))
                {
                    Helper.EntityCompomponentDumper("ModLogs/RelicDestroy.json", entity);
                    ShardExtraLife.removeShardFromlist(entity);
                }
            }
            ShardExtraLife.UpdateShardslist();
        }
    }
}
