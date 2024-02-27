using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{

    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(animData.JumpParameterHash);

        // Jump Ω√¿€
        forceReceiver.Jump(player.GetJumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (!controller.isGrounded && controller.velocity.y < 0)
            stateMachine.ChangeState(stateMachine.FallState);

        if (controller.isGrounded && controller.velocity.y < 0)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.JumpParameterHash);
    }
}
