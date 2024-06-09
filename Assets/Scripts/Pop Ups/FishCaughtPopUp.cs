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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.lastFishCaught == null) return;

        fishCaught.text = playerController.lastFishCaught.Name;
        icon.sprite = playerController.lastFishCaught.Sprite.sprite;
    }
}
