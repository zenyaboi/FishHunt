using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaitCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public int maxBait = 5;
    public int bait;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        bait = maxBait;
    }

    void Update()
    {
        text.text = bait.ToString();
    }
}
