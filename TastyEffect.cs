using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace EnigmasUsefulTastyMead
{
    // Re-tunes the vanilla Tasty Mead (prefab MeadTasty, status effect Potion_tasty) when the item database loads.
    // The status effect stays, so the icon, the countdown and the game's own "can't drink another" message are vanilla.
    // Only the numbers inside it change: duration comes from config, and the old regen changes are switched off.
    internal static class TastyEffect
    {
        internal const string ItemName = "MeadTasty";
        internal const string EffectName = "Potion_tasty";

        // The vanilla status effect object. Used to find the live copy on the player.
        internal static StatusEffect Effect;

        // Short item description (top of the item tooltip). The detailed text lives in the effect tooltip at the bottom,
        // so the two never repeat each other.
        internal const string Description = "A sweet mead that leaves you feeling full and refreshed again.";

        internal const string Tooltip =
            "Your active foods count as freshly eaten, lifting your max health, stamina and Eitr to the full values they gave when eaten. " +
            "The boost fades back to what your food really gives. Does not heal you and does not change food timers.";

        private static string _lastReport = "";

        internal static void Apply(ObjectDB db)
        {
            // The main menu has an empty item database. The real one arrives later through CopyOtherDB.
            if (db == null || db.m_items == null || db.m_items.Count == 0) return;

            GameObject prefab = db.m_items.Find(go => go != null && go.name == ItemName);
            ItemDrop drop = prefab != null ? prefab.GetComponent<ItemDrop>() : null;
            StatusEffect fromItem = drop != null ? drop.m_itemData.m_shared.m_consumeStatusEffect : null;
            StatusEffect fromList = db.m_StatusEffects != null
                ? db.m_StatusEffects.Find(se => se != null && se.name == EffectName)
                : null;

            if (fromItem == null && fromList == null)
            {
                Plugin.Log.LogWarning("Tasty Mead (" + ItemName + " / " + EffectName + ") not found in the item database, nothing changed.");
                return;
            }

            var seen = new HashSet<StatusEffect>();
            foreach (StatusEffect se in new[] { fromItem, fromList })
            {
                if (se == null || !seen.Add(se)) continue;
                Tune(se);
            }
            Effect = fromItem != null ? fromItem : fromList;

            if (drop != null)
                drop.m_itemData.m_shared.m_description = Description;

            string report = "Tasty Mead retuned: duration " + Plugin.DurationSeconds.Value + "s, fade power " + Plugin.FadePower.Value +
                            ", Eitr " + (Plugin.ApplyToEitr.Value ? "on" : "off") + ".";
            if (report != _lastReport)
            {
                Plugin.Log.LogInfo(report);
                _lastReport = report;
            }
        }

        private static void Tune(StatusEffect se)
        {
            se.m_ttl = Plugin.DurationSeconds.Value;
            se.m_tooltip = Tooltip;

            // Regen multipliers: 1 means no change. 0 would stop regen completely, so never use 0 here.
            SE_Stats stats = se as SE_Stats;
            if (stats != null)
            {
                stats.m_healthRegenMultiplier = 1f;
                stats.m_staminaRegenMultiplier = 1f;
                stats.m_eitrRegenMultiplier = 1f;
            }
            else
            {
                Plugin.Log.LogWarning(se.name + " is not an SE_Stats, regen values were not reset.");
            }
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "Awake")]
    internal static class ObjectDBAwakePatch
    {
        private static void Postfix(ObjectDB __instance) { TastyEffect.Apply(__instance); }
    }

    [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.CopyOtherDB))]
    internal static class ObjectDBCopyPatch
    {
        private static void Postfix(ObjectDB __instance) { TastyEffect.Apply(__instance); }
    }
}
