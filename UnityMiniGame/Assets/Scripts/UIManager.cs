using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Slider energyBar;

    [Header("타이틀 뮤트 이미지")]
    [SerializeField] private Sprite mute;
    [SerializeField] private Sprite unMute;
    [SerializeField] private Image muteImage;
 
 
    private void Awake()
    { 
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }

    }

 

    public void EnergyBarUpdate(float value)
    {
        energyBar.value = value * 0.01f; 
    }

    public void MuteImage()
    {
        if (SoundManager.Instance.MuteState)
        {
            muteImage.sprite = mute;
        }
        else
        {
            muteImage.sprite = unMute;
        }
    }
 

}
