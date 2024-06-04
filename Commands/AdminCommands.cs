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
            public static void EditAmountCommand(ChatCommandContext ctx, string ShardName = "?", int amount = 1)
            {
                if (DB.EnabledEditAmountCommand)
                {
                    var sb = new Il2CppSystem.Text.StringBuilder();
                    if (ShardName.ToLower() == "?" || ShardName.ToLower() == "help" || ShardName.ToLower() == "h")
                    {
                        sb.Clear();
                        sb.AppendLine($"[Dracula||Solarus||TheMonster||WingedHorror||OldTheMonster||OldWingedHorror||Behemoth||All] [amount]");
                        sb.AppendLine($"This command is for changing the number of shards on the server.");
                        sb.AppendLine($"The first parameter is the name of the shard or \"All\" for all of them together.");
                        sb.AppendLine($"The second parameter is the maximum number of shards.");
                        ctx.Reply(sb.ToString());
                    }
                    else
                    {
                        if (ShardUtils.RelicTypeCheck(ShardName, out var type))
                        {
                            ShardUtils.UpdateMaxAmount(amount, DB.ShardsData, type);
                            ctx.Reply($"Max amount [{ShardName}] shard changed and now [{amount}].");
                        }
                        else if (ShardName.ToLower() == "all")
                        {
                            ShardUtils.UpdateMaxAmountAll(amount, DB.ShardsData);
                            ctx.Reply($"Max amount [All] shards changed and now [{amount}].");
                        }
                        else
                        {
                            ctx.Reply("Incorrect argument. Use [Dracula||Solarus||TheMonster||WingedHorror||OldTheMonster||OldWingedHorror||Behemoth||All].");
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
                            if (NewChance >= 0 && NewChance <= 100 && OldChance >= 0 && OldChance <= 100)
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
                            else
                            {
                                ctx.Reply("Incorrect argument. Drop chance must be between 0 and 100 inclusive.");
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
                    var type = RelicTypeMod.None;
                    if (ShardsType.ToLower() == "?")
                    {
                        sb.Clear();
                        sb.AppendLine($"[.sela asd] [Dracula||Solarus||TheMonster||WingedHorror||OldTheMonster||OldWingedHorror||Behemoth] [amount]");
                        sb.AppendLine($"This admin only command create shard drop despite the limits.");
                        sb.AppendLine($"This command was created because the console command for issuing an item cannot bypass my created limit. And yes, I know this is a bug. I will fix it...");
                        ctx.Reply(sb.ToString());
                        return;
                    }
                    else
                    {
                        if (name.ToLower() == "dracula") { type = RelicTypeMod.Dracula; }
                        else if (name.ToLower() == "solarus") { type = RelicTypeMod.Solarus; }
                        else if (name.ToLower() == "wingedhorror") { type = RelicTypeMod.WingedHorror; }
                        else if (name.ToLower() == "themonster") { type = RelicTypeMod.TheMonster; }
                        else if (name.ToLower() == "behemoth") { type = RelicTypeMod.OldBehemoth; }
                        else if (name.ToLower() == "oldwingedhorror") { type = RelicTypeMod.OldWingedHorror; }
                        else if (name.ToLower() == "oldthemonster") { type = RelicTypeMod.OldTheMonster; }
                        else { ctx.Reply($"Incorrect argument. Use one of these: [Dracula||Solarus||TheMonster||WingedHorror||OldTheMonster||OldWingedHorror||Behemoth]."); return; }
                        ShardDropper.AdminDropShards(ctx, amount, type);
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
