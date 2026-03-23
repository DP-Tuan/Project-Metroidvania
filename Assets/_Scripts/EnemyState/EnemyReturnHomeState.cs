using UnityEngine;

public class EnemyReturnHomeState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering ReturnHome State");
        enemy.anim.SetBool("isPatrol", true);
    }

    public override void UpdateState(EnemyController enemy)
    {
        Debug.Log("Enter Update Return");
        if (enemy.CanSeePlayer() && enemy.DistanceToPlayer() <= enemy.chaseRange)
        {
            enemy.SwitchState(enemy.chaseState);
            return;
        }

        if (Mathf.Abs(enemy.transform.position.x - enemy.homePatrol.x) < 0.35f)
        {
            enemy.SwitchState(enemy.patrolState);
            return;
        }

        ReturnHome(enemy);
    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting ReturnHome State");
        enemy.anim.SetBool("isPatrol", false);
    }

    private void ReturnHome(EnemyController enemy)
    {
        Vector2 dirToHome = (enemy.homePatrol - (Vector2)enemy.transform.position).normalized;

        if (dirToHome.x > 0 && !enemy.facingRight) enemy.Flip();
        if (dirToHome.x < 0 && enemy.facingRight) enemy.Flip();

        enemy.rb.linearVelocity = new Vector2(dirToHome.x * enemy.moveSpeed, enemy.rb.linearVelocity.y);
    }
}
