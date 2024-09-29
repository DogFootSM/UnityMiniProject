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

    [Header("������ ������")]
    [SerializeField] private Item _item;            //�־��� ���۹� ������
    private int cropItemID;        //���۹� ID
    private string name;           //���۹� �̸�
    private float growTime;        //���۹� ���� �ð�
    private int effect;            //�۹� ���� ȿ��

    
    private SpriteRenderer cropRenderer;            //�ʿ� ����� ���۹� �̹���
    private Coroutine growRoutine;
    private WaitForSeconds growWait;

    //���۹� �ִϸ��̼� �迭
    private List<Sprite> cropSprite = new List<Sprite>();

    //���۹� �̹��� �迭 ������
    public int CropSize { get { return cropSprite.Count; } }

    //�۹� ���� �ܰ�
    private int growthStage;
    public int GrowthStage { get { return growthStage; } }

     
    //��Ȯ ����
    private bool isHarvestable = false;
    public bool IsHarvestable { get { return isHarvestable; } }

    //���� �� ����
    private bool isWatering = false;
    public bool IsWatering { get { return isWatering; } set { isWatering = value; } }

    //���콺 ���� ����
    private bool onMouse;
    public bool OnMouse { get { return onMouse; } set { onMouse = value; } }

    //���콺 ��ġ
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
        //��Ȯ ȿ���� ���
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
    public Item Harvest()
    {
        Debug.Log($"{name} ��Ȯ");

        //������ ������ ��ȯ
        return _item; 
    }



    private IEnumerator CropGrowthCoroutine()
    {

        //���� �ܰ迡 �´� �̹��� ���� 
        //���� �ְ� ���� �ð��� ���� �� ���۹� ����
        //�÷��̾� ��ħ �� ���� �ð� ����
        yield return growWait;

        isWatering = false;
        cropRenderer.sprite = cropSprite[growthStage++];

        if (growthStage == cropSprite.Count)
        {
            isHarvestable = true; 
        }
    }
 

}
