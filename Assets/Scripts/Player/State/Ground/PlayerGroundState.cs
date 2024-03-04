using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(animData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();
        
        if (player.isKnockback)
        {
            stateMachine.ChangeState(stateMachine.HitState);
            return;
        }

        if (player.isWarp)
        {
            stateMachine.ChangeState(stateMachine.WarpState);
            return;
        }

        // ClimbState�� �ƴ� ���� �������� �������� �� ������ ���� ������ ClimbState�� �ٷ� �����
        // ���ǹ� �ϳ��� �� ���� ���� ���� : ���ʿ��� !controller.isGrounded && controller.velocity.y < -0.5f ������ ���� �ʰԲ�
        // ���� ���� ������� ���� ��쿡 FallState��
        if (stateMachine.GetCurState() != stateMachine.ClimbState)
        {
            if (!controller.isGrounded && controller.velocity.y < -0.5f)
                stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    public override void Exit() 
    { 
        base.Exit(); 

        StopAnimation(animData.GroundParameterHash);

        player.saveTransform.SetTransform(player.transform);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }
    
    
}
