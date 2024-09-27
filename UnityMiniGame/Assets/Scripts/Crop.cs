using System.Collections;
using System.Collections.Generic;
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


    //작물 성장 단계
    private int growthStage;

    //작물 섭취 효과
    private int effect;

    //수확 상태
    private bool isHarvestable = false;
    private bool isWater = false;


    private void Awake()
    {
        growWait = new WaitForSeconds(2.5f);
        cropRenderer = GetComponent<SpriteRenderer>();
    }

 

    private void Update()
    {
        Debug.Log($"작물 성장 상태:{growthStage}");
    }


    //작물 성장 함수
    public void Grow()
    { 
        growRoutine =  StartCoroutine(CropGrowthCoroutine());
    }

    //작물 수확 함수
    public void Harvest()
    {
        //작물 수확 시 마지막 스프라이트 이미지로 교체


    }

    private IEnumerator CropGrowthCoroutine()
    {
        //성장 단계에 맞는 이미지 노출
        yield return new WaitForSeconds(2.5f);
        cropRenderer.sprite = cropSprite[growthStage];
         

    }



}
