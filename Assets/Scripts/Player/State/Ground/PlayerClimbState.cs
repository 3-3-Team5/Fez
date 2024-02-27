using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ClimbState를 Air를 상속받을지 Ground를 상속 받을지 고민을 했는데
// Ground를 상속받은 이유 : 시작은 Air상태 일때만 시작하지만 오르기가 완료된 후 바로 idle 상태로 가기때문에 Ground를 상속받게 했지만
// 사실 Air였어도 크게 다를건 없을 것 같다. 아니면 그냥 상속을 받게하지 않고 독자적인 상태로 만드는게 나았으려나?
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
        // ClimbState에서는 점프를 하게 하지 않기 위해
    }
}
