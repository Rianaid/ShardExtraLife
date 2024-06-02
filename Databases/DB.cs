using ProjectM.Shared;
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
        //--------Commands------------
        internal static bool EnabledEditAmountCommand = true;
        internal static bool EnabledEditChanceCommand = true;
        internal static bool EnabledShardDropCommand = true;
        internal static bool PlayerCommandEnabled = true;
        //--------Params------------
        internal static bool UpdateExistingShards = true;
        internal static bool DropNewShards = true;
        internal static bool DropOldShards = true;
        internal static bool DestroyItemWhenBroken = true;
        internal static bool EnableUltimateReplace = true;

        internal static ConcurrentDictionary<RelicType, ItemsData> NewShardsData = new ConcurrentDictionary<RelicType, ItemsData>();
        internal static ConcurrentDictionary<RelicType, ItemsData> OldShardsData = new ConcurrentDictionary<RelicType, ItemsData>();
        internal static List<PrefabGUID> ShardPrefabs = new List<PrefabGUID>();


        internal static List<string> NewRelicBuildingName = new List<string>()
        {
            "TM_Castle_Container_Specialized_Soulshards_Solarus",
            "TM_Castle_Container_Specialized_Soulshards_Monster",
            "TM_Castle_Container_Specialized_Soulshards_Manticore",
            "TM_Castle_Container_Specialized_Soulshards_Dracula"
        };
        internal static List<string> OldRelicBuildingName = new List<string>()
        {
            "TM_Relic_SoulShard_Manticore",
            "TM_Relic_SoulShard_Monster",
            "TM_Relic_SoulShard_Paladin"
        };
        internal static List<string> OldShardName = new List<string>()
        {
            "Item_Building_Relic_Manticore",
            "Item_Building_Relic_Monster",
            "Item_Building_Relic_Paladin"
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
        internal static ImmutableDictionary<string, RelicType> BossNames = ImmutableDictionary.Create<string, RelicType>()
            .Add("CHAR_ChurchOfLight_Paladin_VBlood", RelicType.Solarus)
            .Add("CHAR_Gloomrot_Monster_VBlood", RelicType.TheMonster)
            .Add("CHAR_Manticore_VBlood", RelicType.WingedHorror)
            .Add("CHAR_Vampire_Dracula_VBlood", RelicType.Dracula);
        internal static ImmutableDictionary<RelicType, string> NewShard = ImmutableDictionary.Create<RelicType, string>()
            .Add(RelicType.Solarus, "Item_MagicSource_SoulShard_Solarus")
            .Add(RelicType.TheMonster, "Item_MagicSource_SoulShard_Monster")
            .Add(RelicType.WingedHorror, "Item_MagicSource_SoulShard_Manticore")
            .Add(RelicType.Dracula, "Item_MagicSource_SoulShard_Dracula");
        internal static ImmutableDictionary<RelicType, string> OldShard = ImmutableDictionary.Create<RelicType, string>()
            .Add(RelicType.Solarus, "Item_Building_Relic_Paladin")
            .Add(RelicType.TheMonster, "Item_Building_Relic_Monster")
            .Add(RelicType.WingedHorror, "Item_Building_Relic_Manticore")
            .Add(RelicType.Dracula, "Item_MagicSource_SoulShard_Dracula");
    }
}
