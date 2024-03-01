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

        if (!controller.isGrounded && controller.velocity.y < 0) // 지금 땅을 밟고있지 않을 경우에 FallState로
            stateMachine.ChangeState(stateMachine.FallState);
    }

    public override void Exit() 
    { 
        base.Exit(); 

        StopAnimation(animData.GroundParameterHash);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }
    
    
}
