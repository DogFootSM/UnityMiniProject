using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Flags]
public enum ItemType
{
    Crop,
    Tool,
     
    //포션 등등 추가..

}



[CreateAssetMenu(fileName ="Item", menuName ="Create Item")]
public class Item : ScriptableObject
{
    [SerializeField] private int itemID;

    public int ItemID { get { return itemID; } }

    [SerializeField] private bool overlap;

    public bool Overlap { get { return overlap; } }

    [SerializeField] private bool interactive;
    public bool Interactive { get { return interactive; } }

    [SerializeField] private bool consumable;
    public bool Consumables { get { return consumable; } }

    [SerializeField] private ItemType itemType;
    public ItemType IType { get { return itemType; } }

    [SerializeField] private Sprite itemImage;
    public Sprite ItemImage { get { return itemImage; } }

}
