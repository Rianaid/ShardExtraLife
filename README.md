# ShardExtraLife
`Server side only` mod to change shards params like is: durability, time to break, destroy when broken and drop amount. 

Return old shard drop. Repair in special pedestal. Turn off|on replace ultimate from shards. 

`Stress tests are required to find possible bugs.`
<details>
<summary>Changelog</summary>

0.2.1

-Add check "RelicSpawnType" for ignore limits when RelicSpawnType = Plentiful.

0.2.0 
- All mod rework. 
- Delete drop shard from VBlood Boss. 
- Create new drop system.
- Return Old shards and add to new drop system.
- Add repair system for shards special pedestal.

0.1.1
- Fix hot fix 5 errors.

0.1.0
- Initial public release of the mod.

</details>

## Installation
* Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/)
* Install [VampireCommandFramework](https://v-rising.thunderstore.io/package/deca/VampireCommandFramework/)
* Extract _ShardExtraLife.dll_ into _(VRising server folder)/BepInEx/plugins_

## Commands
Commands use CommandGroup. A commands start from `.shardextralife ` or `.sel `

For example:  `.sel ss me` - Show shard stats in player inventory. 

<details>
<summary>For Admins</summary>

 -`editamount [Type of shards] [Name of shard] [amount]` or `ea [type of shards] [name of shard] [amount]`

	Change amount for [New,Old or All] shard type of ["Dracula","Solarus","TheMonster","WingedHorror" or "All"] shards.

-`editchance [Action] [Chance drop new shards] [Chance drop old shards]` or `ec [Action] [Chance drop new shards] [Chance drop old shards]` 

	 [Set] [NewChance] [OldChance] - Setup new chance drop for new shards: [NewChance] and old shards: [OldChance].
	 [Check] - Show current drop shard chances.

-`adminsharddrop [Type of shard] [Name of shard] [Amount]` or `asd [Type of shard] [Name of shard] [Amount]`

     Drop ["New" or "Old"] shard type of ["Dracula","Solarus","TheMonster" or "WingedHorror"] in quantity [Amount]
	 This command throws out shards despite the limits.
</details>

<details>
<summary>For Players</summary>

 -`shardstatus [Where]` or `ss [Where]`

	Check status of shards in inventory: ["Me"] or global status of drop:["All"].

-`shardultimatereplace [Where]` or `sur [Where]` 

	 Change status of replace ultimate on shards. search shards for edit in ["Inventory", "Equipment" or "All"].

</details>

## Configurable Values

```ini
[Commands]

## Enable edit amount command. Admins only.
# Setting type: Boolean
# Default value: true
EnabledEditAmountCommand = true

## Enable edit chance command. Admins only
# Setting type: Boolean
# Default value: true
EnabledEditChanceCommand = true

## Enable shard drop command. Admins only
# Setting type: Boolean
# Default value: true
EnabledAdminShardDropCommand = true

## Enable player commands. All players.
# Setting type: Boolean
# Default value: true
PlayerCommandEnabled = true

[Params]

## Enable update existing shards.
# Setting type: Boolean
# Default value: true
UpdateExistingShards = true

## Enable Drop new shard.
# Setting type: Boolean
# Default value: true
DropNewShards = true

## Enable drop old shard.
# Setting type: Boolean
# Default value: true
DropOldShards = true

## Enable ultimate skill replace.
# Setting type: Boolean
# Default value: true
EnableUltimateReplace = true

[ShardAmount]

## Maximum new "Dracula" shard amount if drop new shards "True".
# Setting type: Int32
# Default value: 1
MaxShardAmountDracula = 1

## Maximum new "Solarus" shard amount if drop new shards "True".
# Setting type: Int32
# Default value: 1
MaxShardAmountSolarus = 1

## Maximum new "TheMonster" shard amount if drop new shards "True".
# Setting type: Int32
# Default value: 1
MaxShardAmountTheMonster = 1

## Maximum new "Winged Horror" shard amount if drop new shards "True".
# Setting type: Int32
# Default value: 1
MaxShardAmountWingedHorror = 1

## Maximum old "Solarus" shard amount if drop old shards "True".
# Setting type: Int32
# Default value: 1
MaxOldShardAmountSolarus = 1

## Maximum old "TheMonster" shard amount if drop old shards "True".
# Setting type: Int32
# Default value: 1
MaxOldShardAmountTheMonster = 1

## Maximum old "Winged Horror" shard amount if drop old shards "True".
# Setting type: Int32
# Default value: 1
MaxOldShardAmountWingedHorror = 1

[ShardChanceDrop]

## Chance drop new shard.
# Setting type: Single
# Default value: 0.5
ChanceDropNewShard = 0.5

## Chance drop old shard.
# Setting type: Single
# Default value: 0.5
ChanceDropOldShard = 0.5

## Enable drop chance for new shard drop. Work if drop new shard on.
# Setting type: Boolean
# Default value: true
UseDropChanceForNewShard = true

## Enable drop chance for Old shard drop. Work If drop old shard on.
# Setting type: Boolean
# Default value: true
UseDropChanceForOldShard = true

## Enable Drop New and Old Shard together. Work if active a both drop.
# Setting type: Boolean
# Default value: false
DropNewAndOldShardTogether = false

[ShardExtraLife]

## Max durability for new shards.
# Setting type: Single
# Default value: 2500
MaxDurability = 2500

## Max lifetime for old shard.
# Setting type: Single
# Default value: 4200
LifeTimeOldShard = 4200

## Shard time before destroy.
# Setting type: Single
# Default value: 1296000
TimeUntilBroken = 1296000

## Shard recovery multiplier. Currently the timer runs every 60 seconds.
# Setting type: Single
# Default value: 1.01
RepairMultiplier = 1.01

## Enable repair shard in special pedestal.
# Setting type: Boolean
# Default value: true
EnabledRepairInAltar = true

## Enable destroy shard when broken.
# Setting type: Boolean
# Default value: true
DestroyItemWhenBroken = true
```

### Credits

This mod idea was suggested by [@Bromelda](https://ideas.vrisingmods.com/posts/112/shard-replenish-mod) and [@Vex](https://ideas.vrisingmods.com/posts/152/old-shards-back) on our community idea tracker. Please vote and suggest your ideas [here](https://ideas.vrisingmods.com/).

[V Rising Mod Community](https://discord.gg/vrisingmods) is the best community of mods for V Rising.

[@Deca](https://github.com/decaprime), thank you for the exceptional frameworks [VampireCommandFramework](https://github.com/decaprime/VampireCommandFramework).

