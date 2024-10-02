using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] public AudioClip[] bgmList = new AudioClip[2];

    private bool bgmMute;
    public bool BGMMute { get { return bgmMute; } set { bgmMute = value; } }

    private void Awake()
    {
        

        if(Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
         
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);

    }
    
    public void ChangeBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void BGMOff()
    {
        bgmSource.mute = false;
        bgmMute = false;
    }

    public void BGMOn()
    {
        bgmSource.mute = true;
        bgmMute = true;
    }



}
