using UnityEngine;

public class RunState : BasePlayerState
{


    public RunState(PlayerController controller, PlayerStateMachine sm) : base(controller, sm) { }

    public override void Enter()
    {
        controller._animation.SetBool("isRunning", true);
    }

    public override void Execute()
    {

        if (controller.PlayerCollisionSensor._isGrounded)
        {
            controller.PlayerMovementHandler.Move(controller.MoveStats.GroundAcceleration,
                controller.MoveStats.GroundDeceleration, InputManager.Instance.Movement);
        }
        else
        {
            controller.PlayerMovementHandler.Move(controller.MoveStats.AirAcceleration,
                controller.MoveStats.AirDeceleration, InputManager.Instance.Movement);
        }

        if (controller.PlayerMovementHandler._moveVelocity == Vector2.zero)
            stateMachine.ChangeState(PlayerController.Instance.IdleState);

        if (InputManager.Instance.JumpWasPressed)
        {
            Debug.Log("Run State to Jump State");
            stateMachine.ChangeState(PlayerController.Instance.JumpState);
        }
    }

    public override void Exit()
    {
        controller._animation.SetBool("isRunning", false);
    }


}
