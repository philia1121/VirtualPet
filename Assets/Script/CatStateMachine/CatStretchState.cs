using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatStretchState : CatBaseState
{
    public CatStretchState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    bool setTimer = false;
    public override void EnterState()
    {
        Debug.Log("Stretch state enter");
        _ctx.Transitioning = true;
        _ctx.NextAnimation = _ctx.Stretch;
        _ctx.NextAnimationAwait = true;
        setTimer = false;
    }

    public override void UpdateState()
    {
        if(!setTimer && _ctx.SimpleCurrentAnimationProgress == 1)
        {
            setTimer = true;
            _ctx.CallRandomSwitchState(true);
        }
    }

    public override void ExitState()
    {
        _ctx.UseTransitionSpeedUp = false;
    }

    public override void CheckSwitchStates()
    {
        SwitchState(_factory.Idle());
    }

    public override void InitializeSubState()
    {

    }
}
