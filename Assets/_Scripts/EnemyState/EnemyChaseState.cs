using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{

    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Chase State");
        enemy.anim.SetBool("isChasing", true);
    }

    public override void UpdateState(EnemyController enemy)
    {
        if (!enemy.CanSeePlayer() || enemy.DistanceToPlayer() > enemy.chaseRange || !enemy.CheckGroundAhead())
        {
            enemy.SwitchState(enemy.idleState);
            return;
        }



        if (enemy.DistanceToPlayer() <= enemy.attackRange)
        {
            enemy.rb.linearVelocity = new Vector2(0, enemy.rb.linearVelocity.y);
            enemy.anim.SetBool("isChasing", false);
            Vector2 dirToPlayer = (enemy.player.position - enemy.transform.position).normalized;
            if (dirToPlayer.x > 0 && !enemy.facingRight) enemy.Flip();
            if (dirToPlayer.x < 0 && enemy.facingRight) enemy.Flip();

            if (enemy.attackTimer >= enemy.attackCoolDown)
            {
                enemy.SwitchState(enemy.attackState);
                return;
            }
        }
        else
        {
            enemy.anim.SetBool("isChasing", true);
            Chase(enemy);
        }
    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting Chase State");
        enemy.anim.SetBool("isChasing", false);
        enemy.rb.linearVelocity = new Vector2(0, enemy.rb.linearVelocity.y);
    }

    private void Chase(EnemyController enemy)
    {
        Vector2 dirToPlayer = (enemy.player.position - enemy.transform.position).normalized;

        if (dirToPlayer.x > 0 && !enemy.facingRight) enemy.Flip();
        if (dirToPlayer.x < 0 && enemy.facingRight) enemy.Flip();

        enemy.rb.linearVelocity = new Vector2(dirToPlayer.x * enemy.chaseSpeed, enemy.rb.linearVelocity.y);
    }
}
