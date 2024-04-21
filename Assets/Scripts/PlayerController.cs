using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FishingMinigame fishing;
    
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed = 7f;

    // this is for knockback for the player
    public Vector2 forceToApply;
    public float forceDamping = 1.2f;

    // variable for checking if the player is fishing
    public bool isFishing = false;
    public bool hasWon = false;
    public bool isOverlap = false;

    public Collider2D fishCollider;
    public Collider2D playerCollider;

    public GameObject fishingMinigame;
    public GameObject icons;

    void Start()
    {
        // getting the rigidbody2D component from gameobject
        rb = GetComponent<Rigidbody2D>();
        // getting the animator component from gameobject
        animator = GetComponent<Animator>();
        // getting the box collider 2d component from gameobject
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // input variables
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Checking if the player is fishing
        if (isFishing || hasWon) {
            horizontal = 0;
            vertical = 0;
            rb.velocity = Vector2.zero;
        }

        // Assining animator's float with movement variables
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Speed", moveSpeed);

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

        if (fishing.pause) {
            isFishing = false;
            isOverlap = false;
        }
        
        isOverlapping();

        fishingMinigame.SetActive(isFishing);
        icons.SetActive(isOverlap);
    }
    
    private void isOverlapping() 
    {
        // checking if the fish's collider is colliding with the player's colliding
        // I don't fucking know why it's like this.
        // To be honest, I don't what the fuck I'm doing at all. I hate myself.
        // And also, need to make the fish gameobject a prefab because it will break if it is not a prefab. (This was a foreshadow and I didn't know)
        if (fishCollider == null) return;

        if (fishCollider.IsTouching(playerCollider)) {
            //Debug.Log("estou tocando");
            isOverlap = true;
            if (!isFishing) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    isFishing = !isFishing;
                }
            }
        } else {
            isOverlap = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Fish") {
            // So... making the fish pools as a prefab also broke everything
            // In case I forget why this exists:
            // We are checking if the player is overlapping the gameobject on the function isOverlapping()
            // On the function, we use the IsTouching, which is from Collider2D, to check if the player is colliding with the fish pool
            // But, that will only work if the gameobject is referenced inside the editor
            // If there's MORE THAN ONE FISH POOL the player can't fish again since it can't find the Collider2D of the fish
            // Now, this here "fixes" the problem
            // We are checking if we are trigerring any gameobject with the tag "Fish"
            // If so, we are assining the collider of the gameobject we triggered to the fishCollider variable (which is used on isOverlapping).
            // Debug.Log("aqui");
            fishCollider = other.gameObject.GetComponent<BoxCollider2D>();
        }
    }
}