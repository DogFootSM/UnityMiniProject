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

    [SerializeField] private AudioSource titleBGM;
    private bool muteState;

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

        if (titleBGM.mute)
        {
            muteState = false;
            titleBGM.mute = muteState;
        }
        else
        {
            muteState = true;
            titleBGM.mute = muteState;
        }

        MuteImage();
    }


}
