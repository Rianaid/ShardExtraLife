# ShardExtraLife
`Server side only` mod to change shards params like is: durability, time to break, destroy when broken and drop amount. 
<details>
<summary>Changelog</summary>
`0.1.0`
- Initial public release of the mod
</details>

## Installation
* Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/)
* Install [VampireCommandFramework](https://v-rising.thunderstore.io/package/deca/VampireCommandFramework/)
* Extract _ShardExtraLife.dll_ into _(VRising server folder)/BepInEx/plugins_

## Configurable Values
```ini
[ShardExtraLife]

## Maximum "Dracula" shard amount.
# Setting type: Int32
# Default value: 1
MaxShardAmountDracula = 1

## Maximum "Solarus" shard amount.
# Setting type: Int32
# Default value: 1
MaxShardAmountSolarus = 1

## Maximum "TheMonster" shard amount.
# Setting type: Int32
# Default value: 1
MaxShardAmountTheMonster = 1

## Maximum "Winged Horror" shard amount.
# Setting type: Int32
# Default value: 1
MaxShardAmountWingedHorror = 1

## Shard Max durability.
# Setting type: Single
# Default value: 2500
MaxDurability = 2500

## Shard time before destroy.
# Setting type: Single
# Default value: 1296000
TimeUntilBroken = 1296000

## Destroy shard when broken.
# Setting type: Boolean
# Default value: true
DestroyItemWhenBroken = true

## Update existing shards.
# Setting type: Boolean
# Default value: true
UpdateExistingShards = true

```
### Credits

This mod idea was suggested by [@Bromelda](https://ideas.vrisingmods.com/posts/112/shard-replenish-mod) on our community idea tracker. Please vote and suggest your ideas [here](https://ideas.vrisingmods.com/).

[V Rising Mod Community](https://discord.gg/vrisingmods) is the best community of mods for V Rising.

[@Deca](https://github.com/decaprime), thank you for the exceptional frameworks [VampireCommandFramework](https://github.com/decaprime/VampireCommandFramework).

