using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace EnigmasUsefulTastyMead
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "EnigmaPrime.EnigmasUsefulTastyMead";
        public const string PluginName = "Enigma's Useful Tasty Mead";
        public const string PluginVersion = "0.1.0";

        internal static ManualLogSource Log;

        internal static ConfigEntry<float> DurationSeconds;
        internal static ConfigEntry<float> FadePower;
        internal static ConfigEntry<bool> ApplyToEitr;

        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;

            DurationSeconds = Config.Bind("General", "DurationSeconds", 120f,
                new ConfigDescription("How long the Tasty Mead boost lasts, in seconds. This is also the time shown on the status icon.",
                    new AcceptableValueRange<float>(5f, 600f)));
            FadePower = Config.Bind("General", "FadePower", 0.5f,
                new ConfigDescription("Shape of the fade. The boost strength is (time left / duration) raised to this power. Food itself uses 0.3. Higher means it drops sooner, lower means it holds longer and then crashes at the end.",
                    new AcceptableValueRange<float>(0.05f, 2f)));
            ApplyToEitr = Config.Bind("General", "ApplyToEitr", true,
                "Also boost the Eitr cap from food (health and stamina are always boosted).");

            // Re-tune the mead when a setting is changed in-game (for example with the configuration manager).
            Config.SettingChanged += (sender, args) => TastyEffect.Apply(ObjectDB.instance);

            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll();
            foreach (var m in _harmony.GetPatchedMethods())
                Log.LogInfo($"Patched: {m.DeclaringType?.Name}.{m.Name}");

            Log.LogInfo($"{PluginName} {PluginVersion} loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
