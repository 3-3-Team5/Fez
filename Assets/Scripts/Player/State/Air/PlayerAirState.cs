using System.Collections;
using System.Collections.Generic;

using UnityEditor.PackageManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerAirState : PlayerBaseState
{
    protected bool frontCheck = true;
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(animData.AirParameterHash);
        frontCheck = true;
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.isVisible) // Player�� ó������ ������ �ִ� ���¶�� ������ ���ܿ��� �ʾƾ���.
        {
            if (frontCheck)
            {
                CheckFront();
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (player.isKnockback)
        {
            stateMachine.ChangeState(stateMachine.HitState);
        }
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

    Vector3 ClimbableCheck()
    {
        RaycastHit hit;
        bool isHit = false;
        Vector3 rayStartPos = Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY); // �߽���
        Vector3 PlayerFornt = (Camera.main.transform.right * player.transform.localScale.x) * RayCastData.PlayerFrontPivot;
        rayStartPos += PlayerFornt; // Player�� �������� ���� ����
        Ray ray = new Ray(rayStartPos, Camera.main.transform.forward);

        for (int i = 0; i < 3; ++i)
        {
            Vector3 modifier = Vector3.zero;
            modifier.y += i * 0.1f;

            LayerMask targetLayer = LayerData.Ground;
            isHit = Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, targetLayer);
            if (isHit)
            {
                // Tag�� �߰�?
                Vector3 topPosition = hit.collider.bounds.max;
                float distanceToTop = topPosition.y - hit.point.y;

                if (distanceToTop < RayCastData.ClimbableDistance + modifier.y)
                {
                    //Debug.Log($"������Ʈ ��ܱ����� �Ÿ� : {distanceToTop}, " +
                    //    $"start.y : {rayStartPos.y - modifier.y}, " +
                    //    $"i : {i}, " +
                    //    $"able : {player.climbableDistance + modifier.y}");

                    // ���� Ż �� �����ϱ� ���� Ÿ�� �ö� �������� �÷��̾� ��ġ�� ����ؼ� ����
                    float movePosY = (distanceToTop + controller.height);
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
        CheckSpaceAvailability(movePosition, controller);
        stateMachine.ChangeState(stateMachine.ClimbState);
    }
}
