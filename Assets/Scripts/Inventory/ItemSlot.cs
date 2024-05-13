using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, ISelectHandler
{
    [SerializeField] private ItemData _itemData;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected");
    }

    private void Start()
    {
        if (_itemData == null) return;

        var spawnedSprite = Instantiate<Image>(_itemData.Sprite, transform.position, Quaternion.identity, transform);
    }
}
