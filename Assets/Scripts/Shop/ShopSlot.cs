using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private MoneyCounter moneyCounter;
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BaitCounter baitCounter;

    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemAmount;
    public TMP_Text itemPrice;

    public ItemData itemData;
    public UpgradeData upgradeData;    
    public int _itemAmount;
    public TMP_Text buyPriceText;

    // This bool prevents the player to buy the same upgrade twice
    public bool isUpgradeBought = false;

    void Start()
    {
        // Checking if it's an item or upgrade to show proper stats (i.e. name, value)
        if (itemData != null) {
            itemName.text = itemData.Name;
            //itemImage.sprite = itemData.Sprite;
            Instantiate<Image>(itemData.Sprite, itemImage.transform.position, Quaternion.identity, transform);
            buyPriceText.text = "Price: " + itemData.Price.ToString();
        } else if (upgradeData != null) {
            itemName.text = upgradeData.Name;
            isUpgradeBought = false;
            //itemImage.sprite = itemData.sprite;
            Instantiate<Image>(upgradeData.Sprite, itemImage.transform.position, Quaternion.identity, transform);
            buyPriceText.text = "Price: " + upgradeData.Price.ToString();
        }
    }

    void Update()
    {
        // For now, this won't be used
        // But it's good to have it here in case we use it
        // If not, just delete it
        //itemAmount.text = "Amount: " + _itemAmount.ToString();

        // This if is here to check if the upgrade 1 or 2 is bought
        // It's to prevent that we don't have both upgrades bought
        // I hate it... very much
        if (upgradeData != null) {
            switch(upgradeData.Type) {
                case "Inventory Upgrade 1":
                {
                    if (playerController.hasInvUpgradeII || playerController.hasSpdUpgradeI) {
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Inventory Upgrade 2":
                {
                    if (playerController.hasInvUpgradeI || playerController.hasSpdUpgradeI) {
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Speed Upgrade 1":
                {
                    if (playerController.hasInvUpgradeI || playerController.hasInvUpgradeII) {
                        isUpgradeBought = false;
                    }
                    break;
                }
            }
        }
    }

    public void Buy() 
    {
        // Checking if it's an item or data so that we can properly buy it
        if (itemData != null) {
            for (int i = 0; i < inventory.slots.Count; i++) {
                if (inventory._isFull[i] == false && moneyCounter.money >= itemData.Price) {
                    if (itemData.Type == "Bait") {
                        if (baitCounter.bait < baitCounter.maxBait) {
                            baitCounter.bait++;
                            moneyCounter.money -= itemData.Price;
                        }
                    } else {
                        moneyCounter.money -= itemData.Price;
                        // We are using the same event that we use for when we succesfully finish the fishing minigame
                        EventBus.Instance.PickUpItem(itemData);
                        // We need this break here so that when we press the Buy button we don't buy a bunch of the same item
                        // So, if we have 50 coins and the item costs 10... we will buy 5 of them and we don't want that
                        // The break will only be used in the Buy function and not on the Sell function
                        // Since we do want to sell every instance of the item if it's on the inventory
                    }
                    break;
                }
            }
        } 
        else if (upgradeData != null) {
            if (moneyCounter.money >= upgradeData.Price && !isUpgradeBought) {
                switch(upgradeData.Type) 
                {
                    case "Inventory Upgrade 1":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasInvUpgradeI = true;
                            playerController.hasInvUpgradeII = false;
                            playerController.hasSpdUpgradeI = false;

                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }
                        break;
                    }
                    case "Inventory Upgrade 2":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasInvUpgradeI = false;
                            playerController.hasInvUpgradeII = true;
                            playerController.hasSpdUpgradeI = false;

                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }
                        break;
                    }
                    case "Speed Upgrade 1":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasInvUpgradeI = false;
                            playerController.hasInvUpgradeII = false;
                            playerController.hasSpdUpgradeI = true;
                            
                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }
                        break;
                    }
                }
            }
        }
    }

    public void Sell()
    {
        // Checking if it's an item or data so that we can properly sell it
        if (itemData != null) {
            if (itemData.Type == "Bait") {
                if (baitCounter.bait > 0) {
                    baitCounter.bait--;
                    moneyCounter.money += itemData.Price / 2;
                }
            } else {
                for (int i = 0; i < inventory.slots.Count; i++) {
                    if (itemData == inventory.slots[i].itemData) {
                        // Debugging to see if the slot is being selected corretcly
                        // If at some point it logs "null", it will be needed to look at what is causing this
                        Debug.Log(inventory.slots[i].itemData);

                        // We are checking if the itemData type is equal to "Fish"
                        // So when the player sells it, it gets the full price
                        // The price will be half of the original price if its another kind of item
                        if (inventory.slots[i].itemData.Type == "Fish")
                            moneyCounter.money += itemData.Price;
                        else
                            moneyCounter.money += itemData.Price / 2;

                        inventory.slots[i].itemData = null;
                    }
                }
            }
        } 
        else if (upgradeData != null) {
            if (isUpgradeBought) {
                switch(upgradeData.Type) 
                {
                    case "Inventory Upgrade 1":
                    {
                        if (isUpgradeBought) {
                            playerController.hasInvUpgradeI = false;
                            playerController.hasInvUpgradeII = false;
                            playerController.hasSpdUpgradeI = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }

                        break;
                    }
                    case "Inventory Upgrade 2":
                    {
                        if (isUpgradeBought) {
                            playerController.hasInvUpgradeI = false;
                            playerController.hasInvUpgradeII = false;
                            playerController.hasSpdUpgradeI = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                    case "Speed Upgrade 1":
                    {
                        if (isUpgradeBought) {
                            playerController.hasInvUpgradeI = false;
                            playerController.hasInvUpgradeII = false;
                            playerController.hasSpdUpgradeI = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                }
            }
        }
    }
}
