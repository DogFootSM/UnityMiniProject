using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("커서 이미지")]
    [SerializeField] private Texture2D cursorTexture;

    public Vector2 mousePos;
    private Camera cam;
 




    private void Awake()
    {
        //마우스 커서 설정
        Cursor.SetCursor(cursorTexture, new Vector2(0, 0),CursorMode.ForceSoftware);

        cam = Camera.main;
         
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        } 
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
