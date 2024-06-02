using ProjectM;
using ProjectM.Shared;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using Stunlock.Core;
using VampireCommandFramework;

namespace ShardExtraLife.Commands
{
    internal class ShardStatusPlayerCommands
    {
        [CommandGroup("shardextralife", shortHand: "sel")]
        internal class ShardStatCommand
        {
            [Command("shardstatus", shortHand: "ss", usage: "[.sel ss ?]", description: "Check Shards status.", adminOnly: false)]
            public static void ShardStatusCommand(ChatCommandContext ctx, string where = "!")
            {
                if (DB.PlayerCommandEnabled)
                {
                    var sb = new Il2CppSystem.Text.StringBuilder();
                    if (where.ToLower() == "me")
                    {
                        var findstatus = ShardInventory.SearchShard(ctx, "all", out var shardlist);
                        if (findstatus)
                        {
                            sb.Clear();
                            sb.AppendLine($"Shard found :[{shardlist.Count}]");
                            for (int i = 0; i < shardlist.Count; i++)
                            {
                                var shard = shardlist[i];
                                var prefab = Helper.EntityManager.GetComponentData<PrefabGUID>(shard);
                                var name = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary[prefab];
                                name = name.Replace("Item_MagicSource_SoulShard_", "New ");
                                name = name.Replace("Item_Building_Relic_", "Old ");
                                var CurDurab = 0f;
                                var MaxDurab = 0f;
                                if (Helper.EntityManager.HasComponent<Durability>(shard))
                                {
                                    var durability = Helper.EntityManager.GetComponentData<Durability>(shard);
                                    MaxDurab = durability.MaxDurability;
                                    CurDurab = durability.Value;
                                }
                                else if (Helper.EntityManager.HasComponent<Age>(shard) && Helper.EntityManager.HasComponent<LifeTime>(shard))
                                {
                                    var age = Helper.EntityManager.GetComponentData<Age>(shard);
                                    var lifetime = Helper.EntityManager.GetComponentData<LifeTime>(shard);
                                    MaxDurab = lifetime.Duration;
                                    CurDurab = age.Value;
                                }
                                if (sb.Length > 400)
                                {
                                    ctx.Reply(sb.ToString());
                                    sb.Clear();
                                    sb.AppendLine($"");
                                }
                                sb.AppendLine($"\t[{i + 1}]:[{shard}] [{name}] Durability: [{CurDurab}/{MaxDurab}]");
                            }
                            ctx.Reply(sb.ToString());
                        }
                        else
                        {
                            ctx.Reply($"Shard not found in your inventory.");
                        }
                        return;
                    }
                    else if (where.ToLower() == "all")
                    {
                        if (DB.DropNewShards)
                        {
                            sb.Clear();
                            sb.AppendLine($"New shard status:");
                            foreach (var type in DB.NewShardsData.Keys)
                            {
                                var data = DB.NewShardsData[type];
                                sb.AppendLine($"\t[{type}]: [{data.Count}/{data.MaxCount}]. Can drop: {data.Count < data.MaxCount} ");
                            }
                            ctx.Reply(sb.ToString());
                            sb.Clear();
                        }

                        if (DB.DropOldShards)
                        {
                            sb.Clear();
                            sb.AppendLine($"Old shard status:");
                            foreach (var type in DB.OldShardsData.Keys)
                            {
                                if (type == RelicType.Dracula) { continue; }
                                var data = DB.OldShardsData[type];
                                sb.AppendLine($"\t[{type}]: [{data.Count}/{data.MaxCount}]. Can drop: {data.Count < data.MaxCount} ");
                            }
                            ctx.Reply(sb.ToString());
                            sb.Clear();

                        }
                        if (DB.DropNewShards && DB.DropOldShards)
                        {
                            ctx.Reply($"Server drop chance: New [{DB.ChanceDropNewShard * 100:F2}%]; Old [{DB.ChanceDropOldShard * 100:F2}%]");
                        }
                        else if (!DB.DropNewShards && DB.DropOldShards && DB.UseDropChanceForNewShard)
                        {
                            ctx.Reply($"Server drop chance: Old [{DB.ChanceDropOldShard * 100:F2}%]");
                        }
                        else if (DB.DropNewShards && !DB.DropOldShards && DB.UseDropChanceForNewShard)
                        {
                            ctx.Reply($"Server drop chance: New [{DB.ChanceDropNewShard * 100:F2}%];");
                        }
                        else
                        {
                            ctx.Reply($"Server shard drop off!!! Mayby this error!! Report admins!!");
                        }
                        return;
                    }
                    else
                    {
                        sb.Clear();
                        sb.AppendLine($"[.sel ss] [Me || All] ");
                        sb.AppendLine("This command allows you to find out the residual durability of a shard. This is mainly necessary for old shards. ");
                        ctx.Reply(sb.ToString());
                        return;
                    }
                }
                else
                {
                    ctx.Reply("Command disable. Report to server admins.");
                    return;
                }
            }
            [Command("shardultimatereplace", shortHand: "sur", usage: "[.sel sur ?]", description: "Change replace ultimate option.", adminOnly: false)]
            public static void ShardUltimateReplaceCommand(ChatCommandContext ctx, string where = "!")
            {
                if (DB.PlayerCommandEnabled)
                {
                    var sb = new Il2CppSystem.Text.StringBuilder();
                    if (where == "?" || where == "help" || where == "h")
                    {
                        sb.Clear();
                        sb.AppendLine($"[.sel sur] [Inventory||Inv||I||Equipment||Equip||E||All]");
                        sb.AppendLine($"This command switches the replacement of ultimate from shards.");
                        sb.AppendLine($"After using command, you will have to change the equipped shard for the change to take effect.");
                        ctx.Reply(sb.ToString());
                    }
                    else if (where.ToLower() == "inventory" || where.ToLower() == "i" || where.ToLower() == "inv")
                    {
                        var result = ShardInventory.ShardUltimateReplace(ctx, "inventory");
                        switch (result)
                        {
                            case 0:
                                ctx.Reply($"Replacing Utimate in your shards in inventory was successful. Please change the shard to apply the changes.");
                                break;
                            case 1:
                                ctx.Reply($"You not have Shard in inventory for replace ultimate.");
                                break;
                            default:
                                ctx.Reply($"Incorrect result. Report mod devoloper. this bug!!");
                                break;
                        }
                    }
                    else if (where.ToLower() == "equipment" || where.ToLower() == "e" || where.ToLower() == "equip")
                    {
                        var result = ShardInventory.ShardUltimateReplace(ctx, "equipment");
                        switch (result)
                        {
                            case 0:
                                ctx.Reply($"Replacing Utimate in your shard in equipment was successful. Please change the shard to apply the changes.");
                                break;
                            case 1:
                                ctx.Reply($"You not have Shard in equipment for replace ultimate.");
                                break;
                            default:
                                ctx.Reply($"Incorrect result. Report mod devoloper. this bug!!");
                                break;
                        }
                    }
                    else
                    {
                        var result = ShardInventory.ShardUltimateReplace(ctx, "all");
                        switch (result)
                        {
                            case 0:
                                ctx.Reply($"Replacing Utimate in your shard in equipment was successful. Please change the shard to apply the changes.");
                                break;
                            case 1:
                                ctx.Reply($"You not have Shard in equipment for replace ultimate.");
                                break;
                            default:
                                ctx.Reply($"Incorrect result. Report mod devoloper. this bug!!");
                                break;
                        }
                    }
                    return;
                }
                else
                {
                    ctx.Reply("Command disable. Report to server admins.");
                    return;
                }
            }
        }
    }
}
