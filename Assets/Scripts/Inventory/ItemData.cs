using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string Name => _name;
    public string Type => _type;
    /*
    public int Age => _age;
    public int Price => _price;
    */
    public int Age 
    {
        get { return _age; }
        set { _age = value; }
    }
    public float Price
    {
        get { return _price; }
        set { _price = value; }
    }
    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public Image Sprite => _sprite;
    public string Description => _description;

    [SerializeField] private string _name;
    [SerializeField] private string _type;
    [SerializeField] private int _age;
    [SerializeField] private float _weight;
    [SerializeField] private float _price;
    [SerializeField] private Image _sprite;
    [SerializeField] private string _description;

    public virtual string GetItemDescription()
    {
        return Description;
    } 
}
