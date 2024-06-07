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
    public float maxTimer = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        hook.position = fish.position;

        if (Input.GetKey(KeyCode.Space)) {
            timer += Time.deltaTime;
        } else {
            timer -= Time.deltaTime;
        }

        if (timer <= 0) {
            timer = 0;
        }

        if (timer >= maxTimer) {
            StartCoroutine(cooldown());
        }

        Hook();
    }

    void Hook()
    {
        if (timer < maxTimer) {
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

    IEnumerator cooldown() {
        yield return new WaitForSeconds(5f);
        timer = 0f;
    }
}
