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


    //�������� �۹� ����
    [SerializeField] private Crop cropSeed;
    [SerializeField] private Inventory inventory;



    public enum State { Idle, Move, Attack, Hurt, Death, Water, SIZE }

    //�տ� ��� �ִ� ���� ����
    public enum FarmTool { Knife, Axe, Hammer, Shovels, Pickax, Rod, Sprayer, SIZE }


    //���� �÷��̾� ����
    private State curState = State.Idle;

    //���� ��� �ִ� ���� ����
    private FarmTool farmTool = FarmTool.Knife;

    //�÷��̾� ���� ���� �迭
    private PlayerState[] playerStates = new PlayerState[(int)State.SIZE];

    private Rigidbody2D rb;
    private Animator animator;

    //���� �ڷ�ƾ ����
    private WaitForSeconds attackWait;
    private float attackTimer = 1f;

    //������ �ڷ�ƾ ����
    private WaitForSeconds hurtWait;
    private float hurtTimer = 1f;

    //���ֱ� �ڷ�ƾ ����
    private WaitForSeconds waterWait;
    private float wateringTimer = 1.5f;


    RaycastHit2D hit;
    private LayerMask cropLayerMask;
    private Vector2 rayDir;

    //�ִϸ����� hash
    private int idleHash;
    private int walkHash;
    private int runHash;
    private int attackHash;
    private int hurtHash;
    private int deathHash;
    private int waterhHash;

    //�ǰ��� ����
    private bool underAttack;

    //���� �� ����
    private bool isWatering;


    //x �� �̵� ����
    private float x;
    //y �� �̵� ����
    private float y;

    //�÷��̾� ���� ���
    private int gold;

    //�÷��̾� Ȱ�� ������
    private float energy = 100;
    public float Energy { get { return energy; } set { energy = value; } }

    //�÷��̾� ü��
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

        //�۹� ������ ���̾��ũ
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

        //������ ���� ����
        if (x > 0)
        {
            rayDir = transform.right;
        }
        else if (x < 0)
        {
            rayDir = -transform.right;
        }

        //���۹�, ���� Ȯ�� ����
        Debug.DrawRay(transform.position, rayDir * 1.5f, Color.red);
        hit = Physics2D.Raycast(transform.position, rayDir, 1.5f, cropLayerMask);
 
        //�κ��丮 ��Ȱ��ȭ ���¿����� ������ ����
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
        //���� ����
        curState = state;
        //Enter
        playerStates[(int)curState].Enter();
    }


    public void InputKey()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }


    //�������� ������ ���� �����ִ� �����϶� �ɴ� ���
    public void Plant()
    {

    }


    //���Ϳ� �浹 ó��
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

                    //��Ȯ ���� ���°� �ƴ� ���¿��� ���۹��� �� �ֱ�
                    if (!player.cropSeed.IsHarvestable && Input.GetKeyDown(KeyCode.V))
                    {
                        //���� ���� ���� ���¿����� �� �ֱ�
                        if (!player.cropSeed.IsWatering)
                        {
                            player.cropSeed.IsWatering = true;
                            player.ChangeState(State.Water);
                        }

                    }
                    //��Ȯ ���� ���¿��� ���콺 Ŭ�� �� ��Ȯ
                    else if (player.cropSeed.IsHarvestable)
                    {
                        if(player.cropSeed.OnMouse && Input.GetMouseButtonDown(0))
                        {
                            //���۹� ��Ȯ �� �κ��丮 �߰� 
                            //player.inventory.AddItem(player.cropSeed.Harvest());

                            player.ChangeState(State.Idle);

                            //��Ȯ �� ������Ʈ ����
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

            //�ȱ�, �޸��� ���� ��ȯ
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                curMoveState = MoveState.Walk;
                //�ȴ� �ð��� ������ �Ҹ�
                player.energy -= Time.deltaTime * 0.1f;
                UIManager.Instance.EnergyBarUpdate(player.energy);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                curMoveState = MoveState.Run;
                //�ٴ� �ð��� ������ �Ҹ�
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

            //������Ʈ(����, �� ��), Monster ���̾� Ȯ�� �� ���̾ �°� ����, ��Ȯ ���� 
            player.animator.Play(player.attackHash);






            //���� Ÿ�̸� �ڷ�ƾ
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
            //HP ���� ����

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

        //���� ���� ����
        yield return attackWait;

        //Idle ��ȯ
        ChangeState(State.Idle);
    }

    private IEnumerator HurtCoroutine()
    {

        yield return hurtWait;

        //HP�� 1 �̻��� ������ idle
        ChangeState(State.Idle);

        //HP�� 1 ������ ������ Death
        //ChangeState(State.Death);
    }

    private IEnumerator WateringCoroutine()
    {
        //�� �ֱ� ��� ���� 
        Debug.Log("���� ����ϴ�.");
        cropSeed.Grow();
        yield return waterWait;

        ChangeState(State.Idle);
    }


}
