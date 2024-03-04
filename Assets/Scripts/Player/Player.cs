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
    public float moveSpeedModifier = 1f; // Ω∫≈»ø° ∞°¡ﬂƒ°∏¶ ¥ı«œ¥¬ Ω∫≈»µÈ¿∫ ∏πæ∆¡ˆ∞‘ µ«∏È µ˚∑Œ ≈¨∑°Ω∫∑Œ ∞¸∏Æ«œ¥¬∞‘ ¡¡¿ªµÌ
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
        // ∑π¿Ã ±◊∏Æ±‚
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

        // ±‚¡Ó∏ ø°∑Ø¥¬ ±◊≥… Ω««‡¡ﬂ¿Ã æ∆¥“ ∂ß ¿Œ«≤∞™ ∞°¡Æø¿∑¡«ÿº≠ ±◊∑≥
        //Gizmos.DrawRay(front, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        //Gizmos.DrawRay(transform.forward, transform.forward * 10f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) //ÎØ∏ÎÅÑ?ü¨Ïß? Íµ¨ÌòÑ?óê ?ïÑ?öî?ïú Î©îÏÑú?ìú
    {
        #region ÎØ∏ÎÅÑ?ü¨Ïß?

        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Water"))
        {
            isslipped = true;
        }
        else isslipped = false;

        #endregion

        #region ?Ç¨?ùºÏß??äî Î∞úÌåê

        if (hit.gameObject.TryGetComponent<DisappearBlock>(out DisappearBlock disappearBlock))
        {
            if (Mathf.Approximately(hit.point.y, hit.gameObject.GetComponent<Collider>().bounds.max.y))
                disappearBlock.StartAnim();
        }

        #endregion

        #region ?ÑâÎ∞?

        //?ëêÍ∞? Í≤πÏπò?äî Í≤ΩÏö∞ ?ÉúÍ∑∏Ï? ?†à?ù¥?ñ¥Î°? Í¥?Î¶¨Ìï¥Î≥¥Ïûê. 
        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Trap"))
        {
            if (!isKnockback)
            {
                Vector3 cameraRightabs = mainCamera.transform.right.Abs(); //Camera.main Ï∫êÏã±?ï¥?Ñú ?Ç¨?ö© Abs
                isKnockback = true;
                Vector3 knockback = (hit.point - hit.collider.bounds.center).normalized;
                //Ïπ¥Î©î?ùº?óê Í∑??Üç?êò?äîÍ≤? ?ïÑ?ãà?ùº Ï∂©Îèå?óê Í∑??Üç?êòÍ≤? ÏΩîÎìúÎ•? ?ûë?Ñ±?ï¥?ïº?ïú?ã§.
                //4Î∞©Ìñ•?ùÑ Ï≤¥ÌÅ¨?ï¥?Ñú ?ä§?úÑÏπòÎ?? ?èåÎ¶¨Ïàò?èÑ ?ûàÍ≥?, Ïπ¥Î©î?ùºÍ∞? ?ñ¥?ñ§ Ïπ¥Î©î?ùº?ù∏Ïß? ?ïåÍ≥? ?ï¥?ïº?ï®.
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

    #region ?è¨?Éà ?ù¥?èô Í¥??†®

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