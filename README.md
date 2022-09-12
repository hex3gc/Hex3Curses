# Hex3Curses

Adds a new tier of item called **Curses**. Curses inflict negative effects that you must build around, and are intended to add more difficulty to the game.

Curses are obtained at the end of each stage, after the teleporter event ends. You will gain a certain amount of them depending on your difficulty level:
* **Drizzle:** No curses
* **Rainstorm:** 1 curse every stage
* **Monsoon:** 2 curses every stage
* **Eclipse 3:** 3 curses every stage <sup>(Replaces existing modifier)</sup>

Ways to cleanse curses and support for modded difficulties will be added in later updates.

| Icon  | Description |
| ------------- | ------------- |
| | <p align="center">**COMMON**</p> |
| <img src="https://cdn.discordapp.com/attachments/980836743894941696/1016887347222151178/Forgetfulness.png?raw=true" width=128> | **Forgetfulness**<br>Each time you enter a stage, you will **lose** 2 random items <sup>(+2 per stack)</sup>. Prefers rarities: [<sup>C</sup>80%/<sup>Uc</sup>20%/<sup>L</sup>0%] |
| <img src="https://cdn.discordapp.com/attachments/980836743894941696/1018264171147694140/wip.png?raw=true" width=128> | **Exposed**<br>Enemies gain **10% critical strike chance** <sup>(+10% per stack)</sup>. |
| <img src="https://cdn.discordapp.com/attachments/980836743894941696/1018264171147694140/wip.png?raw=true" width=128> | **Tortured**<br>All interactables have a **LETHAL blood cost** of **10% hp** <sup>(+5% per stack, capped at 90%)</sup>. |
| | <p align="center">**UNCOMMON**</p> |
| <img src="https://cdn.discordapp.com/attachments/980836743894941696/1016887347478011934/Fragility.png?raw=true" width=128> | **Fragility**<br>Fall damage is **100%** stronger <sup>(+100% per stack)</sup> and **lethal**. |
| | <p align="center">**RARE**</p> |
| <img src="https://cdn.discordapp.com/attachments/980836743894941696/1017083093598883902/Impatience.png?raw=true" width=128> | **Impatience**<br>Spending more than **5** minutes on a stage will begin to **drain your max health.** By **10** minutes, your max health will be fully reduced to **25%** of its original value.</style> Stacking causes health drain to speed up by **10%.** |

All of these items can be removed or configured to your liking. Special thanks to the RoR2 Modding Discord for teaching me how to do this... again.

# To do:

* More curses
* Ensure the core mechanics work in multiplayer
* Mod integration (Difficulties, Eclipse artifacts)

# Bugs

* Please give feedback/bug reports on the RoR2 Modding discord, or by messaging directly: hex3#7952

# Changelog

### 0.1.0
* **Initial release**
* **Added curse "Forgetfulness" (Common)**
* **Added curse "Exposed" (Common)**
* **Added curse "Tortured" (Common)**
* **Added curse "Fragility" (Uncommon)**
* **Added curse "Impatience" (Rare)**