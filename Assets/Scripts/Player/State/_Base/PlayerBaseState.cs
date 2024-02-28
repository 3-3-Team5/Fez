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

        if (movementDirection != Vector3.zero)
        {
            player.slideDir = movementDirection.x*Camera.main.transform.right;
        }

        Move(movementDirection);
        LookRotation(movementDirection);
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = player.GetMoveSpeed;

        Vector3 cameraRight = Camera.main.transform.right;
        
        movementDirection = cameraRight * movementDirection.x; // ī�޶� �������� �̵� ������ ����

        // ��/�� �̵� - movementDirection , ��/��(����, �߷�) - ForceReceiver.Movement
        Vector3 finalMovement = movementDirection * movementSpeed + player.ForceReceiver.Movement+player.knockbackDir;
        // Z ������ �̵��� �� 0.0000007213769 ~ -0.0000008539862 ���� �߰�
        // Ư�� ������ ������ �׳� 0���� ó��
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

            if (player.isslipped) //미끄러지지 않는 부분 들어갈 때 바로 멈출 수 있도록
                controller.SimpleMove(player.slideDir * slidingSpeed);
            return;
        }

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