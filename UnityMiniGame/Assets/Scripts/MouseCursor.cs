using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Sprite cursorImage;  
 
    private SpriteRenderer mouseRender;
     

    private void Awake()
    {
         
        mouseRender = GetComponent<SpriteRenderer>();
        mouseRender.sprite = cursorImage;
    }
 

    private void Update()
    {
        //���콺 Ŀ�� �̹��� ��ġ
        transform.position = GameManager.Instance.mousePos;
  
    }

   


     
}
