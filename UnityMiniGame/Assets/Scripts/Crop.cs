using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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


    public int CropSize { get { return cropSprite.Count; } }

    //�۹� ���� �ܰ�
    private int growthStage;
    public int GrowthStage { get { return growthStage; } }


    //�۹� ���� ȿ��
    private int effect;

    //��Ȯ ����
    private bool isHarvestable = false;
    public bool IsHarvestable { get { return isHarvestable; } }


    private void Awake()
    {
        growWait = new WaitForSeconds(2.5f);
        cropRenderer = GetComponent<SpriteRenderer>();
    }



    private void Update()
    {
        Debug.Log($"�۹� ���� ����:{growthStage}");


        if (isHarvestable && growRoutine != null)
        { 
            StopCoroutine(growRoutine);
            
        }

    }


    //�۹� ���� �Լ�
    public void Grow()
    {

        growRoutine = StartCoroutine(CropGrowthCoroutine());

    }

    //�۹� ��Ȯ �Լ�
    public void Harvest()
    {

        //destroy���� �κ��丮 �������� ���� �ʿ�
        Debug.Log("��Ʈ ��Ȯ");

        Destroy(gameObject);
    }

    private IEnumerator CropGrowthCoroutine()
    {

        //���� �ܰ迡 �´� �̹��� ����
        growthStage++;

        //���� �ְ� ���� �ð��� ���� �� ���۹� ����
        //�÷��̾� ��ħ �� ���� �ð� ����
        yield return new WaitForSeconds(2.5f);
        cropRenderer.sprite = cropSprite[growthStage];


        if (growthStage == cropSprite.Count - 1)
        {
            isHarvestable = true;
        }
    }



}
