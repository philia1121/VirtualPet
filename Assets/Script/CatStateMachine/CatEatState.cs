using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatEatState : CatBaseState
{
    public CatEatState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    bool preAnimation = false;
    bool postAnimation = false;
    bool eating = false;
    bool walkToFood = false;
    bool reachTarget = true;
    Vector3 Target = Vector3.zero;
    public override void EnterState()
    {
        _ctx.Transitioning = true;
        if(_ctx.FoodOnStage)
        {
            _ctx.NextAnimation = _ctx.Walk;
        }
        else
        {
            _ctx.NextAnimation = _ctx.PreWaitForEat;
        }
        _ctx.NextAnimationAwait = true;
        _ctx.CatTransform.localScale = new Vector3(1, 1, 1);
        preAnimation = false;
        postAnimation = false;
        eating = false;
        walkToFood = false;
    }

    public override void UpdateState()
    {
        if(!eating & !_ctx.FoodOnStage)
        {
            if(!_ctx.Transitioning && !preAnimation && _ctx.SimpleCurrentAnimationProgress == 1)
            {
                _ctx.Transitioning = true;
                _ctx.NextAnimation = _ctx.WaitForEat;
                _ctx.NextAnimationAwait = true;
                preAnimation = true;
                _ctx.CallRandomSwitchState(); //Timer: Waiting for food
                _ctx.WaitForFood = true;

                _ctx.Audio_CallForFood();
            }

            if(_ctx.WaitForFood) //Wait For Food
            {
                if(!_ctx.Transitioning && !_ctx.TimeUp && _ctx.GetFood) // food served
                {
                    _ctx.WaitForFood = false;
                    _ctx.Transitioning = true;
                    _ctx.NextAnimation = _ctx.PreEat;
                    _ctx.NextAnimationAwait = true;
                    preAnimation = false;
                    eating = true;

                    _ctx.MyAudioSource.Stop();
                }

                if(!_ctx.Transitioning && _ctx.TimeUp && !_ctx.GetFood) // lost patience before food served
                {
                    postAnimation = true;
                    _ctx.WaitForFood = false;
                    _ctx.Transitioning = true;
                    _ctx.NextAnimation = _ctx.PostWaitForEat;
                    _ctx.NextAnimationAwait = true;
                }
            }
        }
        else if(!eating & _ctx.FoodOnStage) // walk to the food already on stage
        {   
            var catPos = _ctx.CatTransform.position;
            if(!_ctx.Transitioning)
            {
                Target = _ctx.MyCatFood.transform.position + new Vector3(-0.6f, -1.94f, 0)*-1;
                reachTarget = false;
                var dir = Target - catPos;
                _ctx.CatTransform.localScale = (dir.x > 0) ? new Vector3(-1, 1, 1): new Vector3(1, 1, 1); //facing

                _ctx.CatTransform.position = Vector3.MoveTowards(catPos, Target, _ctx.WalkSpeed* Time.deltaTime);
            }
            
            if(Vector3.Distance(catPos, Target) < 0.05f) //reach the food
            {
                _ctx.CatTransform.localScale = new Vector3(1, 1, 1);
                _ctx.WaitForFood = false;
                _ctx.Transitioning = true;
                _ctx.NextAnimation = _ctx.PreEat;
                _ctx.NextAnimationAwait = true;
                preAnimation = false;
                eating = true;
            }
        }
        else
        {
            if(!_ctx.Transitioning && !preAnimation && _ctx.SimpleCurrentAnimationProgress == 1) //start eating
            {
                _ctx.Transitioning = true;
                _ctx.NextAnimation = _ctx.Eat;
                _ctx.NextAnimationAwait = true;
                preAnimation = true;
                _ctx.MyCatFood.EatCatFood();
                _ctx.CallRandomSwitchState(); //Timer: Eating

                _ctx.Audio_Eating();
            }
            if(!_ctx.Transitioning && !postAnimation && preAnimation && _ctx.TimeUp) // finished
            {
                _ctx.Transitioning = true;
                _ctx.NextAnimation = _ctx.PostEat;
                _ctx.NextAnimationAwait = true;
                postAnimation = true;

                _ctx.MyAudioSource.Stop();
            }
            if(postAnimation & _ctx.Transitioning & _ctx.SimpleCurrentAnimationProgress == 1) // food animation
            {
                _ctx.MyCatFood.EatCatFood();
            }
        }

        

        if(postAnimation && !_ctx.Transitioning & _ctx.SimpleCurrentAnimationProgress == 1)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void ExitState()
    {
        _ctx.GetFood = false;
        _ctx.FoodOnStage = false;
        _ctx.MyAudioSource.Stop();
    }

    public override void CheckSwitchStates()
    {
        _ctx.MyAudioSource.Stop();
    }

    public override void InitializeSubState()
    {

    }
}
