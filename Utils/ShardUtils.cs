using ProjectM;
using ProjectM.Shared;
using ShardExtraLife.Databases;
using Stunlock.Core;
using System.Collections.Concurrent;
using Unity.Collections;
using Unity.Entities;

namespace ShardExtraLife.Utils
{
    internal class ShardUtils
    {
        public static void UpdateShardslist()
        {
            var query = Helper.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
            { All = new ComponentType[] { ComponentType.ReadOnly<Relic>() }, Options = EntityQueryOptions.IncludeDisabled });
            var relicEntities = query.ToEntityArray(Allocator.Temp);
            for (RelicType type = RelicType.TheMonster; type <= RelicType.Dracula; type++)
            {
                var data = DB.NewShardsData[type];
                data.Entities.Clear();
                data.Count = data.Entities.Count;
                DB.NewShardsData[type] = data;
                data = DB.OldShardsData[type];
                data.Entities.Clear();
                data.Count = data.Entities.Count;
                DB.OldShardsData[type] = data;
            }

            foreach (var entity in relicEntities)
            {
                var relicType = Helper.EntityManager.GetComponentData<Relic>(entity).RelicType;
                if (Helper.EntityManager.HasComponent<Age>(entity) || Helper.EntityManager.HasComponent<DismantleDestroyData>(entity))
                {
                    DB.OldShardsData.TryGetValue(relicType, out var data);
                    if (!data.Entities.Contains(entity))
                    {
                        data.Entities.Add(entity);
                    }
                    data.Count = data.Entities.Count;
                    DB.OldShardsData[relicType] = data;
                }
                else
                {
                    DB.NewShardsData.TryGetValue(relicType, out var data);
                    if (!data.Entities.Contains(entity))
                    {
                        data.Entities.Add(entity);
                    }
                    data.Count = data.Entities.Count;
                    DB.NewShardsData[relicType] = data;
                }


            }
            updateStatus();
        }

        public static void ChangeLifeTime()
        {
            var PrefabGuidToEntityMap = Helper.PrefabCollectionSystem._PrefabGuidToEntityMap;
            var NameToPrefabGuidDictionary = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary;
            foreach (var key in DB.NewShardNames)
            {
                if (NameToPrefabGuidDictionary.TryGetValue(key, out var name))
                {
                    if (PrefabGuidToEntityMap.TryGetValue(name, out var entity))
                    {
                        if (!DB.EnableUltimateReplace)
                        {
                            if (Helper.EntityManager.HasComponent<EquippableData>(entity))
                            {
                                var equippableData = Helper.EntityManager.GetComponentData<EquippableData>(entity);
                                equippableData.BuffGuid = new PrefabGUID(0);
                                Helper.EntityManager.SetComponentData(entity, equippableData);
                            }
                        }
                        if (Helper.EntityManager.HasComponent<LoseDurabilityOverTime>(entity))
                        {
                            var losedurability = Helper.EntityManager.GetComponentData<LoseDurabilityOverTime>(entity);
                            losedurability.TimeUntilBroken = DB.TimeUntilBroken;
                            Helper.EntityManager.SetComponentData(entity, losedurability);
                        }
                        if (Helper.EntityManager.HasComponent<Durability>(entity))
                        {
                            var durability = Helper.EntityManager.GetComponentData<Durability>(entity);
                            durability.Value = DB.MaxDurability;
                            durability.MaxDurability = DB.MaxDurability;
                            durability.DestroyItemWhenBroken = DB.DestroyItemWhenBroken;
                            Helper.EntityManager.SetComponentData(entity, durability);
                        }
                    }
                }
            }
            foreach (var key in DB.OldShardName)
            {
                if (NameToPrefabGuidDictionary.TryGetValue(key, out var name))
                {
                    if (PrefabGuidToEntityMap.TryGetValue(name, out var entity))
                    {
                        if (Helper.EntityManager.HasComponent<LifeTime>(entity))
                        {
                            var LifeTime = Helper.EntityManager.GetComponentData<LifeTime>(entity);
                            if (DB.DestroyItemWhenBroken)
                            {
                                LifeTime.EndAction = LifeTimeEndAction.Destroy;
                            }
                            else
                            {
                                LifeTime.EndAction = LifeTimeEndAction.None;
                            }
                            LifeTime.Duration = DB.LifeTimeOldShard;
                            Helper.EntityManager.SetComponentData(entity, LifeTime);
                        }
                    }
                }
            }
            if (DB.UpdateExistingShards)
            {
                var query = Helper.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
                { All = new ComponentType[] { ComponentType.ReadOnly<Relic>() }, Options = EntityQueryOptions.IncludeDisabled });
                var relicEntities = query.ToEntityArray(Allocator.Temp);

                foreach (var entity in relicEntities)
                {
                    if (!DB.EnableUltimateReplace)
                    {
                        if (Helper.EntityManager.HasComponent<EquippableData>(entity))
                        {
                            var equippableData = Helper.EntityManager.GetComponentData<EquippableData>(entity);
                            equippableData.BuffGuid = new PrefabGUID(0);
                            Helper.EntityManager.SetComponentData(entity, equippableData);
                        }
                    }
                    if (Helper.EntityManager.HasComponent<LoseDurabilityOverTime>(entity))
                    {
                        var losedurability = Helper.EntityManager.GetComponentData<LoseDurabilityOverTime>(entity);
                        losedurability.TimeUntilBroken = DB.TimeUntilBroken;
                        Helper.EntityManager.SetComponentData(entity, losedurability);
                    }
                    if (Helper.EntityManager.HasComponent<Durability>(entity))
                    {
                        var durability = Helper.EntityManager.GetComponentData<Durability>(entity);
                        if (durability.Value == 0) { durability.Value = 15; }
                        durability.MaxDurability = DB.MaxDurability;
                        durability.DestroyItemWhenBroken = DB.DestroyItemWhenBroken;
                        Helper.EntityManager.SetComponentData(entity, durability);
                    }
                    if (Helper.EntityManager.HasComponent<LifeTime>(entity))
                    {
                        var LifeTime = Helper.EntityManager.GetComponentData<LifeTime>(entity);
                        if (DB.DestroyItemWhenBroken)
                        {
                            LifeTime.EndAction = LifeTimeEndAction.Destroy;
                        }
                        else
                        {
                            LifeTime.EndAction = LifeTimeEndAction.None;
                        }
                        LifeTime.Duration = DB.LifeTimeOldShard;
                        Helper.EntityManager.SetComponentData(entity, LifeTime);
                    }
                }
                relicEntities.Dispose();
            }
        }
        public static void updateStatus()
        {
            EntityQuery query = Helper.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
            { All = new ComponentType[] { ComponentType.ReadOnly<RelicDropped>() }, Options = EntityQueryOptions.IncludeDisabled });
            var relicEntity = query.ToEntityArray(Allocator.Temp)[0];
            for (RelicType relic = RelicType.TheMonster; relic <= RelicType.Dracula; relic++)
            {
                var Newdata = DB.NewShardsData[relic];
                var OldData = DB.OldShardsData[relic];
                bool status = (Newdata.Count < Newdata.MaxCount) || (OldData.Count < OldData.MaxCount);
                var relicdrop = Helper.EntityManager.GetBuffer<RelicDropped>(relicEntity);
                var rd = relicdrop[(int)relic];
                if (rd.Value == status)
                {
                    relicdrop[(int)relic] = new RelicDropped() { Value = !status };
                }
            }
        }
        internal static void UpdateMaxAmountAll(int amount, ConcurrentDictionary<RelicType, ItemsData> ShardData)
        {
            foreach (var types in ShardData.Keys)
            {

                if (ShardData.TryGetValue(types, out var existingData))
                {
                    var updatedData = new ItemsData(existingData.Count, amount)
                    {
                        Entities = existingData.Entities
                    };
                    ShardData.TryUpdate(types, updatedData, existingData);
                }
            }
        }
        internal static void UpdateMaxAmount(int amount, ConcurrentDictionary<RelicType, ItemsData> ShardData, RelicType type)
        {
            if (ShardData.TryGetValue(type, out var existingData))
            {
                var updatedData = new ItemsData(existingData.Count, amount)
                {
                    Entities = existingData.Entities
                };
                ShardData.TryUpdate(type, updatedData, existingData);
            }
        }
        internal static bool RelicTypeCheck(string type, out RelicType relicType)
        {
            if (type.ToLower() == "dracula") { relicType = RelicType.Dracula; return true; }
            else if (type.ToLower() == "wingedhorror") { relicType = RelicType.WingedHorror; return true; }
            else if (type.ToLower() == "solarus") { relicType = RelicType.Solarus; return true; }
            else if (type.ToLower() == "themonster") { relicType = RelicType.TheMonster; return true; }
            else { relicType = RelicType.None; return false; }
        }
    }
}
