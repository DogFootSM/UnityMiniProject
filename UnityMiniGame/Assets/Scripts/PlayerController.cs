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
    [SerializeField] private SpriteRenderer toolRender;


    //보유중인 작물 씨앗
    [SerializeField] private Crop cropSeed;
    [SerializeField] private Inventory inventory;



    public enum State { Idle, Move, Attack, Hurt, Death, Water, SIZE }

    //손에 들고 있는 도구 상태
    public enum FarmTool { Knife, Axe, Hammer, Shovels, Pickax, Rod, Sprayer, SIZE }


    //현재 플레이어 상태
    private State curState = State.Idle;

    //현재 들고 있는 도구 상태
    private FarmTool farmTool = FarmTool.Knife;

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

    //물주기 코루틴 변수
    private WaitForSeconds waterWait;
    private float wateringTimer = 1.5f;


    RaycastHit2D hit;
    private LayerMask cropLayerMask;
    private Vector2 rayDir;

    //애니메이터 hash
    private int idleHash;
    private int walkHash;
    private int runHash;
    private int attackHash;
    private int hurtHash;
    private int deathHash;
    private int waterhHash;

    //피격중 상태
    private bool underAttack;

    //물을 준 상태
    private bool isWatering;


    //x 축 이동 변수
    private float x;
    //y 축 이동 변수
    private float y;

    //플레이어 소지 골드
    private int gold;

    //플레이어 활동 에너지
    private float energy = 100;
    public float Energy { get { return energy; } set { energy = value; } }

    //플레이어 체력
    private int hp;
    public int HP { get { return hp; } set { hp = value; } }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackWait = new WaitForSeconds(attackTimer);
        hurtWait = new WaitForSeconds(hurtTimer);
        waterWait = new WaitForSeconds(wateringTimer);

        playerStates[(int)State.Idle] = new PlayerIdle(this);
        playerStates[(int)State.Move] = new PlayerMove(this);
        playerStates[(int)State.Attack] = new PlayerAttack(this);
        playerStates[(int)State.Hurt] = new PlayerHurt(this);
        playerStates[(int)State.Death] = new PlayerDeath(this);
        playerStates[(int)State.Water] = new PlayerWater(this);

        //작물 감지할 레이어마스크
        cropLayerMask = 1 << LayerMask.NameToLayer("Crop");
        //objectMask = (1 << LayerMask.NameToLayer("Object")) + (1 << LayerMask.NameToLayer("Monster"));

    }

    private void Start()
    {
        playerStates[(int)curState].Enter();
    }




    private void Update()
    {
         
        playerStates[(int)curState].Update();

        //레이의 방향 조정
        if (x > 0)
        {
            rayDir = transform.right;
        }
        else if (x < 0)
        {
            rayDir = -transform.right;
        }

        //농작물, 몬스터 확인 레이
        Debug.DrawRay(transform.position, rayDir * 1.5f, Color.red);
        hit = Physics2D.Raycast(transform.position, rayDir, 1.5f, cropLayerMask);
 
        //인벤토리 비활성화 상태에서만 움직임 가능
        if (!Inventory.isInventoryActive)
        {
            InputKey();
             
        }
        else
        {
            ChangeState(State.Idle);
        }



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


    //보유중인 씨앗을 땅이 파져있는 상태일때 심는 기능
    public void Plant()
    {

    }


    //몬스터와 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            underAttack = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
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
            player.waterhHash = Animator.StringToHash("PlayerWater");
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

            if ((player.x != 0 || player.y != 0))
            {
                player.ChangeState(State.Move);
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.ChangeState(State.Attack);
            }
            else if (player.underAttack)
            {
                player.ChangeState(State.Hurt);
            }


            if (player.hit.collider != null)
            {
                Debug.Log(player.hit.collider.gameObject.tag);
                if (player.hit.collider.gameObject.tag == "Crop")
                {
                    player.cropSeed = player.hit.collider.gameObject.GetComponent<Crop>();

                    //수확 가능 상태가 아닌 상태에서 농작물에 물 주기
                    if (!player.cropSeed.IsHarvestable && Input.GetKeyDown(KeyCode.V))
                    {
                        //물을 주지 않은 상태에서만 물 주기
                        if (!player.cropSeed.IsWatering)
                        {
                            player.cropSeed.IsWatering = true;
                            player.ChangeState(State.Water);
                        }

                    }
                    //수확 가능 상태에서 마우스 클릭 시 수확
                    else if (player.cropSeed.IsHarvestable)
                    {
                        if(player.cropSeed.OnMouse && Input.GetMouseButtonDown(0))
                        {
                            //농작물 수확 후 인벤토리 추가 
                            //player.inventory.AddItem(player.cropSeed.Harvest());

                            player.ChangeState(State.Idle);

                            //수확 후 오브젝트 삭제
                            Destroy(player.cropSeed.gameObject);

                        } 
                    }
                }
                else
                {
                    player.cropSeed = null;
                }
            }
            else
            {
                player.cropSeed = null;
            }

        }

    }

    public class PlayerMove : PlayerState
    {
        private enum MoveState { Walk, Run }
        private MoveState curMoveState = MoveState.Walk;
        private float xMove;
        private float yMove;
        private Vector2 normal;



        public PlayerMove(PlayerController player) : base(player) { }


        public override void Update()
        {
            if (player.x == 0 && player.y == 0)
            {
                player.ChangeState(State.Idle);
            }

            //걷기, 달리기 상태 전환
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                curMoveState = MoveState.Walk;
                //걷는 시간당 에너지 소모
                player.energy -= Time.deltaTime * 0.1f;
                UIManager.Instance.EnergyBarUpdate(player.energy);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                curMoveState = MoveState.Run;
                //뛰는 시간당 에너지 소모
                player.energy -= Time.deltaTime * 0.3f;
                UIManager.Instance.EnergyBarUpdate(player.energy);
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.ChangeState(State.Attack);
            }

            if (player.x < 0)
            {
                player.bodyRender.flipX = true;
                player.hairRender.flipX = true;
                player.toolRender.flipX = true;
            }
            else if (player.x > 0)
            {
                player.bodyRender.flipX = false;
                player.hairRender.flipX = false;
                player.toolRender.flipX = false;
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
            player.rb.MovePosition(player.rb.position + normal * player.walkSpeed * Time.fixedDeltaTime);

        }

        private void Run()
        {
            player.rb.MovePosition(player.rb.position + normal * player.runSpeed * Time.fixedDeltaTime);
        }


    }



    public class PlayerAttack : PlayerState
    {
        private Coroutine attackRoutine;


        public PlayerAttack(PlayerController player) : base(player) { }

        public override void Enter()
        {

            if (player.x > 0)
            {
                player.bodyRender.flipX = false;
                player.hairRender.flipX = false;
            }
            else if (player.x < 0)
            {
                player.bodyRender.flipX = true;
                player.hairRender.flipX = true;
            }

            //오브젝트(나무, 돌 등), Monster 레이어 확인 후 레이어에 맞게 공격, 수확 진행 
            player.animator.Play(player.attackHash);






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
            if (attackRoutine != null)
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
            player.animator.Play(player.hurtHash);
            hurtRoutine = player.StartCoroutine(player.HurtCoroutine());
            //HP 감소 로직

        }

        public override void Exit()
        {
            if (hurtRoutine != null)
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

    public class PlayerWater : PlayerState
    {
        private Coroutine wateringRoutine;

        public PlayerWater(PlayerController player) : base(player) { }

        public override void Enter()
        {

            player.animator.Play(player.waterhHash);
            wateringRoutine = player.StartCoroutine(player.WateringCoroutine());


        }

        public override void Exit()
        {
            if (wateringRoutine != null)
            {
                player.StopCoroutine(wateringRoutine);
            }
        }

    }

    private IEnumerator AttackCoroutine()
    {

        //공격 로직 진행
        yield return attackWait;

        //Idle 전환
        ChangeState(State.Idle);
    }

    private IEnumerator HurtCoroutine()
    {

        yield return hurtWait;

        //HP가 1 이상일 때에는 idle
        ChangeState(State.Idle);

        //HP가 1 이하일 때에는 Death
        //ChangeState(State.Death);
    }

    private IEnumerator WateringCoroutine()
    {
        //물 주기 기능 동작 
        Debug.Log("물을 줬습니다.");
        cropSeed.Grow();
        yield return waterWait;

        ChangeState(State.Idle);
    }


}
