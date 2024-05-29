using ProjectM.Shared;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using Stunlock.Core;
using System;
using System.IO;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Authoring;

namespace ShardExtraLife
{
    internal class ShardExtraLife
    {
        public static void UpdateShardslist()
        {
            var query = Helper.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
            { All = new ComponentType[] { ComponentType.ReadOnly<Relic>() }, Options = EntityQueryOptions.IncludeDisabled });
            var relicEntities = query.ToEntityArray(Allocator.Temp);

            foreach (var entity in relicEntities)
            {
                var relicType = Helper.EntityManager.GetComponentData<Relic>(entity).RelicType;
                DB.ShardsData.TryGetValue(relicType, out var data);
                if (!data.Entities.Contains(entity))
                {
                    data.Entities.Add(entity);
                }
                data.Count = data.Entities.Count;
                DB.ShardsData[relicType] = data;
            }
            foreach (var key in DB.ShardsData.Keys)
            {
                var data = DB.ShardsData[key];
            }
            updateStatus();
        }
        public static void removeShardFromlist(Entity entity)
        {
            var relicType = Helper.EntityManager.GetComponentData<Relic>(entity).RelicType;
            DB.ShardsData.TryGetValue(relicType, out var data);
            if (data.Entities.Contains(entity))
            {
                data.Entities.Remove(entity);
                data.Count = data.Entities.Count;
                DB.ShardsData[relicType] = data;
            }
        }
        public static void ChangeLifeTime()
        {
            var PrefabGuidToEntityMap = Helper.PrefabCollectionSystem._PrefabGuidToEntityMap;
            var NameToPrefabGuidDictionary = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary;
            foreach (var key in DB.ShardNames)
            {
                if (NameToPrefabGuidDictionary.TryGetValue(key, out var name))
                {
                    if (PrefabGuidToEntityMap.TryGetValue(name, out var entity))
                    {
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
        }
        public static void InitData()
        {
            int amount = 1;
            for (RelicType type = RelicType.TheMonster; type <= RelicType.Dracula; type++)
            {
                if (type == RelicType.TheMonster) { amount = DB.MaxShardAmountTheMonster; }
                if (type == RelicType.Solarus) { amount = DB.MaxShardAmountSolarus; }
                if (type == RelicType.Dracula) { amount = DB.MaxShardAmountDracula; }
                if (type == RelicType.WingedHorror) { amount = DB.MaxShardAmountWingedHorror; }
                DB.ShardsData.TryAdd(type, new ItemsData(0, amount));
            }
        }
        public static void updateStatus()
        {
            EntityQuery query = Helper.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
            { All = new ComponentType[] { ComponentType.ReadOnly<RelicDropped>() }, Options = EntityQueryOptions.IncludeDisabled });
            var relicEntity = query.ToEntityArray(Allocator.Temp)[0];
            for (RelicType relic = RelicType.TheMonster; relic <= RelicType.Dracula; relic++)
            {
                var data = DB.ShardsData[relic];
                var status = data.Count < data.MaxCount;
                var relicdrop = Helper.EntityManager.GetBuffer<RelicDropped>(relicEntity);
                var rd = relicdrop[((int)relic)];
                if (rd.Value == status)
                {
                    relicdrop[((int)relic)] = new RelicDropped() { Value = !status };
                }
            }
        }
    }
}
