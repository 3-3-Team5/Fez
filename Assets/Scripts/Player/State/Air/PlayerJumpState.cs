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

        // Jump 시작
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
        // 카메라에서 캐릭터의 상단 지점으로 레이캐스트를 해야함
        Vector3 targetPos = Camera.main.transform.position + (Vector3.up * RayCastData.UpPivot);
        Ray ray = new Ray(targetPos, Camera.main.transform.forward);
        RaycastHit hit;

        // 레이캐스트 수행
        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        if (!player.isVisible) // Player가 보이고 있지 않다면 벽 앞으로 땡겨오는건 하지 않게.
            targetLayer = LayerData.Ground;

        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, targetLayer))
        {
            // 여기도 가야하는 지점의 위치에 따라 값을 조정해야할듯?
            Vector3 modifier = Camera.main.transform.position - player.transform.position; // 카메라의 방향
            modifier = InitPlayerPosModifier(modifier); // 수정자 초기화

            Vector3 newPos = player.transform.position;
            bool absX = Mathf.Abs(player.transform.position.x - hit.point.x) > controller.radius;
            bool absZ = Mathf.Abs(player.transform.position.z - hit.point.z) > controller.radius;
            // 이제 여기서 캐릭터의 위치 변경
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
