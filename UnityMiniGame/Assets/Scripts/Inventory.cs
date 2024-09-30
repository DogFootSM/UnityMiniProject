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

    [Header("아이템 정보창")]
    [SerializeField] private GameObject itemIndicator;
    [SerializeField] private TextMeshProUGUI itemIndicatorName;
    [SerializeField] private TextMeshProUGUI itemIndicatorType;
    [SerializeField] private TextMeshProUGUI itemIndicatorDesc;
    [SerializeField] private Image itemIndicatorImage;
 
    //UI 캔버스
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
        //인벤토리 오픈 효과음 재생
 
    }

    private void OnDisable()
    {
        //인벤토리 클로즈 효과음 재생

 
    }

    //아이템 정보창 Offset
    Vector3 offset = new Vector3(80, 100);

    public void InventoryIndicator()
    {
        if (Inventory.isInventoryActive)
        {
            //ped의 위치
            ped.position = Input.mousePosition;

            //감지된 ray의 결과를 모두 계속 저장히기 때문에 리스트를 Clear
            results.Clear();

            //감지된 ped 위치의 Canvas 요소를 results에 저장
            ray.Raycast(ped, results);


            if (results.Count > 0)
            {
                //레이 리스트의 가장 첫 번째 ItemSlot 컴포넌트 가져옴
                InventorySlot slot = results[0].gameObject.GetComponent<InventorySlot>();

                //태그가 Slot 이면서 아이템을 가지고 있을 경우 아이템 정보창 활성화
                if (results[0].gameObject.tag == "InventorySlot" && slot.Item != null)
                {
                    //해당 아이템 정보를 반영 
                    itemIndicator.SetActive(true);
                    itemIndicator.transform.position = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y - offset.y);
                    itemIndicatorName.text = slot.Item.ItemName;
                    itemIndicatorType.text = slot.Item.IType.ToString();
                    itemIndicatorDesc.text = slot.Item.ItemName + "입니다...";
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
