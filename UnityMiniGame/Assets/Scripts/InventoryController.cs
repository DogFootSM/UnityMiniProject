using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] protected GameObject inventoryBase;
    [SerializeField] protected GameObject inventorySlotsParent;
    [SerializeField] protected List<InventorySlot> slots = new List<InventorySlot>(); 

    protected void Awake()
    {
 
 
        for(int i = 0; i <inventorySlotsParent.transform.childCount; i++)
        {
            slots.Add(inventorySlotsParent.transform.GetChild(i).gameObject.GetComponent<InventorySlot>());
        }

            
    }


}
