public abstract class CatBaseState
{
    protected CatStateMachine _ctx;
    protected CatStateFactory _factory;
    protected CatBaseState _currentState;

    public CatBaseState(CatStateMachine currentContext, CatStateFactory stateFactory)
    {
        _ctx = currentContext;
        _factory = stateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    protected void SwitchState(CatBaseState newState)
    {
        ExitState();
        newState.EnterState();
        _ctx.CurrentState = newState;
    }
}

