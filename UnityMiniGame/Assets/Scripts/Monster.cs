using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private int hp = 3;


    private void Update()
    {
        Debug.Log($"몬스터 체력 : {hp}");

        if(hp < 1)
        {
            Destroy(gameObject);
        }

    }



}
