using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScratchState : CatBaseState
{
    public CatScratchState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    public bool preAnimation = false;
    public bool postAnimation = false; 
    public override void EnterState()
    {
        Debug.Log("scratch state enter");
        _ctx.Transitioning = true;
        _ctx.NextAnimation = _ctx.PreScratch;
        _ctx.NextAnimationAwait = true;
        preAnimation = false;
        postAnimation = false;
    }

    public override void UpdateState()
    {
        if(!_ctx.Transitioning && !preAnimation && _ctx.SimpleCurrentAnimationProgress == 1)
        {
            _ctx.Transitioning = true;
            _ctx.NextAnimation = _ctx.Scratch;
            _ctx.NextAnimationAwait = true;
            preAnimation = true;
            _ctx.CallRandomSwitchState();
        }

        if(!_ctx.Transitioning && !postAnimation && preAnimation && _ctx.TimeUp)
        {
            _ctx.Transitioning = true;
            _ctx.NextAnimation = _ctx.PostScratch;
            _ctx.NextAnimationAwait = true;
            postAnimation = true;
        }

        if(!_ctx.Transitioning && postAnimation & _ctx.SimpleCurrentAnimationProgress == 1)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void ExitState()
    {
        _ctx.UseTransitionSpeedUp = false;
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void InitializeSubState()
    {

    }
}
