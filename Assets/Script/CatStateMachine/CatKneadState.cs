using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatKneadState : CatBaseState
{
    public CatKneadState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    bool setTimer = false;
    bool wandering = false;
    bool kneading = false;
    bool placing = false;
    bool ignoreCushion = false;
    bool reachTarget = true;
    bool preAnimation = false;
    bool postAnimation = false;
    Vector3 randomTarget = Vector3.zero;
    Vector3 cushionPos = Vector3.zero;
    public override void EnterState()
    {
        Debug.Log("Knead state enter");
        _ctx.Transitioning = true;
        _ctx.NextAnimation = _ctx.Walk;
        _ctx.NextAnimationAwait = true;
        setTimer = false;
        wandering = false;
        kneading = false;
        placing = false;
        ignoreCushion = false;
        reachTarget = true;
        preAnimation = false;
        postAnimation = false;
    }

    public override void UpdateState()
    {
        if(!setTimer && !_ctx.Transitioning)
        {
            setTimer = true;
            wandering = true;
            _ctx.CallRandomSwitchState(false, 10, 15);
        }

        if(setTimer & _ctx.TimeUp & !placing) // place the cushion
        {
            placing = true;
            cushionPos = new Vector3(Random.Range(_ctx.ObjectBoundary[0].position.x, _ctx.ObjectBoundary[1].position.x), Random.Range(_ctx.ObjectBoundary[0].position.y, _ctx.ObjectBoundary[1].position.y), _ctx.CatTransform.position.z);
            ignoreCushion = (Random.Range(0, 1f) < 0.1f)? true : false; // slight chance that cat will ignore the cushion
            _ctx.PlaceCushion(cushionPos);

            _ctx.CallRandomSwitchState();
        }

        if(wandering)
        {
            if(!_ctx.TimeUp | !placing) // keep wandering
            {
                var catPos = _ctx.CatTransform.position;
                if(reachTarget)
                {
                    randomTarget = new Vector3(Random.Range(_ctx.Boundary[0].position.x, _ctx.Boundary[1].position.x), Random.Range(_ctx.Boundary[0].position.y, _ctx.Boundary[1].position.y), catPos.z);
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
            else
            {
                if(!ignoreCushion) // done wandering, walk to the cushion
                {
                    var catPos = _ctx.CatTransform.position;
                    var tempDir = cushionPos - catPos; //temp dir
                    var offset = (tempDir.x > 0) ? new Vector3(-0.5f, 2f, 0): new Vector3(.5f, 2f, 0);
                    randomTarget = cushionPos + offset;
                    var dir = randomTarget - catPos;
                    _ctx.CatTransform.localScale = (dir.x > 0) ? new Vector3(-1, 1, 1): new Vector3(1, 1, 1); //facing
                    _ctx.CatTransform.position = Vector3.MoveTowards(catPos, randomTarget, _ctx.WalkSpeed* Time.deltaTime);

                    if(Vector3.Distance(catPos, randomTarget) < 0.05f) //reach the cushion
                    {
                        wandering = false;
                        kneading = true;
                        _ctx.UseTransitionSpeedUp = true;
                        _ctx.Transitioning = true;
                        _ctx.NextAnimation = _ctx.PreKnead;
                        _ctx.NextAnimationAwait = true;
                    }
                }
                else // will ignore the cushion, just leave the state
                {
                    _ctx.RemoveCushion();
                    _ctx.UseTransitionSpeedUp = true;
                    SwitchState(_factory.Idle());
                }
                
            }
        }
        if(kneading)
        {
            if(!_ctx.Transitioning && !preAnimation && _ctx.SimpleCurrentAnimationProgress == 1)
            {
                preAnimation = true;
                _ctx.UseTransitionSpeedUp = false;
                _ctx.Transitioning = true;
                _ctx.NextAnimation = _ctx.Knead;
                _ctx.NextAnimationAwait = true;
                _ctx.CallRandomSwitchState();

                _ctx.Audio_KneadingPurr();
            }

            if(!_ctx.Transitioning && !postAnimation && preAnimation && _ctx.TimeUp)
            {
                _ctx.Transitioning = true;
                _ctx.NextAnimation = _ctx.PostKnead;
                _ctx.NextAnimationAwait = true;
                postAnimation = true;

                _ctx.MyAudioSource.Stop();
            }

            if(!_ctx.Transitioning && postAnimation & _ctx.SimpleCurrentAnimationProgress == 1)
            {
                _ctx.RemoveCushion();
                SwitchState(_factory.Idle());
            }
            
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
}
