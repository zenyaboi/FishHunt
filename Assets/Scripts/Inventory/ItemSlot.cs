using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, ISelectHandler
{
    public ItemData itemData;
    public InventoryManager inventory;

    public int i;

    public Image spawnedSprite;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected");
    }

    private void OnEnable()
    {
        // We are spawning the sprite prefab of the item on the slot
        if (itemData == null) return;

        // TODO: When we create this sprite, it keeps creating forever. Need to find a way to prevent this
        if (spawnedSprite == null) 
            spawnedSprite = Instantiate<Image>(itemData.Sprite, transform.position, Quaternion.identity, transform);
    }

    private void OnDisable()
    {
        if (spawnedSprite != null) {
            Destroy(spawnedSprite);
        }
    }

    public void Update()
    {
        //Debug.Log(transform.childCount);

        if (transform.childCount > 1)
            inventory._isFull[i] = true;
    }

    public void OnCursorEnter() 
    {
        // display item info
        if (itemData == null) return;
        
        InventoryManager.instance.DisplayItemInfo(itemData.Name, itemData.GetItemDescription(), itemData.Age.ToString(), transform.position);
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
