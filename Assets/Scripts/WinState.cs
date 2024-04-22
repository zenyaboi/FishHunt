using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : MonoBehaviour
{
    public FishingMinigame fishingMinigame;
    public PlayerController playerController;
    public GameObject activateUI;

    void Start()
    {
        activateUI.SetActive(false);
        playerController.hasWon = false;
    }

    void Update()
    {
        // Find a way to find the amount of fish in the scene without making it manual because fuck me
        if (fishingMinigame.fishCount >= 15) {
            activateUI.SetActive(true);
            playerController.hasWon = true;

            StartCoroutine(waitInput());
        }
    }

    IEnumerator waitInput() 
    {
        yield return new WaitForSeconds(1.5f);
        if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(0);
    }
}
