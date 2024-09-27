using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Crop : MonoBehaviour
{
    //농작물 스파라이트 이미지 저장 배열
    [SerializeField] private List<Sprite> cropSprite = new List<Sprite>();

    //작물 이름
    [SerializeField] private string name;


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


    private void Awake()
    {
        growWait = new WaitForSeconds(2.5f);
        cropRenderer = GetComponent<SpriteRenderer>();
    }



    private void Update()
    {
        Debug.Log($"작물 성장 상태:{growthStage}");


        if (isHarvestable && growRoutine != null)
        { 
            StopCoroutine(growRoutine);
            
        }

    }


    //작물 성장 함수
    public void Grow()
    {

        growRoutine = StartCoroutine(CropGrowthCoroutine());

    }

    //작물 수확 함수
    public void Harvest()
    {

        //destroy에서 인벤토리 저장으로 변경 필요
        Debug.Log("비트 수확");

        Destroy(gameObject);
    }

    private IEnumerator CropGrowthCoroutine()
    {

        //성장 단계에 맞는 이미지 노출
        growthStage++;

        //물을 주고 일정 시간이 지난 후 농작물 성장
        //플레이어 취침 및 맵의 시간 영향
        yield return new WaitForSeconds(2.5f);
        cropRenderer.sprite = cropSprite[growthStage];


        if (growthStage == cropSprite.Count - 1)
        {
            isHarvestable = true;
        }
    }



}
