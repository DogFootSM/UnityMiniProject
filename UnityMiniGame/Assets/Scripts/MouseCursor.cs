using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Sprite cursorImage;
    

    private Vector2 mousePos;
 
    private SpriteRenderer mouseRender;


    private void Awake()
    {
        mouseRender = GetComponent<SpriteRenderer>();
        mouseRender.sprite = cursorImage;
     }

 

    private void Update()
    {
 
        transform.position = GameManager.Instance.mousePos;
 
        

        //농작물이 수확 가능한 상태일 때 농작물 인디케이터 노출

        //인벤토리 > 아이템 인디케이터 노출



    }



}
