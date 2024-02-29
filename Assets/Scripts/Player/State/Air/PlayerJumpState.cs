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

        // Jump ����
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        CheckOverHead();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.JumpParameterHash);
    }

    void CheckOverHead()
    {
        // ī�޶󿡼� ĳ������ ��� �������� ����ĳ��Ʈ�� �ؾ���
        Vector3 targetPos = Camera.main.transform.position + (Vector3.up * RayCastData.UpPivot);
        Ray ray = new Ray(targetPos, Camera.main.transform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ ����
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance,
            LayerData.Ground | LayerData.Wall))
        {
            Vector3 modifier = Camera.main.transform.position - player.transform.position; // ī�޶��� ����
            modifier.y = 0f; //y���� ������ �������.
            modifier.Normalize();
            modifier = modifier * (controller.radius + 0.1f); // ���� * �÷��̾��� �ݶ��̴��� ������ ��ŭ ������ ���ܿ�
            // ���� ���⼭ ĳ������ ��ġ�� �ű�� �ɵ�
            player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z) + modifier;
            //Debug.Log($"Change : {hit.transform.name}, Layer : {hit.transform.gameObject.layer}");
            //Debug.Log($"Change : {player.transform.position}, Hit : {hit.point}");
        }
    }
}
