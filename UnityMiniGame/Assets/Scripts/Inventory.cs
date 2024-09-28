using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : InventoryController
{


    public static bool isInventoryActive = false;


    void Awake()
    {
        base.Awake();
    }

    private void Update()
    {

        OpenInventory();
    }


    private void OnEnable()
    {
        //인벤토리 오픈 효과음 재생
 
    }

    private void OnDisable()
    {
        //인벤토리 클로즈 효과음 재생

 
    }

    public void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isInventoryActive)
            {
                inventoryBase.SetActive(true);
                isInventoryActive = true;
            }
            else
            {
                inventoryBase.SetActive(false);
                isInventoryActive = false;
            }
        }
    }

    public void ButtonClose()
    {

        inventoryBase.SetActive(false);
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
                    Debug.Log("중첩");
                    return;
                }
                else if (slots[i].Item == null)
                {
                    slots[i].AddItem(item);
                    Debug.Log("중첩3");
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
                    Debug.Log("중첩x");
                    return;
                }
            }

        }


    }

 

 


}
