using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishActivate : MonoBehaviour
{
    public FishingMinigame fishingMinigame;
    public Collider2D playerCollider;
    public Collider2D myCollider;
    
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (fishingMinigame.pause) {
            if (myCollider.IsTouching(playerCollider)) Destroy(this.gameObject);
        }
    }
}
