using UnityEngine;

public class PlayerCollisionSensor
{
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    public bool _isGrounded;
    public bool _bumpedHead;

    private void HeadBumpCheck()
    {
        Vector2 boxCastOrigin = new Vector2(PlayerController.Instance._col.bounds.center.x,
            PlayerController.Instance._col.bounds.max.y);

        Vector2 boxCastSize = new Vector2(PlayerController.Instance._col.bounds.size.x * PlayerController.Instance.MoveStats.HeadWidth,
            PlayerController.Instance.MoveStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, PlayerController.Instance.MoveStats.HeadDetectionRayLength,
            PlayerController.Instance.MoveStats.GroundLayer);

        if (_headHit.collider != null)
        {
            _bumpedHead = true;
            PlayerController.Instance._animation.SetBool("isJumping", false);
        }
        else { _bumpedHead = false; }

        #region Debug Visualization

        if (PlayerController.Instance.MoveStats.DebugShowHeadBumpBox)
        {
            float headWidth = PlayerController.Instance.MoveStats.HeadWidth;
            Color rayColor;
            if (_bumpedHead)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * PlayerController.Instance.MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * PlayerController.Instance.MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + PlayerController.Instance.MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        }
        #endregion
    }

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(PlayerController.Instance._col.bounds.center.x,
            PlayerController.Instance._col.bounds.min.y);

        Vector2 boxCastSize = new Vector2(PlayerController.Instance._col.bounds.size.x,
            PlayerController.Instance.MoveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down,
            PlayerController.Instance.MoveStats.GroundDetectionRayLength, PlayerController.Instance.MoveStats.GroundLayer);

        if (_groundHit.collider != null)
        {
            _isGrounded = true;
            PlayerController.Instance._animation.SetBool("isJumping", false);
        }
        else
        {
            _isGrounded = false;
        }

        #region Debug Visualization

        if (PlayerController.Instance.MoveStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * PlayerController.Instance.MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * PlayerController.Instance.MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - PlayerController.Instance.MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }

        #endregion
    }

    public void CollisionChecks()
    {
        IsGrounded();
        HeadBumpCheck();
    }
}
