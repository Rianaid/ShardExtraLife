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
                                sb.AppendLine($"\t[{i + 1}]: [{name}] Durability: [{CurDurab}/{MaxDurab}]");
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
                            for (RelicTypeMod type = RelicTypeMod.TheMonster; type <= RelicTypeMod.Dracula; type++)
                            {
                                var data = DB.ShardsData[type];
                                if (Helper.serverGameSettings.Settings.RelicSpawnType == RelicSpawnType.Plentiful)
                                {
                                    sb.AppendLine($"\t[{type}]: [{data.Count}]. Can drop: {data.Count < int.MaxValue} ");
                                }
                                else
                                {
                                    sb.AppendLine($"\t[{type}]: [{data.Count}/{data.MaxCount}]. Can drop: {data.Count < data.MaxCount} ");
                                }
                            }
                            ctx.Reply(sb.ToString());
                            sb.Clear();
                        }

                        if (DB.DropOldShards)
                        {
                            sb.Clear();
                            sb.AppendLine($"Old shard status:");
                            for (RelicTypeMod type = RelicTypeMod.OldTheMonster; type <= RelicTypeMod.OldBehemoth; type++)
                            {
                                var data = DB.ShardsData[type];
                                if (Helper.serverGameSettings.Settings.RelicSpawnType == RelicSpawnType.Plentiful)
                                {
                                    sb.AppendLine($"\t[{type}]: [{data.Count}]. Can drop: {data.Count < int.MaxValue} ");
                                }
                                else
                                {
                                    sb.AppendLine($"\t[{type}]: [{data.Count}/{data.MaxCount}]. Can drop: {data.Count < data.MaxCount} ");
                                }
                            }
                            ctx.Reply(sb.ToString());
                            sb.Clear();

                        }
                        if (DB.DropNewShards && DB.DropOldShards && !DB.DropNewAndOldShardTogether)
                        {
                            ctx.Reply($"Server drop chance: New [{DB.ChanceDropNewShard * 100:F2}%]; Old [{DB.ChanceDropOldShard * 100:F2}%]");
                        }
                        else if (!DB.DropNewShards && DB.DropOldShards && DB.UseDropChanceForOldShard)
                        {
                            ctx.Reply($"Server drop chance: Old [{(DB.ChanceDropNewShard + DB.ChanceDropOldShard) * 100:F2}%]");
                        }
                        else if (DB.DropNewShards && !DB.DropOldShards && DB.UseDropChanceForNewShard)
                        {
                            ctx.Reply($"Server drop chance: New [{(DB.ChanceDropNewShard+ DB.ChanceDropOldShard) * 100:F2}%];");
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
        }
    }
}
