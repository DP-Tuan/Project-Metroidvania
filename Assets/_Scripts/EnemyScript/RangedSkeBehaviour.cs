using UnityEngine;

public class RangedSkeBehaviour : EnemyController
{
    [Header("Ranged Attack")]
    public GameObject projectilePrefab; public Transform firePoint; public string projectilePoolTag = "Arrow";
    public float projectileSpeed = 10f;

    public readonly EnemyRangedAttackState rangedAttackState = new EnemyRangedAttackState();
    public readonly EnemyRangedChaseState rangedChaseState = new EnemyRangedChaseState();

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {

        if (enemyStateMachine.CurrentState == patrolState &&
            CanSeePlayer() &&
            DistanceToPlayer() <= detectionRange)
        {
            SwitchState(rangedChaseState);
        }
        else if (enemyStateMachine.CurrentState == idleState &&
            CanSeePlayer() &&
            DistanceToPlayer() <= chaseRange)
        {
            SwitchState(rangedChaseState);
        }
        else if (enemyStateMachine.CurrentState == returnHomeState &&
            CanSeePlayer() &&
            DistanceToPlayer() <= chaseRange)
        {
            SwitchState(rangedChaseState);
        }
        else if (enemyStateMachine.CurrentState == chaseState)
        {
            SwitchState(rangedChaseState);
        }

        base.Update();
    }

    public void FireProjectile()
    {
        if (firePoint != null)
        {
            GameObject projectile = ObjectPooler.Instance.SpawnFromPool(projectilePoolTag, firePoint.position, firePoint.rotation);
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.damage = this.attackDamage;
            }

            if (projectile == null)
            {
                Debug.LogWarning("Không th? spawn projectile t? pool: " + projectilePoolTag);
                return;
            }

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            Vector2 direction = facingRight ? Vector2.right : Vector2.left;

            projectileRb.linearVelocity = direction * projectileSpeed;

        }
    }

    public override void OnAttackEvent()
    {
        FireProjectile();
    }

    public override void OnAttackFinished()
    {
        Debug.Log("Enter Finished");
        if (CanSeePlayer() && DistanceToPlayer() <= chaseRange)
        {
            Debug.Log("Swithc State");
            SwitchState(rangedChaseState);
        }
        else
        {
            Debug.Log("Idle State Change after Finished");
            SwitchState(idleState);
        }
    }
}


public class EnemyRangedAttackState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Ranged Attack State");
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.anim.SetTrigger("isShot"); enemy.attackTimer = 0f;
    }

    public override void UpdateState(EnemyController enemy)
    {
    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting Ranged Attack State");
    }
}

public class EnemyRangedChaseState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Ranged Chase/Wait State");
        enemy.rb.linearVelocity = Vector2.zero; enemy.anim.SetBool("isPatrol", false);
    }

    public override void UpdateState(EnemyController enemy)
    {
        if (!enemy.CanSeePlayer() || enemy.DistanceToPlayer() > enemy.chaseRange)
        {
            enemy.SwitchState(enemy.idleState);
            return;
        }

        Vector2 dirToPlayer = (enemy.player.position - enemy.transform.position).normalized;
        if (dirToPlayer.x > 0 && !enemy.facingRight) enemy.Flip();
        if (dirToPlayer.x < 0 && enemy.facingRight) enemy.Flip();

        if (enemy.attackTimer >= enemy.attackCoolDown)
        {
            RangedSkeBehaviour rangedEnemy = (RangedSkeBehaviour)enemy;
            enemy.SwitchState(rangedEnemy.rangedAttackState);
        }

    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting Ranged Chase/Wait State");
    }
}
