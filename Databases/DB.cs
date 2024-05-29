using ProjectM.Shared;
using Stunlock.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Entities;

namespace ShardExtraLife.Databases
{
    internal struct ItemsData
    {
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public List<Entity> Entities { get; set; }
        public ItemsData(int count = 0, int maxcount = 5)
        {
            Count = count;
            MaxCount = maxcount;
            Entities = new List<Entity>();
        }
    }
    internal class DB
    {
        internal static ConcurrentDictionary<RelicType, ItemsData> ShardsData = new ConcurrentDictionary<RelicType, ItemsData>();
        internal static List<PrefabGUID> ShardPrefabs = new List<PrefabGUID>();
        internal static bool UpdateExistingShards = true;
        internal static bool EnabledCommand = true;
        internal static int MaxShardAmountTheMonster = 5;
        internal static int MaxShardAmountSolarus = 5;
        internal static int MaxShardAmountWingedHorror = 5;
        internal static int MaxShardAmountDracula = 5;
        internal static float TimeUntilBroken = 129600f;//129600
        internal static float MaxDurability = 2500f;//2500
        internal static bool DestroyItemWhenBroken = false;
        internal static List<string> ShardNames = new List<string>()
        {
            "Item_MagicSource_SoulShard_Dracula",
            "Item_MagicSource_SoulShard_Manticore",
            "Item_MagicSource_SoulShard_Monster",
            "Item_MagicSource_SoulShard_Solarus"
        };
    }
}
