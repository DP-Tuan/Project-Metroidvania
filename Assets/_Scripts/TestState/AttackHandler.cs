using UnityEngine;

public class AttackHandler
{
    private PlayerController player;
    private Rigidbody2D rb;
    private Animator anim;

    public float lastAttackTime { get; private set; }

    public AttackHandler(PlayerController player, Rigidbody2D rb, Animator anim)
    {
        this.player = player;
        this.rb = rb;
        this.anim = anim;
        this.lastAttackTime = -player.attackCooldown;
    }

    public void CheckForAttackInput()
    {
        if (Time.time < lastAttackTime + player.attackCooldown) return;

        if (player.StateMachine.CurrentState == player.AttackState) return;


        player.StateMachine.ChangeState(player.AttackState);
    }

    public void ExecuteAttack()
    {
        lastAttackTime = Time.time;


        PerformPushForward();

        PerformDamageScan();
    }

    private void PerformDamageScan()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            player.attackPoint.position,
            player.attackRange,
            player.enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInParent<EnemyController>()?.TakeDamage(player.damage);
            Debug.Log("ENEMY: " + enemy);
        }
    }

    private void PerformPushForward()
    {
        int direction = player.PlayerMovementHandler._isFacingRight ? 1 : -1;
        Vector2 pushVelocity = new Vector2(direction * player.pushForce, rb.linearVelocity.y);
        rb.linearVelocity = pushVelocity;
    }

    public void OnAttackFinished()
    {
        player.StateMachine.ChangeState(player.IdleState);
    }

    void OnDrawGizmosSelected()
    {
        if (player.attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.attackPoint.position, player.attackRange);
        }
    }
}