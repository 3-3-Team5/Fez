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
        AddInputActionsCallbacks(); // ��ǲ ó���� ���� �̺�Ʈ �߰�
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks(); // ���°� ����Ǵ� ���� ��ϵǾ� �ִ� ������ �̺�Ʈ ����
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
            ((movementDirection * movementSpeed ) // x�� �̵�
            + (stateMachine.Player.ForceReceiver.Movement))// Y�� ���� �־�� ���� ����� �ɵ�
            * Time.deltaTime);
    }

    private void LookRotation(Vector2 movementDirection)
    {
        Quaternion rot = stateMachine.Player.transform.rotation; // ĳ���Ͱ� �ٶ󺸴� ���� ���� flipX�� �ݶ��̴��� ������ �ȵǴ��ɷ� ��ﳪ�� �θ� ������Ʈ�� rot ���� �ǵ帲
        if (movementDirection.x > 0)
            rot.y = 0;
        else if (movementDirection.x < 0)
            rot.y = 180f;
        stateMachine.Player.transform.rotation = rot;
    }

    private void ReadMovementInput() // PlayerInput.Move �� �ִ� ���� �о��
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
