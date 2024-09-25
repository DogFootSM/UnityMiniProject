using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    private enum State { Idle, Walk, Run, Attack, Hurt, Death, SIZE}

    //현재 상태
    private State curState;




    private Rigidbody2D rb;

    private float x;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    


    public class PlayerState : MachineState
    {
        protected PlayerController player;



    }

    


}
