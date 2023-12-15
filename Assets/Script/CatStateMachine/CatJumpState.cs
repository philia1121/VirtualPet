using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatJumpState : CatBaseState
{
    public CatJumpState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    bool reachTarget = true;
    bool setTimer = false;
    bool reset = false;
    Vector3 randomTarget = Vector3.zero;
    Vector3 center, startRelCenter, targetRelCenter;
    public override void EnterState()
    {
        Debug.Log("jump state enter");
        _ctx.Transitioning = true;
        _ctx.NextAnimation = _ctx.Jump;
        _ctx.NextAnimationAwait = true;
        _ctx.UseAdditionalSpeedAdjust = true;
        _ctx.AdditionalSpeedAdjust = 1.5f;

        reachTarget = true;
        setTimer = false;
        reset = false;
    }

    public override void UpdateState()
    {
        if(!setTimer && _ctx.SimpleCurrentAnimationProgress == 1)
        {
            setTimer = true;
            if(!_ctx.Calling)
            {
                _ctx.CallRandomSwitchState();
            }
        }

        if(!_ctx.Transitioning && (_ctx.CurrentAnimationProgress > 0.1f & _ctx.CurrentAnimationProgress < 0.5f))
        {
            var catPos = _ctx.CatTransform.position;

            if(reachTarget & reset)
            {
                if(_ctx.Calling)
                {
                    randomTarget = _ctx.AnswerCallPos;
                }
                else
                {
                    randomTarget = new Vector3(Random.Range(_ctx.Boundary[0].position.x, _ctx.Boundary[1].position.x), Random.Range(_ctx.Boundary[0].position.y, _ctx.Boundary[1].position.y), catPos.z);
                }
                var dir = randomTarget - catPos;
                _ctx.CatTransform.localScale = (dir.x > 0) ? new Vector3(-1, 1, 1): new Vector3(1, 1, 1); //facing
                reset = false;
                reachTarget = false;
            }
            center = catPos - randomTarget;
            center -= new Vector3(0, 5, 0);
            startRelCenter = catPos - center;
            targetRelCenter = randomTarget - center;
            
            float jumpProgress = (_ctx.CurrentAnimationProgress - 0.1f) / (0.5f - 0.1f);
            if(jumpProgress >= 0.95f)
            {
                if(_ctx.Calling)
                {
                    _ctx.Audio_AnswerCall();
                    _ctx.Calling = false;
                    CheckSwitchStates();
                }
                else
                {
                   reachTarget = true; 
                }
                
            }
            else
            {
                _ctx.CatTransform.position = Vector3.Slerp(startRelCenter, targetRelCenter, 0.05f) + center;
            }
        }
        else
        {
            reset = true; //only get random once after the jump end
        }

        if(_ctx.TimeUp && _ctx.SimpleCurrentAnimationProgress == 1)
        {
            CheckSwitchStates();
        }
    }

    public override void ExitState()
    {
        _ctx.UseTransitionSpeedUp = true;
        _ctx.UseAdditionalSpeedAdjust = false;
    }

    public override void CheckSwitchStates()
    {
        SwitchState(_factory.Idle());
    }

    public override void InitializeSubState()
    {

    }
}
