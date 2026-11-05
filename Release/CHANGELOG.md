# Changelog

## 0.1.1
Fix.
- The mead no longer overwrites max health, stamina or Eitr that come from other mods. It now adds only its own extra on top of the current max, and takes exactly that back out when the boost ends. This fixes the Eitr bar disappearing, and Eitr regen stopping, for players whose max Eitr comes partly from gear such as Magic Plugin items. The Eitr no longer needs a re-equip to come back.

## 0.1.0
First release.
- Drinking a Tasty Mead makes your active foods count as freshly eaten. Max health, stamina and Eitr go up to the full values the foods gave when eaten, then fade back to what the food really gives.
- Does not heal you, and never changes a food timer.
- Uses the normal Tasty Mead item, recipe and status icon. A second mead cannot be drunk while the boost is active.
- Settings for the duration, the shape of the fade, and whether Eitr is boosted.
