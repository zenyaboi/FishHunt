using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region singleton
    public static InventoryManager instance;
    private void Awake() 
    {
        if (instance == null) {
            instance = this;
        }
    }
    #endregion

    public Transform inv;
    public GameObject itemInfoPrefab;
    private GameObject currentItemInfo = null;

    [SerializeField] private List<ItemSlot> _slots;
    [SerializeField] private List<Button> _btnIgnore;
    public List<ItemSlot> slots => _slots;

    public bool[] _isFull;

    private void Update()
    {
        if (inv.gameObject.activeSelf) {
            foreach (var button in _btnIgnore) {
                button.interactable = false;
            }
        } else {
            foreach (var button in _btnIgnore) {
                button.interactable = true;
            }
        }
    }

    private void OnEnable()
    {
        EventBus.Instance.onPickUpItem += OnItemPickedUp;
    }

    private void OnDisable()
    {
        EventBus.Instance.onPickUpItem -= OnItemPickedUp;
    }

    private void OnItemPickedUp(ItemData itemData)
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty()) {
                slot.itemData = itemData;
                break;
            }
        }
    }

    public void DisplayItemInfo(string itemName, string itemDescription, Vector2 buttonPos) 
    {
        if (currentItemInfo != null) Destroy(currentItemInfo.gameObject);

        buttonPos.x -= 150;
        buttonPos.y -= 100;

        currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, inv);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(itemName, itemDescription);
    }

    public void DestroyItemInfo()
    {
        if (currentItemInfo != null) Destroy(currentItemInfo.gameObject);
    }
}
