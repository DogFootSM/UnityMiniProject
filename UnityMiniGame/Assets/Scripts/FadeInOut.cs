using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private GameObject fadePanel;
    public bool isFadeIn;

    private Action OnAfterFade;

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }
 
    public void FadeOut()
    {
        fadePanel.SetActive(true);

        StartCoroutine(FadeOutCoroutine()); 
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
        
        OnAfterFade?.Invoke();
        yield break;
    }

    //¾À ÀÌµ¿ ¾×¼Ç
    public void SceneChange(Action action)
    {
        OnAfterFade = action;
         
    }

}
