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
        itemAmount.text = "Amount: " + _itemAmount.ToString();
    }

    public void Buy() 
    {
        for (int i = 0; i < inventory.slots.Count; i++) {
            if (inventory._isFull[i] == false && moneyCounter.money >= itemData.Price) {
                moneyCounter.money -= itemData.Price;
                EventBus.Instance.PickUpItem(itemData);
            }
        }
    }

    public void Sell()
    {

    }
}
