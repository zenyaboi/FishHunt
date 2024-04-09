using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FishingMinigame fishing;
    
    public Rigidbody2D rb;
    public float moveSpeed = 7f;

    // this is for knockback for the player
    public Vector2 forceToApply;
    public float forceDamping = 1.2f;

    // variable for checking if the player is fishing
    public bool isFishing = false;

    public Collider2D fishCollider;
    public Collider2D playerCollider;

    public GameObject fishingMinigame;

    void Start()
    {
        // getting the rigidbody2D component from gameobject
        rb = GetComponent<Rigidbody2D>();
        // getting the box collider 2d component from gameobject
        playerCollider = GetComponent<BoxCollider2D>();
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
                forceToApply = Vector2.zero;

            // applying the movement force to rigidbody's velocity
            rb.velocity = moveForce;
        } else {
            rb.velocity = Vector2.zero;
        }

        if (fishing.pause) {
            isFishing = false;
        }
        
        isOverlapping();

        fishingMinigame.SetActive(isFishing);
    }
    
    private void isOverlapping() 
    {
        // checking if the fish's collider is colliding with the player's colliding
        // I don't fucking know why it's like this.
        // To be honest, I don't what the fuck I'm doing at all. I hate myself.
        // And also, need to make the fish gameobject a prefab because it will break if it is not a prefab.
        if (fishCollider == null) return;

        if (fishCollider.IsTouching(playerCollider)) {
            Debug.Log("estou tocando");
            if (!isFishing) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    //Debug.Log("Fuck");
                    isFishing = !isFishing;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Fish") {
            Debug.Log("aqui");
            fishCollider = other.gameObject.GetComponent<BoxCollider2D>();
        }
    }
}