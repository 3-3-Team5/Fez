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
        // 카메라에서 캐릭터의 상단 지점으로 레이캐스트를 해야함
        Vector3 targetPos = Camera.main.transform.position + (Vector3.up * RayCastData.UpPivot);
        Ray ray = new Ray(targetPos, Camera.main.transform.forward);
        RaycastHit hit;

        // 레이캐스트 수행
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance,
            LayerData.Ground | LayerData.Wall))
        {
            Vector3 modifier = Camera.main.transform.position - player.transform.position; // 카메라의 방향
            modifier.y = 0f; //y축은 변경이 없어야함.
            modifier.Normalize();
            modifier = modifier * (controller.radius + 0.1f); // 방향 * 플레이어의 콜라이더의 반지름 만큼 앞으로 땡겨옴
            // 이제 여기서 캐릭터의 위치를 옮기면 될듯
            player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z) + modifier;
            //Debug.Log($"Change : {hit.transform.name}, Layer : {hit.transform.gameObject.layer}");
            //Debug.Log($"Change : {player.transform.position}, Hit : {hit.point}");
        }
    }
}
