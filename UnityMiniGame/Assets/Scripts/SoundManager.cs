using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource titleBGM;


     
    private bool muteState;
    public bool MuteState { get { return muteState; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
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
        UIManager.Instance.MuteImage();
    }

    


}
