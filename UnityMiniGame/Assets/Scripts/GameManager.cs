using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 
    public Vector2 mousePos;
    private Camera cam;
 

    public static GameManager Instance;


    private void Awake()
    {
        cam = Camera.main;
         
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(Instance);
        }

        Cursor.visible = false;
    }

    private void Update()
    {
        Mouse();
    }

    public void Mouse()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
         
    }


}
