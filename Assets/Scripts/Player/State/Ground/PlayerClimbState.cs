using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ClimbState�� Air�� ��ӹ����� Ground�� ��� ������ ����� �ߴµ�
// Ground�� ��ӹ��� ���� : ������ Air���� �϶��� ���������� �����Ⱑ �Ϸ�� �� �ٷ� idle ���·� ���⶧���� Ground�� ��ӹް� ������
// ��� Air��� ũ�� �ٸ��� ���� �� ����. �ƴϸ� �׳� ����� �ް����� �ʰ� �������� ���·� ����°� ����������?
public class PlayerClimbState : PlayerGroundState   
{
    public PlayerClimbState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    bool isClimbing;

    public override void Enter()
    {
        base.Enter();
        StartAnimation(animData.ClimbParameterHash);

        Debug.Log("Climb Start");

        isClimbing = true;
        player.SetPlayerControlEnabled(false);// Ŭ���̹� ���߿� �Է°� �̵��� �����ϱ� ���ؼ�.
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
        Debug.Log("Climb end");
        StopAnimation(animData.ClimbParameterHash);

        player.SetPlayerControlEnabled(true);// Ŭ���̹� ���߿� �Է°� �̵� ���� ����
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        // ClimbState������ ������ �ϰ� ���� �ʱ� ����
    }
}
