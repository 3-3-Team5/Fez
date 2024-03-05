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
        // 카메라에서 캐릭터의 하단 지점으로 레이캐스트를 해야함 : 카메라 피벗의 높이 + 발 아래 = RayCastData.DownPivot
        Vector3 targetPos = Camera.main.transform.position + (Vector3.down * RayCastData.DownPivot);
        Ray ray = new Ray(targetPos, Camera.main.transform.forward);
        RaycastHit hit;

        // 레이캐스트 수행
        LayerMask targetLayer = LayerData.Ground;
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, targetLayer))
        {
            Vector3 closetPoint = hit.transform.GetComponent<Collider>().ClosestPoint(player.transform.position);
            closetPoint.y = player.transform.position.y;

            Vector3 modifier; // 플레이어가 움직인 후에 앞/뒤 콜라이더만큼 어디로 움직일지
            // 플레이어의 전방 벡터
            Vector3 playerForward = player.transform.forward;
            // 플레이어와 가장 가까운 오브젝트 사이의 벡터
            Vector3 toTarget = (closetPoint - player.transform.position).normalized;
            // 벡터의 내적 계산
            float dotProduct = Vector3.Dot(playerForward, toTarget);
            if (dotProduct > 0)
            {
                // 부딪힌 장소가 플레이어보다 앞에 있다면
                modifier =  Camera.main.transform.position - player.transform.position;
            }
            else
            {
                // 부딪힌 장소가 플레이어보다 뒤에 있다면
                modifier = player.transform.position - Camera.main.transform.position;
            }
            modifier = InitPlayerPosModifier(modifier); // 수정자 초기화

            bool absX = Mathf.Abs(player.transform.position.x - closetPoint.x) > controller.radius;
            bool absZ = Mathf.Abs(player.transform.position.z - closetPoint.z) > controller.radius;
            // 캐릭터의 위치 변경
            if (absX || absZ)
            {
                closetPoint.x = absX ? closetPoint.x : player.transform.position.x;
                closetPoint.z = absZ ? closetPoint.z : player.transform.position.z;

                CheckSpaceAvailability(closetPoint + modifier, controller);
            }
        }
    }

}
