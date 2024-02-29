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
        // 카메라에서 캐릭터의 하단 지점으로 레이캐스트를 해야함 : 카메라 피벗의 높이 + 발 아래 = RayCastData.DownPivot
        Vector3 targetPos = Camera.main.transform.position + (Vector3.down * RayCastData.DownPivot);
        Ray ray = new Ray(targetPos, Camera.main.transform.forward);
        RaycastHit hit;

        // 레이캐스트 수행
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, LayerData.Ground))
        {
            // 카메라의 방향 (Player의 뒤쪽으로 이동해야해서) 반대로 뺏음
            Vector3 modifier = player.transform.position - Camera.main.transform.position;
            modifier.y = 0f; //y축은 변경이 없어야함.
            modifier.Normalize();
            modifier = modifier * (controller.radius + 0.1f); // 방향 * 플레이어의 콜라이더의 반지름 만큼 앞으로 땡겨옴

            // 이제 여기서 캐릭터의 위치를 옮기면 될듯
            player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z) + modifier;
            //Debug.Log($"Change : {hit.transform.name}, Layer : {hit.transform.gameObject.layer}");
            //Debug.Log($"Change : {new Vector3(hit.point.x, player.transform.position.y, hit.point.z)}, Hit : {hit.point}");
        }
    }
}
