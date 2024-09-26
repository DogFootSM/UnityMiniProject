using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private SpriteRenderer bodyRender;
    [SerializeField] private SpriteRenderer hairRender;


    public enum State { Idle, Move, Attack, Hurt, Death, SIZE}

    //현재 상태
    private State curState = State.Idle;

    //플레이어 현재 상태 배열
    private PlayerState[] playerStates = new PlayerState[(int)State.SIZE];
    
    private Rigidbody2D rb;
    private Animator animator;

    //애니메이터 hash
    private int idleHash;
    private int walkHash;
    private int runHash;
    private int rightAttackHash;
    private int leftAttackHash; 
    private int hurtHash;
    private int deathHash;
    private int curAnimHash;

    private bool isAttack;

    //x 축 이동 변수
    private float x;
    //y 축 이동 변수
    private float y;

    //플레이어 소지 골드
    private int gold;
    //플레이어 활동 에너지
    private float energy;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerStates[(int)State.Idle] = new PlayerIdle(this);
        playerStates[(int)State.Move] = new PlayerMove(this);
        playerStates[(int)State.Attack] = new PlayerAttack(this);
        playerStates[(int)State.Hurt] = new PlayerHurt(this);
        playerStates[(int)State.Death] = new PlayerDeath(this);
         
    }

    private void Start()
    {
        playerStates[(int)curState].Enter();
    }

    private void Update()
    {
        InputKey();
        Debug.Log($"현재 상태 : {curState}"); 
        Debug.Log($"x : {x}");

        playerStates[(int)curState].Update();

        Debug.DrawRay(transform.position, transform.right, Color.red);

        animator.Play(curAnimHash);


    } 

    private void FixedUpdate()
    {
        
        playerStates[(int)curState].FixedUpdate();
    }

    //ChangeState() 메서드 구현
    //이전 상태 종료 > 현재 상태 재생
    //Update에서 상태를 바로 전환하지 않고
    //어택은 코루틴 사용해서 딜레이를 주면서 전환
    public void ChangeState(State state)
    {
        //Exit
        //상태 변경
        //Enter
    }


    public void InputKey()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }
 

    public class PlayerState : MachineState
    {   
        protected PlayerController player;

        public PlayerState(PlayerController player)
        {
            this.player = player;
 
        }

        public override void Enter()
        { 
            player.idleHash = Animator.StringToHash("PlayerIdle");
            player.walkHash = Animator.StringToHash("PlayerWalk");
            player.runHash = Animator.StringToHash("PlayerRun");
            player.rightAttackHash = Animator.StringToHash("RightPlayerAttack");
            player.leftAttackHash = Animator.StringToHash("LeftPlayerAttack");
            player.hurtHash = Animator.StringToHash("PlayerHurt");
            player.deathHash = Animator.StringToHash("PlayerDeath"); 
        } 
    }

    public class PlayerIdle : PlayerState
    {
        public PlayerIdle(PlayerController player) : base(player) { }

        public override void Update()
        {

            player.curAnimHash = player.idleHash; 

            if (player.x != 0 || player.y != 0)
            {
                player.curState = State.Move;
            } 

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.curState = State.Attack;
            }

        }

    }

    public class PlayerMove : PlayerState
    {
        private enum MoveState {Walk, Run}
        private MoveState curMoveState = MoveState.Walk;
        private float xMove;
        private float yMove;
        private Vector2 normal;



        public PlayerMove(PlayerController player) : base(player) { }

        public override void Update()
        { 

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                curMoveState = MoveState.Walk;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                curMoveState = MoveState.Run;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.curState = State.Attack;
            }


            //Enter
            if(player.x < 0)
            {
                Debug.Log("flip true"); 
                player.bodyRender.flipX = true;
                player.hairRender.flipX = true;
            }
            else if (player.x > 0)
            {
                Debug.Log("flip false");
                player.bodyRender.flipX = false;
                player.hairRender.flipX = false;
            }
            
            if(player.x == 0 && player.y == 0)
            {
                player.curState = State.Idle;
            }


        }

        public override void FixedUpdate()
        {
            xMove = player.x;
            yMove = player.y;

            normal = new Vector2(xMove, yMove);

            if (normal.sqrMagnitude > 1)
            {
                normal.Normalize();
            }

            switch (curMoveState)
            {
                case MoveState.Walk:
                    player.curAnimHash = player.walkHash;
                    Walk();
                    break;

                case MoveState.Run:
                    player.curAnimHash = player.runHash;
                    Run();
                    break;
            }
        }
         
        private void Walk()
        {
            //걷고 있을 때 에너지 소모 추가 필요
             
            player.rb.MovePosition(player.rb.position + normal * player.walkSpeed * Time.fixedDeltaTime);
 
        }

        private void Run()
        {
            //뛰는 상태일 때 에너지 빠르게 소모 추가 필요

            player.rb.MovePosition(player.rb.position+normal * player.runSpeed * Time.fixedDeltaTime);
 
        }


    }
  
    public class PlayerAttack : PlayerState
    {
        private float attackTimer;
        private Coroutine attackRoutine;

        public PlayerAttack(PlayerController player) : base(player) { }

        public override void Enter()
        {
            

        }

        public override void Update()
        {
            
            if(player.x > 0)
            {
                player.curAnimHash = player.rightAttackHash;
                //공격 효과 적용
                
            }
            else if(player.x < 0)
            {
                player.curAnimHash = player.leftAttackHash;
                //공격 효과 적용

            }

            //공격이 끝났다면 Idle 상태로 전환

             
             
        }

        private IEnumerator AttackCoroutine()
        {
            //어택 진행
            yield return null;  

            //Idle 전환

        }
    }

    public class PlayerHurt : PlayerState
    {
        public PlayerHurt(PlayerController player) : base(player) { }
    }

    public class PlayerDeath : PlayerState
    {
        public PlayerDeath(PlayerController player) : base(player) { }
    }

 

}
