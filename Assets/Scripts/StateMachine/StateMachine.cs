using UnityEngine;

public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        Debug.Log($"Chage State : {newState.GetType()}");
        currentState.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    public IState GetCurState()
    {
        return currentState;
    }
}
