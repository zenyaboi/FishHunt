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

    public GameObject itemImage;
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
            //itemImage.sprite = itemData.sprite;
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
            if (upgradeData.Type == "Upgrade 1") {
                if (playerController.hasUpgradeII) {
                    isUpgradeBought = false;
                }
            } else if (upgradeData.Type == "Upgrade 2") {
                if (playerController.hasUpgradeI) {
                    isUpgradeBought = false;
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
                    moneyCounter.money -= itemData.Price;
                    // We are using the same event that we use for when we succesfully finish the fishing minigame
                    EventBus.Instance.PickUpItem(itemData);
                    // We need this break here so that when we press the Buy button we don't buy a bunch of the same item
                    // So, if we have 50 coins and the item costs 10... we will buy 5 of them and we don't want that
                    // The break will only be used in the Buy function and not on the Sell function
                    // Since we do want to sell every instance of the item if it's on the inventory
                    break;
                }
            }
        } else if (upgradeData != null) {
            if (moneyCounter.money >= upgradeData.Price && !isUpgradeBought) {
                if (upgradeData.Type == "Upgrade 1") {
                    // I hate this
                    // I don't how to make it better
                    // But I think it works, I'm afraid to touch it
                    if (!playerController.hasUpgradeI || !playerController.hasUpgradeII || playerController.hasUpgradeII) {
                        playerController.hasUpgradeI = true;
                        playerController.hasUpgradeII = false;

                        moneyCounter.money -= upgradeData.Price;
                        isUpgradeBought = true;
                    }
                } else if (upgradeData.Type == "Upgrade 2") {
                    if (!playerController.hasUpgradeI || !playerController.hasUpgradeII || playerController.hasUpgradeI) {
                        playerController.hasUpgradeI = false;
                        playerController.hasUpgradeII = true;

                        moneyCounter.money -= upgradeData.Price;
                        isUpgradeBought = true;
                    }
                }
                /*
                moneyCounter.money -= upgradeData.Price;
                isUpgradeBought = true;
                */
            }
        }
    }

    public void Sell()
    {
        // Checking if it's an item or data so that we can properly sell it
        if (itemData != null) {
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
        } else if (upgradeData != null) {
            if (isUpgradeBought) {
                if (upgradeData.Type == "Upgrade 1") {
                    // I also hate this
                    if (playerController.hasUpgradeI && !playerController.hasUpgradeII) {
                        playerController.hasUpgradeI = false;
                        playerController.hasUpgradeII = false;

                        moneyCounter.money += upgradeData.Price / 2;
                        isUpgradeBought = false;
                    }
                } else if (upgradeData.Type == "Upgrade 2") {
                    if (playerController.hasUpgradeII && !playerController.hasUpgradeI) {
                        playerController.hasUpgradeI = false;
                        playerController.hasUpgradeII = false;

                        moneyCounter.money += upgradeData.Price / 2;
                        isUpgradeBought = false;
                    }
                }
                /*
                moneyCounter.money += upgradeData.Price / 2;
                isUpgradeBought = false;
                */
            }
        }
    }
}
