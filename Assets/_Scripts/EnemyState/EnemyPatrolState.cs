using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private bool isWaiting = false;
    private Coroutine waitCoroutine;

    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering Patrol State");
        enemy.anim.SetBool("isPatrol", true);
        isWaiting = false; waitCoroutine = null;
    }

    public override void UpdateState(EnemyController enemy)
    {
        if (enemy.CanSeePlayer() && enemy.DistanceToPlayer() <= enemy.detectionRange)
        {
            enemy.SwitchState(enemy.chaseState);
            return;
        }

        if (isWaiting) return;
        Patrol(enemy);
    }

    public override void ExitState(EnemyController enemy)
    {
        Debug.Log("Exiting Patrol State");
        enemy.anim.SetBool("isPatrol", false);
        enemy.rb.linearVelocity = new Vector2(0, enemy.rb.linearVelocity.y);
        if (waitCoroutine != null)
        {
            enemy.StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
        isWaiting = false;
    }

    private void Patrol(EnemyController enemy)
    {
        Vector2 dir = enemy.facingRight ? Vector2.right : Vector2.left;

        bool hitWall = enemy.CheckWall();
        bool groundAhead = enemy.CheckGroundAhead();

        if (hitWall || !groundAhead || Mathf.Abs(enemy.transform.position.x - enemy.homePatrol.x) >= enemy.distancePatrol)
        {
            Debug.Log("hitwall !ground adhead");
            waitCoroutine = enemy.StartCoroutine(WaitAndFlip(enemy));
            return;
        }

        enemy.rb.linearVelocity = new Vector2(dir.x * enemy.moveSpeed, enemy.rb.linearVelocity.y);
    }

    private System.Collections.IEnumerator WaitAndFlip(EnemyController enemy)
    {
        isWaiting = true;
        enemy.rb.linearVelocity = Vector2.zero; enemy.anim.SetBool("isPatrol", false);

        float waitTime = Random.Range(enemy.waitMin, enemy.waitMax);
        yield return new WaitForSeconds(waitTime);

        enemy.Flip();
        isWaiting = false;
        enemy.anim.SetBool("isPatrol", true);
        waitCoroutine = null;
    }
}
