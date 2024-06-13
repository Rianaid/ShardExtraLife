using ShardExtraLife.Utils;
using Stunlock.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using Unity.Entities;

namespace ShardExtraLife.Databases
{
    internal interface IItemsData
    {
        ItemsData SetMaxAmount(int amount);
    }
    internal struct ItemsData : IItemsData
    {
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public List<Entity> Entities { get; set; }
        public bool canDrop()
        {
            return Count < MaxCount;

        }
        public ItemsData(int count = 0, int maxcount = 5)
        {
            Count = count;
            MaxCount = maxcount;
            Entities = new List<Entity>();
        }
        public ItemsData SetMaxAmount(int amount)
        {
            return new ItemsData(Count, amount) { Entities = this.Entities };
        }
    }
    public enum RelicTypeMod : byte
    {
        None,
        TheMonster,
        Solarus,
        WingedHorror,
        Dracula,
        OldTheMonster,
        OldWingedHorror,
        OldBehemoth
    }
    internal class DB
    {
        //--------ChanceDrop------------
        internal static float ChanceDropNewShard = 0.5f; // 0.5
        internal static float ChanceDropOldShard = 0.5f; // 0.5
        internal static bool UseDropChanceForNewShard = true;
        internal static bool UseDropChanceForOldShard = true;
        internal static bool DropNewAndOldShardTogether = true;
        //--------Durability------------
        internal static float TimeUntilBroken = 129600f;//129600
        internal static float MaxDurability = 2500f;//2500
        internal static float LifeTimeOldShard = 5400f;//5400
        internal static float RepairMultiplier = 1.01f; // +1%
        internal static bool EnabledRepairInAltar = true;
        internal static float AdditionalRepairPoints = 1f;
        //--------Commands------------
        internal static bool EnabledEditAmountCommand = true;
        internal static bool EnabledEditChanceCommand = true;
        internal static bool EnabledShardDropCommand = true;
        internal static bool PlayerCommandEnabled = true;
        //--------Params------------
        internal static bool UpdateExistingShards = true;
        internal static bool DropNewShards = true;
        internal static bool DropOldShards = true;
        internal static bool DestroyNewWhenBroken = true;
        internal static bool DestroyOldWhenBroken = true;
        //--------Message------------
        internal static bool EnableSendMessages = true;
        internal static string ReachShardLimit = "";
        internal static string NoDropLucky = "";
        internal static ConcurrentDictionary<RelicTypeMod, ItemsData> ShardsData = new ConcurrentDictionary<RelicTypeMod, ItemsData>();
        internal static List<PrefabGUID> ShardSpellPrefabs = new List<PrefabGUID>();


        internal static List<string> NewRelicBuildingName = new List<string>()
        {
            "TM_Castle_Container_Specialized_Soulshards_Solarus",
            "TM_Castle_Container_Specialized_Soulshards_Monster",
            "TM_Castle_Container_Specialized_Soulshards_Manticore",
            "TM_Castle_Container_Specialized_Soulshards_Dracula"
        };
        internal static List<string> OldShardName = new List<string>()
        {
            "Item_Building_Relic_Manticore",
            "Item_Building_Relic_Monster",
            "Item_Building_Relic_Paladin",
            "Item_Building_Relic_Behemoth"
        };
        internal static List<string> NewShardNames = new List<string>()
        {
            "Item_MagicSource_SoulShard_Dracula",
            "Item_MagicSource_SoulShard_Manticore",
            "Item_MagicSource_SoulShard_Monster",
            "Item_MagicSource_SoulShard_Solarus"
        };
        internal static ImmutableDictionary<string, string> ShardNameBuff = ImmutableDictionary.Create<string, string>()
            .Add("Item_MagicSource_SoulShard_Dracula", "Item_EquipBuff_MagicSource_Soulshard_Dracula")
            .Add("Item_MagicSource_SoulShard_Manticore", "Item_EquipBuff_MagicSource_Soulshard_Manticore")
            .Add("Item_MagicSource_SoulShard_Monster", "Item_EquipBuff_MagicSource_Soulshard_TheMonster")
            .Add("Item_MagicSource_SoulShard_Solarus", "Item_EquipBuff_MagicSource_Soulshard_Solarus");
        internal static ImmutableDictionary<string, RelicTypeMod> BossNames = ImmutableDictionary.Create<string, RelicTypeMod>()
            .Add("CHAR_ChurchOfLight_Paladin_VBlood", RelicTypeMod.Solarus)
            .Add("CHAR_Gloomrot_Monster_VBlood", RelicTypeMod.TheMonster)
            .Add("CHAR_Manticore_VBlood", RelicTypeMod.WingedHorror)
            .Add("CHAR_Vampire_Dracula_VBlood", RelicTypeMod.Dracula)
            .Add("CHAR_Cursed_MountainBeast_VBlood", RelicTypeMod.OldBehemoth);
        internal static ImmutableDictionary<RelicTypeMod, string> RelicModShards = ImmutableDictionary.Create<RelicTypeMod, string>()
            .Add(RelicTypeMod.Solarus, "Item_MagicSource_SoulShard_Solarus")
            .Add(RelicTypeMod.TheMonster, "Item_MagicSource_SoulShard_Monster")
            .Add(RelicTypeMod.WingedHorror, "Item_MagicSource_SoulShard_Manticore")
            .Add(RelicTypeMod.Dracula, "Item_MagicSource_SoulShard_Dracula")
            .Add(RelicTypeMod.OldTheMonster, "Item_Building_Relic_Monster")
            .Add(RelicTypeMod.OldWingedHorror, "Item_Building_Relic_Manticore")
            .Add(RelicTypeMod.OldBehemoth, "Item_Building_Relic_Behemoth");

    }
}
