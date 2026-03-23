

public class EnemyStateMachine
{
    private EnemyBaseState _currentState;
    public EnemyBaseState CurrentState => _currentState;

    private EnemyController _enemy;

    public EnemyStateMachine(EnemyController enemy)
    {
        _enemy = enemy;
    }
    public void ChangeState(EnemyBaseState newState)
    {
        if (_currentState == newState) return;
        _currentState?.ExitState(_enemy);
        _currentState = newState;
        _currentState.EnterState(_enemy);
    }

    public void Update()
    {
        _currentState?.UpdateState(_enemy);
    }
}
