using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    //현재 아이템의 이미지
    [SerializeField] private Image itemImage;
    public Image ItemImage { get { return itemImage; } set { itemImage = value; } }

    //현재 아이템의 개수 텍스트
    [SerializeField] private TextMeshProUGUI itemCountText;
    public TextMeshProUGUI ItemCountText { get { return itemCountText; } set { itemCountText = value; } }

    [SerializeField] private Button itemRemoveButton;

    //아이템 인스턴스 변수
    private Item _item;
    public Item Item { get { return _item; } set { _item = value; } }

    //현재 아이템의 개수
    private int itemCount;
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }

    private void Update()
    {
        //인벤토리가 비어있으면 텍스트 공백처리
        if(_item == null)
        {
            itemCountText.text = "";
            itemRemoveButton.gameObject.SetActive(false);
        }
        else
        {
            itemRemoveButton.gameObject.SetActive(true);
        }
    }
 

    //아이템을 새로 추가
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

    //아이템 정보 업데이트
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
        itemRemoveButton.gameObject.SetActive(false);
    }



}
