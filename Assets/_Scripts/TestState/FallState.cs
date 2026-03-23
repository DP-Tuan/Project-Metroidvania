

using UnityEngine;

public class FallState : BasePlayerState
{
    public FallState(PlayerController controller, PlayerStateMachine sm) : base(controller, sm) { }

    public override void Enter()
    {
        controller._animation.SetBool("isJumping", true);
        Debug.Log("FallState");
    }

    public override void Execute()
    {
        Debug.Log("Vertical Velocity" + controller.PlayerMovementHandler.VerticalVelocity);
        if (controller.PlayerCollisionSensor._isGrounded)
            stateMachine.ChangeState(PlayerController.Instance.IdleState);
    }
}
