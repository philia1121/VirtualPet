using System.Collections.Generic;

enum CatStates
{
    idle,
    walk,
    jump,
    stretch,
    sit,
    sleep,
    scratch,
    knead,
    eat,
    observe,
    dragged,
    chase
}

public class CatStateFactory
{
    CatStateMachine _context;
    Dictionary<CatStates, CatBaseState> _states = new Dictionary<CatStates, CatBaseState>();

    public CatStateFactory(CatStateMachine currentContext)
    {
        _context = currentContext;
        _states[CatStates.idle] = new CatIdleState(_context, this);
        _states[CatStates.walk] = new CatWalkState(_context, this);
        _states[CatStates.jump] = new CatJumpState(_context, this);
        _states[CatStates.stretch] = new CatStretchState(_context, this);
        _states[CatStates.sit] = new CatSitState(_context, this);
        _states[CatStates.sleep] = new CatSleepState(_context, this);
        _states[CatStates.scratch] = new CatScratchState(_context, this);
        _states[CatStates.knead] = new CatKneadState(_context, this);
        _states[CatStates.eat] = new CatEatState(_context, this);
        _states[CatStates.observe] = new CatObserveState(_context, this);
        _states[CatStates.dragged] = new CatDraggedState(_context, this);
        _states[CatStates.chase] = new CatChaseState(_context, this);
    }

    public CatBaseState Idle()
    {   return _states[CatStates.idle];}
    public CatBaseState Walk()
    {   return _states[CatStates.walk];}
    public CatBaseState Jump()
    {   return _states[CatStates.jump];}
    public CatBaseState Stretch()
    {   return _states[CatStates.stretch];}
    public CatBaseState Sit()
    {   return _states[CatStates.sit];}
    public CatBaseState Sleep()
    {   return _states[CatStates.sleep];}
    public CatBaseState Scratch()
    {   return _states[CatStates.scratch];}
    public CatBaseState Knead()
    {   return _states[CatStates.knead];} 
    public CatBaseState Eat()
    {   return _states[CatStates.eat];}
    public CatBaseState Observe()
    {   return _states[CatStates.observe];}
    public CatBaseState Dragged()
    {   return _states[CatStates.dragged];}
    public CatBaseState Chase()
    {   return _states[CatStates.chase];}
}

