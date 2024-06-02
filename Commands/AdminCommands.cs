using ProjectM.Shared;
using ShardExtraLife.Configs;
using ShardExtraLife.Databases;
using ShardExtraLife.Utils;
using VampireCommandFramework;

namespace ShardExtraLife.Commands
{
    internal class AdminCommands
    {
        [CommandGroup("shardextralife", shortHand: "sel")]
        internal class ShardStatCommand
        {
            [Command("editamount", shortHand: "ea", usage: "[.sel ea ?]", description: "Edit shard amount drop.", adminOnly: true)]
            public static void EditAmountCommand(ChatCommandContext ctx, string ShardsType = "?", string ShardName = "!", int amount = 1)
            {
                if (DB.EnabledEditAmountCommand)
                {
                    var sb = new Il2CppSystem.Text.StringBuilder();
                    if (ShardsType.ToLower() == "?" || ShardsType.ToLower() == "help" || ShardsType.ToLower() == "h")
                    {
                        sb.Clear();
                        sb.AppendLine($"[New||Old||All] [Dracula||Solarus||TheMonster||WingedHorror||All] [amount]");
                        sb.AppendLine($"This command is for changing the number of shards on the server.");
                        sb.AppendLine($"The first parameter is: \"New\" for new shards, \"Old\" for old ones and \"All\" for all of them together.");
                        sb.AppendLine($"The second parameter is the name of the shard or \"All\" for all of them together.");
                        sb.AppendLine($"The third is the maximum number of shards.");
                        ctx.Reply(sb.ToString());
                    }
                    else
                    {
                        if (ShardsType.ToLower() == "new")
                        {
                            if (ShardUtils.RelicTypeCheck(ShardName, out var type))
                            {
                                ShardUtils.UpdateMaxAmount(amount, DB.NewShardsData, type);
                            }
                            else if (ShardName.ToLower() == "all")
                            {
                                ShardUtils.UpdateMaxAmountAll(amount, DB.NewShardsData);
                            }
                            else
                            {
                                ctx.Reply("Incorrect argument. Use [Dracula||Solarus||TheMonster||WingedHorror||All].");
                            }
                        }
                        else if (ShardsType.ToLower() == "old")
                        {
                            if (ShardUtils.RelicTypeCheck(ShardName, out var type))
                            {
                                ShardUtils.UpdateMaxAmount(amount, DB.OldShardsData, type);
                            }
                            else if (ShardName.ToLower() == "all")
                            {
                                ShardUtils.UpdateMaxAmountAll(amount, DB.OldShardsData);
                            }
                            else
                            {
                                ctx.Reply("Incorrect argument. Use [Dracula||Solarus||TheMonster||WingedHorror||All].");
                            }
                        }
                        else if (ShardsType.ToLower() == "all")
                        {
                            if (ShardUtils.RelicTypeCheck(ShardName, out var type))
                            {
                                ShardUtils.UpdateMaxAmount(amount, DB.NewShardsData, type);
                                ShardUtils.UpdateMaxAmount(amount, DB.OldShardsData, type);
                            }
                            else if (ShardName.ToLower() == "all")
                            {
                                ShardUtils.UpdateMaxAmountAll(amount, DB.NewShardsData);
                                ShardUtils.UpdateMaxAmountAll(amount, DB.OldShardsData);
                            }
                            else
                            {
                                ctx.Reply("Incorrect argument. Use [Dracula||Solarus||TheMonster||WingedHorror||All].");
                            }
                        }
                        else
                        {
                            ctx.Reply("Incorrect argument.Use one of these: [New||Old||All]. Use [?||help||h] for help");
                        }
                    }
                    MainConfig.Save();
                    return;
                }
                else
                {
                    ctx.Reply("Command disable. Report to server admins.");
                    return;
                }
            }
            [Command("editchance", shortHand: "ec", usage: "[.sel ec ?]", description: "Edit shard chance drop.", adminOnly: true)]
            public static void EditChanceCommand(ChatCommandContext ctx, string action = "?", float NewChance = 1, float OldChance = 1)
            {
                if (DB.EnabledEditChanceCommand)
                {
                    var sb = new Il2CppSystem.Text.StringBuilder();
                    if (action.ToLower() == "?" || action.ToLower() == "help" || action.ToLower() == "h")
                    {
                        sb.Clear();
                        sb.AppendLine($"[Set||Check] [NewShardChance] [OldShardChance]");
                        sb.AppendLine($"This command is for changing the number of shards on the server.");
                        sb.AppendLine($"The first parameter is: \"Set\" for setup chance or \"check\" for check chance.");
                        sb.AppendLine($"The second parameter is a number from 0 to 100 reflecting percentages for new shard.");
                        sb.AppendLine($"The third parameter is a number from 0 to 100 reflecting percentages for old shard.");
                        sb.AppendLine($"Attention, if both types of shards are included, then the sum of their drop chances cannot exceed 100.");
                        ctx.Reply(sb.ToString());
                        return;
                    }
                    else
                    {
                        if (action.ToLower() == "set")
                        {
                            if (DB.DropNewShards && DB.DropOldShards)
                            {
                                if (NewChance + OldChance <= 100)
                                {
                                    DB.ChanceDropNewShard = NewChance / 100;
                                    DB.ChanceDropOldShard = OldChance / 100;
                                    ctx.Reply("Drop chance setup successful");
                                    return;
                                }
                                else
                                {
                                    ctx.Reply("Incorrect argument. Sum drop chances cannot exceed 100.");
                                    return;
                                }
                            }
                            else
                            {
                                DB.ChanceDropNewShard = NewChance / 100;
                                DB.ChanceDropOldShard = OldChance / 100;
                                ctx.Reply("Drop chance setup successful");
                                return;
                            }
                        }
                        else if (action.ToLower() == "check")
                        {
                            sb.Clear();
                            sb.AppendLine("Drop chance:");
                            sb.AppendLine($"New shard: [{DB.ChanceDropNewShard * 100:F2}%], Drop enable [{DB.DropNewShards}]");
                            sb.AppendLine($"Old shard: [{DB.ChanceDropOldShard * 100:F2}%], Drop enable [{DB.DropOldShards}]");
                            ctx.Reply(sb.ToString());
                            return;
                        }
                        else
                        {
                            ctx.Reply("Incorrect argument. Use one of these: [Set||Check]. Use [?||help||h] for help");
                        }
                    }
                    MainConfig.Save();
                    return;
                }
                else
                {
                    ctx.Reply("Command disable. Report to server admins.");
                    return;
                }
            }
            [Command("adminsharddrop", shortHand: "asd", usage: "[.sel asd ?]", description: "Manual drop shard.", adminOnly: true)]
            public static void AdminShardDropCommand(ChatCommandContext ctx, string ShardsType = "?", string name = "none", int amount = 1)
            {
                if (DB.EnabledShardDropCommand)
                {
                    var sb = new Il2CppSystem.Text.StringBuilder();
                    var type = RelicType.None;
                    if (ShardsType.ToLower() == "?")
                    {
                        sb.Clear();
                        sb.AppendLine($"[.sela asd] [New||Old] [Dracula||Solarus||TheMonster||WingedHorror] [amount]");
                        sb.AppendLine($"This admin only command create shard drop despite the limits.");
                        sb.AppendLine($"This command was created because the console command for issuing an item cannot bypass my created limit. And yes, I know this is a bug. I will fix it...");
                        ctx.Reply(sb.ToString());
                        return;
                    }
                    else
                    {
                        if (name.ToLower() == "dracula") { type = RelicType.Dracula; }
                        else if (name.ToLower() == "solarus") { type = RelicType.Solarus; }
                        else if (name.ToLower() == "wingedhorror") { type = RelicType.WingedHorror; }
                        else if (name.ToLower() == "themonster") { type = RelicType.TheMonster; }
                        else { ctx.Reply($"Incorrect argument. Use one of these: [Dracula][Solarus][TheMonster][WingedHorror]."); return; }
                        if (ShardsType.ToLower() == "old")
                        {
                            if (type == RelicType.Dracula)
                            {
                                ctx.Reply($"Old Dracula shard not fount.");
                            }
                            else
                            {
                                ShardDropper.AdminDropShards(ctx, amount, type, true);
                            }
                        }
                        else if (ShardsType.ToLower() == "new")
                        {
                            ShardDropper.AdminDropShards(ctx, amount, type, false);
                        }
                        else { ctx.Reply($"Incorrect argument. Use one of these: [New] or [Old]. Use [?||help||h] for help."); return; }
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
