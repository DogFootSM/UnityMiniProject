using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    //농작물 스파라이트 이미지 저장 배열
    [SerializeField] private List<Sprite> cropSprite = new List<Sprite>();

    [SerializeField] private TextMeshProUGUI harvestText;

    //농작물 성장 시간
    [SerializeField] private float growTime;

    //작물 이름
    [SerializeField] private string name;

    //농작물 ID
    [Header("아이템 ID")]
    [SerializeField] private int cropItemID;

    private SpriteRenderer cropRenderer;
    private Coroutine growRoutine;
    private WaitForSeconds growWait;
 
    public int CropSize { get { return cropSprite.Count; } }

    //작물 성장 단계
    private int growthStage;
    public int GrowthStage { get { return growthStage; } }


    //작물 섭취 효과
    private int effect;

     
    //수확 상태
    private bool isHarvestable = false;
    public bool IsHarvestable { get { return isHarvestable; } }

    private bool isWatering = false;
    public bool IsWatering { get { return isWatering; } set { isWatering = value; } }

    private bool onMouse;
    public bool OnMouse { get { return onMouse; } }

    private Vector2 mousePos; 

    private void Awake()
    {
        growWait = new WaitForSeconds(growTime);
        cropRenderer = GetComponent<SpriteRenderer>();  
    }



    private void Update()
    {

        if (isHarvestable && growRoutine != null)
        {
            StopCoroutine(growRoutine);
        }


        MouseOnCrop();
         
    }

    private void OnDisable()
    {
        //수확 효과음 재생
    }

    public void MouseOnCrop()
    {
        if (isHarvestable)
        {
 
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Crop")
                {
                    //마우스로 감지한 녀석을 받아옴
                    Crop instance = hit.collider.GetComponent<Crop>();

                    //내 자신과 비교해서 같으면 수확 가능 안내 활성화
                    if (instance == this)
                    {
                        harvestText.text = name + " 수확 가능";
                        harvestText.gameObject.SetActive(true);
                        onMouse = true;
                    }
                }
                else
                {
                    harvestText.gameObject.SetActive(false);
                    onMouse = false;
                }
            }
            else
            {
                harvestText.gameObject.SetActive(false);
                onMouse = false;
            }
        }

    }



    //작물 성장 함수
    public void Grow()
    {

        growRoutine = StartCoroutine(CropGrowthCoroutine());

    }

    //작물 수확 함수
    public Crop Harvest()
    {

        //destroy에서 인벤토리 저장으로 변경 필요
        //수확한 농작물을 Return?
        Debug.Log("비트 수확");

        return this;

    }



    private IEnumerator CropGrowthCoroutine()
    {

        //성장 단계에 맞는 이미지 노출
        growthStage++;

        //물을 주고 일정 시간이 지난 후 농작물 성장
        //플레이어 취침 및 맵의 시간 영향
        yield return growWait;

        isWatering = false;
        cropRenderer.sprite = cropSprite[growthStage];

        //cropSprite.count -1 이미지 > 인벤토리에 노출될 이미지
        if (growthStage == cropSprite.Count - 1)
        {
            isHarvestable = true;
 
        }
    }
 

}
