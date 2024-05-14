using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishActivate : MonoBehaviour
{
    public FishingMinigame fishingMinigame;
    public Collider2D playerCollider;

    [SerializeField] private Collider2D myCollider;
    [SerializeField] private ItemData _itemData;
    
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
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
                if (fishingMinigame.won) {
                    EventBus.Instance.PickUpItem(_itemData);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
