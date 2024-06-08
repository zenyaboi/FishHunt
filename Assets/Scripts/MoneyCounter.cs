using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public double money = 0f;
    public double totalMoneyWon = 0f;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            money++;
        }

        text.text = money.ToString();
    }
}
