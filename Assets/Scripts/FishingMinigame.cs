using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    public GameObject fishing;

    [SerializeField] Transform topPivot, bottomPivot, fish;

    float fishPosition;
    float fishDestination;
    float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;

    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    private void Update() 
    {
        if (fishing.activeSelf) {
            fishTimer -= Time.deltaTime;

            if (fishTimer <= 0f) {
                fishTimer = UnityEngine.Random.value * timerMultiplicator;
                fishDestination = UnityEngine.Random.value;
            }

            fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
            fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
        } else {
            // Change later the random value of the fish
            Debug.Log("to desativado bro");
            fishTimer = 0f;
            fishPosition = UnityEngine.Random.value;
            fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
        }
    }
}
