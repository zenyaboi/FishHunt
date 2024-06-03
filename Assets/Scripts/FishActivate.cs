using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishActivate : MonoBehaviour
{
    public FishingMinigame fishingMinigame;
    public Collider2D playerCollider;

    [SerializeField] private Collider2D myCollider;
    [SerializeField] private SpriteRenderer mySpriteRenderer;
    [SerializeField] private ItemData _itemData;

    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private List<ItemData> _fishDatas;
    
    void Start()
    {
        float randomScale = Random.Range(1.5f, 3f);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        myCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        RandomFish();

        ItemData newItem = Object.Instantiate(_itemData);
        Debug.Log(newItem);
    }

    void Update()
    {
        // So... another problem with the fish pool being a prefab
        // If we finish ONE single mini-game, it destroys EVERY INSTANCE of the fish pools
        // And we kinda don't want that
        // So I just made this awful fucking if statement to check if the instance is colliding with the player to destroy itself.
        // And it works, don't ask. it looks dumb.
        if (fishingMinigame.pause) {
            if (myCollider.IsTouching(playerCollider)) {
                // I hate this
                if (fishingMinigame.won) {
                    EventBus.Instance.PickUpItem(_itemData);
                }
                Destroy(this.gameObject);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Dock" || other.gameObject.tag == "Fish") {
            Debug.Log("holy shit spider-man");
            Destroy(this.gameObject);
        }
    }

    private void RandomFish() {
        #region random sprite
        int randomNum = Random.Range(0, _sprites.Count);
        //print(randomNum);
        switch(randomNum) {
            case 0:
            {
                mySpriteRenderer.sprite = _sprites[randomNum];
                break;
            }
            case 1:
            {
                mySpriteRenderer.sprite = _sprites[randomNum];
                break;
            }
            case 2:
            {
                mySpriteRenderer.sprite = _sprites[randomNum];
                break;
            }
        }
        #endregion

        #region random item data
        int randomFish = Random.Range(0, _fishDatas.Count);
        switch(randomFish) {
            case 0:
            {
                _itemData = _fishDatas[randomFish];
                break;
            }
            case 1:
            {
                _itemData = _fishDatas[randomFish];
                break;
            }
        }
        #endregion
    }
}
