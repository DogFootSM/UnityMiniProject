using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Flags]
public enum ItemType
{
    Crop,
    Tool,
     
    //���� ��� �߰�..

}



[CreateAssetMenu(fileName = "Item", menuName = "Create Item")]
public class Item : ScriptableObject
{
    [Header("������ ���� ID")]
    [SerializeField] private int itemID;
    public int ItemID { get { return itemID; } }

    [Header("������ ��ø ���� ����")]
    [SerializeField] private bool overlap;
    public bool Overlap { get { return overlap; } }

    //True : ��� Fase : ��Ÿ ������
    [Header("������ ��ȣ�ۿ� ���� ����")]
    [SerializeField] private bool interactive;
    public bool Interactive { get { return interactive; } }

    [Header("������ �Ҹ�ǰ ����")]
    [SerializeField] private bool consumable;
    public bool Consumables { get { return consumable; } }

    [Header("������ Ÿ��")]
    [SerializeField] private ItemType itemType;
    public ItemType IType { get { return itemType; } }

    [Header("�κ��丮�� ����� ������ �̹���")]
    [SerializeField] private Sprite itemImage;
    public Sprite ItemImage { get { return itemImage; } }

    [Header("���۹� ���� �ð�")]
    [SerializeField] private float growthTime;
    public float GrowthTime { get { return growthTime; } }

    [Header("������ �ִϸ��̼� �̹��� ����Ʈ")]
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    public List<Sprite> Sprites { get { return sprites; } }

    [Header("������ �̸�")]
    [SerializeField] private string itemName;
    public string ItemName { get { return itemName; } }


}
