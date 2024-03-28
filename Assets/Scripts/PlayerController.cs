using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 7f;

    // this is for knockback for the player
    public Vector2 forceToApply;
    public float forceDamping = 1.2f;

    // variable for checking if the player is fishing
    public bool isFishing = false;

    void Start()
    {
        // getting the rigidbody2D component from gameobject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // input variables
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Checking if the player is not fishing
        if (!isFishing) {
            // getting the player's input value
            Vector2 playerInput = new Vector2(horizontal, vertical).normalized;

            // applying force to the player movement
            Vector2 moveForce = playerInput * moveSpeed;

            // in case the player gets hit, the players gets knocked back affecting its position
            moveForce += forceToApply;
            forceToApply /= forceDamping;
            if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
            {
                forceToApply = Vector2.zero;
            }

            // applying the movement force to rigidbody's velocity
            rb.velocity = moveForce;
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        // Checking if the player is colliding with a gameobject with the tag Fish
        if (collision.gameObject.tag == "Fish") {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("Fish");
            }
        }
    }
}
