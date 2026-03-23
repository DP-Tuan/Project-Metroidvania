using System.Collections;
using UnityEngine;
public class BossAI : MonoBehaviour, IDamageable
{
    public static BossAI Instance;
    public enum BossState
    {
        Idle,
        ChargeLeft,
        ChargeRight,
        TripleCharge,
        Stun
    }

    [Header("State Machine")]
    public BossState currentState = BossState.Idle;
    public Animator _anim;

    [Header("Dependencies")]
    public Rigidbody2D rb;
    public BossHealth bossHealth;

    [Header("Charge Attack Settings")]
    public float chargeSpeed = 15f;
    public float chargeDuration = 1.0f;
    public float stunDuration = 1.5f;
    public float delayBetweenAttacks = 3.0f; public float chargeRange = 15f;
    [Header("Wall Detection")]
    public LayerMask wallLayer; public float wallCheckDistance = 0.5f;
    private bool isAttacking = false;
    public bool IsAttacking { get => isAttacking; }
    private bool isPhase2Ready = false;

    private float lastChargeDirection = 1f; private bool isActive = false;
    private void Awake()
    {
        Instance = this;
        _anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (bossHealth == null) this.bossHealth = GetComponentInChildren<BossHealth>();

    }

    public void ActivateBoss()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(AILoop());
        }
    }

    IEnumerator AILoop()
    {
        while (true)
        {
            switch (currentState)
            {
                case BossState.Idle:
                    yield return HandleIdleState();
                    break;

                case BossState.ChargeLeft:
                    yield return HandleChargeAttack(-chargeRange); break;

                case BossState.ChargeRight:
                    yield return HandleChargeAttack(chargeRange); break;

                case BossState.TripleCharge:
                    yield return HandleTripleCharge();
                    break;

                case BossState.Stun:
                    yield return HandleStunState();
                    break;
            }
            yield return null;
        }
    }


    IEnumerator HandleIdleState()
    {
        if (!isActive) yield break;
        yield return new WaitForSeconds(delayBetweenAttacks);

        if (isPhase2Ready)
        {
            ChangeState(BossState.TripleCharge);
        }
        else
        {
            float newDirection;
            do
            {
                newDirection = (Random.Range(0, 2) == 0) ? -1f : 1f;
            } while (newDirection == lastChargeDirection);

            lastChargeDirection = newDirection;

            if (newDirection > 0)
                ChangeState(BossState.ChargeRight);
            else
                ChangeState(BossState.ChargeLeft);
        }
    }


    IEnumerator HandleChargeAttack(float targetXOffset)
    {
        isAttacking = true;
        _anim.SetBool("Attack", true);

        float direction = Mathf.Sign(targetXOffset);
        FlipSprite(direction);
        Debug.Log("Boss chu?n b? Tông.");

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);

        RaycastHit2D hit;

        while (true)
        {
            rb.linearVelocity = new Vector2(direction * chargeSpeed, rb.linearVelocity.y);

            Vector2 rayStart = transform.position;
            Vector2 rayDirection = new Vector2(direction, 0);

            hit = Physics2D.Raycast(rayStart, rayDirection, wallCheckDistance, wallLayer);

            if (hit.collider != null)
            {
                CameraSmoothDamp.instance.Shake(0.2f, 0.06f);
                AudioManager.instance.PlaySFXForSeconds(AudioManager.instance.earthquake, 0.2f);
                TriggerParticle.Instance.Hit(hit.point);
                _anim.SetBool("Attack", false);
                break;
            }

            yield return null;
        }
        #region



        #endregion
        rb.linearVelocity = Vector2.zero; isAttacking = false;
        ChangeState(BossState.Stun);
    }

    IEnumerator HandleStunState()
    {
        Debug.Log("Boss b? choáng (Stun).");
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(stunDuration);

        ChangeState(BossState.Idle);
    }

    IEnumerator HandleTripleCharge()
    {
        Debug.Log("Boss th?c hi?n T?n công liên ti?p (Triple Charge).");

        yield return StartCoroutine(ChargeSequenceStep(-chargeRange, 0.5f));

        yield return StartCoroutine(ChargeSequenceStep(chargeRange, 0.5f));

        yield return StartCoroutine(ChargeSequenceStep(-chargeRange, 1.0f));
        ChangeState(BossState.Stun);
    }

    IEnumerator ChargeSequenceStep(float targetXOffset, float recoveryTime)
    {
        isAttacking = true;
        float direction = Mathf.Sign(targetXOffset);
        FlipSprite(direction);
        RaycastHit2D hit;

        while (true)
        {
            rb.linearVelocity = new Vector2(direction * chargeSpeed, rb.linearVelocity.y);

            Vector2 rayStart = transform.position;
            Vector2 rayDirection = new Vector2(direction, 0);

            hit = Physics2D.Raycast(rayStart, rayDirection, wallCheckDistance, wallLayer);

            if (hit.collider != null)
            {
                break;
            }

            yield return null;
        }


        isAttacking = false;
        yield return new WaitForSeconds(recoveryTime);
    }


    void FlipSprite(float direction)
    {
        Vector3 scale = transform.localScale;

        if (direction > 0 && scale.x < 0)
        {
            scale.x *= -1;
        }
        else if (direction < 0 && scale.x > 0)
        {
            scale.x *= -1;
        }

        transform.localScale = scale;
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
        Debug.Log("Chuy?n tr?ng thái sang: " + newState);
    }

    public void ActivatePhase2()
    {
        if (!isPhase2Ready)
        {
            isPhase2Ready = true;
            ChangeState(BossState.TripleCharge);
        }
    }


    private void OnDrawGizmos()
    {
        if (currentState == BossState.ChargeLeft || currentState == BossState.ChargeRight || currentState == BossState.TripleCharge)
        {
            float direction = Mathf.Sign(transform.localScale.x);

            Gizmos.color = Color.red;

            Vector2 rayStart = transform.position;
            Vector2 rayDirection = new Vector2(direction, 0);

            Gizmos.DrawLine(rayStart, rayStart + rayDirection * wallCheckDistance);

            Gizmos.DrawSphere(rayStart + rayDirection * wallCheckDistance, 0.1f);
        }
    }

    public void TakeDamage(float damage)
    {
        bossHealth.TakeDamage(damage);
    }
}