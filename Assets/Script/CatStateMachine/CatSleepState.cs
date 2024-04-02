using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSleepState : CatBaseState
{
    public CatSleepState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    bool preAnimation = false;
    bool postAnimation = false;
    public override void EnterState()
    {
        _ctx.Transitioning = true;
        _ctx.NextAnimation_str = "sitDown";
        _ctx.NextAnimationAwait = true;
        _ctx.DragBanned = false;
        preAnimation = false;
        postAnimation = false;
    }

    public override void UpdateState()
    {
        if(!_ctx.Transitioning && !preAnimation && _ctx.SimpleCurrentAnimationProgress == 1)
        {
            _ctx.Transitioning = true;
            _ctx.NextAnimation_str = "sleep";
            _ctx.NextAnimationAwait = true;
            preAnimation = true;
            _ctx.CallRandomSwitchState();

            _ctx.Audio_SleepingPurr();
        }

        if(!_ctx.Transitioning && !postAnimation && preAnimation && _ctx.TimeUp)
        {
            _ctx.Transitioning = true;
            _ctx.NextAnimation_str = "sitUp";
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
        _ctx.MyAudioSource.Stop();
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void InitializeSubState()
    {

    }

    void AudioFunction()
    {
        if(_ctx.SimpleTimeUp)
        {
            _ctx.Audio_SleepingPurr();
            _ctx.CallRandomSimpleTimer();
        }
    }
}
