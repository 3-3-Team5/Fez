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

        // ClimbState가 아닐 때만 떨어지게 만들어야함 이 조건이 들어가지 않으면 ClimbState가 바로 종료됨
        // 조건문 하나에 다 넣지 않은 이유 : 불필요한 !controller.isGrounded && controller.velocity.y < -0.5f 연산을 하지 않게끔
        // 지금 땅을 밟고있지 않을 경우에 FallState로
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
