using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimbState : PlayerGroundState
{
    public PlayerClimbState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    bool isClimbing;

    public override void Enter()
    {
        isClimbing = true;

        base.Enter();

        StartAnimation(animData.ClimbParameterHash);
        controller.enabled = false; // �Ǵ� base���� ���� �ɾ ����?
    }

    public override void Update()
    {
        base.Update();

        isClimbing = player.Animator.GetBool(animData.ClimbParameterHash);

        if (!isClimbing)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.ClimbParameterHash);
        controller.enabled = true;  // �Ʊ� ���°� ���� �� �� ��Ʈ�ѷ��� ������ �ٽ� ����
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        
    }
}
