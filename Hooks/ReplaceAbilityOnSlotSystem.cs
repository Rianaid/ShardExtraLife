using HarmonyLib;
using ProjectM;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using Unity.Collections;

namespace ShardExtraLife.Hooks
{
    [HarmonyPatch(typeof(ReplaceAbilityOnSlotSystem), nameof(ReplaceAbilityOnSlotSystem.OnUpdate))]
    public class ReplaceAbilityOnSlotSystem_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(ReplaceAbilityOnSlotSystem __instance)
        {
            var query = __instance.__query_1482480545_0.ToEntityArray(Allocator.Temp);
            foreach (var item in query)
            {
                var replacebuffer = Helper.EntityManager.GetBuffer<ReplaceAbilityOnSlotBuff>(item);
                for (var i = 0; i < replacebuffer.Length; i++)
                {
                    var data = replacebuffer[i];
                    if (data.Slot == 7 && DB.ShardSpellPrefabs.Contains(data.NewGroupId))
                    {
                        data.Priority = 1;
                        replacebuffer[i] = data;
                    }
                }
            }
            query.Dispose();
        }
    }
}
