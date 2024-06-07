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

    void Start()
    {
        canMoveHook = true;
        timer = maxTimer;
        hookSlider.maxValue = timer;
        hookSlider.value = timer;
    }

    void Update()
    {
        // making the hook follow the fish
        hook.position = fish.position;

        Timer();
        Hook();
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

    /*
    IEnumerator cooldown() {
        yield return new WaitForSeconds(2f);
        timer = 0f;
    }
    */
}
