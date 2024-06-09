using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BaitCounter baitCounter;
    [SerializeField] private FishActivate fishActivate;

    public NewFishingMinigame fishing;
    
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed = 7f;

    // this is for knockback for the player
    public Vector2 forceToApply;
    public float forceDamping = 1.2f;

    // variable for checking if the player is doing anything because FUck me
    // TODO: MAKE THIS BETTER FOR THE LOVE OF GOD
    // I've been programming for 4 years now and I still don't know how to do good code
    // Not surprising to be honest
    public bool isFishing = false;
    public bool hasWon = false;
    public bool isInvOpen = false;
    public bool isOverlap = false;
    public bool isShopOpen = false;
    public bool canShop = false;

    // Upgrade varialbes
    public bool hasInvUpgradeI = false;
    public bool hasInvUpgradeII = false;
    public bool hasInvUpgradeIII = false;
    public bool hasSpdUpgradeI = false;
    public bool hasSpdUpgradeII = false;
    public bool hasBaitUpgrade = false;
    public bool hasRodUpgradeI = false;
    public bool hasRodUpgradeII = false;

    public Collider2D fishCollider;
    public Collider2D playerCollider;

    public GameObject fishingMinigame;
    public GameObject icons;
    public GameObject shop;

    public GameObject inventory;

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
        icons.transform.position = this.transform.position;

        Movement();
        isOverlapping();

        if (fishing.pause) {
            isFishing = false;
            isOverlap = false;
            canShop = false;
        }

        // Opening/Closing inventory
        if (Input.GetKeyDown(KeyCode.E) && !shop.activeSelf) {
            isInvOpen = !isInvOpen;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !inventory.activeSelf && canShop) {
            isShopOpen = !isShopOpen;
        }

        if (hasWon) {
            isFishing = false;
            isOverlap = false;
            isInvOpen = false;
            isShopOpen = false;
            canShop = false;
        }

        fishingMinigame.SetActive(isFishing);
        if (canShop) {
            icons.SetActive(canShop);
        } else {
            icons.SetActive(isOverlap);
        }
        inventory.SetActive(isInvOpen);
        shop.SetActive(isShopOpen);

        UpgradeSystem();
    }

    private void Movement() 
    {
        // input variables
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Checking if the player is fishing
        if (isFishing || hasWon || isInvOpen || isShopOpen) {
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
    }
    
    private void isOverlapping() 
    {
        // checking if the fish's collider is colliding with the player's collider
        // I don't fucking know why it's like this.
        // To be honest, I don't know what the fuck I'm doing at all. I hate myself.
        // And also, need to make the fish gameobject a prefab because it will break if it is not a prefab. (This was a foreshadow and I didn't know)
        if (fishCollider == null) return;

        if (fishCollider.IsTouching(playerCollider)) {
            isOverlap = true;

            if (fishActivate.newItem.Species == "Linguado" || fishActivate.newItem.Species == "Robalo") {
                if (!hasRodUpgradeI && !hasRodUpgradeII) 
                    return;
            }

            if (!isFishing && Input.GetKeyDown(KeyCode.Space) && baitCounter.bait > 0) {
                isFishing = !isFishing;
            }
        } else {
            isOverlap = false;
            // Making sure our variable that gets the fish collider is null when not overlapping a fish.
            fishCollider = null;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
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
            fishActivate = other.gameObject.GetComponent<FishActivate>();
            fishCollider = other.gameObject.GetComponent<BoxCollider2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Shop") {
            canShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Shop") {
            canShop = false;
        }
    }

    private void UpgradeSystem() {
        // Upgrade checks
        if (hasInvUpgradeI) {
            InventoryManager.instance.MaxSlots = 9;
            moveSpeed = 5f;
        } else if (hasInvUpgradeII) {
            InventoryManager.instance.MaxSlots = 12;
            moveSpeed = 3.7f;
        } else if (hasInvUpgradeIII) {
            InventoryManager.instance.MaxSlots = 18;
            moveSpeed = 2.4f;
        } else if (hasSpdUpgradeI) {
            InventoryManager.instance.MaxSlots = 4;
            moveSpeed = 9f;
        } else if (hasSpdUpgradeII) { 
            InventoryManager.instance.MaxSlots = 3;
            moveSpeed = 12f;
        } else {
            InventoryManager.instance.MaxSlots = 6;
            moveSpeed = 7f;
        }

        // Fishing Rod Upgrade Check
        if (hasRodUpgradeI) {
            fishing.hookMult = 90f;
            fishing.timerMult = 2f;
            fishing.progressMult = 2f;
        } else if (hasRodUpgradeII) {
            fishing.hookMult = 105f;
            fishing.timerMult = 3.5f;
            fishing.progressMult = 4f;
        } else {
            fishing.hookMult = 10.5f;
            fishing.timerMult = 1.25f;
            fishing.progressMult = 1f;
        }
    }
}