public class PlayerStateMachine
{
    private BasePlayerState _currentState;
    public BasePlayerState CurrentState => _currentState;

    public void ChangeState(BasePlayerState newState)
    {
        if (_currentState == newState) return;
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState?.Execute();
    }

    public void UpdatePhysics()
    {
        _currentState?.ExecutePhysics();
    }
}
