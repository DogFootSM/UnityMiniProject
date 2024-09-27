using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    //���۹� ���Ķ���Ʈ �̹��� ���� �迭
    [SerializeField] private List<Sprite> cropSprite = new List<Sprite>();

    //�۹� �̸�
    [SerializeField] private string name;


    private SpriteRenderer cropRenderer;
    private Coroutine growRoutine;
    private WaitForSeconds growWait;


    //�۹� ���� �ܰ�
    private int growthStage;

    //�۹� ���� ȿ��
    private int effect;

    //��Ȯ ����
    private bool isHarvestable = false;
    private bool isWater = false;


    private void Awake()
    {
        growWait = new WaitForSeconds(2.5f);
        cropRenderer = GetComponent<SpriteRenderer>();
    }

 

    private void Update()
    {
        Debug.Log($"�۹� ���� ����:{growthStage}");
    }


    //�۹� ���� �Լ�
    public void Grow()
    { 
        growRoutine =  StartCoroutine(CropGrowthCoroutine());
    }

    //�۹� ��Ȯ �Լ�
    public void Harvest()
    {
        //�۹� ��Ȯ �� ������ ��������Ʈ �̹����� ��ü


    }

    private IEnumerator CropGrowthCoroutine()
    {
        //���� �ܰ迡 �´� �̹��� ����
        yield return new WaitForSeconds(2.5f);
        cropRenderer.sprite = cropSprite[growthStage];
         

    }



}
