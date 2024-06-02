using ProjectM;
using ProjectM.Shared;
using ShardExtraLife.Databases;
using Stunlock.Core;
using System;
using Unity.Entities;
using VampireCommandFramework;


namespace ShardExtraLife.Utils
{
    internal class ShardDropper
    {
        internal static Random random = new Random();
        public static void DropShard(Entity dropperEntity, PrefabGUID prefabGUID, int amount)
        {
            InventoryUtilitiesServer.CreateDropItem(Helper.EntityManager, dropperEntity, prefabGUID, amount, default(Entity));
        }
        public static void Randomizer(out bool dropOld, out bool dropNew)
        {
            dropOld = false;
            dropNew = false;
            var chance = random.NextDouble();
            if (chance < DB.ChanceDropOldShard)
            {
                dropOld = true;
            }
            else if (chance < (DB.ChanceDropOldShard + DB.ChanceDropNewShard))
            {
                dropNew = true;
            }
        }
        public static void ChoiceDropSystem(RelicType relicType, Entity DropperEntity)
        {
            var NametoGuid = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary;
            var prefabNew = NametoGuid[DB.NewShard[relicType]];
            var prefabOld = NametoGuid[DB.OldShard[relicType]];
            var canDropNew = DB.NewShardsData[relicType].canDrop();
            var canDropOld = DB.OldShardsData[relicType].canDrop();
            if (Helper.serverGameSettings.Settings.RelicSpawnType == RelicSpawnType.Plentiful)
            {
                canDropNew = true;
                canDropOld = true;
            }
            Randomizer(out var dropOld, out var dropNew);
            if (DB.DropNewShards && DB.DropOldShards)
            {
                if (DB.DropNewAndOldShardTogether)
                {
                    if (canDropOld)
                    {
                        DropShard(DropperEntity, prefabOld, 1);
                    }
                    if (canDropNew)
                    {
                        DropShard(DropperEntity, prefabNew, 1);
                    }
                }
                else
                {
                    if (dropOld && canDropOld)
                    {
                        DropShard(DropperEntity, prefabOld, 1);
                    }
                    if (dropNew && canDropNew)
                    {
                        DropShard(DropperEntity, prefabNew, 1);
                    }
                }
            }
            else if (DB.DropNewShards && !DB.DropOldShards)
            {
                if (DB.UseDropChanceForNewShard)
                {
                    if (dropNew && canDropNew)
                    {
                        DropShard(DropperEntity, prefabNew, 1);
                    }
                    else
                    {
                        if (canDropNew)
                        {
                            DropShard(DropperEntity, prefabNew, 1);
                        }
                    }
                }
            }
            else if (!DB.DropNewShards && DB.DropOldShards)
            {
                if (relicType == RelicType.Dracula)
                {
                    Plugin.Logger.LogWarning($"Dracula Shard can't drop because New Shards off.");
                    return;
                }
                if (DB.UseDropChanceForOldShard)
                {
                    if (dropOld && canDropOld)
                    {
                        DropShard(DropperEntity, prefabOld, 1);
                    }
                }
                else
                {
                    if (canDropOld)
                    {
                        DropShard(DropperEntity, prefabOld, 1);
                    }
                }
            }
            else
            {
                Plugin.Logger.LogWarning($"Shard dropper not active. Shards not droping!!!");
            }
        }
        internal static void AdminDropShards(ChatCommandContext ctx, int amount, RelicType relicType, bool oldShard)
        {
            var NametoGuid = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary;
            var prefabNew = NametoGuid[DB.NewShard[relicType]];
            var prefabOld = NametoGuid[DB.OldShard[relicType]];
            if (oldShard)
            {
                DropShard(ctx.Event.SenderCharacterEntity, prefabOld, amount);
            }
            else
            {
                DropShard(ctx.Event.SenderCharacterEntity, prefabNew, amount);
            }
        }
        public static void ClearDropTable()
        {
            foreach (var boss in DB.BossNames.Keys)
            {
                var GuidToEntity = Helper.PrefabCollectionSystem._PrefabGuidToEntityMap;
                var NameToGuid = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary;
                var entity = GuidToEntity[NameToGuid[boss]];
                var Droptablebuffer = Helper.EntityManager.GetBuffer<DropTableBuffer>(entity);
                for (var i = 0; i < Droptablebuffer.Length; i++)
                {
                    if (Droptablebuffer[i].RelicType != RelicType.None)
                    {
                        Droptablebuffer.RemoveAt(i);
                        i = 0;
                    }
                }
                if (Droptablebuffer.Length == 0)
                {
                    Plugin.Logger.LogWarning($"Drop List [{boss.Replace("CHAR_", "").Replace("_VBlood", "").Replace("_", " ")}] is empty . Add standart loot table for [{boss.Replace("CHAR_", "").Replace("_VBlood", "").Replace("_", " ")}]");
                    Droptablebuffer.Add(new DropTableBuffer() { DropTableGuid = NameToGuid["DT_Shared_Unit_VBlood_T06_ShardBoss"], DropTrigger = DropTriggerType.OnDeath, RelicType = RelicType.None });
                }
            }
        }
    }
}
