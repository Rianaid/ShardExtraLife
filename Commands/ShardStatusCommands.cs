using ShardExtraLife.Databases;
using VampireCommandFramework;

namespace ShardExtraLife.Commands
{
    internal class ShardStatusCommands
    {
        [Command("shardstatus", shortHand: "ss", usage: ".shardstatus ?", description: "Check Shards status.", adminOnly: true)]
        public static void ShardStatusCommand(ChatCommandContext ctx, string arg1 = null)
        {
            if (DB.EnabledCommand)
            {
                ShardExtraLife.UpdateShardslist();
                string message = "";
                if (arg1 == null || string.IsNullOrEmpty(arg1))
                {
                    
                    ctx.Reply($"Shard status:");
                    foreach (var type in DB.ShardsData.Keys)
                    {
                       var data = DB.ShardsData[type];
                        ctx.Reply($"[{type}]: [{data.Count}/{data.MaxCount}] can drop: {data.Count<data.MaxCount} ");
                    }
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
