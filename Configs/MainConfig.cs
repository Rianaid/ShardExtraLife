using BepInEx.Configuration;
using ShardExtraLife.Databases;
using System.IO;

namespace ShardExtraLife.Configs
{
    internal class MainConfig
    {
        private static readonly string FileDirectory = Path.Combine("BepInEx", "config");
        private static readonly string FileName = "ShardExtraLife.cfg";
        private static readonly string fullPath = Path.Combine(FileDirectory, FileName);
        private static readonly ConfigFile Conf = new ConfigFile(fullPath, true);

        public static ConfigEntry<float> ChanceDropNewShard;
        public static ConfigEntry<float> ChanceDropOldShard;


        public static ConfigEntry<bool> UpdateExistingShards;
        public static ConfigEntry<bool> DropNewShards;
        public static ConfigEntry<bool> DropOldShards;
        public static ConfigEntry<bool> UseDropChanceForNewShard;
        public static ConfigEntry<bool> UseDropChanceForOldShard;
        public static ConfigEntry<bool> DropNewAndOldShardTogether;
        public static ConfigEntry<bool> EnableUltimateReplace;
        //--------Durability------------
        public static ConfigEntry<float> TimeUntilBroken;
        public static ConfigEntry<float> MaxDurability;
        public static ConfigEntry<float> LifeTimeOldShard;
        public static ConfigEntry<float> RepairMultiplier;
        public static ConfigEntry<float> AdditionalRepairPoints;
        public static ConfigEntry<bool> DestroyItemWhenBroken;
        public static ConfigEntry<bool> EnabledRepairInAltar;
        //--------ShardAmount----------
        public static ConfigEntry<int> MaxNewShardAmountDracula;
        public static ConfigEntry<int> MaxNewShardAmountSolarus;
        public static ConfigEntry<int> MaxNewShardAmountTheMonster;
        public static ConfigEntry<int> MaxNewShardAmountWingedHorror;
        public static ConfigEntry<int> MaxOldShardAmountBehemoth;
        public static ConfigEntry<int> MaxOldShardAmountTheMonster;
        public static ConfigEntry<int> MaxOldShardAmountWingedHorror;
        //--------Commands------------
        public static ConfigEntry<bool> EnabledEditAmountCommand;
        public static ConfigEntry<bool> EnabledEditChanceCommand;
        public static ConfigEntry<bool> EnabledShardDropCommand;
        public static ConfigEntry<bool> PlayerCommandEnabled;
        //--------Message------------
        public static ConfigEntry<bool> EnableSendMessages;
        public static ConfigEntry<string> ReachShardLimit;
        public static ConfigEntry<string> NoDropLucky;

        public static void ConfigInit()
        {
            //--------ShardAmount----------
            MaxNewShardAmountDracula = Conf.Bind("ShardAmount", "MaxShardAmountDracula", 1, "Maximum new \"Dracula\" shard amount if drop new shards \"True\".");
            MaxNewShardAmountSolarus = Conf.Bind("ShardAmount", "MaxShardAmountSolarus", 1, "Maximum new \"Solarus\" shard amount if drop new shards \"True\".");
            MaxNewShardAmountTheMonster = Conf.Bind("ShardAmount", "MaxShardAmountTheMonster", 1, "Maximum new \"TheMonster\" shard amount if drop new shards \"True\".");
            MaxNewShardAmountWingedHorror = Conf.Bind("ShardAmount", "MaxShardAmountWingedHorror", 1, "Maximum new \"Winged Horror\" shard amount if drop new shards \"True\".");
            MaxOldShardAmountBehemoth = Conf.Bind("ShardAmount", "MaxOldShardAmountBehemoth", 1, "Maximum old \"Behemoth\" shard amount if drop old shards \"True\".");
            MaxOldShardAmountTheMonster = Conf.Bind("ShardAmount", "MaxOldShardAmountTheMonster", 1, "Maximum old \"TheMonster\" shard amount if drop old shards \"True\".");
            MaxOldShardAmountWingedHorror = Conf.Bind("ShardAmount", "MaxOldShardAmountWingedHorror", 1, "Maximum old \"Winged Horror\" shard amount if drop old shards \"True\".");
            //--------Durability------------
            MaxDurability = Conf.Bind("ShardExtraLife", "MaxDurability", 2500f, "Max durability for new shards.");
            LifeTimeOldShard = Conf.Bind("ShardExtraLife", "LifeTimeOldShard", 4200f, "Max lifetime for old shard.");
            TimeUntilBroken = Conf.Bind("ShardExtraLife", "TimeUntilBroken", 1296000f, "Shard time before destroy.");
            RepairMultiplier = Conf.Bind("ShardExtraLife", "RepairMultiplier", 1f, "Shard recovery multiplier. Currently the timer runs every 60 seconds.");
            AdditionalRepairPoints = Conf.Bind("ShardExtraLife", "AdditionalRepairPoints", 1f, "Additional durability points for shards during repairs. Currently the timer runs every 60 seconds.");
            EnabledRepairInAltar = Conf.Bind("ShardExtraLife", "EnabledRepairInAltar", true, "Enable repair shard in special pedestal.");
            DestroyItemWhenBroken = Conf.Bind("ShardExtraLife", "DestroyItemWhenBroken", true, "Enable destroy shard when broken.");
            //--------ChanceDrop------------
            ChanceDropNewShard = Conf.Bind("ShardChanceDrop", "ChanceDropNewShard", 0.5f, "Chance drop new shard.");
            ChanceDropOldShard = Conf.Bind("ShardChanceDrop", "ChanceDropOldShard", 0.5f, "Chance drop old shard.");
            UseDropChanceForNewShard = Conf.Bind("ShardChanceDrop", "UseDropChanceForNewShard", true, "Enable drop chance for new shard drop. Work if drop new shard on.");
            UseDropChanceForOldShard = Conf.Bind("ShardChanceDrop", "UseDropChanceForOldShard", true, "Enable drop chance for Old shard drop. Work If drop old shard on.");
            DropNewAndOldShardTogether = Conf.Bind("ShardChanceDrop", "DropNewAndOldShardTogether", false, "Enable Drop New and Old Shard together. Work if active a both drop.");
            //--------Commands------------
            EnabledEditAmountCommand = Conf.Bind("Commands", "EnabledEditAmountCommand", true, "Enable edit amount command. Admins only.");
            EnabledEditChanceCommand = Conf.Bind("Commands", "EnabledEditChanceCommand", true, "Enable edit chance command. Admins only");
            EnabledShardDropCommand = Conf.Bind("Commands", "EnabledAdminShardDropCommand", true, "Enable shard drop command. Admins only");
            PlayerCommandEnabled = Conf.Bind("Commands", "PlayerCommandEnabled", true, "Enable player commands. All players.");
            //--------Params------------
            UpdateExistingShards = Conf.Bind("Params", "UpdateExistingShards", true, "Enable update existing shards.");
            DropNewShards = Conf.Bind("Params", "DropNewShards", true, "Enable Drop new shard.");
            DropOldShards = Conf.Bind("Params", "DropOldShards", true, "Enable drop old shard.");
            EnableUltimateReplace = Conf.Bind("Params", "EnableUltimateReplace", true, "Enable ultimate skill replace.");
            //--------Message------------
            EnableSendMessages = Conf.Bind("Message", "EnableSendMessages", true, "Enable send messages from mod.");
            ReachShardLimit = Conf.Bind("Message", "ReachShardLimit", "The relic did not fall out. The limit on the number of relics [{relicTypeMod}] has been reached.", "No drop because reach limit.");
            NoDropLucky = Conf.Bind("Message", "NoDropLucky", "You're out of luck this time. The relic did not fall out. Try once more.", "No lucky drop.");
            ConfigBind();
        }
        public static void ConfigBind()
        {
            //--------ShardAmount----------
            InitData();
            //--------Durability------------
            DB.TimeUntilBroken = TimeUntilBroken.Value;
            DB.MaxDurability = MaxDurability.Value;
            DB.LifeTimeOldShard = LifeTimeOldShard.Value;
            DB.EnabledRepairInAltar = EnabledRepairInAltar.Value;
            DB.RepairMultiplier = RepairMultiplier.Value;
            DB.AdditionalRepairPoints = AdditionalRepairPoints.Value;
            //--------ChanceDrop------------
            if (((ChanceDropNewShard.Value + ChanceDropOldShard.Value) > 1) && DropOldShards.Value && DropNewShards.Value)
            {
                ChanceDropNewShard.Value = 0.5f;
                ChanceDropOldShard.Value = 0.5f;
                Plugin.Logger.LogWarning($"Incorrect drop chance. setup default setting (50%). ");
            }
            //--------Commands------------
            DB.EnabledEditAmountCommand = EnabledEditAmountCommand.Value;
            DB.EnabledEditChanceCommand = EnabledEditChanceCommand.Value;
            DB.EnabledShardDropCommand = EnabledShardDropCommand.Value;
            DB.PlayerCommandEnabled = PlayerCommandEnabled.Value;
            //--------Params------------
            DB.ChanceDropNewShard = ChanceDropNewShard.Value;
            DB.ChanceDropOldShard = ChanceDropOldShard.Value;
            DB.DestroyItemWhenBroken = DestroyItemWhenBroken.Value;
            DB.UpdateExistingShards = UpdateExistingShards.Value;
            DB.UseDropChanceForNewShard = UseDropChanceForNewShard.Value;
            DB.UseDropChanceForOldShard = UseDropChanceForOldShard.Value;
            DB.DropNewAndOldShardTogether = DropNewAndOldShardTogether.Value;
            //--------Message------------
            DB.EnableSendMessages = EnableSendMessages.Value;
            DB.ReachShardLimit = ReachShardLimit.Value;
            DB.NoDropLucky = NoDropLucky.Value;
        }
        internal static void Save()
        {

            //--------ShardAmount----------
            ReadData();
            //--------Durability------------
            TimeUntilBroken.Value = DB.TimeUntilBroken;
            MaxDurability.Value = DB.MaxDurability;
            LifeTimeOldShard.Value = DB.LifeTimeOldShard;
            AdditionalRepairPoints.Value = DB.AdditionalRepairPoints;
            EnabledRepairInAltar.Value = DB.EnabledRepairInAltar;
            RepairMultiplier.Value = DB.RepairMultiplier;

            //--------ChanceDrop------------
            ChanceDropNewShard.Value = DB.ChanceDropNewShard;
            ChanceDropOldShard.Value = DB.ChanceDropOldShard;
            //--------Params------------
            DestroyItemWhenBroken.Value = DB.DestroyItemWhenBroken;
            UpdateExistingShards.Value = DB.UpdateExistingShards;
            UseDropChanceForNewShard.Value = DB.UseDropChanceForNewShard;
            UseDropChanceForOldShard.Value = DB.UseDropChanceForOldShard;
            DropNewAndOldShardTogether.Value = DB.DropNewAndOldShardTogether;
            //--------Message------------
            EnableSendMessages.Value = DB.EnableSendMessages;
            ReachShardLimit.Value = DB.ReachShardLimit;
            NoDropLucky.Value = DB.NoDropLucky;
            //--------Commands------------
            EnabledEditAmountCommand.Value = DB.EnabledEditAmountCommand;
            EnabledEditChanceCommand.Value = DB.EnabledEditChanceCommand;
            EnabledShardDropCommand.Value = DB.EnabledShardDropCommand;
            PlayerCommandEnabled.Value = DB.PlayerCommandEnabled;

            Conf.Save();
        }
        public static void InitData()
        {
            int amount = 0;
            for (RelicTypeMod type = RelicTypeMod.TheMonster; type <= RelicTypeMod.OldBehemoth; type++)
            {
                if (type == RelicTypeMod.TheMonster) { amount = MaxNewShardAmountTheMonster.Value; }
                if (type == RelicTypeMod.Solarus) { amount = MaxNewShardAmountSolarus.Value; }
                if (type == RelicTypeMod.Dracula) { amount = MaxNewShardAmountDracula.Value; }
                if (type == RelicTypeMod.WingedHorror) { amount = MaxNewShardAmountWingedHorror.Value; }
                if (type == RelicTypeMod.OldBehemoth) { amount = MaxOldShardAmountBehemoth.Value; }
                if (type == RelicTypeMod.OldTheMonster) { amount = MaxOldShardAmountTheMonster.Value; }
                if (type == RelicTypeMod.OldWingedHorror) { amount = MaxOldShardAmountWingedHorror.Value; }
                DB.ShardsData.TryAdd(type, new ItemsData(0, amount));
                amount = 0;
            }

        }
        public static void ReadData()
        {
            int amount = 0;
            for (RelicTypeMod type = RelicTypeMod.TheMonster; type <= RelicTypeMod.OldBehemoth; type++)
            {
                amount = DB.ShardsData[type].MaxCount;
                if (type == RelicTypeMod.TheMonster) { MaxNewShardAmountTheMonster.Value = amount; }
                if (type == RelicTypeMod.Solarus) { MaxNewShardAmountSolarus.Value = amount; }
                if (type == RelicTypeMod.Dracula) { MaxNewShardAmountDracula.Value = amount; }
                if (type == RelicTypeMod.WingedHorror) { MaxNewShardAmountWingedHorror.Value = amount; }
                if (type == RelicTypeMod.OldTheMonster) { MaxOldShardAmountTheMonster.Value = amount; }
                if (type == RelicTypeMod.OldBehemoth) { MaxOldShardAmountBehemoth.Value = amount; }
                if (type == RelicTypeMod.OldWingedHorror) { MaxOldShardAmountWingedHorror.Value = amount; }
            }
        }
    }
}
