using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishActivate : MonoBehaviour
{
    public FishingMinigame fishingMinigame;

    void Update()
    {
        if (fishingMinigame.pause) {
            Destroy(gameObject);
        }
    }
}
