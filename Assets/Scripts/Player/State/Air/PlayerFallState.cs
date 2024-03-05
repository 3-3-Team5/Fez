using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{

    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.audioManager.PlayFallFX();
        StartAnimation(animData.fallParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (controller.isGrounded)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        CheckUnderFoot();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animData.fallParameterHash);
    }

    void CheckUnderFoot()
    {
        // ī�޶󿡼� ĳ������ �ϴ� �������� ����ĳ��Ʈ�� �ؾ��� : ī�޶� �ǹ��� ���� + �� �Ʒ� = RayCastData.DownPivot
        Vector3 targetPos = Camera.main.transform.position + (Vector3.down * RayCastData.DownPivot);
        Ray ray = new Ray(targetPos, Camera.main.transform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ ����
        LayerMask targetLayer = LayerData.Ground;
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, targetLayer))
        {
            Vector3 closetPoint = hit.transform.GetComponent<Collider>().ClosestPoint(player.transform.position);
            closetPoint.y = player.transform.position.y;

            Vector3 modifier; // �÷��̾ ������ �Ŀ� ��/�� �ݶ��̴���ŭ ���� ��������
            // �÷��̾��� ���� ����
            Vector3 playerForward = player.transform.forward;
            // �÷��̾�� ���� ����� ������Ʈ ������ ����
            Vector3 toTarget = (closetPoint - player.transform.position).normalized;
            // ������ ���� ���
            float dotProduct = Vector3.Dot(playerForward, toTarget);
            if (dotProduct > 0)
            {
                // �ε��� ��Ұ� �÷��̾�� �տ� �ִٸ�
                modifier =  Camera.main.transform.position - player.transform.position;
            }
            else
            {
                // �ε��� ��Ұ� �÷��̾�� �ڿ� �ִٸ�
                modifier = player.transform.position - Camera.main.transform.position;
            }
            modifier = InitPlayerPosModifier(modifier); // ������ �ʱ�ȭ

            bool absX = Mathf.Abs(player.transform.position.x - closetPoint.x) > controller.radius;
            bool absZ = Mathf.Abs(player.transform.position.z - closetPoint.z) > controller.radius;
            // ĳ������ ��ġ ����
            if (absX || absZ)
            {
                closetPoint.x = absX ? closetPoint.x : player.transform.position.x;
                closetPoint.z = absZ ? closetPoint.z : player.transform.position.z;

                CheckSpaceAvailability(closetPoint + modifier, controller);
            }
        }
    }

}
