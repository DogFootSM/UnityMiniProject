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
    [SerializeField] private Slider hpBar;

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
 
 

}
