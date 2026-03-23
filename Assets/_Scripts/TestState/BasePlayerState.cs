public abstract class BasePlayerState
{
    protected PlayerController controller;
    protected PlayerStateMachine stateMachine;

    public BasePlayerState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void ExecutePhysics() { }
    public virtual void Exit() { }
}
