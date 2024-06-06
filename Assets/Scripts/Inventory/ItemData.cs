using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string Name => _name;
    public string Type => _type;
    public string Species => _species;
    /*
    public int Age => _age;
    public int Price => _price;
    */
    public int Age 
    {
        get { return _age; }
        set { _age = value; }
    }
    public double Price
    {
        get { return _price; }
        set { _price = value; }
    }
    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    //public Image Sprite => _sprite;
    public Image Sprite
    {
        get { return _sprite; }
        set { _sprite = value; }
    }
    public string Description => _description;

    [SerializeField] private string _name;
    [SerializeField] private string _type;
    [SerializeField] private string _species;
    [SerializeField] private int _age;
    [SerializeField] private float _weight;
    [SerializeField] private double _price;
    [SerializeField] private Image _sprite;
    [SerializeField] private string _description;

    public virtual string GetItemDescription()
    {
        return Description;
    } 
}
