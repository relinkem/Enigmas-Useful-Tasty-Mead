# Enigma's Useful Tasty Mead: setup

Same tooling as Enigma's Item Salvager and Enhanced Forging (Visual Studio with the .NET desktop workload, .NET Framework 4.8 targeting pack).
Status: version 0.1.0, release candidate (2026-10-20). Working in game in single player. Design notes are in project-notes-2026-10-20.md, research in research-findings-2026-10-20.md.

## Build
1. Close Valheim.
2. In the Mod Preview profile in r2modman, make sure BepInExPack Valheim is installed. Jotunn is NOT needed for this project yet.
3. Open EnigmasUsefulTastyMead.csproj in Visual Studio. Build > Rebuild Solution.
4. The DLL is copied to `%APPDATA%\r2modmanPlus-local\Valheim\profiles\Mod Preview\BepInEx\plugins\EnigmaPrime-Enigmas_Useful_Tasty_Mead\`.
   Only the Mod Preview profile is touched. Valheim Modded is never written to.
   If the profile is not found, the build says so; edit TestProfile and ProfilesRoot at the top of the csproj.

## Files
- Plugin.cs: BepInEx entry point, GUID `EnigmaPrime.EnigmasUsefulTastyMead`, version 0.1.0. Config: DurationSeconds (120), FadePower (0.5), ApplyToEitr (true).
- TastyEffect.cs: when the item database loads, finds MeadTasty and its status effect Potion_tasty, sets the duration from config, switches the old regen changes off (multipliers set to 1, never 0), and replaces the tooltip and item description.
- TastyBoost.cs: the boost. Patches Player.GetTotalFoodValue (the game's once-a-second food total) and Player.UpdateFood (every frame) so the caps follow the fade smoothly. Local player only, only while Potion_tasty is on them.
- BoostMath.cs: the fade weight and per-food blend, no game types, unit tested outside the game.
- MeadDump.cs: TEMPORARY console commands `meaddump`, and `agefood [percent left]` (sets active foods to that percent of their time left). Writes every item with "mead" or "tasty" in its name, plus its consume status effect, to `BepInEx\EnigmasUsefulTastyMead\dump\meads.txt` in the profile. The whole file is inside `#if DEBUG`, so the Release build contains none of it. Build the published DLL with the Release configuration.
- libs\: BepInEx.dll and 0Harmony.dll (copied from Enhanced Forging), plus Valheim's own assembly_valheim.dll, assembly_utils.dll, assembly_guiutils.dll, UnityEngine.dll and UnityEngine.CoreModule.dll (copied from the Valheim install on 2026-10-20). The csproj reads them from here. Re-copy them from `...\Valheim\valheim_Data\Managed` after a Valheim update. These are the game's files: never put libs\ in a Thunderstore zip or a public repo. A release zip holds only the mod DLL, README, manifest, icon, LICENSE.

## In-game test checklist
1. Load a world with the three foods eaten (or use the console to give yourself food). Note max health, stamina, Eitr.
   Food strength drops slowly at first, so fresh-ish foods show almost no boost. To test properly, run `agefood 10` in the console (F5, cheats on) to set every active food to 10% of its time left, which makes them nearly spent. `agefood 50` for halfway. Food values update within a second. `timescale 5` speeds up the whole game if you want to watch a full fade quickly, `timescale 1` to go back. `skiptime` and `sleep` do NOT age food.
2. Drink a Tasty Mead (spawn with `spawn MeadTasty` in the console if cheats are on, or craft one). Expect: icon with a 60 second countdown, bars flash, caps jump to the full values the foods gave when eaten.
3. Current health, stamina and Eitr do NOT jump up. They only fill the normal way.
4. Watch the caps fade: still high at 30 seconds left, around 40% of the boost at 6 seconds left, then a quick drop. At 0 the caps equal the food's real values.
5. Food timers (hover the food icons) keep counting down normally during and after.
6. Try to drink a second mead while the icon is up: it should refuse with the game's "can't consume" message and keep the mead.
7. Eat a new food mid-boost: nothing odd, that food just counts at full anyway.
8. Let a food run out during the boost: it drops out, no errors in BepInEx\LogOutput.log.
9. Save, quit and reload during the boost: it should end cleanly, caps equal the real food values.
10. Check the tooltip on the mead and on the status icon reads the new text, with no regen lines.
11. If nothing happens at all: check LogOutput.log for "Patched: Player.GetTotalFoodValue" and "Patched: Player.UpdateFood" and "Tasty Mead ... not found".
12. Controller: drinking uses the normal consume action. Still untested, list under Known issues in the README.

Config file: `BepInEx\config\EnigmaPrime.EnigmasUsefulTastyMead.cfg` in the profile, appears after the first launch.

## Test the dump
1. Launch the Mod Preview profile, load into any world.
2. Press F5 for the console, type `meaddump`, press Enter.
3. The console prints how many items were written and where. Send that meads.txt (or paste it) to Claude.
4. Also check BepInEx\LogOutput.log shows "Enigma's Useful Tasty Mead 0.0.1 loaded" and "Patched: Terminal.InitTerminal".

## Notes
- Compiled clean (no errors or warnings) with Mono's mcs against these libs on 2026-10-20, and BoostMath passed a standalone test run (fade values, blend limits, the 150 base example). Visual Studio's build is still the real test.

## Release
Files for the published package are in `Release\`: manifest.json, README.md, CHANGELOG.md, LICENSE, media\Ko-Fi-badge-web.png. Still needed before upload: icon.png (256x256), the GitHub repo name (README has REPO_NAME where it goes), and the Thunderstore page address after the first upload (README has THUNDERSTORE_PAGE_URL).
1. Switch Visual Studio to the Release configuration and Rebuild. The DLL is `bin\Release\EnigmasUsefulTastyMead.dll`.
2. Copy that DLL into `Release\`.
3. Zip manifest.json, README.md, CHANGELOG.md, icon.png and EnigmasUsefulTastyMead.dll (same five files as the Salvager zip). Name it Enigmas_Useful_Tasty_Mead-0.1.0.zip.
4. libs\ holds Valheim's own DLLs and is ignored by .gitignore. Never push or zip it.
