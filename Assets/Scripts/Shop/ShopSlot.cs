using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
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
    }

    void Update()
    {
        
    }
}
