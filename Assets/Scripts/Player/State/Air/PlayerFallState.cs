using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{

    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

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
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, LayerData.Ground))
        {
            // ī�޶��� ���� (Player�� �������� �̵��ؾ��ؼ�) �ݴ�� ����
            Vector3 modifier = player.transform.position - Camera.main.transform.position;
            modifier.y = 0f; //y���� ������ �������.
            modifier.Normalize();
            modifier = modifier * (controller.radius + 0.1f); // ���� * �÷��̾��� �ݶ��̴��� ������ ��ŭ ������ ���ܿ�

            // ���� ���⼭ ĳ������ ��ġ�� �ű�� �ɵ�
            player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z) + modifier;
            //Debug.Log($"Change : {hit.transform.name}, Layer : {hit.transform.gameObject.layer}");
            //Debug.Log($"Change : {new Vector3(hit.point.x, player.transform.position.y, hit.point.z)}, Hit : {hit.point}");
        }
    }
}
