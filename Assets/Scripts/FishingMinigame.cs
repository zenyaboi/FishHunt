using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    public GameObject[] fishPool;

    public int fishCount;

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
    #endregion

    #region Progress Bar variables
    [SerializeField] Transform progressBarContainer;

    public bool pause = false;
    public bool won = false;

    [SerializeField] float failTimer = 999f;
    #endregion

    public List<ItemData> fishes;

    void Start()
    {
        // Another problem by making the fish pools a prefab.
        // I kinda forgot why I use this, but it's important
        // I need to add these comments when I do the code, not a day later
        fishPool = GameObject.FindGameObjectsWithTag("Fish");
    }

    private void FixedUpdate() 
    {
        if (pause) return;

        if (!fishing.activeSelf) {
            // Change later the random value of the fish
            // Debug.Log("to desativado bro");
            fishTimer = 0f;
            failTimer = 999f;
            hookProgress = 0f;
            hookPosition = 0f;
            pause = false;
            won = false;
            fishPosition = UnityEngine.Random.value;
            fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
        }

        Fish();
        Hook();
        ProgressCheck();
    }

    void Hook()
    {
        if (Input.GetKey(KeyCode.Space)) 
            hookPullVelocity += hookPullPower * Time.fixedDeltaTime;

        hookPullVelocity -= hookGravityPower * Time.fixedDeltaTime;

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
        fishTimer -= Time.fixedDeltaTime;

        if (fishTimer <= 0f) {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;
            fishDestination = UnityEngine.Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    private void ProgressCheck()
    {
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && fishPosition < max) {
            hookProgress += hookPower * Time.fixedDeltaTime;
        } else {
            hookProgress -= hookProgressDegradation * Time.fixedDeltaTime;

            failTimer -= Time.fixedDeltaTime;

            if (failTimer <= 0f) Lose();
        }

        if (hookProgress >= 1f) Win();
        
        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }

    private void Win()
    {
        Debug.Log("YOU WIN! HOLY FUCKING CUCK FUCK");
        pause = true;
        won = true;
        failTimer = 999f;
        fishing.SetActive(false);
        // Randomizing the fish
        int randomNum = UnityEngine.Random.Range(0, fishes.Count);
        print(randomNum);
        ItemData randFish = fishes[randomNum];
        print(randFish);
        EventBus.Instance.PickUpItem(randFish);
        StartCoroutine(hasWon());
    }

    private void Lose()
    {
        Debug.Log("GET FUCKED NERD");
        pause = true;
        won = false;
        failTimer = 999f;
        fishing.SetActive(false);
        StartCoroutine(hasLost());
    }

    IEnumerator hasWon()
    {
        // Created this IEnumerator just in case the game breaks by not letting the player fish again
        // So we are waiting half of a second to make the pause false to restart the minigame
        yield return new WaitForSeconds(0.1f);
        fishCount += 1;
        pause = false;
        won = false;
    }
    IEnumerator hasLost()
    {
        // Created this IEnumerator just in case the game breaks by not letting the player fish again
        // So we are waiting half of a second to make the pause false to restart the minigame
        yield return new WaitForSeconds(0.1f);
        pause = false;
        won = false;
    }
}