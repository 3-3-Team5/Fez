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

        movementDirection = player.mainCamera.transform.right * movementDirection.x; // ī�޶� �������� �̵� ������ ����

        // ��/�� �̵� - movementDirection , ��/��(����, �߷�) - ForceReceiver.Movement
        Vector3 finalMovement = movementDirection * movementSpeed + player.ForceReceiver.Movement + player.knockbackDir;
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


            // Z ������ �̵��� �� 0.0000007213769 ~ -0.0000008539862 ���� �߰�
            // Ư�� ������ ������ �׳� 0���� ó��
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
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movementDirection.x > 0)
        {
            // �������� �̵��ϰ� �ִ� ���
            player.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ReadMovementInput() // PlayerInput.Move �� �ִ� ���� �о��
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
        // ī�޶󿡼� ĳ������ �ϴ� �������� ����ĳ��Ʈ�� �ؾ��� : ī�޶� �ǹ��� ���� + �� �Ʒ� = RayCastData.DownPivot
        Vector3 cameraPos = Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY);
        Ray ray = new Ray(cameraPos, player.transform.position - cameraPos);
        RaycastHit hit;

        // ����ĳ��Ʈ ����
        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        if (Physics.Raycast(ray, out hit, RayCastData.CameraToPlayerDis, targetLayer))
        {
            // ���̳� ���� Player�� �տ� �ִٸ�.
            player.isVisible = false;
        }
        else
            player.isVisible = true;
    }

    protected Vector3 InitPlayerPosModifier(Vector3 modifier)
    {
        modifier.y = 0f; //y���� ������ �������.
        modifier.Normalize();
        modifier = modifier * (controller.radius + 0.3f); // ���� * �÷��̾��� �ݶ��̴��� ������ ��ŭ ������ ���ܿ�

        return modifier;
    }

    protected void CheckFront()
    {
        // ī�޶󿡼� ĳ������ �������� ����ĳ��Ʈ
        Vector3 rayStartPos =
            Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY); // �߽���
        // �÷��̾��� ����������κ��� ���� ��ġ��ŭ�� �տ� �ִ� ����
        Vector3 PlayerFornt = (Camera.main.transform.right * player.transform.localScale.x) *
                              RayCastData.PlayerFrontPivot;
        rayStartPos += PlayerFornt; // Player�� �������� ���� ����

        Ray ray = new Ray(rayStartPos, Camera.main.transform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ ����
        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        if (Physics.Raycast(ray, out hit, RayCastData.RayFromCameraDistance, targetLayer))
        {
            hit.point -= PlayerFornt; // ���� �浹������ �÷��̾��� ��ġ���� ��¦ �����̴ϱ� �����ظ�ŭ �ٽ� �M
            Vector3 modifier = Camera.main.transform.position - player.transform.position; // ī�޶��� ����
            modifier = InitPlayerPosModifier(modifier); // ������ �ʱ�ȭ

            // x,z ���� ���� ��ġ �̻� ���̳��ٸ� ������Ѿ���
            Vector3 newPos = player.transform.position;
            bool absX = Mathf.Abs(player.transform.position.x - hit.point.x) > controller.radius;
            bool absZ = Mathf.Abs(player.transform.position.z - hit.point.z) > controller.radius;

            // x or z ���� ���氪�� radius���� ũ�ٸ� ĳ������ ��ġ ����
            if (absX || absZ)
            {
                newPos.x = absX ? hit.point.x : newPos.x;
                newPos.z = absZ ? hit.point.z : newPos.z;

                CheckSpaceAvailability(newPos + modifier, controller);
            }
        }
    }

    // �̵� �� ������ Player�� ��� �� �� �ִ��� Ȯ��
    public bool CheckSpaceAvailability(Vector3 targetPosition, CharacterController controller)
    {
        Vector3 colliderSize = new Vector3(controller.radius * 2, controller.height, controller.radius * 2);

        LayerMask targetLayer = LayerData.Ground | LayerData.Wall;
        Collider[] hitColliders = Physics.OverlapBox(targetPosition + controller.center, colliderSize / 2,
            Quaternion.identity, targetLayer);
        if (hitColliders.Length > 0)
        {
            //Debug.Log("Can't Move");
            return false;
        }
        else
        {
            // ������ ��� ����
            //Debug.Log("Can Move");
            player.transform.position = targetPosition;
            return true;
        }
    }
}

