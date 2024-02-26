using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(animData.MoveParameterHash);
    }

    public override void Update()
    {
        if (stateMachine.MovementInput.x == 0)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.MoveParameterHash);
    }
}
