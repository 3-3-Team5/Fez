using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public CharacterSO stats; // baseStat
    public float moveSpeedModifier = 1f;
    public float jumpForceModifier = 1f;
    public float GetMoveSpeed => stats.baseStats.MovementSpeed * moveSpeedModifier;
    public float GetJumpForce => stats.baseStats.jumpForce * jumpForceModifier;
    public Animator Animator { get; private set; }
    public PlayerInput Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    [HideInInspector] public bool isslipped = false;
    [HideInInspector] public Vector3 slideDir = Vector3.zero;
    [HideInInspector] public Vector3 knockbackDir = Vector3.zero;

    public bool isKnockback = false;

    public float knockbackPower;

    public Camera mainCamera;


    PlayerStateMachine stateMachine;

    private void Awake()
    {
        AnimationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        stateMachine = new(this);
    }

    void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        mainCamera = Camera.main;
    }

    void Update()
    {
        stateMachine?.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) //미끄러짐 구현에 필요한 메서드
    {
        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Water"))
        {
            isslipped = true;
        }
        else isslipped = false;

        if (hit.gameObject.TryGetComponent<DisappearBlock>(out DisappearBlock disappearBlock))
        {
            if (Mathf.Approximately(hit.point.y, hit.gameObject.GetComponent<Collider>().bounds.max.y))
                disappearBlock.StartAnim();
        }

        //두개 겹치는 경우 태그와 레이어로 관리해보자. 
        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Trap"))
        {
            if (!isKnockback)
            {
                Vector3 cameraRightabs = mainCamera.transform.right.Abs(); //Camera.main 캐싱해서 사용 Abs
                isKnockback = true;
                Vector3 knockback = (hit.point - hit.collider.bounds.center).normalized;
                //카메라에 귀속되는게 아니라 충돌에 귀속되게 코드를 작성해야한다.
                //4방향을 체크해서 스위치를 돌리수도 있고, 카메라가 어떤 카메라인지 알고 해야함. 
                //knockbackDir = cameraRight.x == 0 ? ((knockback.z * cameraRight)+ knockback.y*Vector3.up)*knockbackPower : ((knockback.x * cameraRight)+ knockback.y*Vector3.up)*knockbackPower;
                if (cameraRightabs.x > cameraRightabs.z)
                {
                    knockbackDir = ((knockback.x * cameraRightabs) + knockback.y * Vector3.up) * knockbackPower;
                }
                else
                {
                    knockbackDir = ((knockback.z * cameraRightabs) + knockback.y * Vector3.up) * knockbackPower;
                }
            }
        }
    }
}