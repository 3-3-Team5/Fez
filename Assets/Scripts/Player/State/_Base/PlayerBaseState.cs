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
        Vector2 movementDirection = GetMovementDirection();

        Move(movementDirection);
        LookRotation(movementDirection);
    }

    private void Move(Vector2 movementDirection)
    {
        float movementSpeed = player.GetMoveSpeed;

        controller.Move(
            ((movementDirection * movementSpeed ) // x축 이동
            + (stateMachine.Player.ForceReceiver.Movement))// Y축 점프 넣어야 점프 제대로 될듯
            * Time.deltaTime);
    }

    private void LookRotation(Vector2 movementDirection)
    {
        Quaternion rot = stateMachine.Player.transform.rotation; // 캐릭터가 바라보는 방향 수정 flipX는 콜라이더는 반전이 안되던걸로 기억나서 부모 오브젝트의 rot 값을 건드림
        if (movementDirection.x > 0)
            rot.y = 0;
        else if (movementDirection.x < 0)
            rot.y = 180f;
        stateMachine.Player.transform.rotation = rot;
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
