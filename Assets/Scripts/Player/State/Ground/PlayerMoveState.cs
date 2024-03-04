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
        if (stateMachine.MovementInput.x == 0) // �������� ������ ��쿡 idle�� ��ȯ
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.isVisible) // Player�� ó������ ������ �ִ� ���¶�� ������ ���ܿ��� �ʾƾ���.
            CheckFront();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.MoveParameterHash);
    }
}
