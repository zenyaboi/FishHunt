using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishCaughtPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text fishCaught;
    [SerializeField] private PlayerController playerController;
    [SerializeField] Image icon;

    void Update()
    {
        if (playerController.lastFishCaught == null) return;

        fishCaught.text = playerController.lastFishCaught.Name;
        icon.sprite = playerController.lastFishCaught.Sprite.sprite;
    }
}
