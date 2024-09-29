using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
     
    [SerializeField] private TextMeshProUGUI harvestText;
    public TextMeshProUGUI HarvestText { get { return harvestText; } }

    [Header("아이템 데이터")]
    [SerializeField] private Item _item;            //넣어줄 농작물 데이터
    private int cropItemID;        //농작물 ID
    private string name;           //농작물 이름
    private float growTime;        //농작물 성장 시간
    private int effect;            //작물 섭취 효과

    
    private SpriteRenderer cropRenderer;            //맵에 노출될 농작물 이미지
    private Coroutine growRoutine;
    private WaitForSeconds growWait;

    //농작물 애니메이션 배열
    private List<Sprite> cropSprite = new List<Sprite>();

    //농작물 이미지 배열 사이즈
    public int CropSize { get { return cropSprite.Count; } }

    //작물 성장 단계
    private int growthStage;
    public int GrowthStage { get { return growthStage; } }

     
    //수확 상태
    private bool isHarvestable = false;
    public bool IsHarvestable { get { return isHarvestable; } }

    //물을 준 상태
    private bool isWatering = false;
    public bool IsWatering { get { return isWatering; } set { isWatering = value; } }

    //마우스 오버 상태
    private bool onMouse;
    public bool OnMouse { get { return onMouse; } set { onMouse = value; } }

    //마우스 위치
    private Vector2 mousePos; 

    private void Awake()
    {
        cropSprite = _item.Sprites;
        cropItemID = _item.ItemID;
        name = _item.name;
        growTime = _item.GrowthTime;

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

            RaycastHit2D hit = Physics2D.Raycast(GameManager.Instance.mousePos, Vector2.zero);

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
    public Item Harvest()
    {
        Debug.Log($"{name} 수확");

        //아이템 데이터 반환
        return _item; 
    }



    private IEnumerator CropGrowthCoroutine()
    {

        //성장 단계에 맞는 이미지 노출 
        //물을 주고 일정 시간이 지난 후 농작물 성장
        //플레이어 취침 및 맵의 시간 영향
        yield return growWait;

        isWatering = false;
        cropRenderer.sprite = cropSprite[growthStage++];

        if (growthStage == cropSprite.Count)
        {
            isHarvestable = true; 
        }
    }
 

}
