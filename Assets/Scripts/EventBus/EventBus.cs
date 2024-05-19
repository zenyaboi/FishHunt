using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    public event Action<ItemData> onPickUpItem;
    public event Action<ItemData> onItemSold;

    private void Awake() 
    {
        Instance = this;
    }

    public void PickUpItem(ItemData itemData)
    {
        onPickUpItem?.Invoke(itemData);
    }

    public void SellItem(ItemData item)
    {
        onItemSold?.Invoke(item);
    }
}
