using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using Cinemachine.Utility;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] public CharacterSO stats; // baseStat

    [Header("Modifier")]
    public float moveSpeedModifier = 1f; // 스탯에 가중치를 더하는 스탯들은 많아지게 되면 따로 클래스로 관리하는게 좋을듯
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
    [HideInInspector] public bool isKnockback = false;
    public float knockbackPower;

    [HideInInspector] public bool isWarp = false;
    [HideInInspector] public Transform warpPos;

    [HideInInspector] public Camera mainCamera;

    PlayerStateMachine stateMachine;
    public bool isVisible;

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

        //Debug.Log($"Current State : {stateMachine.GetCurState()}");
    }

    public void SetPlayerControlEnabled(bool active)
    {
        Controller.enabled = active;
        Input.enabled = active;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 front = Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY);
        front += (Camera.main.transform.right * transform.localScale.x) * RayCastData.PlayerFrontPivot;
        // 레이 그리기
        for (int i = 0; i < 3; ++i)
        {
            Vector3 modifier = Vector3.zero;
            modifier.y += i * 0.2f;

            Gizmos.DrawRay(front - modifier, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);
        }

        Gizmos.color = Color.red;

        Vector3 down = Camera.main.transform.position + (Vector3.down * RayCastData.DownPivot);
        Gizmos.DrawRay(down, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        Vector3 up = Camera.main.transform.position + (Vector3.up * RayCastData.UpPivot);
        Gizmos.DrawRay(up, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        Vector3 center = Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY);
        Gizmos.DrawRay(center, transform.position - center);

        // 기즈모 에러는 그냥 실행중이 아닐 때 인풋값 가져오려해서 그럼
        //Gizmos.DrawRay(front, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        //Gizmos.DrawRay(transform.forward, transform.forward * 10f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) //誘몃걚?윭吏? 援ы쁽?뿉 ?븘?슂?븳 硫붿꽌?뱶
    {
        #region 誘몃걚?윭吏?

        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Water"))
        {
            isslipped = true;
        }
        else isslipped = false;

        #endregion

        #region ?궗?씪吏??뒗 諛쒗뙋

        if (hit.gameObject.TryGetComponent<DisappearBlock>(out DisappearBlock disappearBlock))
        {
            if (Mathf.Approximately(hit.point.y, hit.gameObject.GetComponent<Collider>().bounds.max.y))
                disappearBlock.StartAnim();
        }

        #endregion

        #region ?꼮諛?

        //?몢媛? 寃뱀튂?뒗 寃쎌슦 ?깭洹몄? ?젅?씠?뼱濡? 愿?由ы빐蹂댁옄. 
        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Trap"))
        {
            if (!isKnockback)
            {
                Vector3 cameraRightabs = mainCamera.transform.right.Abs(); //Camera.main 罹먯떛?빐?꽌 ?궗?슜 Abs
                isKnockback = true;
                Vector3 knockback = (hit.point - hit.collider.bounds.center).normalized;
                //移대찓?씪?뿉 洹??냽?릺?뒗寃? ?븘?땲?씪 異⑸룎?뿉 洹??냽?릺寃? 肄붾뱶瑜? ?옉?꽦?빐?빞?븳?떎.
                //4諛⑺뼢?쓣 泥댄겕?빐?꽌 ?뒪?쐞移섎?? ?룎由ъ닔?룄 ?엳怨?, 移대찓?씪媛? ?뼱?뼡 移대찓?씪?씤吏? ?븣怨? ?빐?빞?븿.
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

        #endregion
    }

    #region ?룷?깉 ?씠?룞 愿??젴

    public void WarpIn(Transform warpTransform)
    {
        warpPos = warpTransform;
        Input.PlayerActions.Interactionportal.started += OnWarpStart;
        Debug.Log("WarpIn");
    }

    public void WarpOut()
    {
        warpPos = null;
        Input.PlayerActions.Interactionportal.started -= OnWarpStart;
        Debug.Log("WarpOut");
    }

    public void OnWarpStart(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isWarp = true;
        }
    }

    #endregion
}