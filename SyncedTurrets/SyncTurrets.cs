using RoR2;
using System.Linq;
using UnityEngine;
using static RoR2.RoR2Content.Items;
using static RoR2.DLC1Content.Items;
using static RoR2.RoR2Content.Buffs;

namespace SyncedTurrets
{
    public class SyncTurrets : MonoBehaviour
    {
        internal CharacterMaster characterMaster;
#if DEBUG
        internal static BuffIndex[] Buffs;
#endif

        internal void Init()
        {
            characterMaster.inventory.onInventoryChanged += UpdateInventory;
#if DEBUG
            if (SyncedTurretsConfig.SharedBuffsToggle.Value)
            {
                On.RoR2.CharacterBody.UpdateBuffs += BuffShare;
                Buffs = SyncedTurretsConfig.SharedBuffs.Value.Split(',').Select(buff => (BuffIndex)int.Parse(buff)).ToArray();
            }
#endif
        }

        private void UpdateInventory()
        {
            if (gameObject == null)
            {
                Destroy(this);
                return;
            }

            var cMasterCurrent = gameObject.GetComponent<CharacterMaster>();
            if (cMasterCurrent == null || !cMasterCurrent)
                return;

            var turretInv = cMasterCurrent.inventory;
            if (characterMaster.inventory == null || !characterMaster.inventory)
                characterMaster = gameObject.GetComponent<Deployable>().ownerMaster;

            turretInv.CopyItemsFrom(characterMaster.inventory);

            turretInv.ResetItem(WardOnLevel);
            turretInv.ResetItem(BeetleGland);
            turretInv.ResetItem(CrippleWardOnLevel);

            turretInv.ResetItem(ExtraLife);
            turretInv.ResetItem(ExtraLifeConsumed);
            turretInv.GiveItem(ExtraLife, turretInv.GetItemCount(ExtraLife));
            turretInv.GiveItem(ExtraLifeConsumed, turretInv.GetItemCount(ExtraLifeConsumed));

            //DLC
            int itemCount = characterMaster.inventory.GetItemCount(DroneWeapons);

            turretInv.ResetItem(DroneWeapons);
            turretInv.ResetItem(DroneWeaponsBoost);
            turretInv.ResetItem(DroneWeaponsDisplay1);
            turretInv.ResetItem(DroneWeaponsDisplay2);
            if (itemCount > 0)
            {
                turretInv.GiveItem(DroneWeaponsBoost, itemCount);

                var itemDef = DroneWeaponsDisplay1;
                if (Random.value < 0.1f)
                    itemDef = DroneWeaponsDisplay2;

                turretInv.GiveItem(itemDef, itemCount);
            }

            turretInv.ResetItem(HealingPotion);
            turretInv.ResetItem(HealingPotionConsumed);
            turretInv.GiveItem(HealingPotion, turretInv.GetItemCount(HealingPotion));
            turretInv.GiveItem(HealingPotionConsumed, turretInv.GetItemCount(HealingPotionConsumed));

            turretInv.ResetItem(ExtraLifeVoid);
            turretInv.ResetItem(ExtraLifeVoidConsumed);
            turretInv.GiveItem(ExtraLifeVoid, turretInv.GetItemCount(ExtraLifeVoid));
            turretInv.GiveItem(ExtraLifeVoidConsumed, turretInv.GetItemCount(ExtraLifeVoidConsumed));

            turretInv.ResetItem(FragileDamageBonus);
            turretInv.ResetItem(FragileDamageBonusConsumed);
            turretInv.GiveItem(FragileDamageBonus, turretInv.GetItemCount(FragileDamageBonus));
            turretInv.GiveItem(FragileDamageBonusConsumed, turretInv.GetItemCount(FragileDamageBonusConsumed));

            cMasterCurrent.GetBody().RecalculateStats();
        }

#if DEBUG
        private void BuffShare(On.RoR2.CharacterBody.orig_UpdateBuffs orig, CharacterBody self, float deltaTime)
        {
            orig(self, deltaTime);

            if (gameObject == null)
            {
                Destroy(this);
                return;
            }

            var currentCm = gameObject.GetComponent<CharacterMaster>();

            if (currentCm == null || !currentCm)
                return;

            if (currentCm.GetBody() == null || !currentCm.GetBody())
                return;

            if (characterMaster == null || !characterMaster)
                return;

            if (characterMaster.GetBody() == null || !characterMaster.GetBody())
                return;

            foreach (var buffType in Buffs)
            {
                var characterMaster = gameObject.GetComponent<CharacterMaster>();

                if (this.characterMaster.GetBody().HasBuff(buffType))
                {
                    if (buffType == AffixBlue.buffIndex ||
                        buffType == AffixWhite.buffIndex ||
                        buffType == AffixRed.buffIndex ||
                        buffType == AffixPoison.buffIndex ||
                        buffType == NoCooldowns.buffIndex)
                    {
                        if (characterMaster.GetBody().HasBuff(buffType))
                            continue;

                        var timedBuffs = this.characterMaster.GetBody().timedBuffs;
                        foreach (var timedBuff in timedBuffs)
                        {
                            if (timedBuff.buffIndex == buffType)
                            {
                                var buffDuration = timedBuff.timer;
                                if (buffDuration <= 1f)
                                    break;

                                characterMaster.GetBody().AddTimedBuff(buffType, buffDuration);
                                break;
                            }
                        }
                    }
                    else if (buffType != NoCooldowns.buffIndex)
                    {
                        characterMaster.GetBody().AddBuff(buffType);
                    }
                }
                else if (characterMaster.GetBody().HasBuff(buffType))
                {
                    if (buffType == AffixBlue.buffIndex || buffType == AffixWhite.buffIndex ||
                        buffType == AffixRed.buffIndex || buffType == AffixPoison.buffIndex ||
                        buffType == NoCooldowns.buffIndex)
                    {
                        if (this.characterMaster.GetBody().HasBuff(buffType))
                            continue;

                        var timedBuffs = characterMaster.GetBody().timedBuffs;
                        foreach (var timedBuff in timedBuffs)
                        {
                            if (timedBuff.buffIndex == buffType)
                            {
                                var buffDuration = timedBuff.timer;
                                if (buffDuration <= 1f)
                                    break;

                                this.characterMaster.GetBody().AddTimedBuff(buffType, buffDuration);
                                break;
                            }
                        }
                    }
                    else
                    {
                        characterMaster.GetBody().RemoveBuff(buffType);
                    }
                }
            }
        }
#endif

        private void OnDisable()
        {
            characterMaster.inventory.onInventoryChanged -= UpdateInventory;
#if DEBUG
            if (SyncedTurretsConfig.SharedBuffsToggle.Value)
                On.RoR2.CharacterBody.UpdateBuffs -= BuffShare;
#endif
            Destroy(this);
        }
    }
}