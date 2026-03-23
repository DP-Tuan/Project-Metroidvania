using UnityEngine;
public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Attack State");

        enemy.rb.linearVelocity = Vector2.zero;

        enemy.anim.SetTrigger("Attack");

        enemy.attackTimer = 0f;
    }

    public override void UpdateState(EnemyController enemy)
    {
    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting Attack State");
    }
}
