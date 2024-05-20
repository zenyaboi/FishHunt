using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private MoneyCounter moneyCounter;
    [SerializeField] private InventoryManager inventory;

    public GameObject itemImage;
    public TMP_Text itemName;
    public TMP_Text itemAmount;
    public TMP_Text itemPrice;

    public ItemData itemData;    
    public int _itemAmount;
    public TMP_Text buyPriceText;

    void Start()
    {
        itemName.text = itemData.Name;
        //itemImage.sprite = itemData.sprite;
        Instantiate<Image>(itemData.Sprite, itemImage.transform.position, Quaternion.identity, transform);
        buyPriceText.text = "Price: " + itemData.Price.ToString();
    }

    void Update()
    {
        // For now, this won't be used
        // But it's good to have it here in case we use it
        // If not, just delete it
        //itemAmount.text = "Amount: " + _itemAmount.ToString();
    }

    public void Buy() 
    {
        for (int i = 0; i < inventory.slots.Count; i++) {
            if (inventory._isFull[i] == false && moneyCounter.money >= itemData.Price) {
                moneyCounter.money -= itemData.Price;
                // We are using the same event that we use for when we succesfully finish the fishing minigame
                EventBus.Instance.PickUpItem(itemData);
            }
        }
    }

    public void Sell()
    {
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
