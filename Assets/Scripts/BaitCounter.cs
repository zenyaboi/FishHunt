using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaitCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public int bait = 5;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = "Bait: " + bait.ToString();
    }
}
