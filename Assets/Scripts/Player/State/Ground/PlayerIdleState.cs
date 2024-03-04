using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(animData.IdleParameterHash);
    }

    public override void Update()
    {
        
        if(stateMachine.MovementInput.x != 0)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.IdleParameterHash);
    }
}
