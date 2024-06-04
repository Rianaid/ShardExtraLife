using HarmonyLib;
using ProjectM;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using Stunlock.Core;
using Unity.Collections;

namespace ShardExtraLife.Hooks
{
    [HarmonyPatch(typeof(DeathEventListenerSystem), nameof(DeathEventListenerSystem.OnUpdate))]
    public class DeathEventListenerSystem_patch
    {
        [HarmonyPostfix]
        public static void postfix(DeathEventListenerSystem __instance)
        {
            var deathevent_querry = __instance._DeathEventQuery.ToComponentDataArray<DeathEvent>(Allocator.Temp);
            foreach (var deathevent in deathevent_querry)
            {
                if (__instance.EntityManager.HasComponent<PlayerCharacter>(deathevent.Killer) && __instance.EntityManager.HasComponent<Movement>(deathevent.Died))
                {
                    bool isNPC = __instance.EntityManager.HasComponent<UnitLevel>(deathevent.Died);
                    if (isNPC)
                    {
                        var prefabguid = __instance.EntityManager.GetComponentData<PrefabGUID>(deathevent.Died);
                        var guidtoname = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary;
                        if (DB.BossNames.TryGetValue(guidtoname[prefabguid], out var relicType))
                        {
                            ShardDropper.ChoiceDropSystem(relicType, deathevent);
                        }
                    }
                }
            }
            deathevent_querry.Dispose();
        }
    }
}
