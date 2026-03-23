using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerCollisionSensor PlayerCollisionSensor { get; private set; }
    public PlayerMovementHandler PlayerMovementHandler { get; private set; }
    public AttackHandler AttackHandler { get; private set; }

    public IdleState IdleState { get; private set; }
    public RunState RunState { get; private set; }
    public JumpState JumpState { get; private set; }
    public FallState FallState { get; private set; }
    public AttackState AttackState { get; private set; }

    public PlayerMovementStats MoveStats;
    public SpriteRenderer sprite;
    public Rigidbody2D _rb;
    public Collider2D _col;
    [SerializeField] public Animator _animation;

    public bool canMove;

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    public Transform attackPoint;
    public float attackRange = 1f;
    public int damage = 1;
    public LayerMask enemyLayer;
    public float pushForce = 1.5f;
    [HideInInspector] public float lastAttackTime;

    private void Awake()
    {
        _instance = this;
        StateMachine = new PlayerStateMachine();
        PlayerMovementHandler = new PlayerMovementHandler();
        PlayerCollisionSensor = new PlayerCollisionSensor();
        AttackHandler = new AttackHandler(this, _rb, _animation);

        IdleState = new IdleState(this, StateMachine);
        RunState = new RunState(this, StateMachine);
        JumpState = new JumpState(this, StateMachine);
        FallState = new FallState(this, StateMachine);
        AttackState = new AttackState(this, StateMachine);

        _rb = GetComponent<Rigidbody2D>();
        this._col = GetComponentInChildren<Collider2D>();
        this.sprite = GetComponentInChildren<SpriteRenderer>();
        this._animation = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StateMachine.ChangeState(IdleState);
        PlayerMovementHandler._isFacingRight = true;
        canMove = true;
    }

    private void Update()
    {
        PlayerCollisionSensor.CollisionChecks();
        StateMachine.Update();
        PlayerMovementHandler.JumpChecks();
    }

    private void FixedUpdate()
    {

        StateMachine.UpdatePhysics();
        PlayerMovementHandler.Jump();
    }

}
