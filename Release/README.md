# Enigma's Useful Tasty Mead

*Makes the Tasty Mead worth drinking.*

Drink a Tasty Mead and your active foods count as freshly eaten again. Your max health, stamina and Eitr jump back up to the full values your foods gave when you ate them, then fade smoothly back down to what your food really gives. It is client side only, a dedicated server does not need it.

## Features

- Lifts max health, stamina and Eitr to the full values of your active foods.
- The boost fades back to what your food really gives. It holds up for a while and then drops toward the end, the same way food does.
- Does not heal you or refill anything. Only the maximums change. Your current health, stamina and Eitr recover the normal way.
- Never touches a food timer. When the boost ends, everything is exactly where it would have been without the mead.
- Keeps the normal Tasty Mead item, recipe and status icon with its countdown. You cannot drink a second one while the boost is active.
- Duration and fade shape are configurable.

## How it works

Every food gives you health, stamina and Eitr that shrink as it wears off, slowly at first and faster near the end. While the mead is active, each food is counted somewhere between what it really gives right now and what it gave when freshly eaten. Right after drinking that is the full amount, and it fades back to the real amount over the duration.

A food you just ate is already at full, so the mead adds nothing to it. A food that runs out during the boost simply drops out. If a maximum drops below your current health, stamina or Eitr, the game trims the current value down like it always does.

With the default settings the boost lasts 2 minutes. The mead replaces the vanilla Tasty Mead effect, so it no longer changes your health and stamina regen.

## Download

Install from Thunderstore with r2modman or the Thunderstore app. Dependencies are installed automatically:

[https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Useful_Tasty_Mead/](https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Useful_Tasty_Mead/)

To change settings in game, install Configuration Manager and press F1.

## Settings

Settings are in `BepInEx/config/EnigmaPrime.EnigmasUsefulTastyMead.cfg` after the first run.

- `DurationSeconds` (default 120): how long the boost lasts, and the time shown on the status icon. Range 5 to 600.
- `FadePower` (default 0.5): the shape of the fade. Boost strength is the time left divided by the duration, raised to this power. Food itself uses 0.3. A lower number holds the boost higher for longer and then crashes at the end, a higher number drops it sooner. Range 0.05 to 2.
- `ApplyToEitr` (default true): also boost the Eitr cap. Health and stamina are always boosted.

## Multiplayer and servers

Client side only. A dedicated server does not need it, and other players do not need it either. Only the player who has the mod gets the boost.

## Compatibility

- It replaces the effect of the vanilla Tasty Mead. Another mod that changes the Tasty Mead's duration or effects will conflict with it, and whichever changes it last wins.
- Mods that change how food or max health are calculated may interact with it, and have not been tested.
- Tested with Enigma's Item Salvager, Enigma's Realistic Smoke Rework, Extra Slots and Configuration Manager installed, with no problems.

## Known issues

- Controller support is built in but untested, please report problems.
- Only tested in single player so far. Multiplayer with another player has not been tested.

## Support

<a href="https://ko-fi.com/enigmaprime"><img src="https://raw.githubusercontent.com/relinkem/Enigmas-Useful-Tasty-Mead/main/Release/media/Ko-Fi-badge-web.png" alt="I asked AI for help and it told me to get Ko-fi" width="220" align="left" hspace="16"></a>

If you like the mod and want to say thanks, you can:<br>[buy me a coffee on Ko-fi](https://ko-fi.com/enigmaprime)

<br clear="left">

Bug reports and ideas are just as welcome. Open an issue on [GitHub](https://github.com/relinkem/Enigmas-Useful-Tasty-Mead/issues) and include your BepInEx log (`LogOutput.log`) and, if it is a settings problem, your config file.

## More mods by Enigma Prime

<a href="https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Realistic_Smoke_Rework/"><img src="https://raw.githubusercontent.com/RELinkem/Enigmas-Realistic-Smoke-Rework/main/images/icon.png" alt="Enigma's Realistic Smoke Rework icon" width="96" align="left" hspace="16"></a>

**[Enigma's Realistic Smoke Rework](https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Realistic_Smoke_Rework/)**<br>Smoke that rises, pools and bends in the wind, with flames that follow their fuel.

<br clear="left">

<a href="https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Item_Salvager/"><img src="https://raw.githubusercontent.com/relinkem/Enigmas-Item-Salvager/main/Release/icon.png" alt="Enigma's Item Salvager icon" width="96" align="left" hspace="16"></a>

**[Enigma's Item Salvager](https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Item_Salvager/)**<br>Take your gear apart at the Workbench, Forge, Black Forge or Galdr Table and get some of the materials back.

<br clear="left">

<a href="https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Enhanced_Forging/"><img src="https://raw.githubusercontent.com/relinkem/Enigmas-Enhanced-Forging/main/Release/icon.png" alt="Enigma's Enhanced Forging icon" width="96" align="left" hspace="16"></a>

**[Enigma's Enhanced Forging](https://thunderstore.io/c/valheim/p/Enigma_Prime/Enigmas_Enhanced_Forging/)**<br>Smelters, kilns and the Eitr refinery become walk-away machines with inventory screens, a Light button and winding animations.

<br clear="left">

## How this mod was made

The ideas, the design, and every decision about how the Tasty Mead should work are mine: it only raises your maximums and never heals you, it leaves your food timers alone, and it fades out like food does instead of cutting off. However, I am not a programmer. The code was written by Claude, an AI assistant made by Anthropic, working from my direction through [Claude Code](https://claude.com/claude-code).

I want to be upfront about that. I am the one who plays the game, so I test every build in Valheim, read the game log for errors, and send back anything that looks or feels wrong until it behaves the way I intended. The code was also checked against Valheim's own code so it hooks in the right places. If you find a bug, please open an issue and I will work on a fix.

Not affiliated with or endorsed by Anthropic.

## Changelog

See [CHANGELOG.md](https://github.com/relinkem/Enigmas-Useful-Tasty-Mead/blob/main/Release/CHANGELOG.md).

## License

All rights reserved. You are free to download and use the mod, but please don't redistribute, modify or re-upload it without asking first, see [LICENSE](https://github.com/relinkem/Enigmas-Useful-Tasty-Mead/blob/main/Release/LICENSE). Valheim is the property of Iron Gate AB and Coffee Stain Publishing. This mod is not affiliated with or endorsed by them.
