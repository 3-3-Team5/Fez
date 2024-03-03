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
        if (stateMachine.MovementInput.x == 0) // 움직임이 멈췄을 경우에 idle로 전환
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.isVisible) // Player가 처음부터 가려져 있는 상태라면 앞으로 땡겨오지 않아야함.
            CheckFront();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.MoveParameterHash);
    }
}
