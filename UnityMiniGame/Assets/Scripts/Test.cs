using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] private Inventory inventory;

    [SerializeField] private Item Beet, Carrot;

    private void OnGUI()
    {
        if(GUI.Button(new Rect(20, 20, 300, 40),"∫Ò∆Æ »πµÊ"))
        {
            inventory.PickUpItemp(Beet);
        }

        if(GUI.Button(new Rect(400,20, 300, 40), "¥Á±Ÿ »πµÊ"))
        {
            inventory.PickUpItemp(Carrot);
        }

    }


}
