using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewFishingMinigame : MonoBehaviour
{
    #region Hook variables
    public Transform hook;
    public float hookPower = 5f;
    public float hookPull = 1.5f;
    public float hookMax = 250f;
    #endregion
    public Transform fish;

    public float timer;
    public float maxTimer = 6f;

    public bool canMoveHook = true;

    void Start()
    {
        canMoveHook = true;
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
        // if the timer is less or equal to zero, set it to zero always
        if (timer < 0) {
            timer = 0;
        }

        // checking if the timer is more or equal to the max timer
        if (timer >= maxTimer) {
            //StartCoroutine(cooldown());
            canMoveHook = false;
            timer -= Time.deltaTime;
        } else if (timer == 0) {
            canMoveHook = true;
        }

        // if the player is holding the space bar, the timer will start working
        if (Input.GetKey(KeyCode.Space) && canMoveHook && hookPower != 0) {
            timer += Time.deltaTime;
        } else {
            if (timer > 0)
                timer -= Time.deltaTime;
        }
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
