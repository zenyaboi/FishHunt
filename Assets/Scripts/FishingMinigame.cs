using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    #region Fish variables
    public GameObject fishing;

    [SerializeField] Transform topPivot, bottomPivot, fish;

    float fishPosition;
    float fishDestination;
    float fishTimer;
    [SerializeField] float timerMultiplicator = 3f; // higher the value faster the fish will go to the destination but will spend more time "stuck" in one place

    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;
    #endregion

    #region Hook variables
    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float hookPower = 5f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] float hookProgressDegradation = 1f;
    #endregion
    
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
