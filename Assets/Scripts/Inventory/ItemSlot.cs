using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, ISelectHandler
{
    public ItemData itemData;

    private Image spawnedSprite;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected");
    }

    private void OnEnable()
    {
        if (itemData == null) return;

        spawnedSprite = Instantiate<Image>(itemData.Sprite, transform.position, Quaternion.identity, transform);
    }

    private void OnDisable()
    {
        if (spawnedSprite != null) {
            Destroy(spawnedSprite);
        }
    }

    public void OnCursorEnter() 
    {
        // display item info
        if (itemData == null) return;
        
        InventoryManager.instance.DisplayItemInfo(itemData.Name, itemData.GetItemDescription(), transform.position);
    }

    public void OnCursorExit() 
    {
        InventoryManager.instance.DestroyItemInfo();
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }
}
