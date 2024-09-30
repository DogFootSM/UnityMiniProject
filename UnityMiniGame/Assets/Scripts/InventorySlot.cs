using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    //���� �������� �̹���
    [SerializeField] private Image itemImage;

    //���� �������� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI itemCountText;
 

    //������ �ν��Ͻ� ����
    private Item _item;
    public Item Item { get { return _item; } }

    //���� �������� ����
    private int itemCount;


    private void Update()
    {
        //�κ��丮�� ��������� �ؽ�Ʈ ����ó��
        if(_item == null)
        {
            itemCountText.text = "";
        }
 


    }

    public InventorySlot GetSlotItem()
    {
        return this;
    }

    //�������� ���� �߰�
    public void AddItem(Item item, int count =1)
    {
        _item = item;
        itemCount = count;
        itemImage.sprite = item.ItemImage;

        if (!item.Consumables)
        {
            itemCountText.text = "";
        }
        else
        {
            itemCountText.text = itemCount.ToString();
        }

    }

    //������ ���� ������Ʈ
    public void UpdateSlotCount(int count)
    {
        itemCount += count;
        itemCountText.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        _item = null;
        itemCount = 0;
        itemImage.sprite = null;
        itemCountText.text = "";

    }



}
