public class AttackState : BasePlayerState
{
    public AttackState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        controller.canMove = false; controller._animation.SetTrigger("Attack");
        controller.AttackHandler.ExecuteAttack();
    }

    public override void Execute()
    {

    }

    public override void ExecutePhysics()
    {
    }

    public override void Exit()
    {
        controller.canMove = true;
    }
}