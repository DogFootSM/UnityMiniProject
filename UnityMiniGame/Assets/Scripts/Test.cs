using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] private Inventory inventory;

    [SerializeField] private Item Beet, Carrot, Pumpkin;

    private void OnGUI()
    {
        if(GUI.Button(new Rect(20, 20, 300, 40),"��Ʈ ȹ��"))
        {
            inventory.PickUpItemp(Beet);
        }

        if(GUI.Button(new Rect(400,20, 300, 40), "��� ȹ��"))
        {
            inventory.PickUpItemp(Carrot);
        }

        if (GUI.Button(new Rect(200, 50, 300, 40), "ȣ�� ȹ��"))
        {
            inventory.PickUpItemp(Pumpkin);
        }

    }


}
