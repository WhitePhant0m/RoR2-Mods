using BepInEx;
using R2API.Utils;
using RoR2;

namespace SyncedTurrets
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.WPhantom.SyncedTurrets", "Synced Turrets", "1.0.0")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    internal class SyncedTurretsPlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.CharacterMaster.AddDeployable += new On.RoR2.CharacterMaster.hook_AddDeployable(AddBATComponentOnAddDeployableHook);
        }

        private static void AddBATComponentOnAddDeployableHook(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot)
        {
            orig(self, deployable, slot);
            if (slot == DeployableSlot.EngiTurret)
            {
                SyncedTurrets syncedTurrets = deployable.gameObject.AddComponent<SyncedTurrets>();
                syncedTurrets.OwnerCharacterMaster = self;
                syncedTurrets.Init();
            }
        }
    }
}