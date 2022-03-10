using RoR2;
using UnityEngine;
using static RoR2.RoR2Content.Items;

namespace SyncedTurrets
{
    public class SyncedTurrets : MonoBehaviour
    {
        internal void Init()
        {
            OwnerCharacterMaster.inventory.onInventoryChanged += UpdateInventory;
        }

        private void UpdateInventory()
        {
            if (gameObject == null)
            {
                Destroy(this);
                return;
            }

            CharacterMaster component = gameObject.GetComponent<CharacterMaster>();
            if (component == null || !component)
            {
                return;
            }

            Inventory inventory = component.inventory;
            if (OwnerCharacterMaster.inventory == null || !OwnerCharacterMaster.inventory)
            {
                OwnerCharacterMaster = gameObject.GetComponent<Deployable>().ownerMaster;
            }

            int itemCount = inventory.GetItemCount(ExtraLife.itemIndex);

            int itemCount2 = inventory.GetItemCount(ExtraLifeConsumed.itemIndex);

            inventory.CopyItemsFrom(OwnerCharacterMaster.inventory);
            inventory.ResetItem(WardOnLevel.itemIndex);
            inventory.ResetItem(BeetleGland.itemIndex);
            inventory.ResetItem(CrippleWardOnLevel.itemIndex);
            inventory.ResetItem(ExtraLife.itemIndex);
            inventory.ResetItem(ExtraLifeConsumed.itemIndex);
            inventory.GiveItem(ExtraLife.itemIndex, itemCount);
            inventory.GiveItem(ExtraLifeConsumed.itemIndex, itemCount2);

            component.GetBody().RecalculateStats();
        }

        private void OnDisable()
        {
            OwnerCharacterMaster.inventory.onInventoryChanged -= UpdateInventory;
            Destroy(this);
        }

        internal CharacterMaster OwnerCharacterMaster;
    }
}