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
        if (player.isVisible)
            CheckOverHead();

        base.PhysicsUpdate();
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
            // ���⵵ �����ϴ� ������ ��ġ�� ���� ���� �����ؾ��ҵ�?
            Vector3 modifier = Camera.main.transform.position - player.transform.position; // ī�޶��� ����
            modifier = InitPlayerPosModifier(modifier); // ������ �ʱ�ȭ

            Vector3 newPos = player.transform.position;
            bool absX = Mathf.Abs(player.transform.position.x - hit.point.x) > controller.radius;
            bool absZ = Mathf.Abs(player.transform.position.z - hit.point.z) > controller.radius;
            // ���� ���⼭ ĳ������ ��ġ ����
            if (absX || absZ)
            {
                newPos.x = absX ? hit.point.x : newPos.x;
                newPos.z = absZ ? hit.point.z : newPos.z;

                CheckSpaceAvailability(newPos + modifier, controller);
            }
            frontCheck = false;
        }
        else
            frontCheck = true;
    }
}
