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
        //�κ��丮 ���� ȿ���� ���
 
    }

    private void OnDisable()
    {
        //�κ��丮 Ŭ���� ȿ���� ���

 
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
                    Debug.Log("��ø");
                    return;
                }
                else if (slots[i].Item == null)
                {
                    slots[i].AddItem(item);
                    Debug.Log("��ø3");
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
                    Debug.Log("��øx");
                    return;
                }
            }

        }


    }

 

 


}
