using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWalkState : CatBaseState
{
    public CatWalkState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    bool reachTarget = true;
    bool setTimer = false;
    Vector3 randomTarget = Vector3.zero;
    public override void EnterState()
    {
        _ctx.Transitioning = true;
        _ctx.NextAnimation_str = "walk";
        _ctx.NextAnimationAwait = true;
        _ctx.DragBanned = false;
        reachTarget = true;
        setTimer = false;
    }

    public override void UpdateState()
    {
        if(!setTimer && !_ctx.Transitioning)
        {
            setTimer = true;
            if(!_ctx.Calling)
            {
                _ctx.CallRandomSwitchState();
            }
        }

        if(!_ctx.Transitioning)
        {
            var catPos = _ctx.CatTransform.position;
            if(_ctx.Calling) // answering the call
            {
                var dir = _ctx.AnswerCallPos - catPos;
                _ctx.CatTransform.localScale = (dir.x > 0) ? new Vector3(-1, 1, 1): new Vector3(1, 1, 1); //facing
                _ctx.CatTransform.position = Vector3.MoveTowards(catPos, _ctx.AnswerCallPos, _ctx.WalkSpeed* Time.deltaTime);

                if(Vector3.Distance(catPos, _ctx.AnswerCallPos) < 0.05f)
                {
                    _ctx.Audio_AnswerCall();
                    _ctx.Calling = false;
                    SwitchState(_factory.Idle());
                }
            }
            else // normal random walk
            {
                if(reachTarget)
                {
                    randomTarget = new Vector3(Random.Range(_ctx.Boundary[0].position.x, _ctx.Boundary[1].position.x), Random.Range(_ctx.Boundary[0].position.y, _ctx.Boundary[1].position.y), Random.Range(_ctx.Boundary[0].position.z, _ctx.Boundary[1].position.z));
                    reachTarget = false;
                    var dir = randomTarget - catPos;
                    _ctx.CatTransform.localScale = (dir.x > 0) ? new Vector3(-1, 1, 1): new Vector3(1, 1, 1); //facing
                }
                _ctx.CatTransform.position = Vector3.MoveTowards(catPos, randomTarget, _ctx.WalkSpeed* Time.deltaTime);

                if(Vector3.Distance(catPos, randomTarget) < 0.05f)
                {
                    reachTarget = true;
                }
            }

            if(_ctx.TimeUp & !_ctx.Calling)
            {
                SwitchState(_factory.Idle());
            }
        }
    }

    public override void ExitState()
    {
        _ctx.UseTransitionSpeedUp = true;
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void InitializeSubState()
    {

    }
}
