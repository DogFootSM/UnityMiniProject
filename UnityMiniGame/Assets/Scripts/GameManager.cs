using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private FadeInOut fade;


    public Vector2 mousePos;

    private Action OnGameStart;

    private void Awake()
    {
 
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
        OnGameStart = GameStart;
    }

    private void Start()
    {
        fade.SceneChange(OnGameStart);
    }

    private void Update()
    {
        Mouse();
        
  

    }

    public void Mouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }
     
    //FadeOut이 완료된 후 Main 씬 이동 진행
    public void GameStart()
    {

        StartCoroutine(GameStartCo());
    }

    public IEnumerator GameStartCo()
    {
        AsyncOperation sync = SceneManager.LoadSceneAsync(1);
        sync.allowSceneActivation = false;

        while (!sync.isDone)
        {
            if(sync.progress >= 0.9f)
            {
                sync.allowSceneActivation = true;

                fade.FadeIn();
            }
             
            yield return null;
        }


    }


    //게임 시작 버튼 클릭 > 로딩 진행 후 씬 전환
    //옵션 버튼 >

}
