using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public EnemyStateMachine enemyStateMachine;
    public readonly EnemyPatrolState patrolState = new EnemyPatrolState();
    public readonly EnemyChaseState chaseState = new EnemyChaseState();
    public readonly EnemyIdleState idleState = new EnemyIdleState();
    public readonly EnemyReturnHomeState returnHomeState = new EnemyReturnHomeState();
    public readonly EnemyAttackState attackState = new EnemyAttackState();
    public readonly EnemyHurtState hurtState = new EnemyHurtState();
    public readonly EnemyDeadState deadState = new EnemyDeadState();

    [Header("General")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Detection")]
    public Transform player;
    public PlayerAttack playerAttack;
    public float detectionRange = 5f;
    public float chaseRange = 8f;

    [Header("Patrol")]
    public float checkDistance = 0.3f;
    public float waitMin = 0.5f;
    public float waitMax = 2f;
    public float distancePatrol;
    public LayerMask groundLayer;

    [Header("Attack")]
    [SerializeField] public Transform attPos;
    [SerializeField] public float attackRange = 0.5f;
    [SerializeField] public float attackCoolDown = 4f;
    [SerializeField] public float attackDamage = 5f;
    [SerializeField] public LayerMask playerLayer;
    [HideInInspector] public float attackTimer = 0f;

    [Header("Combat")]
    public float maxHealth = 5;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private Coroutine flashRoutine;
    [HideInInspector] public float currentHealth;

    [Header("Drop")]
    public GameObject coinPrefab;
    public int coinAmount = 1;


    [HideInInspector] public Vector2 homePatrol;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool facingRight = true;

    public GameObject floatingText;

    public static EnemyController Instance;
    private void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").transform;
        playerAttack = player.GetComponent<PlayerAttack>();
        homePatrol = transform.position;

        currentHealth = maxHealth;
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            defaultMaterial = spriteRenderer.material;

        enemyStateMachine = new EnemyStateMachine(this);
        enemyStateMachine.ChangeState(patrolState);
    }

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;
        enemyStateMachine.CurrentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        enemyStateMachine.CurrentState.ExitState(this);
        enemyStateMachine.ChangeState(newState);
        newState.EnterState(this);
    }

    #region Helper Methods (Các hàm h? tr?)

    public float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    public bool CanSeePlayer()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit;
        float distToPlayer = DistanceToPlayer();

        if (enemyStateMachine.CurrentState == chaseState)
        {
            hit = Physics2D.Raycast(transform.position, dirToPlayer, distToPlayer, groundLayer);
            Debug.DrawRay(transform.position, dirToPlayer * distToPlayer, hit.collider == null ? Color.green : Color.red);
        }

        else if ((facingRight && dirToPlayer.x < 0) || (!facingRight && dirToPlayer.x > 0))
        {
            return false;
        }

        hit = Physics2D.Raycast(transform.position, dirToPlayer, distToPlayer, groundLayer);
        Debug.DrawRay(transform.position, dirToPlayer * distToPlayer, hit.collider == null ? Color.green : Color.red);

        return hit.collider == null;
    }

    public bool CheckWall()
    {
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;
        Vector2 wallOrigin = (Vector2)transform.position + Vector2.up * 0.1f;
        return Physics2D.Raycast(wallOrigin, dir, checkDistance, groundLayer);
    }

    public bool CheckGroundAhead()
    {
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;
        Vector2 groundOrigin = (Vector2)transform.position + dir * 0.3f;
        return Physics2D.Raycast(groundOrigin, Vector2.down, checkDistance, groundLayer);
    }


    public void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
        float offset = 0.1f;
        transform.position += facingRight ? Vector3.right * offset : Vector3.left * offset;
    }

    public void DropCoin()
    {
        if (coinPrefab == null) return;

        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rbCoin = coin.GetComponent<Rigidbody2D>();
            if (rbCoin != null)
            {
                float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                float speed = Random.Range(5f, 15f);
                Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                rbCoin.linearVelocity = velocity;
                rbCoin.gravityScale = 1f;
            }
        }
    }
    #endregion

    public virtual void OnAttackEvent()
    {
        Collider2D hit = Physics2D.OverlapCircle(this.attPos.position, this.attackRange, playerLayer);
        if (hit != null)
        {
            Debug.Log("Hit Player!");
            PlayerDamageReceiver.instance.TakeDamage(attackDamage);
        }
    }

    public virtual void OnAttackFinished()
    {
        if (CanSeePlayer() && DistanceToPlayer() <= chaseRange)
        {
            SwitchState(chaseState);
        }
        else
        {
            SwitchState(idleState);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        if (this.attPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.attPos.position, this.attackRange);
        }

        Gizmos.color = Color.magenta;
        float patrolLeftX = homePatrol.x - distancePatrol;
        float patrolRightX = homePatrol.x + distancePatrol;
        float patrolY = homePatrol.y;

        Vector3 leftPoint = new Vector3(patrolLeftX, patrolY);
        Vector3 rightPoint = new Vector3(patrolRightX, patrolY);

        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawLine(leftPoint - Vector3.up * 0.5f, leftPoint + Vector3.up * 0.5f);
        Gizmos.DrawLine(rightPoint - Vector3.up * 0.5f, rightPoint + Vector3.up * 0.5f);

        Vector3 dir = facingRight ? Vector3.right : Vector3.left;

        Gizmos.color = Color.blue;
        Vector3 wallOrigin = (Vector3)transform.position + Vector3.up * 0.1f;
        Gizmos.DrawLine(wallOrigin, wallOrigin + dir * checkDistance);

        Gizmos.color = Color.cyan;
        Vector3 groundOrigin = (Vector3)transform.position + dir * 0.3f;
        Gizmos.DrawLine(groundOrigin, groundOrigin + Vector3.down * checkDistance);
    }

    public void TakeDamage(float amount)
    {
        if (enemyStateMachine.CurrentState == deadState) return;

        currentHealth -= amount;

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(Flash());

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
        else
        {
            HandleHurt();
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defaultMaterial;
    }

    public void HandleHurt()
    {
        if (enemyStateMachine.CurrentState == deadState) return;

        SwitchState(hurtState);
    }

    public void HandleDeath()
    {
        if (enemyStateMachine.CurrentState == deadState) return;
        DropCoin();
        SwitchState(deadState);
    }

    public void OnHurtFinished()
    {
        if (CanSeePlayer() && DistanceToPlayer() <= chaseRange)
        {
            SwitchState(chaseState);
        }
        else
        {
            SwitchState(idleState);
        }
    }

    public void ShowTextFloating()
    {
        var go = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        var textScript = go.GetComponent<FloatingText>();

        if (textScript != null)
        {
            if (playerAttack != null)
            {
                Debug.LogWarning("player Attack to floating text");
                textScript.SetText("-" + playerAttack.damage.ToString());
            }
            else
            {
                textScript.SetText("-1");
            }
        }
        else
        {
            Debug.LogError("L?I: Prefab FloatingText chýa g?n script 'FloatingText'!");
        }
    }

    public void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }
}
