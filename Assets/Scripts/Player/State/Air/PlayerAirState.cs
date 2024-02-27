using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(animData.AirParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(animData.AirParameterHash);
    }

    // 메소드 이름은 JumpStarted지만 공중 상태에선 점프키를 사용하지 않기 때문에 이 메소드를 이용했음. input을 Space로 바꿔야하나?
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);

        // 여기서 레이 쏘고 결과로 벽타기 시작
        Vector3 movePosition = ClimbableCheck();
        if (movePosition != Vector3.zero)   // zero가 아니면 벽타기가 가능하기 떄문에
        {
            ChangeClimbState(movePosition);
        }
    }

    Vector3 ClimbableCheck()    // 여기서 추가해야 할점은 Ray 검사시 Layer 검사로 Climbable을 걸러내는 것을 추가 해줘야 함
    {
        RaycastHit hit;
        bool isHit = false;
        Vector3 rayStartPos = player.transform.position;
        Vector3 rayDirection = Camera.main.transform.right * player.transform.localScale.x;

        for (int i = 0; i < 3; ++i)
        {
            Vector3 modifier = Vector3.zero;
            modifier.y += i * 0.2f;

            isHit = Physics.Raycast(rayStartPos - modifier, rayDirection, out hit, player.layDistance);
            if (isHit)
            {
                Vector3 topPosition = hit.collider.bounds.max;
                float distanceToTop = topPosition.y - hit.point.y;

                if (distanceToTop < player.climbableDistance + modifier.y)
                {
                    Debug.Log($"오브젝트 상단까지의 거리 : {distanceToTop}, " +
                        $"start.y : {rayStartPos.y - modifier.y}, " +
                        $"i : {i}, " +
                        $"able : {player.climbableDistance + modifier.y}");

                    // 벽을 탈 수 있으니까 벽을 타고 올라 갔을때의 플레이어 위치를 계산해서 리턴
                    float movePosY = (distanceToTop + controller.height / 2);
                    Vector3 movePosition = hit.point + (Vector3.up * movePosY);

                    return movePosition;
                }
            }
        }

        return Vector3.zero;
    }

    void ChangeClimbState(Vector3 movePosition)
    {
        // 부딪힌 벽의 최상단으로 이동시키고 그대로 이동시키면 땅에 박혀있으니 height/2 만큼 보정을 준다.
        player.transform.position = movePosition; // 이것도 ClimbState.Enter() 실행 될 때 실행시키고 싶은데 그러면 추가해야할게 많아지는 것 같음
        stateMachine.ChangeState(stateMachine.ClimbState);
    }
}
