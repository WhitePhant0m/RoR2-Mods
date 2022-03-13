#if DEBUG
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;

namespace SyncedTurrets
{
    internal class SyncedTurretsConfig
    {
        internal static ConfigEntry<bool> SharedBuffsToggle { get; set; }

        internal static ConfigEntry<string> SharedBuffs { get; set; }

        internal static void Init(ConfigFile Config)
        {
            SharedBuffsToggle = Config.Bind("!Toggles", "Shared Buffs", true, "Toggles if buffs should be shared with Engineer and their turret.");

            string defaultSharedBuffs = string.Join(",", new[]
            {
                10,
                11,
                17,
                27,
                28,
                29,
                30,
                31,
                33
            });

            SharedBuffs = Config.Bind("Shared", "Buffs", defaultSharedBuffs, "Buffs which the turret and Engineer share.");

            ModSettingsManager.AddOption(new CheckBoxOption(SharedBuffsToggle));
            ModSettingsManager.AddOption(new StringInputFieldOption(SharedBuffs));
            ModSettingsManager.SetModDescription("Syncs items/buffs between Engineer and his turrets whenever one of them gains/loses an item/buff");
        }
    }
}
#endif