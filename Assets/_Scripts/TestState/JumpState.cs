using UnityEngine;

public class JumpState : BasePlayerState
{
    public JumpState(PlayerController controller, PlayerStateMachine sm) : base(controller, sm) { }

    public override void Enter()
    {
        controller._animation.SetBool("isJumping", true);
        AudioManager.instance.PlaySFX(AudioManager.instance.jump);
        Debug.Log("JumpState");
    }

    public override void Execute()
    {
        controller.PlayerMovementHandler.Move(
            controller.MoveStats.AirAcceleration,
            controller.MoveStats.AirDeceleration,
            InputManager.Instance.Movement
        );

        if (controller.PlayerCollisionSensor._isGrounded && !controller.PlayerMovementHandler._isJumping)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.landed);
            stateMachine.ChangeState(controller.IdleState);
        }


        if (controller.PlayerMovementHandler.VerticalVelocity < 0
            && !controller.PlayerCollisionSensor._isGrounded
            && !controller.PlayerMovementHandler._isJumping)
        {
            CameraSmoothDamp.instance.Shake(0.2f, 0.05f);
            stateMachine.ChangeState(controller.FallState);
        }


    }

    public override void ExecutePhysics()
    {

    }

    public override void Exit()
    {
        controller._animation.SetBool("isJumping", false);
    }

}
