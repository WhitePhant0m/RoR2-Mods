using BepInEx;
using R2API.Utils;
using RoR2;
using System.Security.Permissions;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace SyncedTurrets
{
    [BepInDependency("com.bepis.r2api")]
#if DEBUG
    [BepInDependency("com.rune580.riskofoptions")]
#endif
    [BepInPlugin(ModID, ModName, ModVer)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    internal class SyncedTurrets : BaseUnityPlugin
    {
        private const string ModID = "com.WPhantom." + ModName;
        private const string ModName = "SyncedTurrets";
        private const string ModVer = "0.0.2";

        public void Awake()
        {
            On.RoR2.CharacterMaster.AddDeployable += new On.RoR2.CharacterMaster.hook_AddDeployable(HookThingy);
#if DEBUG
            SyncedTurretsConfig.Init(Config);
#endif
        }

        private static void HookThingy(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot)
        {
            orig(self, deployable, slot);
            if (slot == DeployableSlot.EngiTurret)
            {
                var syncTurrets = deployable.gameObject.AddComponent<SyncTurrets>();
                syncTurrets.characterMaster = self;
                syncTurrets.Init();
            }
        }
    }
}