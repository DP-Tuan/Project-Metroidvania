using UnityEngine;

public class EnemyHurtState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Hurt State");
        enemy.anim.SetTrigger("Hurt");
        enemy.ShowTextFloating();
        AudioManager.instance.PlaySFX(AudioManager.instance.enemyHurt);
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState(EnemyController enemy)
    {
    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting Hurt State");
    }
}
