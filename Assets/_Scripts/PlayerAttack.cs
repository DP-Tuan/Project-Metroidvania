using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack instance;
    public static PlayerAttack Instance { get => instance; }
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    public Transform attackPoint;
    public float attackRange = 1f;
    public int damage = 1;
    public LayerMask enemyLayer;
    public bool isAttacking;
    public Animator animator;
    public PlayerController player;

    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private float lastAttackTime;

    private void Awake()
    {
        PlayerAttack.instance = this;
        animator = GetComponent<Animator>();
        this.enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        this._rb = GetComponentInParent<Rigidbody2D>();
        this.player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        this.HandleInput();
    }

    public virtual void HandleInput()
    {
        if (!Input.GetKeyDown(KeyCode.J)) return;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        TryAttack();
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        player.canMove = false;

        PushForward();
        Attack();

        yield return new WaitForSeconds(attackCooldown);

        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        player.canMove = true;
        isAttacking = false;
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {

            this.isAttacking = true;
            Debug.Log("Is Attacking");
            InputManager.Instance.canReceiveAttackInput = true;
            InputManager.Instance.AttackPressing();
            StartCoroutine(AttackCoroutine());

            lastAttackTime = Time.time;
        }
        isAttacking = false;
    }

    public virtual void Attack()
    {
        isAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponentInParent<IDamageable>();
            damageable?.TakeDamage(damage);
            Debug.Log("ENEMY: " + enemy);
        }
    }


    public float pushForce = 1.5f; public void PushForward()
    {
        int direction = (int)Mathf.Sign(transform.parent.localScale.x);

        if (player.PlayerMovementHandler._isFacingRight != true)
        {
            direction *= -1;
        }

        Vector2 pushVelocity = new Vector2(direction * pushForce, _rb.linearVelocity.y);
        _rb.linearVelocity = pushVelocity;

    }

    public void PlayFootStep()
    {
        AudioManager.instance.PlayFootStep();
    }

    public void CameraShake()
    {
        CameraSmoothDamp.instance.Shake(0.2f, 0.05f);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

}
