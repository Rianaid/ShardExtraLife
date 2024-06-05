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
