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
        public static ConfigEntry<float> TimeUntilBroken;
        public static ConfigEntry<float> MaxDurability;
        public static ConfigEntry<int> MaxShardAmountDracula;
        public static ConfigEntry<int> MaxShardAmountSolarus;
        public static ConfigEntry<int> MaxShardAmountTheMonster;
        public static ConfigEntry<int> MaxShardAmountWingedHorror;
        public static ConfigEntry<bool> DestroyItemWhenBroken;
        public static ConfigEntry<bool> UpdateExistingShards;
        public static void SettingsInit()
        {
            MaxShardAmountDracula = Conf.Bind("ShardExtraLife", "MaxShardAmountDracula", 1, "Maximum \"Dracula\" shard amount.");
            MaxShardAmountSolarus = Conf.Bind("ShardExtraLife", "MaxShardAmountSolarus", 1, "Maximum \"Solarus\" shard amount.");
            MaxShardAmountTheMonster = Conf.Bind("ShardExtraLife", "MaxShardAmountTheMonster", 1, "Maximum \"TheMonster\" shard amount.");
            MaxShardAmountWingedHorror = Conf.Bind("ShardExtraLife", "MaxShardAmountWingedHorror", 1, "Maximum \"Winged Horror\" shard amount.");
            MaxDurability = Conf.Bind("ShardExtraLife", "MaxDurability", 2500f, "Shard Max durability.");
            TimeUntilBroken = Conf.Bind("ShardExtraLife", "TimeUntilBroken", 1296000f, "Shard time before destroy.");
            DestroyItemWhenBroken = Conf.Bind("ShardExtraLife", "DestroyItemWhenBroken", true, "Destroy shard when broken.");
            UpdateExistingShards = Conf.Bind("ShardExtraLife", "UpdateExistingShards", true, "Update existing shards.");
            SettingsBind();
        }
        public static void SettingsBind()
        {
            DB.MaxShardAmountDracula = MaxShardAmountDracula.Value;
            DB.MaxShardAmountSolarus = MaxShardAmountSolarus.Value;
            DB.MaxShardAmountTheMonster = MaxShardAmountTheMonster.Value;
            DB.MaxShardAmountWingedHorror = MaxShardAmountWingedHorror.Value;
            DB.TimeUntilBroken = TimeUntilBroken.Value;
            DB.MaxDurability = MaxDurability.Value;
            DB.DestroyItemWhenBroken = DestroyItemWhenBroken.Value;
            DB.UpdateExistingShards = UpdateExistingShards.Value;
        }
    }
}
