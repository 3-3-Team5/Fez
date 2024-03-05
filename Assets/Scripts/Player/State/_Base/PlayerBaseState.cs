using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;

    protected Player player;
    protected PlayerInput input;
    protected PlayerAnimationData animData;
    protected CharacterController controller;
    protected ForceReceiver forceReceiver;

    private bool isSliding = false;
    private float slideTime = 2f; // 미끄러지는 동작 지속 시간
    private float slideTimer = 0f;
    protected float slidingSpeed = 2f;


    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        player = stateMachine.Player;
        input = stateMachine.Player.Input;
        animData = stateMachine.Player.AnimationData;
        controller = stateMachine.Player.Controller;
        forceReceiver = stateMachine.Player.ForceReceiver;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks(); // 인풋 처리를 위한 이벤트 추가
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks(); // 상태가 변경되니 현재 등록되어 있는 상태의 이벤트 제거
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
        Move();

        //if (stateMachine.GetCurState() != stateMachine.IdleState)
        CheckVisible();
    }

    protected virtual void AddInputActionsCallbacks()
    {
        input.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        input.PlayerActions.Jump.started -= OnJumpStarted;
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        if (movementDirection != Vector3.zero)
        {
            player.slideDir = movementDirection.x * player.mainCamera.transform.right;
        }

        Move(movementDirection);
        LookRotation(movementDirection);
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = player.GetMoveSpeed;

        movementDirection = player.mainCamera.transform.right * movementDirection.x; // 카메라 기준으로 이동 방향을 설정

        // 좌/우 이동 - movementDirection , 상/하(점프, 중력) - ForceReceiver.Movement
        Vector3 finalMovement = movementDirection * movementSpeed + player.ForceReceiver.Movement + player.knockbackDir;
        if (player.slideDir != Vector3.zero && player.isslipped && Mathf.Approximately(finalMovement.magnitude, 0f) &&
            !isSliding)
            // 이동이 종료되고, 미끄러운 상태라면
        {
            isSliding = true;
            slideTimer = 0f;
        }

        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer >= slideTime)
            {
                isSliding = false;
                player.slideDir = Vector3.zero;
                return;
            }


            // Z 축으로 이동할 때 0.0000007213769 ~ -0.0000008539862 오차 발견
            // 특정 수보다 작으면 그냥 0으로 처리
            if (Mathf.Abs(finalMovement.x) < 1e-6f) // 1e-6f = 0.000001
                finalMovement.x = 0;
            if (Mathf.Abs(finalMovement.z) < 1e-6f) // 1e-6f = 0.000001
                finalMovement.z = 0;

            if (player.isslipped) //미끄러지지 않는 부분 들어갈 때 바로 멈출 수 있도록
                controller.SimpleMove(player.slideDir * slidingSpeed);
            return;
        }


        controller.Move(finalMovement * Time.deltaTime); // ������ ����
    }

    private void LookRotation(Vector3 movementDirection)
    {
        Transform cameraTransform = player.mainCamera.transform;
        // 카메라의 위치를 기준으로 캐릭터가 바라볼 방향 계산
        Vector3 direction = cameraTransform.position - player.transform.position;
        direction.y = 0; // 수평만 바라보게 하기 위해서

        // 캐릭터가 카메라 방향을 바라보도록 회전
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        player.transform.rotation = lookRotation;

        // 캐릭터의 좌/우 바라보는 방향 설정
        if (movementDirection.x < 0) // 움직임이 없을떄는 그냥 냅두기 위해서 0인 경우는 처리하지 않음.
        {
            // 좌측으로 이동하고 있는 경우
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movementDirection.x > 0)
        {
            // 우측으로 이동하고 있는 경우
            player.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ReadMovementInput() // PlayerInput.Move 에 있는 값을 읽어옴
    {
        stateMachine.MovementInput = input.PlayerActions.Move.ReadValue<Vector2>();
    }

    protected Vector2 GetMovementDirection()
    {
        return stateMachine.MovementInput;
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        //Debug.Log("input Jump");
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }


    void CheckVisible()
    {
        // 카메라에서 캐릭터의 하단 지점으로 레이캐스트를 해야함 : 카메라 피벗의 높이 + 발 아래 = RayCastData.DownPivot
        Vector3 cameraPos = Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY);
        Ray ray = new Ray(cameraPos, player.transform.position - cameraPos);
        RaycastHit hit;

        // 레이캐스트 수행
        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        if (Physics.Raycast(ray, out hit, RayCastData.CameraToPlayerDis, targetLayer))
        {
            // 땅이나 벽이 Player의 앞에 있다면.
            player.isVisible = false;
        }
        else
            player.isVisible = true;
    }

    protected Vector3 InitPlayerPosModifier(Vector3 modifier)
    {
        modifier.y = 0f; // y축은 변경이 없어야함.
        modifier.Normalize();
        modifier = modifier * (controller.radius + 0.3f); // 방향 * 플레이어의 콜라이더의 반지름 만큼 앞으로 땡겨옴

        return modifier;
    }

    protected void CheckFront()
    {
        // 카메라에서 캐릭터의 정면으로 레이캐스트
        Vector3 rayStartPos =
            Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY); // 중심점
        //플레이어의 진행방향으로부터 일정 수치만큼의 앞에 있는 지점
        Vector3 PlayerFornt = (Camera.main.transform.right * player.transform.localScale.x) *
                              RayCastData.PlayerFrontPivot;
        rayStartPos += PlayerFornt; // Player�� �������� ���� ����

        Ray ray = new Ray(rayStartPos, Camera.main.transform.forward);
        RaycastHit hit;

        // 레이캐스트 수행
        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, targetLayer))
        {
            hit.point -= PlayerFornt; // 실제 충돌지점은 플레이어의 위치보다 살짝 앞쪽이니까 더해준만큼 다시 뻄
            Vector3 modifier = Camera.main.transform.position - player.transform.position; // 카메라의 방향
            modifier = InitPlayerPosModifier(modifier); // 수정자 초기화

            // x,z 축이 일정 수치 이상 차이난다면 변경시켜야함
            Vector3 newPos = player.transform.position;
            bool absX = Mathf.Abs(player.transform.position.x - hit.point.x) > controller.radius + ModifierCollection.RadiusModifier;
            bool absZ = Mathf.Abs(player.transform.position.z - hit.point.z) > controller.radius + ModifierCollection.RadiusModifier;

            //  x or z 축의 변경값이 radius보다 크다면 캐릭터의 위치 변경
            if (absX || absZ)
            {
                newPos.x = absX ? hit.point.x : newPos.x;
                newPos.z = absZ ? hit.point.z : newPos.z;

                CheckSpaceAvailability(newPos + modifier, controller);
            }
        }
    }

    // 이동 할 공간이 Player가 들어 갈 수 있는지 확인
    public bool CheckSpaceAvailability(Vector3 targetPosition, CharacterController controller)
    {
        Vector3 colliderSize = new Vector3(controller.radius * 2, controller.height, controller.radius * 2);

        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        Collider[] hitColliders = Physics.OverlapBox(targetPosition + controller.center, colliderSize / 2,
            Quaternion.identity, targetLayer);
        if (hitColliders.Length > 0)
        {
            // 공간에 다른 콜라이더가 있음
            //Debug.Log("Can't Move");
            return false;
        }
        else
        {
            // 공간이 비어 있음
            //Debug.Log("Can Move");
            player.transform.position = targetPosition;
            return true;
        }
    }
}

