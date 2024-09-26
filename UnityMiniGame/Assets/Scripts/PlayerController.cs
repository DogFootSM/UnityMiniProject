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

    //공격 코루틴 변수
    private WaitForSeconds attackWait;
    private float attackTimer = 1f;

    //데미지 코루틴 변수
    private WaitForSeconds hurtWait;
    private float hurtTimer = 1f;

    //애니메이터 hash
    private int idleHash;
    private int walkHash;
    private int runHash;
    private int attackHash; 
    private int hurtHash;
    private int deathHash;
    private int curAnimHash;

    private bool underAttack;

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

        attackWait = new WaitForSeconds(attackTimer);
        hurtWait = new WaitForSeconds(hurtTimer);

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
         
        playerStates[(int)curState].Update();

        Debug.DrawRay(transform.position, transform.right, Color.red);
         
    } 

    private void FixedUpdate()
    {
 
        playerStates[(int)curState].FixedUpdate();
    }
 
    public void ChangeState(State state)
    {
        //Exit
        playerStates[(int)curState].Exit();
        //상태 변경
        curState = state;
        //Enter
        playerStates[(int)curState].Enter();
    }


    public void InputKey()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }


 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Monster")
        {
            Debug.Log("몬스터랑 접촉");
            underAttack = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Monster")
        {
            underAttack = false;
        }

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
            player.attackHash = Animator.StringToHash("PlayerAttack"); 
            player.hurtHash = Animator.StringToHash("PlayerHurt");
            player.deathHash = Animator.StringToHash("PlayerDeath");
        } 
    }

    public class PlayerIdle : PlayerState
    {
        public PlayerIdle(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.animator.Play(player.idleHash);
        }

        public override void Update()
        {
            
            if (player.x != 0 || player.y != 0)
            {
                player.ChangeState(State.Move);
            } 

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.ChangeState(State.Attack); 
            }

            if (player.underAttack)
            {
                player.ChangeState(State.Hurt);
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
                player.ChangeState(State.Attack);
            }

            

            if (player.x < 0)
            {
                player.bodyRender.flipX = true;
                player.hairRender.flipX = true;
            }
            else if (player.x > 0)
            {
                player.bodyRender.flipX = false;
                player.hairRender.flipX = false;
            }
            
            if(player.x == 0 && player.y == 0)
            {
                player.ChangeState(State.Idle);
            }
            
            if (player.underAttack)
            {
                player.ChangeState(State.Hurt);
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
                    player.animator.Play(player.walkHash);
                    Walk();
                    break;

                case MoveState.Run: 
                    player.animator.Play(player.runHash);
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
        private Coroutine attackRoutine;


        public PlayerAttack(PlayerController player) : base(player) { }

        public override void Enter()
        {
             
            if(player.x > 0)
            {
                player.bodyRender.flipX = false;
                player.hairRender.flipX = false;
            }
            else if(player.x < 0)
            {
                player.bodyRender.flipX = true;
                player.hairRender.flipX = true;
            }

            //공격 타이머 코루틴
            attackRoutine = player.StartCoroutine(player.AttackCoroutine());

        }

        public override void Update()
        {
            if (player.underAttack)
            {
                player.ChangeState(State.Hurt);
            }

        }

        public override void Exit()
        {
            if(attackRoutine != null)
            {
                player.StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
        }


    }

    public class PlayerHurt : PlayerState
    {
        private Coroutine hurtRoutine;

        public PlayerHurt(PlayerController player) : base(player) { }

        public override void Enter()
        {

            hurtRoutine = player.StartCoroutine(player.HurtCoroutine());
            //HP 감소 로직

        }

        public override void Exit()
        {
            if(hurtRoutine != null)
            {
                player.StopCoroutine(hurtRoutine);
            }
             
        }

    } 

    public class PlayerDeath : PlayerState
    {
        public PlayerDeath(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.animator.Play(player.deathHash);

        }

    }

    private IEnumerator AttackCoroutine()
    {
        
        animator.Play(attackHash);
        //공격 로직 진행
        yield return attackWait;

        //Idle 전환
        ChangeState(State.Idle);
    }

    private IEnumerator HurtCoroutine()
    {
        animator.Play(hurtHash);

        yield return hurtWait;

        //HP가 1 이상일 때에는 idle
        ChangeState(State.Idle);

        //HP가 1 이하일 때에는 Death
        //ChangeState(State.Death);
    }

}
