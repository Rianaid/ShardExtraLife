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
            for (RelicTypeMod type = RelicTypeMod.TheMonster; type <= RelicTypeMod.OldBehemoth; type++)
            {
                var data = DB.ShardsData[type];
                data.Entities.Clear();
                data.Count = data.Entities.Count;
                DB.ShardsData[type] = data;
            }

            foreach (var entity in relicEntities)
            {
                var relicType = Helper.EntityManager.GetComponentData<Relic>(entity).RelicType;
                var relicTypeMod = RelicTypeMod.None;
                if (Helper.EntityManager.HasComponent<Age>(entity) || Helper.EntityManager.HasComponent<DismantleDestroyData>(entity))
                {
                    if (relicType == RelicType.Solarus) { relicTypeMod = RelicTypeMod.OldBehemoth; }
                    else if (relicType == RelicType.WingedHorror) { relicTypeMod = RelicTypeMod.OldWingedHorror; }
                    else if (relicType == RelicType.TheMonster) { relicTypeMod = RelicTypeMod.OldTheMonster; }
                    DB.ShardsData.TryGetValue(relicTypeMod, out var data);
                    if (!data.Entities.Contains(entity))
                    {
                        data.Entities.Add(entity);
                    }
                    data.Count = data.Entities.Count;
                    DB.ShardsData[relicTypeMod] = data;
                }
                else
                {
                    if (relicType == RelicType.Solarus) { relicTypeMod = RelicTypeMod.Solarus; }
                    else if (relicType == RelicType.WingedHorror) { relicTypeMod = RelicTypeMod.WingedHorror; }
                    else if (relicType == RelicType.TheMonster) { relicTypeMod = RelicTypeMod.TheMonster; }
                    else if (relicType == RelicType.Dracula) { relicTypeMod = RelicTypeMod.Dracula; }
                    DB.ShardsData.TryGetValue(relicTypeMod, out var data);
                    if (!data.Entities.Contains(entity))
                    {
                        data.Entities.Add(entity);
                    }
                    data.Count = data.Entities.Count;
                    DB.ShardsData[relicTypeMod] = data;
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
                        if (Helper.EntityManager.HasComponent<EquippableData>(entity))
                        {
                            var guid = Helper.EntityManager.GetComponentData<PrefabGUID>(entity);
                            var prefabname = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary[guid];
                            var buffname = DB.ShardNameBuff[prefabname];
                            guid = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary[buffname];
                            var equippableData = Helper.EntityManager.GetComponentData<EquippableData>(entity);
                            equippableData.BuffGuid = guid;
                            Helper.EntityManager.SetComponentData(entity, equippableData);
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
                            durability.DestroyItemWhenBroken = DB.DestroyNewWhenBroken;
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
                            if (DB.DestroyOldWhenBroken)
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
                    if (Helper.EntityManager.HasComponent<EquippableData>(entity))
                    {
                        var guid = Helper.EntityManager.GetComponentData<PrefabGUID>(entity);
                        var prefabname = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary[guid];
                        var buffname = DB.ShardNameBuff[prefabname];
                        guid = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary[buffname];
                        var equippableData = Helper.EntityManager.GetComponentData<EquippableData>(entity);
                        equippableData.BuffGuid = guid;
                        Helper.EntityManager.SetComponentData(entity, equippableData);
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
                        durability.DestroyItemWhenBroken = DB.DestroyNewWhenBroken;
                        Helper.EntityManager.SetComponentData(entity, durability);
                    }
                    if (Helper.EntityManager.HasComponent<LifeTime>(entity))
                    {
                        var LifeTime = Helper.EntityManager.GetComponentData<LifeTime>(entity);
                        if (DB.DestroyOldWhenBroken)
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
                var Newdata = new ItemsData(0, -1);
                var OldData = new ItemsData(0, -1);
                if (relic == RelicType.Solarus) { Newdata = DB.ShardsData[RelicTypeMod.Solarus]; }
                else if (relic == RelicType.WingedHorror) { Newdata = DB.ShardsData[RelicTypeMod.WingedHorror]; OldData = DB.ShardsData[RelicTypeMod.OldWingedHorror]; }
                else if (relic == RelicType.TheMonster) { Newdata = DB.ShardsData[RelicTypeMod.TheMonster]; OldData = DB.ShardsData[RelicTypeMod.OldTheMonster]; }
                else if (relic == RelicType.Dracula) { Newdata = DB.ShardsData[RelicTypeMod.Dracula]; }
                bool status = true;
                if (DB.DropNewShards && DB.DropOldShards)
                {
                    status = (Newdata.Count < Newdata.MaxCount) || (OldData.Count < OldData.MaxCount);
                }
                else if (!DB.DropNewShards && DB.DropOldShards)
                {
                    status = OldData.Count < OldData.MaxCount;
                }
                else if (DB.DropNewShards && !DB.DropOldShards)
                {
                    status = Newdata.Count < Newdata.MaxCount;
                }
                else
                {
                    status = false;
                }
                var relicdrop = Helper.EntityManager.GetBuffer<RelicDropped>(relicEntity);
                var rd = relicdrop[(int)relic];
                if (Helper.serverGameSettings.Settings.RelicSpawnType == RelicSpawnType.Plentiful)
                {
                    relicdrop[(int)relic] = new RelicDropped() { Value = true };
                }
                else
                {
                    if (rd.Value == status)
                    {
                        relicdrop[(int)relic] = new RelicDropped() { Value = !status };
                    }
                }
            }
        }
        internal static void UpdateMaxAmountAll(int amount, ConcurrentDictionary<RelicTypeMod, ItemsData> ShardData)
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
        internal static void UpdateMaxAmount(int amount, ConcurrentDictionary<RelicTypeMod, ItemsData> ShardData, RelicTypeMod type)
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
        internal static bool RelicTypeCheck(string type, out RelicTypeMod relicType)
        {
            if (type.ToLower() == "dracula")
            {
                relicType = RelicTypeMod.Dracula;
                return true;
            }
            else if (type.ToLower() == "solarus")
            {
                relicType = RelicTypeMod.Solarus;
                return true;
            }
            else if (type.ToLower() == "wingedhorror")
            {
                relicType = RelicTypeMod.WingedHorror;
                return true;
            }
            else if (type.ToLower() == "oldwingedhorror")
            {
                relicType = RelicTypeMod.OldWingedHorror;
                return true;
            }
            else if (type.ToLower() == "themonster")
            {
                relicType = RelicTypeMod.TheMonster;
                return true;
            }
            else if (type.ToLower() == "oldthemonster")
            {
                relicType = RelicTypeMod.OldTheMonster;
                return true;
            }
            else if (type.ToLower() == "behemoth")
            {
                relicType = RelicTypeMod.OldBehemoth;
                return true;
            }
            else
            {
                relicType = RelicTypeMod.None;
                return false;
            }
        }
    }
}
