using UnityEngine;

public class IdleState : BasePlayerState
{
    public IdleState(PlayerController controller, PlayerStateMachine sm) : base(controller, sm) { }

    public override void Enter()
    {
        controller._animation.SetBool("isRunning", false);
        controller._rb.linearVelocity = new Vector2(0f, controller._rb.linearVelocity.y);
        if (!controller.canMove) return;

    }

    public override void Execute()
    {
        controller.PlayerMovementHandler._moveVelocity = Vector2.zero;
        if (!controller.canMove) return;
        if (InputManager.Instance.Movement.x != 0)
            stateMachine.ChangeState(PlayerController.Instance.RunState);

        if (InputManager.Instance.JumpWasPressed && controller.PlayerCollisionSensor._isGrounded)
            stateMachine.ChangeState(PlayerController.Instance.JumpState);
    }
}
