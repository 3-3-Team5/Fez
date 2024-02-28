using System;
using System.Collections;
using System.Collections.Generic;
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
    [field : SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    
    [HideInInspector]public bool isslipped = false;
    [HideInInspector]public Vector2 slideDir = Vector2.zero; //다른 State스크립트에서 간섭하기때문에 static으로 선언하면 문제없이 동작함.
    //TODO : Player 스크립트로 옮기던가 할 것.

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
        if (1<<hit.gameObject.layer == 1 << LayerMask.NameToLayer("Water"))
        {
            isslipped = true;
        }
        else isslipped = false;

        if (hit.gameObject.TryGetComponent<DisappearBlock>(out DisappearBlock disappearBlock))
        {
            if(Mathf.Approximately(hit.point.y,hit.gameObject.GetComponent<Collider>().bounds.max.y))
                disappearBlock.StartAnim();
        }
    }
}
