using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private int hp = 3;


    private void Update()
    {
        Debug.Log($"���� ü�� : {hp}");

        if(hp < 1)
        {
            Destroy(gameObject);
        }

    }



}
