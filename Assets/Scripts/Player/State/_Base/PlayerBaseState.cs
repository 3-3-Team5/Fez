using System;
using System.Collections;
using System.Collections.Generic;
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

        Move(movementDirection);
        LookRotation(movementDirection);
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = player.GetMoveSpeed;

        Vector3 cameraRight = Camera.main.transform.right;
        movementDirection = cameraRight * movementDirection.x; // 카메라 기준으로 이동 방향을 설정

        // 좌/우 이동 - movementDirection , 상/하(점프, 중력) - ForceReceiver.Movement
        Vector3 finalMovement = movementDirection * movementSpeed + player.ForceReceiver.Movement;

        // Z 축으로 이동할 때 0.0000007213769 ~ -0.0000008539862 오차 발견
        // 특정 수보다 작으면 그냥 0으로 처리
        if (Mathf.Abs(finalMovement.x) < 1e-6f) // 1e-6f = 0.000001
            finalMovement.x = 0;

        controller.Move(finalMovement * Time.deltaTime); // 움직임 지정
    }

    private void LookRotation(Vector3 movementDirection)
    {
        Transform cameraTransform = Camera.main.transform;
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
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movementDirection.x > 0)
        {
            // 우측으로 이동하고 있는 경우
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ReadMovementInput() // PlayerInput.Move 에 있는 값을 읽어옴
    {
        stateMachine.MovementInput = input.PlayerActions.Move.ReadValue<Vector2>();
    }

    private Vector2 GetMovementDirection()
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
}
