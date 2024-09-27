using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] Transform target;

    Vector3 offset;

    private void Awake()
    {
        offset = new Vector3(0, 0, -10);
    }


    // Update is called once per frame
    void Update()
    {
        CamFollow();
         
    }

    public void CamFollow()
    {
        //카메라 이동 속도
        float followSpeed = 5.5f * Time.deltaTime;
        //카메라 따라갈 위치
        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);
    }



}
