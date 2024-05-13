using UnityEngine;

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

    void Start()
    {
        
    }

    void Update()
    {
        
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
