using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    //���۹� ���Ķ���Ʈ �̹��� ���� �迭
    [SerializeField] private List<Sprite> cropSprite = new List<Sprite>();

    [SerializeField] private TextMeshProUGUI harvestText;

    //���۹� ���� �ð�
    [SerializeField] private float growTime;

    //�۹� �̸�
    [SerializeField] private string name;

    //���۹� ID
    [Header("������ ID")]
    [SerializeField] private int cropItemID;

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
        //��Ȯ ȿ���� ���
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
                    //���콺�� ������ �༮�� �޾ƿ�
                    Crop instance = hit.collider.GetComponent<Crop>();

                    //�� �ڽŰ� ���ؼ� ������ ��Ȯ ���� �ȳ� Ȱ��ȭ
                    if (instance == this)
                    {
                        harvestText.text = name + " ��Ȯ ����";
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



    //�۹� ���� �Լ�
    public void Grow()
    {

        growRoutine = StartCoroutine(CropGrowthCoroutine());

    }

    //�۹� ��Ȯ �Լ�
    public Crop Harvest()
    {

        //destroy���� �κ��丮 �������� ���� �ʿ�
        //��Ȯ�� ���۹��� Return?
        Debug.Log("��Ʈ ��Ȯ");

        return this;

    }



    private IEnumerator CropGrowthCoroutine()
    {

        //���� �ܰ迡 �´� �̹��� ����
        growthStage++;

        //���� �ְ� ���� �ð��� ���� �� ���۹� ����
        //�÷��̾� ��ħ �� ���� �ð� ����
        yield return growWait;

        isWatering = false;
        cropRenderer.sprite = cropSprite[growthStage];

        //cropSprite.count -1 �̹��� > �κ��丮�� ����� �̹���
        if (growthStage == cropSprite.Count - 1)
        {
            isHarvestable = true;
 
        }
    }
 

}
