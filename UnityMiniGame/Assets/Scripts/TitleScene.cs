using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [Header("타이틀 뮤트 이미지")]
    [SerializeField] private Sprite mute;
    [SerializeField] private Sprite unMute;
    [SerializeField] private Image muteImage;
     
    private bool muteState;

    

    public void Awake()
    {
        SoundManager.Instance.ChangeBGM(SoundManager.Instance.bgmList[0]);
    }

    public void MuteImage()
    {
        if (muteState)
        {
            muteImage.sprite = mute;
        }
        else
        {
            muteImage.sprite = unMute;
        }
    }


    public void Mute()
    {

        if (SoundManager.Instance.BGMMute)
        {
            muteState = false;
            SoundManager.Instance.BGMOff();

        }
        else
        {
            muteState = true;
            SoundManager.Instance.BGMOn();
        }

        MuteImage();
    }


}
