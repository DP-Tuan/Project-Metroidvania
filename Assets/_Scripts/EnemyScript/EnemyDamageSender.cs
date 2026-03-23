using UnityEngine;

public class EnemyDamageSender : MonoBehaviour
{
    [SerializeField] protected float attackCoolDown = 4f;
    [SerializeField] protected float timer = 0;
    [SerializeField] protected Transform attPos;
    public Transform AttPos { get => attPos; }
    [SerializeField] protected float attackRange = 0.5f;
    public float AttackRange { get => attackRange; }
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Animator _animation;

    protected EnemyBehaviour _enemyController;

    private void Awake()
    {


        this._enemyController = GetComponentInParent<EnemyBehaviour>();
        this._animation = _enemyController.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        this.attPos = GameObject.Find("AttackPoint").transform;
    }

    private void FixedUpdate()
    {

        this.Attack();
    }

    protected virtual void Attack()
    {
        if (!(_enemyController.currentState == EnemyBehaviour.State.Chase)) return;
        timer += Time.fixedDeltaTime;
        if (timer < attackCoolDown) return;
        _enemyController.isWaitingAttack = false;
        if (!this._enemyController.facingRight)
        {
            transform.localScale = new Vector3(this._enemyController.facingRight ? -1 : 1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(this._enemyController.facingRight ? 1 : -1, 1, 1);
        }
        Collider2D hit = Physics2D.OverlapCircle(this.attPos.position, this.attackRange, playerLayer);

        if (hit == null) return;
        timer = 0;
        this._animation.SetTrigger("Attack");
        _enemyController.isAttacking = true;
        _enemyController.isWaitingAttack = true;
        this._enemyController.rb.linearVelocity = Vector2.zero;
        PlayerDamageReceiver.instance.TakeDamage(1f);
    }

    private void OnDrawGizmos()
    {
        if (this.attPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.attPos.position, this.attackRange);
        }
    }
}
