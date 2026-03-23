using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public enum State { Patrol, Detect, Chase, ReturnHome, IdleAfterLost }
    public State currentState = State.Patrol;

    [Header("General")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Detection")]
    public Transform player;
    public float detectionRange = 5f;
    public float chaseRange = 8f;

    [Header("Patrol")]
    public float checkDistance = 0.3f; public LayerMask groundLayer;
    public float waitMin = 0.5f;
    public float waitMax = 2f;
    public float distancePatrol;
    public Vector2 homePatrol;

    public Rigidbody2D rb;
    public Animator _anim;
    public bool facingRight = true;
    public bool hitWall;
    public bool groundAhead;
    private bool isWaiting = false;
    private bool delayReturnHome = false;
    private float distanceToPlayer;

    public bool isAttacking = false;
    public bool isWaitingAttack = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        rb.gravityScale = 1;
        homePatrol = transform.position;
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position,
            player.position);

        switch (currentState)
        {
            case State.Patrol:
                if (isAttacking || isWaitingAttack)
                {
                    rb.linearVelocity = Vector2.zero;
                    _anim.SetBool("isPatrol", false);
                    break;
                }
                if (!isWaiting) Patrol();
                if (distanceToPlayer <= detectionRange && CanSeePlayer())
                    currentState = State.Detect;

                break;

            case State.Detect:
                if (distanceToPlayer <= chaseRange)
                {
                    currentState = State.Chase;
                    if (distanceToPlayer <= GetComponentInChildren
                        <EnemyDamageSender>().AttackRange)
                    {
                        rb.linearVelocity = Vector2.zero;
                        _anim.SetBool("isChasing", false);
                        _anim.SetBool("isPatrol", false);
                    }
                }
                else
                    currentState = State.Patrol;
                break;

            case State.Chase:
                if (isAttacking || isWaitingAttack)
                {
                    rb.linearVelocity = Vector2.zero;
                    _anim.SetBool("isChasing", false);
                    _anim.SetBool("isPatrol", false);
                    break;
                }
                Chase();
                if (distanceToPlayer > chaseRange || !CanSeePlayer())
                    if (!delayReturnHome)
                    {
                        currentState = State.IdleAfterLost;
                        StartCoroutine(MissedPlayer());
                    }
                break;

            case State.ReturnHome:

                ReturnHome();
                if (distanceToPlayer <= chaseRange && CanSeePlayer())
                    currentState = State.Chase;
                break;

            case State.IdleAfterLost:
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                if (distanceToPlayer <= chaseRange && CanSeePlayer())
                    currentState = State.Chase;
                break;

        }
    }

    void Patrol()
    {

        Vector2 dir = facingRight ? Vector2.right : Vector2.left;

        Vector2 wallOrigin = rb.position + Vector2.up * 0.1f;
        hitWall = Physics2D.Raycast(wallOrigin, dir, checkDistance,
            groundLayer);

        Vector2 groundOrigin = rb.position + dir * 0.3f;
        groundAhead = Physics2D.Raycast(groundOrigin, Vector2.down,
            checkDistance, groundLayer);

        Debug.DrawLine(wallOrigin, wallOrigin + dir * checkDistance,
            Color.blue);
        Debug.DrawLine(groundOrigin,
            groundOrigin + Vector2.down * checkDistance, Color.green);

        if (hitWall || !groundAhead || Mathf.Abs(transform.position.x - homePatrol.x) >= distancePatrol)
        {

            StartCoroutine(WaitAndFlip());

            Debug.Log("HitWall: " + hitWall + " | GroundAhead: " + groundAhead);
            return;
        }

        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);
        _anim.SetBool("isPatrol", true);
    }

    void Chase()
    {

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= GetComponentInChildren<EnemyDamageSender>().AttackRange)
        {
            rb.linearVelocity = Vector2.zero;
            _anim.SetBool("isChasing", false);
            _anim.SetBool("isPatrol", false);
            return;
        }

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        _anim.SetBool("isChasing", true);
        _anim.SetBool("isPatrol", false);
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * chaseSpeed, rb.linearVelocity.y);

        if (dir.x > 0 && !facingRight) Flip();
        if (dir.x < 0 && facingRight) Flip();
        Debug.Log("Chase");
    }

    void ReturnHome()
    {
        if (Vector2.Distance(transform.position, homePatrol) > 5f)
        {
            homePatrol = transform.position; currentState = State.Patrol;
            return;
        }

        _anim.SetBool("isChasing", false);
        _anim.SetBool("isPatrol", true);

        Vector2 dir = (homePatrol - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(transform.position.x - homePatrol.x) < 0.1f)
        {
            currentState = State.Patrol;
        }

        if (dir.x > 0 && !facingRight) Flip();
        if (dir.x < 0 && facingRight) Flip();

        Debug.Log("ReturnHome");
    }
    bool CanSeePlayer()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float distToPlayer = Vector2.Distance(transform.position, player.position);


        if (facingRight && dirToPlayer.x < 0) return false;
        if (!facingRight && dirToPlayer.x > 0) return false;


        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, distToPlayer, groundLayer);
        Debug.DrawRay(transform.position, dirToPlayer * distToPlayer, Color.red);

        return hit.collider == null;
    }

    IEnumerator MissedPlayer()
    {
        delayReturnHome = true;


        _anim.SetBool("isPatrol", false);
        _anim.SetBool("isChasing", false);
        yield return new WaitForSeconds(2f);

        currentState = State.ReturnHome;
        delayReturnHome = false;
    }

    IEnumerator WaitAndFlip()
    {
        isWaiting = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        _anim.SetBool("isPatrol", false);
        _anim.SetBool("isChasing", false);
        float waitTime = Random.Range(waitMin, waitMax);
        yield return new WaitForSeconds(waitTime);

        Flip();
        isWaiting = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        float offset = 0.1f;
        transform.position += facingRight ? Vector3.right * offset : Vector3.left * offset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
