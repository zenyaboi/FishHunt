using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishActivate : MonoBehaviour
{
    public FishingMinigame fishingMinigame;
    public Collider2D playerCollider;

    [SerializeField] private Collider2D myCollider;
    [SerializeField] private SpriteRenderer mySpriteRenderer;
    [SerializeField] private ItemData _itemData;

    [SerializeField] private List<Image> _sprites;
    [SerializeField] private List<ItemData> _fishDatas;

    public ItemData newItem;
    
    void Start()
    {
        float randomScale = Random.Range(1.5f, 3f);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        myCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        RandomFish();
    }

    void Update()
    {
        // So... another problem with the fish pool being a prefab
        // If we finish ONE single mini-game, it destroys EVERY INSTANCE of the fish pools
        // And we kinda don't want that
        // So I just made this awful fucking if statement to check if the instance is colliding with the player to destroy itself.
        // And it works, don't ask. it looks dumb.
        if (fishingMinigame.pause && myCollider.IsTouching(playerCollider)) {
            if (fishingMinigame.won) {
                EventBus.Instance.PickUpItem(newItem);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Dock" || other.gameObject.tag == "Fish") {
            Debug.Log("holy shit spider-man");
            Destroy(gameObject);
        }
    }

    private void RandomFish() {
        int randomFish = Random.Range(0, _fishDatas.Count);
        _itemData = _fishDatas[randomFish];
        Debug.Log(_fishDatas[randomFish]);

        // Creating a instance of itemData
        // We are doing this so we can randomize the age and price  
        newItem = Object.Instantiate(_itemData);
        // Randomizing values
        int randomNum = Random.Range(0, _sprites.Count);
        if (_itemData.Species == "Anchova") {
            mySpriteRenderer.sprite = _sprites[randomNum].sprite;
            newItem.Sprite = _sprites[randomNum];
        } else {
            mySpriteRenderer.sprite = newItem.Sprite.sprite;
        }
        newItem.Age = Random.Range(1, 10);
        newItem.Weight = Random.Range(1, 10);
        newItem.Price = (5f * newItem.Weight) / newItem.Age;
        // Rounding the price so it has one decimal
        newItem.Price = System.Math.Round(newItem.Price, 1);
        //Debug.Log("Fish Age: " + newItem.Age + ", Fish Weight: " + newItem.Weight + ", Fish Price: " + newItem.Price);
    }
}
