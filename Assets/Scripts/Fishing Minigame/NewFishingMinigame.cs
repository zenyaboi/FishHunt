using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewFishingMinigame : MonoBehaviour
{
    #region Hook variables
    public Transform hook;
    public float hookPower = 5f;
    public float hookPull = 1.5f;
    public float hookMax = 250f;
    #endregion
    public Transform fish;

    #region progress bar variables
    public Slider hookSlider, progressBarSlider;

    public float timer;
    public float maxTimer = 5f;
    #endregion

    public bool canMoveHook = true;
    public bool isInGreen = false;
    public bool isInRed = false;

    public float progress = 5f;
    public float progressMax = 10f;
    public float progressMin = 0f;

    void Start()
    {
        // Setting booleans
        canMoveHook = true;
        isInGreen = false;
        isInRed = false;
        // TODO: LATER CHANGE THE VALUES FOR VARIABLES FOR UPGRADES
        // Setting timer
        timer = maxTimer;
        // Setting hook slider values
        hookSlider.maxValue = timer;
        hookSlider.value = timer;
        // Setting progress slider values
        progress = 5f;
        progressMax = 10f;
        progressBarSlider.maxValue = progressMax;
        progressBarSlider.value = timer;

    }

    /*
    void OnEnable()
    {
        canMoveHook = true;
        isInGreen = false;
        isInRed = false;
        timer = maxTimer;
        hookSlider.maxValue = timer;
        hookSlider.value = timer;
    }
    */

    void Update()
    {
        // making the hook follow the fish
        hook.position = fish.position;

        Timer();
        Hook();
        ProgressBar();
    }

    void Timer()
    {
        // if the player is holding the space bar, the timer will start working
        if (Input.GetKey(KeyCode.Space) && canMoveHook && hookPower != 0) {
            timer -= Time.deltaTime;
        } else {
            if (timer >= 0 && timer <= maxTimer)
                timer += Time.deltaTime;
        }

        // if the timer is less or equal to zero, set it to zero always
        if (timer < 0) {
            timer = 0;
        }

        // checking if the timer is equal to 0
        if (timer == 0) {
            //StartCoroutine(cooldown());
            canMoveHook = false;
            timer += Time.deltaTime;
        }

        // Checking if the timer is more or equal to max timer
        if (timer >= maxTimer) {
            canMoveHook = true;
        }

        // Hook Bar
        hookSlider.value = timer;
    }

    void Hook()
    {
        if (canMoveHook) {
            if (Input.GetKey(KeyCode.Space)) {
                if (hookPower <= hookMax) {
                    hookPower += hookPull * Time.deltaTime;
                }
                fish.GetComponent<Rigidbody2D>().velocity = new Vector2(0, hookPower);
            } else {
                if (hookPower > 0)
                    hookPower--;
                else if (hookPower <= 0) 
                    hookPower = 0;
            }
        }
    }

    void ProgressBar() 
    {
        if (isInGreen && progress < progressMax) {
            progress += Time.deltaTime;
            if (progress > progressMax) {
                progress = progressMax;
            }
        } else if (isInRed && progress > progressMin) {
            progress -= Time.deltaTime;
            if (progress < progressMin) {
                progress = progressMin;
            }
        } else {
            if (progress < 5f && progress != 5f) {
                progress += Time.deltaTime;

            } else if (progress > 5f && progress != 5f) {
                progress -= Time.deltaTime;
            }
        }

        // Progress bar
        progressBarSlider.value = progress;
    }

    /*
    IEnumerator cooldown() {
        yield return new WaitForSeconds(2f);
        timer = 0f;
    }
    */
}
