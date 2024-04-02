using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatObserveState : CatBaseState
{
    public CatObserveState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    bool chasing = false;
    bool reachTarget = false;
    bool reset = false;
    bool ballRolling = false; 
    Vector3 randomTarget = Vector3.zero;
    Vector3 center, startRelCenter, targetRelCenter;
    public override void EnterState()
    {
        _ctx.Transitioning = true;
        _ctx.NextAnimation_str = "observe";
        _ctx.NextAnimationAwait = true;
        _ctx.DragBanned = true;
        if(_ctx.BallTransform.position.x >= _ctx.CatTransform.position.x)
        {
            _ctx.CatTransform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            _ctx.CatTransform.localScale = new Vector3(1,1,1);
        }
        chasing = false;
        reachTarget = true;
        reset = false;
        ballRolling = false;
    }

    public override void UpdateState()
    {
        if(!_ctx.Transitioning & !chasing)
        {
            _ctx.Transitioning = true;
            _ctx.NextAnimation_str = "jump";
            _ctx.NextAnimationAwait = true;
            chasing = true;
        }

        if(chasing)
        {
            if(!ballRolling & !_ctx.Transitioning & _ctx.CurrentAnimationProgress > 0.2f)
            {
                ballRolling = true;
                _ctx.MyBall.StartRolling();
            }
            if(!_ctx.Transitioning && (_ctx.CurrentAnimationProgress > 0.1f & _ctx.CurrentAnimationProgress < 0.5f))
            {
                var catPos = _ctx.CatTransform.position;

                float jumpProgress = (_ctx.CurrentAnimationProgress - 0.1f)/ 0.4f;
                

                if(reachTarget & reset)
                {
                    randomTarget = _ctx.BallTransform.position;
                    _ctx.CatTransform.localScale = ((randomTarget - catPos).x > 0) ? new Vector3(-1, 1, 1): new Vector3(1, 1, 1); //facing
                    
                    reset = false;
                    reachTarget = false;

                    if(Vector3.Distance(randomTarget, catPos) < 0.1f)
                    {
                        SwitchState(_factory.Idle());
                        _ctx.MyBall.SelfDestroy(true);
                        return;
                    }
                }
                center = catPos - randomTarget;
                center -= new Vector3(0, 5, 0);
                startRelCenter = catPos - center;
                targetRelCenter = randomTarget - center;

                if(jumpProgress >= 0.95f)
                {
                    reachTarget = true;
                }
                else
                {
                    _ctx.CatTransform.position = Vector3.Slerp(startRelCenter, targetRelCenter, jumpProgress) + center;
                }
            }
            else if(!_ctx.Transitioning && (_ctx.CurrentAnimationProgress < 0.6f & _ctx.CurrentAnimationProgress > 0.55f))
            {
                randomTarget = _ctx.BallTransform.position;
                _ctx.CatTransform.localScale = ((randomTarget - _ctx.CatTransform.position).x > 0) ? new Vector3(-1, 1, 1): new Vector3(1, 1, 1); //facing
            }
            else
            {
                reset = true; //only get random once after the jump end
            }
        }
    }

    public override void ExitState()
    {
        _ctx.BallApears = false;
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void InitializeSubState()
    {

    }
}
