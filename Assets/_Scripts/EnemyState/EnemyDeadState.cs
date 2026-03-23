using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Dead State");
        CameraSmoothDamp.instance.Shake(0.2f, 0.06f);
        AudioManager.instance.PlaySFX(AudioManager.instance.enemyDie);
        TriggerParticle.Instance.Explosion(enemy.transform.position);
        enemy.anim.SetTrigger("Dead");
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState(EnemyController enemy)
    {
    }

    public override void ExitState(EnemyController enemy)
    {
    }
}
