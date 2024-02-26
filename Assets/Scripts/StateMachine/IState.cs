public interface IState
{
    public void Enter();
    public void Update();
    public void PhysicsUpdate();
    public void HandleInput();
    public void Exit();
}
