using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Sprite cursorImage;

    private Vector2 mousePos;
    [SerializeField]  private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
      
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);




    }



}
