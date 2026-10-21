using System;

namespace EnigmasUsefulTastyMead
{
    // Pure math, no game types, so it can be tested on its own.
    internal static class BoostMath
    {
        // Boost strength from 1 (just drunk) down to 0 (gone).
        // remaining / duration, raised to the fade power. Same idea as the food curve (which uses 0.3).
        internal static float Weight(float remaining, float duration, float power)
        {
            if (duration <= 0f) return 0f;
            float t = remaining / duration;
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            return (float)Math.Pow(t, power);
        }

        // Between what the food really gives now (real) and what it gave when freshly eaten (full).
        // Never below real, never above full. A fresh food has real == full, so the boost adds nothing to it.
        internal static float Blend(float real, float full, float weight)
        {
            if (full <= real) return real;
            if (weight <= 0f) return real;
            if (weight >= 1f) return full;
            return real + (full - real) * weight;
        }
    }
}
