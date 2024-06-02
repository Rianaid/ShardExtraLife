using ProjectM;
using ShardExtraLife.Databases;
using Stunlock.Core;
using System.Collections.Generic;
using Unity.Entities;
using VampireCommandFramework;

namespace ShardExtraLife.Utils
{
    internal class ShardInventory
    {
        internal static int ShardUltimateReplace(ChatCommandContext ctx, string where)
        {
            var result = SearchShard(ctx, where, out var shardlist);
            int count = 0;
            if (result)
            {

                foreach (var shard in shardlist)
                {
                    if (Helper.EntityManager.HasComponent<EquippableData>(shard))
                    {
                        var prefab = Helper.EntityManager.GetComponentData<PrefabGUID>(shard);
                        var name = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary[prefab];
                        if (DB.ShardNameBuff.ContainsKey(name))
                        {
                            var buffname = DB.ShardNameBuff[name];
                            var buffprefab = Helper.PrefabCollectionSystem.NameToPrefabGuidDictionary[buffname];
                            var equippableData = Helper.EntityManager.GetComponentData<EquippableData>(shard);

                            if (equippableData.BuffGuid.Equals(buffprefab))
                            {
                                equippableData.BuffGuid = new PrefabGUID(0);
                            }
                            else
                            {
                                equippableData.BuffGuid = buffprefab;
                            }
                            Helper.EntityManager.SetComponentData(shard, equippableData);
                            count++;
                        }
                    }
                }
            }
            if (count == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        internal static bool SearchShard(ChatCommandContext ctx, string where, out List<Entity> shardlist)
        {
            shardlist = new List<Entity>();
            if (where.ToLower() == "inventory")
            {
                var result = SearchShardInInventory(ctx, out var shardlistInventory);
                shardlist = shardlistInventory;
                return result;
            }
            else if (where.ToLower() == "equipment")
            {
                var result = SearchShardInEquipment(ctx, out var shardlistEquipment);
                shardlist = shardlistEquipment;
                return result;
            }
            else if (where.ToLower() == "all")
            {
                var result = SearchShardInInventory(ctx, out var shardlistInventory);
                shardlist = shardlistInventory;
                var result1 = SearchShardInEquipment(ctx, out var shardlistEquipment);
                shardlist.AddRange(shardlistEquipment);
                return result || result1;
            }
            else
            {
                return false;
            }


        }
        internal static bool SearchShardInInventory(ChatCommandContext ctx, out List<Entity> shardlistInventory)
        {
            var GuidName = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary;
            var getInventoryResponse = InventoryUtilitiesServer.TryGetInventoryBuffer(Helper.EntityManager, ctx.Event.SenderCharacterEntity, out var Inventorybuffer);
            shardlistInventory = new List<Entity>();
            if (getInventoryResponse.Success)
            {
                foreach (var item in Inventorybuffer)
                {
                    if (item.ItemType != new PrefabGUID(0))
                    {
                        var name = GuidName[item.ItemType];
                        if (DB.NewShardNames.Contains(name) || DB.OldShardName.Contains(name))
                        {
                            shardlistInventory.Add(item.ItemEntity._Entity);
                        }
                    }
                }
                if (shardlistInventory.Count > 0) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        internal static bool SearchShardInEquipment(ChatCommandContext ctx, out List<Entity> shardlistEquip)
        {
            var GuidName = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary;
            var getInventoryResponse = InventoryUtilitiesServer.TryGetInventoryBuffer(Helper.EntityManager, ctx.Event.SenderCharacterEntity, out var Inventorybuffer);
            shardlistEquip = new List<Entity>();
            var equipment = Helper.EntityManager.GetComponentData<Equipment>(ctx.Event.SenderCharacterEntity);
            if (equipment.IsEquipped(EquipmentType.MagicSource))
            {
                var name = GuidName[equipment.GetEquipmentItemId(EquipmentType.MagicSource)];
                if (DB.NewShardNames.Contains(name))
                {
                    shardlistEquip.Add(equipment.GetEquipmentEntity(EquipmentType.MagicSource)._Entity);
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
    }
}
