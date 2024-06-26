using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string Name => _name;
    public string Type => _type;
    public int Price => _price;
    public Image Sprite => _sprite;
    public string Description => _description;

    [SerializeField] private string _name;
    [SerializeField] private string _type;
    [SerializeField] private int _price;
    [SerializeField] private Image _sprite;
    [SerializeField] private string _description;

    public virtual string GetItemDescription()
    {
        return Description;
    }
}
