using ProjectM;
using ProjectM.Network;
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
        public static void Randomizer(float chance1, float chance2, out bool drop1, out bool drop2, out double chance)
        {
            drop1 = false;
            drop2 = false;
            chance = random.NextDouble();
            if (chance < chance1)
            {
                drop1 = true;
            }
            else if (chance < (chance1 + chance2))
            {
                drop2 = true;
            }
        }
        public static void ChoiceDropSystem(RelicTypeMod relicTypeMod, DeathEvent deathEvent)
        {
            var DropperEntity = deathEvent.Died;
            var user = new User();
            var CanSendMessage = false;
            if (Helper.EntityManager.HasComponent<PlayerCharacter>(deathEvent.Killer) && DB.EnableSendMessages)
            {
                var userEntity = Helper.EntityManager.GetComponentData<PlayerCharacter>(deathEvent.Killer).UserEntity;
                user = Helper.EntityManager.GetComponentData<User>(userEntity);
                CanSendMessage = true;
            }
            var NametoGuid = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary;
            var prefabNew = new PrefabGUID(0);
            var prefabOld = new PrefabGUID(0);
            var canDropNew = false;
            var canDropOld = false;
            var dropOld = false;
            var dropNew = false;
            var Plentiful = false;
            if (Helper.serverGameSettings.Settings.RelicSpawnType == RelicSpawnType.Plentiful)
            {
                Plentiful = true;
            }
            if (DB.UseDropChanceForNewShard && DB.UseDropChanceForOldShard)
            {
                Randomizer(DB.ChanceDropOldShard, DB.ChanceDropNewShard, out dropOld, out dropNew, out var chance);
            }
            else if (!DB.UseDropChanceForNewShard && DB.UseDropChanceForOldShard)
            {
                Randomizer(DB.ChanceDropOldShard, DB.ChanceDropNewShard, out dropOld, out dropNew, out var chance);
                dropNew = true;
            }
            else if (DB.UseDropChanceForNewShard && !DB.UseDropChanceForOldShard)
            {
                Randomizer(DB.ChanceDropOldShard, DB.ChanceDropNewShard, out dropOld, out dropNew, out var chance);
                dropOld = true;
            }
            else
            {
                Randomizer(0.5f, 0.5f, out dropOld, out dropNew, out var chance);
            }
            if (relicTypeMod == RelicTypeMod.Dracula || relicTypeMod == RelicTypeMod.Solarus)
            {
                prefabNew = NametoGuid[DB.RelicModShards[relicTypeMod]];
                if (Plentiful)
                {
                    canDropNew = true;
                }
                else
                {
                    canDropNew = DB.ShardsData[relicTypeMod].canDrop();
                }
                if (canDropNew && (dropOld || dropNew) && DB.DropNewShards)
                {
                    DropShard(DropperEntity, prefabNew, 1);
                }
            }
            else if (relicTypeMod == RelicTypeMod.OldBehemoth)
            {
                prefabOld = NametoGuid[DB.RelicModShards[relicTypeMod]];
                if (Plentiful)
                {
                    canDropOld = true;
                }
                else
                {
                    canDropOld = DB.ShardsData[relicTypeMod].canDrop();
                }
                if (canDropOld && (dropOld || dropNew)&& DB.DropOldShards)
                {
                    DropShard(DropperEntity, prefabOld, 1);
                }
            }
            else
            {
                if (relicTypeMod == RelicTypeMod.TheMonster)
                {
                    prefabNew = NametoGuid[DB.RelicModShards[RelicTypeMod.TheMonster]];
                    prefabOld = NametoGuid[DB.RelicModShards[RelicTypeMod.OldTheMonster]];
                    if (Plentiful)
                    {
                        canDropNew = true;
                        canDropOld = true;
                    }
                    else
                    {
                        canDropNew = DB.ShardsData[RelicTypeMod.TheMonster].canDrop();
                        canDropOld = DB.ShardsData[RelicTypeMod.OldTheMonster].canDrop();
                    }
                }
                else
                {
                    prefabNew = NametoGuid[DB.RelicModShards[RelicTypeMod.WingedHorror]];
                    prefabOld = NametoGuid[DB.RelicModShards[RelicTypeMod.OldWingedHorror]];
                    if (Plentiful)
                    {
                        canDropNew = true;
                        canDropOld = true;
                    }
                    else
                    {
                        canDropNew = DB.ShardsData[RelicTypeMod.WingedHorror].canDrop();
                        canDropOld = DB.ShardsData[RelicTypeMod.OldWingedHorror].canDrop();
                    }
                }
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
                    if (!DB.DropNewShards) { canDropNew = false; }
                    if (!DB.DropOldShards) { canDropOld = false; }
                    if (dropOld && canDropOld)
                    {
                        DropShard(DropperEntity, prefabOld, 1);
                    }
                    else if (dropNew && canDropNew)
                    {
                        DropShard(DropperEntity, prefabNew, 1);
                    }
                    else if (dropOld && !canDropOld && canDropNew)
                    {
                        DropShard(DropperEntity, prefabNew, 1);
                    }
                    else if (dropNew && !canDropNew && canDropOld)
                    {
                        DropShard(DropperEntity, prefabOld, 1);
                    }
                    else if (!canDropNew && !canDropOld)
                    {
                        if (CanSendMessage)
                        {
                            ServerChatUtils.SendSystemMessageToClient(Helper.EntityManager, user, $"{DB.ReachShardLimit.Replace("{relicTypeMod}", relicTypeMod.ToString())}");
                        }
                    }
                    else if (!dropOld && !dropNew)
                    {
                        if (CanSendMessage)
                        {
                            ServerChatUtils.SendSystemMessageToClient(Helper.EntityManager, user, $"{DB.NoDropLucky}");
                        }
                    }
                }
            }
        }
        internal static void AdminDropShards(ChatCommandContext ctx, int amount, RelicTypeMod relicType)
        {
            var NametoGuid = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary;
            var prefab = NametoGuid[DB.RelicModShards[relicType]];
            DropShard(ctx.Event.SenderCharacterEntity, prefab, amount);
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
