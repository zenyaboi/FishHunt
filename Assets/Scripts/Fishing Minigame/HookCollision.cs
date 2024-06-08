using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollision : MonoBehaviour
{
    [SerializeField] private NewFishingMinigame fishingMinigame;
    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Pivot") {
            fishingMinigame.hookPower = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "GreenArea") {
            fishingMinigame.isInGreen = true;
            fishingMinigame.isInRed = false;
            //Debug.Log("fuck");
        } else if (other.gameObject.tag == "RedArea") {
            fishingMinigame.isInGreen = false;
            fishingMinigame.isInRed = true;
            //Debug.Log("big fuckage");
        } else if (other.gameObject.tag == "WhiteArea") {
            fishingMinigame.isInGreen = false;
            fishingMinigame.isInRed = false;
        }
    }
}
