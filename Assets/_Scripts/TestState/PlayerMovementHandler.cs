using UnityEngine;

public class PlayerMovementHandler
{

    public Vector2 _moveVelocity;
    public bool _isFacingRight;

    public float VerticalVelocity { get; private set; }
    public bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    [HideInInspector] public int _numberOfJumpsUsed;

    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    private float _coyoteTimer;

    public void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (PlayerAttack.Instance != null && PlayerAttack.Instance.isAttacking)
        {
            return;
        }
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            if (InputManager.Instance.RunIsHeld)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * PlayerController.Instance.MoveStats.MaxRunSpeed;
            }
            else
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * PlayerController.Instance.MoveStats.MaxWalkSpeed;
            }

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            PlayerController.Instance._rb.linearVelocity = new Vector2(_moveVelocity.x, PlayerController.Instance._rb.linearVelocity.y);
        }
        else if (moveInput == Vector2.zero && !PlayerAttack.Instance.isAttacking)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            PlayerController.Instance._rb.linearVelocity = new Vector2(_moveVelocity.x, PlayerController.Instance._rb.linearVelocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    public void Turn(bool _facingRight)
    {
        if (!_facingRight)
        {
            PlayerController.Instance.sprite.flipX = true;
            _isFacingRight = false;
            PlayerAttack.Instance.attackPoint.localPosition = new Vector3(-Mathf.Abs(PlayerAttack.Instance.attackPoint.localPosition.x),
                PlayerAttack.Instance.attackPoint.localPosition.y, PlayerAttack.Instance.attackPoint.localPosition.z);
        }
        if (_facingRight)
        {
            PlayerController.Instance.sprite.flipX = false;
            _isFacingRight = true;
            PlayerAttack.Instance.attackPoint.localPosition = new Vector3(Mathf.Abs(PlayerAttack.Instance.attackPoint.localPosition.x),
                PlayerAttack.Instance.attackPoint.localPosition.y, PlayerAttack.Instance.attackPoint.localPosition.z);
        }
    }

    public void JumpChecks()
    {
        if (InputManager.Instance.JumpWasPressed)
        {
            _jumpBufferTimer = PlayerController.Instance.MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }

        if (InputManager.Instance.JumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = PlayerController.Instance.MoveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }

            PlayerController.Instance._animation.SetBool("isJumping", false);
        }

        if (_jumpBufferTimer > 0f && !_isJumping && (PlayerController.Instance.PlayerCollisionSensor._isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }

        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < PlayerController.Instance.MoveStats.NumberOfJumpsAllowed)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.jump);
            _isFastFalling = false;
            InitiateJump(1);
        }

        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < PlayerController.Instance.MoveStats.NumberOfJumpsAllowed - 1)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.jump);
            InitiateJump(2);
            _isFastFalling = false;
        }

        if ((_isJumping || _isFalling) && PlayerController.Instance.PlayerCollisionSensor._isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    public void Jump()
    {
        if (_isJumping)
        {
            PlayerController.Instance._animation.SetBool("isJumping", true);
            if (PlayerController.Instance.PlayerCollisionSensor._bumpedHead)
            {
                _isFastFalling = true;
            }

            if (VerticalVelocity >= 0f)
            {
                _apexPoint = Mathf.InverseLerp(PlayerController.Instance.MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > PlayerController.Instance.MoveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < PlayerController.Instance.MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }

                else
                {
                    VerticalVelocity += PlayerController.Instance.MoveStats.Gravity * Time.fixedDeltaTime;

                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }

            else if (!_isFastFalling)
            {
                VerticalVelocity += PlayerController.Instance.MoveStats.Gravity * PlayerController.Instance.MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        if (_isFastFalling)
        {
            if (_fastFallTime >= PlayerController.Instance.MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += PlayerController.Instance.MoveStats.Gravity * PlayerController.Instance.MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < PlayerController.Instance.MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / PlayerController.Instance.MoveStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        if (!PlayerController.Instance.PlayerCollisionSensor._isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += PlayerController.Instance.MoveStats.Gravity * Time.fixedDeltaTime;
        }

        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -PlayerController.Instance.MoveStats.MaxFallSpeed, 50f);

        PlayerController.Instance._rb.linearVelocity = new Vector2(PlayerController.Instance._rb.linearVelocity.x, VerticalVelocity);

    }
    public void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = PlayerController.Instance.MoveStats.InitialJumpVelocity;
    }
    public void ApplyBounce(float bounceVelocity)
    {
        VerticalVelocity = bounceVelocity;

        _isJumping = true;
        _isFalling = false;
        _isFastFalling = false;
        _fastFallTime = 0f;
        _isPastApexThreshold = false;
        _numberOfJumpsUsed = 1;
        PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.JumpState);
    }
}
