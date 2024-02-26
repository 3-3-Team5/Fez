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

        if (!controller.isGrounded && controller.velocity.y < 0)
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
