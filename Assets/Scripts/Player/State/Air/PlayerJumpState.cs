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

        if (player.isVisible)
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
        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        if (!player.isVisible) // Player�� ���̰� ���� �ʴٸ� �� ������ ���ܿ��°� ���� �ʰ�.
            targetLayer = LayerData.Ground;

        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, targetLayer))
        {
            Vector3 modifier = Camera.main.transform.position - player.transform.position; // ī�޶��� ����
            modifier = InitPlayerPosModifier(modifier); // ������ �ʱ�ȭ

            // ���� ���⼭ ĳ������ ��ġ ����
            player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z) + modifier;
            Debug.Log("Jump");
        }
    }
}
