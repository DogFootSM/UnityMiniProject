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



[CreateAssetMenu(fileName = "Item", menuName = "Create Item")]
public class Item : ScriptableObject
{
    [Header("아이템 고유 ID")]
    [SerializeField] private int itemID;
    public int ItemID { get { return itemID; } }

    [Header("아이템 중첩 가능 여부")]
    [SerializeField] private bool overlap;
    public bool Overlap { get { return overlap; } }

    //True : 장비 Fase : 기타 아이템
    [Header("아이템 상호작용 가능 여부")]
    [SerializeField] private bool interactive;
    public bool Interactive { get { return interactive; } }

    [Header("아이템 소모품 여부")]
    [SerializeField] private bool consumable;
    public bool Consumables { get { return consumable; } }

    [Header("아이템 타입")]
    [SerializeField] private ItemType itemType;
    public ItemType IType { get { return itemType; } }

    [Header("인벤토리에 노출될 아이템 이미지")]
    [SerializeField] private Sprite itemImage;
    public Sprite ItemImage { get { return itemImage; } }

    [Header("농작물 성장 시간")]
    [SerializeField] private float growthTime;
    public float GrowthTime { get { return growthTime; } }

    [Header("아이템 애니메이션 이미지 리스트")]
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    public List<Sprite> Sprites { get { return sprites; } }

    [Header("아이템 이름")]
    [SerializeField] private string itemName;
    public string ItemName { get { return itemName; } }


}
