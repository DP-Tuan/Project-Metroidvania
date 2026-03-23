using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float idleTimer;
    private float idleDuration = 2f;
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Idle State");
        idleTimer = 0f;
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState(EnemyController enemy)
    {

        if (enemy.CanSeePlayer() && enemy.DistanceToPlayer() <= enemy.chaseRange)
        {
            enemy.SwitchState(enemy.chaseState);
            return;
        }

        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            enemy.SwitchState(enemy.returnHomeState);
        }
    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting Idle State");
    }
}
