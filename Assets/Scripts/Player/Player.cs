using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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

    PlayerStateMachine stateMachine;

    public float layDistance = .7f;
    public float climbableDistance = .6f;

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

        Debug.Log($"Current State : {stateMachine.GetCurState()}");
    }

    private void OnDrawGizmos()
    {
        Vector3 direction = Camera.main.transform.right * transform.localScale.x;

        Gizmos.color = Color.red;

        // 레이 그리기
        for (int i = 0; i < 3; ++i)
        {
            Vector3 modifier = Vector3.zero;
            modifier.y += i * 0.2f;

            Gizmos.DrawRay(transform.position - modifier, direction * layDistance);
        }
    }
}
