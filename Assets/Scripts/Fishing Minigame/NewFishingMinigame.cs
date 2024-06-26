using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewFishingMinigame : MonoBehaviour
{
    [SerializeField] private Transform fish, spawnPoint;
    [SerializeField] private GameObject fishing;
    [SerializeField] BaitCounter baitCounter;

    public int fishCount;

    #region Hook variables
    public Transform hook;
    public float hookPower, hookMult, hookMax;
    public bool canMoveHook;
    #endregion

    #region progress bar variables
    public Slider hookSlider, progressBarSlider;

    public float timer, maxTimer, timerMult;
    public float progress, progressMult, progressMax, progressMin;
    public float spamCooldownTimer;

    public bool isInGreen, isInRed;
    #endregion

    public bool pause, won;

    void Start()
    {
        timerMult = 0.5f;
        hookMult = 10.5f;
        progressMult = 1f;
        Setup();
    }

    void Update()
    {
        if (!PauseMenu.isPaused) {
            if (won || pause) {
                fishing.SetActive(false);
                return;
            }

            if (!fishing.activeSelf) {
                Setup();
            }

            // making the hook follow the fish
            hook.position = fish.position;
            
            Timer();
            Hook();
            ProgressBar();
        }
    }

    void Setup()
    {
        // Setting booleans
        canMoveHook = true;
        isInGreen = false;
        isInRed = false;
        pause = false;
        won = false;
        // TODO: LATER CHANGE THE VALUES FOR VARIABLES FOR UPGRADES
        // Setting timer values
        maxTimer = 5f;
        timer = maxTimer;
        // Setting hook slider values
        hookPower = 0f;
        hookMax = 250f;
        hookSlider.maxValue = timer;
        hookSlider.value = timer;
        // Setting progress slider values
        progress = 5f;
        progressMax = 10f;
        progressBarSlider.maxValue = progressMax;
        progressBarSlider.value = timer;
        spamCooldownTimer = 0f;
        // Setting hook and fish position to spawn point
        fish.position = spawnPoint.position;
        hook.position = spawnPoint.position;
    }

    void Timer()
    {
        // if the player is holding the space bar, the timer will start working
        if (Input.GetKey(KeyCode.Space) && canMoveHook && hookPower != 0) {
            // A shitty way to prevent spamming the space bar
            // If the player spams too much, the bar decreases faster
            spamCooldownTimer += Time.deltaTime;
            if (spamCooldownTimer > 0f && spamCooldownTimer < 0.1f) {
                Debug.Log("fast");
                timer -= (Time.deltaTime * (timerMult * 1.45f));
            } else if (spamCooldownTimer > 0.1f) {
                Debug.Log("sl0w");
                timer -= (Time.deltaTime * (timerMult / 2f));
            }
        } else {
            spamCooldownTimer = 0f;
            if (timer >= 0 && timer <= maxTimer)
                timer += (Time.deltaTime * timerMult);
        }

        // if the timer is less or equal to zero, set it to zero always
        if (timer < 0) {
            timer = 0;
        }

        // checking if the timer is equal to 0
        if (timer == 0) {
            //StartCoroutine(cooldown());
            canMoveHook = false;
            timer += (Time.deltaTime * timerMult);
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
        // Checking if we can move the hook
        if (canMoveHook) {
            // Checking input
            if (Input.GetKey(KeyCode.Space)) {
                // if hook power is less or equal to hook max
                if (hookPower <= hookMax) {
                    // increase the hook power per second multiplied by hook mult
                    hookPower += (hookMult * Time.deltaTime);
                }
                // changing the fish's y rigidbody velocity to hook power
                fish.GetComponent<Rigidbody2D>().velocity = new Vector2(0, hookPower);
            } else {
                // if hook power is more than 0
                if (hookPower > 0)
                    // subtract 1 from hook power
                    hookPower--;
                // else if hook power is less or equal to 0
                else if (hookPower <= 0) 
                    // set hook power to 0
                    hookPower = 0;
            }
        }
    }

    void ProgressBar() 
    {
        // Checking if the hook is in the green area and the progress value is less than progress max value
        if (isInGreen && progress < progressMax) {
            // add 1 per second
            progress += (Time.deltaTime * progressMult);
            // checking if progress value is more than progress max value
            if (progress > progressMax) {
                // if true, set progress value to max value
                progress = progressMax;
                pause = true;
                won = true;
                fishCount++;
                baitCounter.bait--;
                StartCoroutine(hasWon());
            }
        // Checking if the hook is in the red area and the progress value is more than progress min value
        } else if (isInRed && progress > progressMin) {
            // subtract 1 per second
            progress -= (Time.deltaTime * progressMult);
            // checking if progress value is less than progress min value
            if (progress < progressMin) {
                // if true, set progress value to min value
                progress = progressMin;
                pause = true;
                won = false;
                baitCounter.bait--;
                StartCoroutine(hasLost());
            }
        } else {
            // checking if progress is not and less than 5
            if (progress < 5f && progress != 5f) {
                progress += (Time.deltaTime * (progressMult / 2));
                // checking if progress is not and more than 5
            } else if (progress > 5f && progress != 5f) {
                progress -= (Time.deltaTime * (progressMult / 2));
            }
        }

        // Progress bar
        progressBarSlider.value = progress;
    }

    IEnumerator hasWon()
    {
        // Created this IEnumerator just in case the game breaks by not letting the player fish again
        // So we are waiting half of a second to make the pause false to restart the minigame
        yield return new WaitForSeconds(0.1f);
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
