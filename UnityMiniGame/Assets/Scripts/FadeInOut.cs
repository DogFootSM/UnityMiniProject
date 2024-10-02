using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private GameObject fadePanel;


    public Coroutine fadeIn;
    public Coroutine fadeOut;
    private Action OnAfterFade;

    private bool isFadeIn;
    public bool IsFadeIn { get { return isFadeIn; } }


    public void FadeIn()
    {
        if(!isFadeIn && fadeOut != null)
        { 
            StopCoroutine(fadeOut);
        }

        fadeIn = StartCoroutine(FadeInCoroutine());
 
        if (isFadeIn && fadeIn != null)
        { 
            StopCoroutine(fadeIn);
        }
    }
 
    public void FadeOut()
    {
        fadePanel.SetActive(true);

        fadeOut = StartCoroutine(FadeOutCoroutine()); 
    }

    public IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        float fadeInTime = 0.8f;

        while(elapsedTime < fadeInTime)
        { 
            fadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        fadePanel.SetActive(false); 
        isFadeIn = true;
        yield break;
    }
     
    public IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        float fadeOutTime = 0.5f;

        while(elapsedTime <= fadeOutTime)
        {
            fadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadeOutTime));
            
            elapsedTime += Time.deltaTime;

            yield return null;

        } 

        isFadeIn = false;
        OnAfterFade?.Invoke();
        yield break;
    }

    //¾À ÀÌµ¿ ¾×¼Ç
    public void SceneChange(Action action)
    {
        OnAfterFade = action;
         
    }

}
