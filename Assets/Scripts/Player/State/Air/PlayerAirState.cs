using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(animData.AirParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.AirParameterHash);
    }

    // �޼ҵ� �̸��� JumpStarted���� ���� ���¿��� ����Ű�� ������� �ʱ� ������ �� �޼ҵ带 �̿�����. input�� Space�� �ٲ���ϳ�?
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);

        // ���⼭ ���� ��� ����� ��Ÿ�� ����
        Vector3 movePosition = ClimbableCheck();
        if (movePosition != Vector3.zero)   // zero�� �ƴϸ� ��Ÿ�Ⱑ �����ϱ� ������
        {
            ChangeClimbState(movePosition);
        }
    }

    Vector3 ClimbableCheck()    // ���⼭ �߰��ؾ� ������ Ray �˻�� Layer �˻�� Climbable�� �ɷ����� ���� �߰� ����� ��
    {
        RaycastHit hit;
        bool isHit = false;
        Vector3 rayStartPos = player.transform.position;
        Vector3 rayDirection = Camera.main.transform.right * player.transform.localScale.x;

        for (int i = 0; i < 3; ++i)
        {
            Vector3 modifier = Vector3.zero;
            modifier.y += i * 0.2f;

            isHit = Physics.Raycast(rayStartPos - modifier, rayDirection, out hit, player.layDistance);
            if (isHit)
            {
                Vector3 topPosition = hit.collider.bounds.max;
                float distanceToTop = topPosition.y - hit.point.y;

                if (distanceToTop < player.climbableDistance + modifier.y)
                {
                    Debug.Log($"������Ʈ ��ܱ����� �Ÿ� : {distanceToTop}, " +
                        $"start.y : {rayStartPos.y - modifier.y}, " +
                        $"i : {i}, " +
                        $"able : {player.climbableDistance + modifier.y}");

                    // ���� Ż �� �����ϱ� ���� Ÿ�� �ö� �������� �÷��̾� ��ġ�� ����ؼ� ����
                    float movePosY = (distanceToTop + controller.height / 2);
                    Vector3 movePosition = hit.point + (Vector3.up * movePosY);

                    return movePosition;
                }
            }
        }

        return Vector3.zero;
    }

    void ChangeClimbState(Vector3 movePosition)
    {
        // �ε��� ���� �ֻ������ �̵���Ű�� �״�� �̵���Ű�� ���� ���������� height/2 ��ŭ ������ �ش�.
        player.transform.position = movePosition; // �̰͵� ClimbState.Enter() ���� �� �� �����Ű�� ������ �׷��� �߰��ؾ��Ұ� �������� �� ����
        stateMachine.ChangeState(stateMachine.ClimbState);
    }
}
