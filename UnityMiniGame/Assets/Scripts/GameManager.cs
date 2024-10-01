using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public enum GameState {None, Ready, Start, End }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState curState = GameState.None; 

    [Header("커서 이미지")]
    [SerializeField] private Texture2D cursorTexture;



    public Vector2 mousePos;
    private Camera cam;


    private void Awake()
    {
        cam = Camera.main;
        //마우스 커서 설정
        Cursor.SetCursor(cursorTexture, new Vector2(0, 0), CursorMode.ForceSoftware);


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
     

    //게임 시작 버튼 클릭 > 로딩 진행 후 씬 전환
    //옵션 버튼 >

}
