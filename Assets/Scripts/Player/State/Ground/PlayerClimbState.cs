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
        controller.enabled = false; // 또는 base에서 조건 걸어서 수정?
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
        controller.enabled = true;  // 아까 상태가 시작 될 때 컨트롤러를 껐으니 다시 켜줌
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        
    }
}
