using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : MonoBehaviour
{
    public NewFishingMinigame fishingMinigame;
    public PlayerController playerController;
    public MoneyCounter moneyCounter;

    public TMP_Text finalTime;
    public TMP_Text finalFishCount;
    public TMP_Text finalMoney;
    
    public GameObject activateUI;

    public float timer;

    void Start()
    {
        activateUI.SetActive(false);
        playerController.hasWon = false;
    }

    void Update()
    {
        // Find a way to find the amount of fish in the scene without making it manual because fuck me
        /*
        if (fishingMinigame.fishCount >= 15) {
            activateUI.SetActive(true);
            playerController.hasWon = true;

            StartCoroutine(waitInput());
        }
        */

        if (Input.GetKeyDown(KeyCode.H)) {
            activateUI.SetActive(true);
            playerController.hasWon = true;
        }

        if (activateUI.activeSelf) {
            DisplayTime();
            finalFishCount.text = "Peixes pescados: " + fishingMinigame.fishCount;
            finalMoney.text = "Quantidade de dinheiro final: " + moneyCounter.money;

        } else {
            timer += Time.deltaTime;
        }
    }

    private void DisplayTime() {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        finalTime.text = string.Format("Tempo total: {0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator waitInput() 
    {
        yield return new WaitForSeconds(1f);
        if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(0);
    }
}
