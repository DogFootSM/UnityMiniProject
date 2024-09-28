using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] List<Crop> crops = new List<Crop>();

    private PlayerController player;

    private bool isActive = false;
    public bool IsActive { get { return isActive; } set { isActive = value; } }


    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        for(int i = 0; i < crops.Count; i++)
        {
            Debug.Log($"�κ��丮 {i} : {crops[i].name}");
        }
    }



    private void OnEnable()
    {
        //�κ��丮 ���� ȿ���� ���

        //�κ��丮 ����
        isActive = true;
    }

    private void OnDisable()
    {
        //�κ��丮 Ŭ���� ȿ���� ���

        //�κ��丮 Ŭ����
        isActive = false;
    }

    public void AddItem(Crop crop)
    {
       
    }

    public void RemoveItem()
    {

    }


}
