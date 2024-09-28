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
            Debug.Log($"인벤토리 {i} : {crops[i].name}");
        }
    }



    private void OnEnable()
    {
        //인벤토리 오픈 효과음 재생

        //인벤토리 오픈
        isActive = true;
    }

    private void OnDisable()
    {
        //인벤토리 클로즈 효과음 재생

        //인벤토리 클로즈
        isActive = false;
    }

    public void AddItem(Crop crop)
    {
       
    }

    public void RemoveItem()
    {

    }


}
