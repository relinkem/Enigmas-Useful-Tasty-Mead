using System;
using System.Reflection;
using HarmonyLib;

namespace EnigmasUsefulTastyMead
{
    // The boost itself. Runs only for the local player, and only while the Tasty Mead status effect is on them.
    // It never touches a food's timer or a current health/stamina/Eitr value (the game clamps those when a cap drops).
    internal static class TastyBoost
    {
        private static bool _wasBoosting;

        // SetMaxEitr is private in Player, so call it through a cached delegate.
        private static Action<Player, float, bool> _setMaxEitr;

        // Returns the boost strength 0..1 if the local player has the mead active, otherwise -1.
        internal static float ActiveWeight(Player p)
        {
            if (p == null || p != Player.m_localPlayer || TastyEffect.Effect == null) return -1f;
            StatusEffect live = p.GetSEMan().GetStatusEffect(TastyEffect.Effect.NameHash());
            if (live == null) return -1f;
            return BoostMath.Weight(live.GetRemaningTime(), live.m_ttl, Plugin.FadePower.Value);
        }

        // Max health, stamina and Eitr the way the game computes them, but with each food blended between its
        // real value now and its full (freshly eaten) value.
        internal static void Compute(Player p, float weight, out float hp, out float stamina, out float eitr)
        {
            hp = p.m_baseHP;
            stamina = p.m_baseStamina;
            eitr = 0f;
            float eitrWeight = Plugin.ApplyToEitr.Value ? weight : 0f;
            foreach (Player.Food food in p.GetFoods())
            {
                var shared = food.m_item.m_shared;
                hp += BoostMath.Blend(food.m_health, shared.m_food, weight);
                stamina += BoostMath.Blend(food.m_stamina, shared.m_foodStamina, weight);
                eitr += BoostMath.Blend(food.m_eitr, shared.m_foodEitr, eitrWeight);
            }
        }

        internal static void ApplyCaps(Player p, float hp, float stamina, float eitr, bool flash)
        {
            // Skip tiny changes so the synced max health value is not rewritten every frame for nothing.
            if (Math.Abs(p.GetMaxHealth() - hp) > 0.05f) p.SetMaxHealth(hp, flash);
            if (Math.Abs(p.GetMaxStamina() - stamina) > 0.05f) p.SetMaxStamina(stamina, flash);
            if (Math.Abs(p.GetMaxEitr() - eitr) > 0.05f)
            {
                if (_setMaxEitr == null)
                {
                    MethodInfo m = AccessTools.Method(typeof(Player), "SetMaxEitr");
                    _setMaxEitr = (Action<Player, float, bool>)Delegate.CreateDelegate(typeof(Action<Player, float, bool>), m);
                }
                _setMaxEitr(p, eitr, flash);
            }
        }

        internal static bool WasBoosting { get { return _wasBoosting; } set { _wasBoosting = value; } }
    }

    // The game recalculates food totals once per second through this method. While the boost is on, hand it the
    // boosted totals, so the game's own once-a-second update agrees with ours and never clamps current values early.
    [HarmonyPatch(typeof(Player), "GetTotalFoodValue")]
    internal static class GetTotalFoodValuePatch
    {
        private static void Postfix(Player __instance, ref float hp, ref float stamina, ref float eitr)
        {
            float w = TastyBoost.ActiveWeight(__instance);
            if (w < 0f) return;
            TastyBoost.Compute(__instance, w, out hp, out stamina, out eitr);
        }
    }

    // Every frame: keep the caps following the smooth fade between the game's once-a-second updates,
    // and put the real values back on the first frame after the boost ends.
    [HarmonyPatch(typeof(Player), "UpdateFood")]
    internal static class UpdateFoodPatch
    {
        private static void Postfix(Player __instance)
        {
            if (__instance != Player.m_localPlayer) return;

            float w = TastyBoost.ActiveWeight(__instance);
            if (w >= 0f)
            {
                float hp, stamina, eitr;
                TastyBoost.Compute(__instance, w, out hp, out stamina, out eitr);
                // Flash the bars once, on the frame the boost starts.
                TastyBoost.ApplyCaps(__instance, hp, stamina, eitr, !TastyBoost.WasBoosting);
                TastyBoost.WasBoosting = true;
            }
            else if (TastyBoost.WasBoosting)
            {
                float hp, stamina, eitr;
                TastyBoost.Compute(__instance, 0f, out hp, out stamina, out eitr);
                TastyBoost.ApplyCaps(__instance, hp, stamina, eitr, false);
                TastyBoost.WasBoosting = false;
            }
        }
    }
}
