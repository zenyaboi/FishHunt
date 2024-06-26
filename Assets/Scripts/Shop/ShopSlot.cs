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
    public Image sprite;
    public Image baitUpgradeSprite;

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
            sprite = Instantiate<Image>(itemData.Sprite, itemImage.transform.position, Quaternion.identity, transform);
            sprite.transform.localScale = new Vector3(.7f, .7f, .7f);
            if (itemData.Type == "Bait") {
                itemAmount.text = "Amount: " + baitCounter.bait.ToString();
                buyPriceText.text = "Price: " + itemData.Price.ToString();
            } else {
                itemAmount.text = " ";
                buyPriceText.text = " ";
            }
        } else if (upgradeData != null) {
            itemName.text = upgradeData.Name;
            isUpgradeBought = false;
            //itemImage.sprite = itemData.sprite;
            sprite = Instantiate<Image>(upgradeData.Sprite, itemImage.transform.position, Quaternion.identity, transform);
            sprite.transform.localScale = new Vector3(.7f, .7f, .7f);
            itemAmount.text = "";
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
                    if (playerController.hasInvUpgradeII || playerController.hasSpdUpgradeI || playerController.hasSpdUpgradeII || playerController.hasInvUpgradeIII) {
                        playerController.hasInvUpgradeI = false;
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Inventory Upgrade 2":
                {
                    if (playerController.hasInvUpgradeI || playerController.hasSpdUpgradeI || playerController.hasSpdUpgradeII || playerController.hasInvUpgradeIII) {
                        playerController.hasInvUpgradeII = false;
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Inventory Upgrade 3":
                {
                    if (playerController.hasInvUpgradeI || playerController.hasInvUpgradeII || playerController.hasSpdUpgradeI || playerController.hasSpdUpgradeII) {
                        playerController.hasInvUpgradeIII = false;
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Speed Upgrade 1":
                {
                    if (playerController.hasInvUpgradeI || playerController.hasInvUpgradeII || playerController.hasInvUpgradeIII || playerController.hasSpdUpgradeII) {
                        playerController.hasSpdUpgradeI = false;
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Speed Upgrade 2":
                {
                    if (playerController.hasInvUpgradeI || playerController.hasInvUpgradeII || playerController.hasSpdUpgradeI || playerController.hasInvUpgradeIII) {
                        playerController.hasSpdUpgradeII = false;
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Fishing Rod Upgrade 1":
                {
                    if (playerController.hasRodUpgradeII) {
                        playerController.hasRodUpgradeI = false;
                        isUpgradeBought = false;
                    }
                    break;
                }
                case "Fishing Rod Upgrade 2":
                {
                    if (playerController.hasRodUpgradeI) {
                        playerController.hasRodUpgradeII = false;
                        isUpgradeBought = false;
                    }
                    break;
                }
            }
        }

        // Changing the bait sprite when bait upgrade bought
        if (itemData != null && baitUpgradeSprite != null) {
            if (itemData.Type == "Bait") {
                if (playerController.hasBaitUpgrade == true)
                    sprite.sprite = baitUpgradeSprite.sprite;
                else 
                    sprite.sprite = itemData.Sprite.sprite;
            }
        }

        if (itemData != null) {
            if (itemData.Type == "Bait") {
                itemAmount.text = "Amount: " + baitCounter.bait.ToString();
            }
        }
    }

    public void Buy() 
    {
        // Checking if it's an item or data so that we can properly buy it
        if (itemData != null) {
            if (itemData.Type == "Bait") {
                if (baitCounter.bait < baitCounter.maxBait && moneyCounter.money >= itemData.Price) {
                    baitCounter.bait++;
                    moneyCounter.money -= itemData.Price;
                }
            } else {
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
            }
        } else if (upgradeData != null) {
            if (moneyCounter.money >= upgradeData.Price && !isUpgradeBought) {
                switch(upgradeData.Type) 
                {
                    case "Boat":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasWon = true;

                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }

                        break;
                    }
                    case "Bait Upgrade":
                    {                        
                        if (!isUpgradeBought) {
                            playerController.hasBaitUpgrade = true;
                            baitCounter.maxBait = 15;
                            if (baitCounter.bait < baitCounter.maxBait) {
                                int amountLeftToMax = baitCounter.maxBait - baitCounter.bait;
                                //Debug.Log(amountLeftToMax);
                                baitCounter.bait += amountLeftToMax;
                            }

                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }

                        break;
                    }
                    case "Inventory Upgrade 1":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasInvUpgradeI = true;
                            
                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }

                        break;
                    }
                    case "Inventory Upgrade 2":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasInvUpgradeII = true;

                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }

                        break;
                    }
                    case "Inventory Upgrade 3":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasInvUpgradeIII = true;

                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }

                        break;
                    }
                    case "Speed Upgrade 1":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasSpdUpgradeI = true;
                            
                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }

                        break;
                    }
                    case "Speed Upgrade 2":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasSpdUpgradeII = true;
                            
                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }

                        break;
                    }
                    case "Fishing Rod Upgrade 1":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasRodUpgradeI = true;

                            moneyCounter.money -= upgradeData.Price;
                            isUpgradeBought = true;
                        }
                        break;
                    }
                    case "Fishing Rod Upgrade 2":
                    {
                        if (!isUpgradeBought) {
                            playerController.hasRodUpgradeII = true;

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
                    moneyCounter.totalMoneyWon += itemData.Price / 2;
                }
            } else {
                for (int i = 0; i < inventory.slots.Count; i++) {
                    if (inventory.slots[i].itemData == null) continue;
                    
                    if (itemData.Type == inventory.slots[i].itemData.Type) {
                        if (inventory.slots[i].itemData.Type == "Fish") {
                            if (itemData.Species == inventory.slots[i].itemData.Species) {
                                moneyCounter.money += inventory.slots[i].itemData.Price;
                                moneyCounter.totalMoneyWon += inventory.slots[i].itemData.Price;
                                inventory.slots[i].itemData = null;
                            }
                        } else {
                            moneyCounter.money += inventory.slots[i].itemData.Price / 2;
                            moneyCounter.totalMoneyWon += inventory.slots[i].itemData.Price / 2;
                            inventory.slots[i].itemData = null;
                        }
                    }
                }
            }
        } else if (upgradeData != null) {
            if (isUpgradeBought) {
                switch(upgradeData.Type) 
                {
                    case "Bait Upgrade":
                    {
                        if (isUpgradeBought) {
                            playerController.hasBaitUpgrade = false;
                            baitCounter.maxBait = 5;
                            if (baitCounter.bait > baitCounter.maxBait) {
                                baitCounter.bait = baitCounter.maxBait;
                            }

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }

                        break;
                    }
                    case "Inventory Upgrade 1":
                    {
                        if (isUpgradeBought) {
                            playerController.hasInvUpgradeI = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }

                        break;
                    }
                    case "Inventory Upgrade 2":
                    {
                        if (isUpgradeBought) {
                            playerController.hasInvUpgradeII = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                    case "Inventory Upgrade 3":
                    {
                        if (isUpgradeBought) {
                            playerController.hasInvUpgradeIII = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                    case "Speed Upgrade 1":
                    {
                        if (isUpgradeBought) {
                            playerController.hasSpdUpgradeI = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                    case "Speed Upgrade 2":
                    {
                        if (isUpgradeBought) {
                            playerController.hasSpdUpgradeII = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                    case "Fishing Rod Upgrade 1":
                    {
                        if (isUpgradeBought) {
                            playerController.hasRodUpgradeI = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                    case "Fishing Rod Upgrade 2":
                    {
                        if (isUpgradeBought) {
                            playerController.hasRodUpgradeII = false;

                            moneyCounter.money += upgradeData.Price / 2;
                            moneyCounter.totalMoneyWon += upgradeData.Price / 2;
                            isUpgradeBought = false;
                        }
                        break;
                    }
                }
            }
        }
    }
}
