using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    [SerializeField] public CharacterSO stats; // baseStat

    [Header("Modifier")]
    public float moveSpeedModifier = 1f; // ���ȿ� ����ġ�� ���ϴ� ���ȵ��� �������� �Ǹ� ���� Ŭ������ �����ϴ°� ������
    public float jumpForceModifier = 1f;
    public float GetMoveSpeed => stats.baseStats.MovementSpeed * moveSpeedModifier;
    public float GetJumpForce => stats.baseStats.jumpForce * jumpForceModifier;

    public Animator Animator { get; private set; }
    public PlayerInput Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    PlayerStateMachine stateMachine;

    public float layDistance = .7f; // Climb�� �������� �� Lay�� �Ÿ�
    public float climbableDistance = .6f; // Lay�浹 ������ �ֻ���� �Ÿ��� Climbable�� �Ǵ� �Ҷ� ����ϴ� ���ذ�

    private void Awake()
    {
        AnimationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();

        stateMachine = new(this);

        //UnderFootPivot = 3.0f + 0.7f;
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

        //Debug.Log($"Current State : {stateMachine.GetCurState()}");
    }

    public void SetPlayerControlEnabled(bool active)
    {
        Controller.enabled = active;
        Input.enabled = active;
    }

    private void OnDrawGizmos()
    {
        Vector3 direction = Camera.main.transform.right * transform.localScale.x;

        Gizmos.color = Color.red;

        // ���� �׸���
        for (int i = 0; i < 3; ++i)
        {
            Vector3 modifier = Vector3.zero;
            modifier.y += i * 0.2f;

            Gizmos.DrawRay(transform.position - modifier, direction * layDistance);
        }

        Vector3 down = Camera.main.transform.position + (Vector3.down * RayCastData.DownPivot);
        Gizmos.DrawRay(down, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        Vector3 up = Camera.main.transform.position + (Vector3.up * RayCastData.UpPivot);
        Gizmos.DrawRay(up, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);
    }
}
