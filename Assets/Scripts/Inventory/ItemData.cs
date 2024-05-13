using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string Name => _name;
    public int Age => _age;
    public Image Sprite => _sprite;
    public string Description => _description;

    [SerializeField] private string _name = "";
    [SerializeField] private int _age = 0;
    [SerializeField] private Image _sprite;
    [SerializeField] private string _description = "";

    public virtual string GetItemDescription()
    {
        return Description;
    } 
}
