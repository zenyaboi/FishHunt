using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BaitCounter baitCounter;
    [SerializeField] private FishActivate fishActivate;
    [SerializeField] private GameObject fishCaughtPopUp;
    [SerializeField] private GameObject noBaitPopUp;

    public NewFishingMinigame fishing;
    
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed = 7f;
    public Vector2 playerInput;
    public Vector2 lastMove;

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

    public ItemData lastFishCaught;

    public RuntimeAnimatorController[] animatorController;

    void Start()
    {
        // getting the rigidbody2D component from gameobject
        rb = GetComponent<Rigidbody2D>();
        // getting the animator component from gameobject
        animator = GetComponent<Animator>();
        // getting the box collider 2d component from gameobject
        playerCollider = GetComponent<BoxCollider2D>();
        animator.runtimeAnimatorController = animatorController[0];
        this.transform.localScale = new Vector3(5f, 5f, 5f);
    }

    void Update()
    {
        if (!PauseMenu.isPaused) {
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

            if (fishing.won) {
                StartCoroutine(popUpCooldown(fishCaughtPopUp));
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
    }

    private void Movement() 
    {
        // input variables
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if ((horizontal == 0 && vertical == 0) && playerInput.x != 0 || playerInput.y != 0) {
            lastMove = playerInput;
        }

        // Checking if the player is fishing
        if (isFishing || hasWon || isInvOpen || isShopOpen) {
            horizontal = 0;
            vertical = 0;
            rb.velocity = Vector2.zero;
        }

        // getting the player's input value
        playerInput = new Vector2(horizontal, vertical).normalized;

        // Assining animator's float with movement variables
        animator.SetFloat("Horizontal", playerInput.x);
        animator.SetFloat("Vertical", playerInput.y);
        animator.SetFloat("Speed", playerInput.sqrMagnitude);
        animator.SetFloat("AnimLastMoveX", lastMove.x);
        animator.SetFloat("AnimLastMoveY", lastMove.y);

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
            } else if (!isFishing && Input.GetKeyDown(KeyCode.Space) && baitCounter.bait <= 0) {
                StartCoroutine(popUpCooldown(noBaitPopUp));
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
            animator.runtimeAnimatorController = animatorController[2];
            this.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            moveSpeed = 5f;
        } else if (hasInvUpgradeII) {
            InventoryManager.instance.MaxSlots = 12;
            animator.runtimeAnimatorController = animatorController[2];
            this.transform.localScale = new Vector3(6f, 6f, 6f);
            moveSpeed = 3.7f;
        } else if (hasInvUpgradeIII) {
            InventoryManager.instance.MaxSlots = 18;
            animator.runtimeAnimatorController = animatorController[2];
            this.transform.localScale = new Vector3(7f, 7f, 7f);
            moveSpeed = 2.4f;
        } else if (hasSpdUpgradeI) {
            InventoryManager.instance.MaxSlots = 4;
            animator.runtimeAnimatorController = animatorController[1];
            this.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
            moveSpeed = 9f;
        } else if (hasSpdUpgradeII) { 
            InventoryManager.instance.MaxSlots = 3;
            animator.runtimeAnimatorController = animatorController[1];
            this.transform.localScale = new Vector3(4f, 4f, 4f);
            moveSpeed = 12f;
        } else {
            InventoryManager.instance.MaxSlots = 6;
            animator.runtimeAnimatorController = animatorController[0];
            this.transform.localScale = new Vector3(5f, 5f, 5f);
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

    IEnumerator popUpCooldown(GameObject go) {
        go.SetActive(true);
        yield return new WaitForSeconds(3f);
        go.SetActive(false);
    }
}