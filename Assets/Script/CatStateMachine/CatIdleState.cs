using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdleState : CatBaseState
{
    public CatIdleState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    public override void EnterState()
    {
        _ctx.Transitioning = true;
        _ctx.NextAnimation_str = "idle";
        _ctx.NextAnimationAwait = true;
        _ctx.CallRandomSwitchState(true, 1, 3);
        _ctx.DragBanned = false;
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        _ctx.UseTransitionSpeedUp = false; //do or don't use the transition speed up method while switch to other state
    }

    public override void CheckSwitchStates()
    {
        float next = Random.Range(0, 1f);

        if(_ctx.Calling) //intervene while there's a call to answer
        {
            next = Random.Range(0.051f, 0.3f);
        }
        else if(_ctx.BallApears) // there's a ball to play
        {
            next = 2;
        }
        else if( _ctx.FoodOnStage)
        {
            next = 3;
        }

        if(_ctx.ManualControl)
        {
            next = _ctx.ManualStateNum;
        }

        switch(next)
        {
            case <= 0.05f: //5%
                SwitchState(_factory.Knead());
                break;
            case <= 0.2f: //15%
                _ctx.RandomTakeShit(0.2f);
                SwitchState(_factory.Walk());
                break;
            case <= 0.3f: //10%
                SwitchState(_factory.Jump());
                break;
            case <= 0.4f: //10%
                _ctx.RandomTakeShit(0.2f);
                SwitchState(_factory.Sit());
                break;
            case <= 0.5f: //10%
                _ctx.RandomTakeShit(0.5f);
                SwitchState(_factory.Eat());
                break;
            case <= 0.55f: //5%
                SwitchState(_factory.Scratch());
                break;
            case <= 0.6f: //5%
                SwitchState(_factory.Stretch());
                break;
            case <= 1f: //40%
                SwitchState(_factory.Sleep());
                break;
                
            case <= 2f:
                SwitchState(_factory.Observe());
                break;
            case <= 3f:
                SwitchState(_factory.Eat());
                break;
            default:
                break;
        }
    }

    public override void InitializeSubState()
    {

    }

}
