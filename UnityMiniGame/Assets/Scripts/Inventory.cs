using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Inventory : InventoryController
{
  
    public static bool isInventoryActive = false;

    [Header("������ ����â")]
    [SerializeField] private GameObject itemIndicator;
    [SerializeField] private TextMeshProUGUI itemIndicatorName;
    [SerializeField] private TextMeshProUGUI itemIndicatorType;
    [SerializeField] private TextMeshProUGUI itemIndicatorDesc;
    [SerializeField] private Image itemIndicatorImage;
 
    //UI ĵ����
    [SerializeField] private Canvas canvas;

    private GraphicRaycaster ray;
    private PointerEventData ped = new PointerEventData(EventSystem.current);
    private List<RaycastResult> results = new List<RaycastResult>();


    void Awake()
    {
        base.Awake();
        ray = canvas.GetComponent<GraphicRaycaster>();
    }


    private void Update()
    {

        OpenInventory();

        InventoryIndicator();

    }


    private void OnEnable()
    {
        //�κ��丮 ���� ȿ���� ���
 
    }

    private void OnDisable()
    {
        //�κ��丮 Ŭ���� ȿ���� ���

 
    }

    //������ ����â Offset
    Vector3 offset = new Vector3(80, 100);

    public void InventoryIndicator()
    {
        if (Inventory.isInventoryActive)
        {
            //ped�� ��ġ
            ped.position = Input.mousePosition;

            //������ ray�� ����� ��� ��� �������� ������ ����Ʈ�� Clear
            results.Clear();

            //������ ped ��ġ�� Canvas ��Ҹ� results�� ����
            ray.Raycast(ped, results);


            if (results.Count > 0)
            {
                //���� ����Ʈ�� ���� ù ��° ItemSlot ������Ʈ ������
                InventorySlot slot = results[0].gameObject.GetComponent<InventorySlot>();

                //�±װ� Slot �̸鼭 �������� ������ ���� ��� ������ ����â Ȱ��ȭ
                if (results[0].gameObject.tag == "InventorySlot" && slot.Item != null)
                {
                    //�ش� ������ ������ �ݿ� 
                    itemIndicator.SetActive(true);
                    itemIndicator.transform.position = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y - offset.y);
                    itemIndicatorName.text = slot.Item.ItemName;
                    itemIndicatorType.text = slot.Item.IType.ToString();
                    itemIndicatorDesc.text = slot.Item.ItemName + "�Դϴ�...";
                    itemIndicatorImage.sprite = slot.Item.ItemImage;
 
                }
                else
                {
                    itemIndicator.SetActive(false);
                }

            }
            else
            {
                itemIndicator.SetActive(false);
            }

        }

    }

    public void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isInventoryActive)
            {
                inventoryUI.SetActive(true);
                isInventoryActive = true; 
            }
            else
            {
                inventoryUI.SetActive(false);
                isInventoryActive = false;

                
                if (itemIndicator.activeSelf)
                {
                    itemIndicator.SetActive(false);
                }

            }
        }
    }


    
    public void ButtonClose()
    {

        inventoryUI.SetActive(false);
        isInventoryActive = false;

    }



    public void PickUpItemp(Item item, int count = 1)
    {
        if (item.Overlap)
        {

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Item != null && slots[i].Item.ItemID == item.ItemID)
                {
                    slots[i].UpdateSlotCount(count);
                    return;
                }
                else if (slots[i].Item == null)
                {
                    slots[i].AddItem(item);
                    return;
                }
            }


        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Item == null)
                {
                    slots[i].AddItem(item);
                    return;
                }
            }

        }


    }

 

 


}
