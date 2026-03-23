using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private IState currentState;


    void Start()
    {
    }

    void Update()
    {
        currentState.Execute();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType()) return;
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

    }
}
