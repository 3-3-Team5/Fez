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
        Vector3 movementDirection = GetMovementDirection();

        Move(movementDirection);
        LookRotation(movementDirection);
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = player.GetMoveSpeed;

        Vector3 cameraRight = Camera.main.transform.right;
        movementDirection = cameraRight * movementDirection.x; // ī�޶� �������� �̵� ������ ����

        // ��/�� �̵� - movementDirection , ��/��(����, �߷�) - ForceReceiver.Movement
        Vector3 finalMovement = movementDirection * movementSpeed + player.ForceReceiver.Movement;

        // Z ������ �̵��� �� 0.0000007213769 ~ -0.0000008539862 ���� �߰�
        // Ư�� ������ ������ �׳� 0���� ó��
        if (Mathf.Abs(finalMovement.x) < 1e-6f) // 1e-6f = 0.000001
            finalMovement.x = 0;

        controller.Move(finalMovement * Time.deltaTime); // ������ ����
    }

    private void LookRotation(Vector3 movementDirection)
    {
        Transform cameraTransform = Camera.main.transform;
        // ī�޶��� ��ġ�� �������� ĳ���Ͱ� �ٶ� ���� ���
        Vector3 direction = cameraTransform.position - player.transform.position;
        direction.y = 0; // ���� �ٶ󺸰� �ϱ� ���ؼ�

        // ĳ���Ͱ� ī�޶� ������ �ٶ󺸵��� ȸ��
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        player.transform.rotation = lookRotation;

        // ĳ������ ��/�� �ٶ󺸴� ���� ����
        if (movementDirection.x < 0) // �������� �������� �׳� ���α� ���ؼ� 0�� ���� ó������ ����.
        {
            // �������� �̵��ϰ� �ִ� ���
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movementDirection.x > 0)
        {
            // �������� �̵��ϰ� �ִ� ���
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
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
