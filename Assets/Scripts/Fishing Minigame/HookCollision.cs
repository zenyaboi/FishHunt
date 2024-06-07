using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollision : MonoBehaviour
{
    [SerializeField] private NewFishingMinigame fishingMinigame;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Pivot") {
            Debug.Log("fuck");
            fishingMinigame.hookPower = 0;
        }
    }
}
