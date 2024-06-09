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

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject itemInfoPrefab;
    [SerializeField] private GameObject currentItemInfo = null;

    [SerializeField] private List<ItemSlot> _slots;
    [SerializeField] private List<Button> _btnIgnore;
    public List<ItemSlot> slots => _slots;

    [SerializeField] private int maxSlots;
    public int MaxSlots {
        get { return maxSlots; }
        set { SetMaxSlots(value); }
    }

    // This variable is here to check if the slots has an item or not
    public List<bool> _isFull;

    private void Start() {
        SetMaxSlots(maxSlots);
    }

    private void Update()
    {
        // We are checking if the inventory is active to make the selector not work if other kind of button
        // It will only work with the slots on the inventory
        if (inv.gameObject.activeSelf) {
            foreach (var button in _btnIgnore) {
                button.interactable = false;
            }
        } else {
            foreach (var button in _btnIgnore) {
                button.interactable = true;
            }
        }

        for (int i = 0; i < _slots.Count; i++) {
            if (_slots[i].IsEmpty()) {
                _isFull[i] = false;
            } else {
                _isFull[i] = true;
            }
        }
    }

    private void SetMaxSlots(int value)
    {
        if (value <= 0) {
            maxSlots = 1;
        } else {
            maxSlots = value;
        }

        if (maxSlots < slots.Count) {
            for (int i = maxSlots; i < slots.Count; i++) {
                Destroy(slots[i].transform.gameObject);
            }
            int diff = slots.Count - maxSlots;
            int diffFull = _isFull.Count - maxSlots;
            slots.RemoveRange(maxSlots, diff);
            _isFull.RemoveRange(maxSlots, diffFull);
        } else if (maxSlots > slots.Count) {
            int diff = maxSlots - slots.Count;

            for (int i = 0; i < diff; i++) {
                GameObject itemSlotGameObj = Instantiate(itemSlotPrefab);
                itemSlotGameObj.transform.SetParent(parent, false);
                slots.Add(itemSlotGameObj.GetComponent<ItemSlot>());
                _isFull.Add(!slots[i].IsEmpty());
                itemSlotGameObj.GetComponent<ItemSlot>().i = i;
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

    //TODO: Fix pop-up position
    public void DisplayItemInfo(string itemName, string itemDescription, string itemAge, Vector2 buttonPos) 
    {
        if (currentItemInfo != null) Destroy(currentItemInfo.gameObject);

        buttonPos.x -= 150;
        buttonPos.y -= 100;

        currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, inv);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(itemName, itemDescription, itemAge);
    }

    public void DestroyItemInfo()
    {
        if (currentItemInfo != null) Destroy(currentItemInfo.gameObject);
    }
}
