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
    
    public bool isslipped = false;

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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (1<<hit.gameObject.layer == 1 << LayerMask.NameToLayer("Water"))
        {
            isslipped = true;
        }
        else isslipped = false;
    }
}
