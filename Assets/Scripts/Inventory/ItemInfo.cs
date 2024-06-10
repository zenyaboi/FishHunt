using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemAge;

    public void SetUp(string name, string description, string age) 
    {
        itemName.text = name;
        itemDescription.text = "Peso: " + description;
        itemAge.text = "Idade: " + age;
    }
}