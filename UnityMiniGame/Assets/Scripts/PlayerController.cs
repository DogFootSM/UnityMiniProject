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


    private enum State { Idle, Move, Attack, Hurt, Death, SIZE}

    //���� ����
    private State curState = State.Idle;

    //�÷��̾� ���� ���� �迭
    private PlayerState[] playerStates = new PlayerState[(int)State.SIZE];
    
    private Rigidbody2D rb;
    private Animator animator;

    //�ִϸ����� hash
    private int idleHash;
    private int walkHash;
    private int runHash;
    private int attackHash;
    private int hurtHash;
    private int deathHash;
    private int curAnimHash;


    //x �� �̵� ����
    private float x;
    //y �� �̵� ����
    private float y;

    //�÷��̾� ���� ���
    private int gold;
    //�÷��̾� Ȱ�� ������
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
        Debug.Log($"���� ���� : {curState}"); 
        playerStates[(int)curState].Update();
    }



    private void FixedUpdate()
    {
        animator.Play(curAnimHash);
        playerStates[(int)curState].FixedUpdate();
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
            player.attackHash = Animator.StringToHash("PlayerHurt");
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
            

            if(player.x < 0)
            {
                //���, �ٵ� FlipX = True;
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
                player.curState = State.Idle;
            }


        }

        public override void FixedUpdate()
        {
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
            //�Ȱ� ���� �� ������ �Ҹ�

            xMove = player.x;
            yMove = player.y;

            normal = new Vector2(xMove, yMove);

            if (normal.sqrMagnitude > 1)
            {
                normal.Normalize();
            }

            player.rb.MovePosition(player.rb.position + normal * player.walkSpeed * Time.fixedDeltaTime);

            //velocity �̵� Rigidbody.MovePosition���� ���� �ʿ�
            //player.rb.velocity = normal * player.walkSpeed;
        }

        private void Run()
        {
            //�ٴ� ������ �� ������ ������ �Ҹ�

            xMove = player.x;
            yMove = player.y;

            normal = new Vector2(xMove, yMove);

            if (normal.sqrMagnitude > 1)
            {
                normal.Normalize();
            }

            player.rb.MovePosition(player.rb.position+normal * player.runSpeed * Time.fixedDeltaTime);
            //player.rb.velocity = normal * player.runSpeed; 
        }


    }
  
    public class PlayerAttack : PlayerState
    {
        public PlayerAttack(PlayerController player) : base(player) { }
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
