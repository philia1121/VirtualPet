using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDraggedState : CatBaseState
{
    public CatDraggedState(CatStateMachine currentContext, CatStateFactory stateFactory)
    : base (currentContext, stateFactory){}

    public override void EnterState()
    {
        Debug.Log("Enter Dragged State");
        _ctx.Transitioning = true;
        _ctx.NextAnimation_str = "idle";
        _ctx.NextAnimationAwait = true;
        _ctx.DragBanned = true;
        _ctx.StopRandomTimer();
    }

    public override void UpdateState()
    {
        
        Vector3 mousePos = Input.mousePosition;
        _ctx.CatTransform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y, _ctx.CatTransform.position.z);
        
        if(!_ctx.BeDragged)
        {
            SwitchState(_factory.Idle());
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
