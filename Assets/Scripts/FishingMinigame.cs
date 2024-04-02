using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] float hookPullPower = 0.005f;
    [SerializeField] float hookGravityPower = 0.0025f;
    [SerializeField] float hookProgressDegradation = 1f;

    [SerializeField] Image hookSpriteRenderer;
    #endregion

    private void Update() 
    {
        if (!fishing.activeSelf) {
            // Change later the random value of the fish
            // Debug.Log("to desativado bro");
            fishTimer = 0f;
            fishPosition = UnityEngine.Random.value;
            fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
        }

        Fish();
        Hook();
    }

    void Hook()
    {
        if (Input.GetKey(KeyCode.Space)) 
            hookPullVelocity += hookPullPower * Time.deltaTime;

        hookPullVelocity -= hookGravityPower * Time.deltaTime;

        hookPosition += hookPullVelocity;

        if (hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f) 
            hookPullVelocity = 0f;

        if (hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
            hookPullVelocity = 0f;

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }

    void Fish() 
    {
        fishTimer -= Time.deltaTime;

        if (fishTimer <= 0f) {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;
            fishDestination = UnityEngine.Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }
}