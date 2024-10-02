using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    [SerializeField] private GameObject itemSelectBox;
    [SerializeField] private Image itemIconImage;

    private GraphicRaycaster ray;
    private PointerEventData ped = new PointerEventData(EventSystem.current);
    private List<RaycastResult> results = new List<RaycastResult>();


    [SerializeField] private GameObject removeAlert;

    private bool isSelecting;

    void Awake()
    {
        base.Awake();
        ray = canvas.GetComponent<GraphicRaycaster>();
    }


    private void Update()
    {

        //ped�� ��ġ
        ped.position = Input.mousePosition;

        //������ ray�� ����� ��� ��� �������� ������ ����Ʈ�� Clear
        results.Clear();

        //������ ped ��ġ�� Canvas ��Ҹ� results�� ����
        ray.Raycast(ped, results);

        OpenInventory();

        InventoryIndicator();
        SelectSlot();
    }


    private void OnEnable()
    {
        //�κ��丮 ���� ȿ���� ���

    }

    private void OnDisable()
    {
        //�κ��丮 Ŭ���� ȿ���� ���


    }



    public void InventoryIndicator()
    {
        //������ ����â Offset
        Vector3 indicatorOffset = new Vector3(80, 100);

        if (isInventoryActive)
        {
            if (results.Count > 0)
            {
                //���� ����Ʈ�� ���� ù ��° ItemSlot ������Ʈ ������
                InventorySlot slot = results[0].gameObject.GetComponent<InventorySlot>();

                //�±װ� Slot �̸鼭 �������� ������ ���� ��� ������ ����â Ȱ��ȭ
                if (results[0].gameObject.tag == "InventorySlot" && slot.Item != null)
                {
                    //�ش� ������ ������ �ݿ� 
                    itemIndicator.SetActive(true);
                    itemIndicator.transform.position = new Vector3(Input.mousePosition.x + indicatorOffset.x, Input.mousePosition.y - indicatorOffset.y);
                    itemIndicatorName.text = slot.Item.ItemName;
                    itemIndicatorType.text = slot.Item.IType.ToString();
                    itemIndicatorDesc.text = slot.Item.ItemName + "�Դϴ�...";
                    itemIndicatorImage.sprite = slot.Item.ItemImage;

                    //�κ��丮 ������ ����
                    if (Input.GetMouseButtonDown(0))
                    {
                        itemSelectBox.transform.position = slot.gameObject.transform.position;
                        itemSelectBox.SetActive(true);
                    }
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

    //�������� ����Ǿ� �ִ� ������ ������ ����
    InventorySlot slot1;
    InventorySlot slot2;

    //�������� ����Ǿ� �ִ� ������ �ε���
    int slotIndexA = 0;
    int slotIndexB = 0;

    public void Changed(InventorySlot aSlot, InventorySlot bSlot)
    {
        //������ ������ �ִ� �������� ������ �ӽ� ����
        Item temp = aSlot.Item;
        int count = aSlot.ItemCount;

        //�� ���� ��� �������� ������ �ִ� ���� 
        if (aSlot.Item != null && bSlot.Item != null)
        {
            //�� ������ ������ �ִ� �������� ID�� ���� ��� �ϳ��� ��ħ
            if (aSlot.Item.ItemID == bSlot.Item.ItemID)
            {
                bSlot.UpdateSlotCount(aSlot.ItemCount);
                aSlot.ClearSlot();
            }
            else
            {
                aSlot.AddItem(bSlot.Item, bSlot.ItemCount);
                bSlot.AddItem(temp, count);
            }


        }
        else if (aSlot.Item != null && bSlot.Item == null)
        {

            bSlot.AddItem(aSlot.Item, aSlot.ItemCount);
            aSlot.ClearSlot();
             
        }

        itemSelectBox.transform.position = bSlot.transform.position;

    }



    public void SelectSlot()
    {
        if (isInventoryActive)
        {
            if (results.Count > 0)
            {

                if (Input.GetMouseButtonDown(0) && results[0].gameObject.tag == "InventorySlot")
                {
                    //ó�� Ŭ���� �κ��丮 ĭ�� ������ ����
                    slot1 = results[0].gameObject.GetComponent<InventorySlot>();

                    //������ Ŭ�� �� �巡�� ����
                    if (slot1.Item != null)
                    {
                        itemIconImage.gameObject.SetActive(true);
                        itemIconImage.sprite = slot1.Item.ItemImage;
                        itemIconImage.color = new Color(255f, 255f, 255f, 0.6f);
                        isSelecting = true;
                    }

                    else if(slot1.Item == null && itemSelectBox.activeSelf)
                    {
                        itemSelectBox.SetActive(false);
                    }

                    for (int i = 0; i < slots.Count; i++)
                    {
                        //������ ���԰� �迭�� ����Ǿ� �ִ� ������ ������ ��
                        if (slots[i] == slot1)
                        {
                            slotIndexA = i;
                            break;
                        }
                    }
                }

                else if (Input.GetMouseButtonUp(0))
                {
                    //�ٲ� ��ġ�� ������ ����
                    slot2 = results[0].gameObject.GetComponent<InventorySlot>();

                    for (int i = 0; i < slots.Count; i++)
                    {
                        //�ٲ� ��ġ�� ������ ���԰� �迭�� ����Ǿ� �ִ� ���� ��
                        if (slots[i] == slot2)
                        {
                            slotIndexB = i;
                            break;
                        }
                    }

                    isSelecting = false;

                    //���� �������� �˻� �� ���� ȣ��
                    if (slots[slotIndexA] != slots[slotIndexB])
                    {
                        Changed(slots[slotIndexA], slots[slotIndexB]);
                    }


                }

                if (isSelecting)
                {
                    itemIconImage.transform.position = Input.mousePosition;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }
            }
        }
    }

    //������ ���� ����
    InventorySlot removeSlot;
     
    public void OpenAlert(InventorySlot removeSlot)
    {
        this.removeSlot = removeSlot;
        removeAlert.SetActive(true);
    }

    public void CloseAlert()
    {
        removeAlert.SetActive(false);
    }
     
    public void RemoveItem()
    {
        removeSlot.ClearSlot();
        removeAlert.SetActive(false);

        if (itemSelectBox.activeSelf)
        {
            itemSelectBox.SetActive(false);
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
                itemIconImage.gameObject.SetActive(false);
                itemIndicator.SetActive(false);
                itemSelectBox.SetActive(false);
            }
        }
    }


    //�κ��丮 X ��ư ����
    public void ButtonClose()
    {

        inventoryUI.SetActive(false);
        isInventoryActive = false;

    }

    public void PickUpItemp(Item item, int count = 1)
    {
        //�ߺ� ������ ������ �߰�
        if (item.Overlap)
        {

            for (int i = 0; i < slots.Count; i++)
            {
                //������ �������� ������ �ְ� �߰��� �����۰� ���� ID��� ���� ����
                if (slots[i].Item != null && slots[i].Item.ItemID == item.ItemID)
                {
                    slots[i].UpdateSlotCount(count);
                    return;
                }
                else if (slots[i].Item == null)
                {
                    for (int j = i; j < slots.Count; j++)
                    {
                        //�ش� ������ �������� ������ ���� ���� ��� i�� ° ���� ���� �������� �ִ��� �˻�
                        if (slots[j].Item == item)
                        {
                            //���� ������ ã�� ��� ������ ���� ����
                            slots[j].UpdateSlotCount(count);
                            return;
                        }
                    }

                    //���� �������� ��ã������ ���Կ� ������ �߰�
                    slots[i].AddItem(item);
                    return;
                }
            }


        }
        //�ߺ� �Ұ� ������ �߰�
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

