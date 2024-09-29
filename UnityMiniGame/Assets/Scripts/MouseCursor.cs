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
 
        

        //���۹��� ��Ȯ ������ ������ �� ���۹� �ε������� ����

        //�κ��丮 > ������ �ε������� ����



    }



}
