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

    [Header("아이템 정보창")]
    [SerializeField] private GameObject itemIndicator;
    [SerializeField] private TextMeshProUGUI itemIndicatorName;
    [SerializeField] private TextMeshProUGUI itemIndicatorType;
    [SerializeField] private TextMeshProUGUI itemIndicatorDesc;
    [SerializeField] private Image itemIndicatorImage;

    //UI 캔버스
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

        //ped의 위치
        ped.position = Input.mousePosition;

        //감지된 ray의 결과를 모두 계속 저장히기 때문에 리스트를 Clear
        results.Clear();

        //감지된 ped 위치의 Canvas 요소를 results에 저장
        ray.Raycast(ped, results);

        OpenInventory();

        InventoryIndicator();
        SelectSlot();
    }


    private void OnEnable()
    {
        //인벤토리 오픈 효과음 재생

    }

    private void OnDisable()
    {
        //인벤토리 클로즈 효과음 재생


    }



    public void InventoryIndicator()
    {
        //아이템 정보창 Offset
        Vector3 indicatorOffset = new Vector3(80, 100);

        if (isInventoryActive)
        {
            if (results.Count > 0)
            {
                //레이 리스트의 가장 첫 번째 ItemSlot 컴포넌트 가져옴
                InventorySlot slot = results[0].gameObject.GetComponent<InventorySlot>();

                //태그가 Slot 이면서 아이템을 가지고 있을 경우 아이템 정보창 활성화
                if (results[0].gameObject.tag == "InventorySlot" && slot.Item != null)
                {
                    //해당 아이템 정보를 반영 
                    itemIndicator.SetActive(true);
                    itemIndicator.transform.position = new Vector3(Input.mousePosition.x + indicatorOffset.x, Input.mousePosition.y - indicatorOffset.y);
                    itemIndicatorName.text = slot.Item.ItemName;
                    itemIndicatorType.text = slot.Item.IType.ToString();
                    itemIndicatorDesc.text = slot.Item.ItemName + "입니다...";
                    itemIndicatorImage.sprite = slot.Item.ItemImage;

                    //인벤토리 아이템 선택
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

    //아이템이 저장되어 있는 슬롯을 가져올 변수
    InventorySlot slot1;
    InventorySlot slot2;

    //아이템이 저장되어 있는 슬롯의 인덱스
    int slotIndexA = 0;
    int slotIndexB = 0;

    public void Changed(InventorySlot aSlot, InventorySlot bSlot)
    {
        //슬롯이 가지고 있는 아이템을 저장할 임시 변수
        Item temp = aSlot.Item;
        int count = aSlot.ItemCount;

        //두 슬롯 모두 아이템을 가지고 있는 상태 
        if (aSlot.Item != null && bSlot.Item != null)
        {
            //두 슬롯이 가지고 있는 아이템의 ID가 같을 경우 하나로 합침
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
                    //처음 클릭한 인벤토리 칸의 아이템 슬롯
                    slot1 = results[0].gameObject.GetComponent<InventorySlot>();

                    //아이템 클릭 후 드래그 동작
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
                        //선택한 슬롯과 배열에 저장되어 있는 슬롯이 같은지 비교
                        if (slots[i] == slot1)
                        {
                            slotIndexA = i;
                            break;
                        }
                    }
                }

                else if (Input.GetMouseButtonUp(0))
                {
                    //바꿀 위치의 아이템 슬롯
                    slot2 = results[0].gameObject.GetComponent<InventorySlot>();

                    for (int i = 0; i < slots.Count; i++)
                    {
                        //바꿀 위치의 아이템 슬롯과 배열에 저장되어 있는 슬롯 비교
                        if (slots[i] == slot2)
                        {
                            slotIndexB = i;
                            break;
                        }
                    }

                    isSelecting = false;

                    //같은 슬롯인지 검사 후 변경 호출
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

    //삭제할 슬롯 변수
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


    //인벤토리 X 버튼 종료
    public void ButtonClose()
    {

        inventoryUI.SetActive(false);
        isInventoryActive = false;

    }

    public void PickUpItemp(Item item, int count = 1)
    {
        //중복 가능한 아이템 추가
        if (item.Overlap)
        {

            for (int i = 0; i < slots.Count; i++)
            {
                //슬롯이 아이템을 가지고 있고 추가할 아이템과 같은 ID라면 개수 증가
                if (slots[i].Item != null && slots[i].Item.ItemID == item.ItemID)
                {
                    slots[i].UpdateSlotCount(count);
                    return;
                }
                else if (slots[i].Item == null)
                {
                    for (int j = i; j < slots.Count; j++)
                    {
                        //해당 슬롯이 아이템을 가지고 있지 않을 경우 i번 째 부터 같은 아이템이 있는지 검사
                        if (slots[j].Item == item)
                        {
                            //같은 아이템 찾을 경우 아이템 개수 증가
                            slots[j].UpdateSlotCount(count);
                            return;
                        }
                    }

                    //같은 아이템을 못찾았으면 슬롯에 아이템 추가
                    slots[i].AddItem(item);
                    return;
                }
            }


        }
        //중복 불가 아이템 추가
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

