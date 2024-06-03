using BepInEx.Unity.IL2CPP.Utils.Collections;
using Il2CppInterop.Runtime.Injection;
using ProjectM;
using ProjectM.Shared;
using ShardExtraLife.Databases;
using Stunlock.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ShardExtraLife.Utils
{
    public class BaseLoop : MonoBehaviour
    {
        private static BaseLoop _instance;
        public static DateTime lastRepair = DateTime.Now;
        public static DateTime LastUpdate = DateTime.Now;
        internal static bool NeedUpdateShardList = true;

        void Update()
        {
        }

        void Start()
        {
            StartCoroutine(UpdateDurabilityInSpecialStorage().WrapToIl2Cpp());
            StartCoroutine(UpdateShardsList().WrapToIl2Cpp());
        }
        private IEnumerator UpdateShardsList()
        {
            while (true)
            {
                var timespan = DateTime.Now - LastUpdate;
                if (timespan.TotalSeconds > 300) { NeedUpdateShardList = true; }
                if (NeedUpdateShardList)
                {
                    ShardUtils.UpdateShardslist();
                    LastUpdate = DateTime.Now;
                    NeedUpdateShardList = false;
                }
                else
                {
                    ShardUtils.updateStatus();
                }
                yield return new WaitForSeconds(15);
            }
        }
        private IEnumerator UpdateDurabilityInSpecialStorage()
        {
            while (true)
            {
                if (DB.EnabledRepairInAltar)
                {
                    var query = Helper.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
                    { All = new ComponentType[] { ComponentType.ReadOnly<Relic>(), ComponentType.ReadOnly<EquippableData>() }, Options = EntityQueryOptions.IncludeDisabled });
                    var relicEntities = query.ToEntityArray(Allocator.Temp);
                    var ListSoulShardRepair = new List<Entity>();
                    foreach (var entity in relicEntities)
                    {
                        var container = Helper.EntityManager.GetComponentData<InventoryItem>(entity).ContainerEntity;
                        if (container != Entity.Null)
                        {
                            var guid = Helper.EntityManager.GetComponentData<PrefabGUID>(container);
                            if (guid == Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary["External_Inventory"])
                            {
                                var attachParent = Helper.EntityManager.GetComponentData<Attach>(container).Parent;
                                if (attachParent != Entity.Null)
                                {
                                    var guidParent = Helper.EntityManager.GetComponentData<PrefabGUID>(attachParent);
                                    var containerName = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary[guidParent];
                                    if (DB.NewRelicBuildingName.Contains(containerName))
                                    {
                                        ListSoulShardRepair.Add(entity);
                                    }
                                }
                            }

                        }
                    }
                    TimeSpan timeSpan = DateTime.Now - lastRepair;
                    lastRepair = DateTime.Now;
                    foreach (var entity in ListSoulShardRepair)
                    {
                        var durability = Helper.EntityManager.GetComponentData<Durability>(entity);
                        var losedurability = Helper.EntityManager.GetComponentData<LoseDurabilityOverTime>(entity);
                        var repairValue = durability.MaxDurability / losedurability.TimeUntilBroken * (float)timeSpan.TotalSeconds;
                        durability.Value += (repairValue * DB.RepairMultiplier) + DB.AdditionalRepairPoints;
                        if (durability.Value > durability.MaxDurability) { durability.Value = durability.MaxDurability; }
                        Helper.EntityManager.SetComponentData(entity, durability);
                    }
                    ListSoulShardRepair = null;
                }

                yield return new WaitForSeconds(60);
            }
        }

        void LateUpdate()
        {
        }

        public static void Initialize()
        {
            if (!ClassInjector.IsTypeRegisteredInIl2Cpp<BaseLoop>())
            {
                ClassInjector.RegisterTypeInIl2Cpp<BaseLoop>();
            }
            Plugin.Instance.AddComponent<BaseLoop>();

        }

        public static void Uninitialize()
        {
            Destroy(_instance);
            _instance = null;
        }
    }
}
