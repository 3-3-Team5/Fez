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
using UnityEngine.Windows;
using UnityEngine.UIElements;


[Serializable]
public struct SaveTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;

    public void SetTransform(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        localScale = transform.localScale;

        position.x += localScale.x * -0.3f; // 그냥 대충 반지름만큼 뒤로 가라는뜻
        position.z += localScale.x * -0.3f;
    }
}

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
    [HideInInspector] public Vector3 warpPos;

    [HideInInspector] public Camera mainCamera;

    PlayerStateMachine stateMachine;
    public bool isVisible;

    public Inventory inventory = new Inventory(4);
    private GameObject inventoryUI;

    public bool isDeath;
    public Action OnDeath;
    public SaveTransform saveTransform;

    public AudioManager audioManager;

    private void Awake()
    {
        AnimationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        stateMachine = new(this);

        isDeath = false;
        saveTransform = new SaveTransform();
    }

    void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        mainCamera = Camera.main;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        OnDeath += ReSpawn;
    }

    void Update()
    {
        if (isDeath)
        {
            return;
        }

        stateMachine?.HandleInput();
        stateMachine.Update();

        Vector3 center = Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY);

        if (UnityEngine.Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit hit;
            if (Physics.Raycast(center, transform.position - center, out hit, RayCastData.RayFromCameraDistance))
            {
                if (hit.collider.TryGetComponent<ItemBox>(out ItemBox itemBox))
                {
                    itemBox.OpenBox();
                }

                if (hit.collider.TryGetComponent<ItemObject>(out ItemObject itemObject))
                {
                    if (!itemObject.canGet) return;

                    inventory.AddItem(itemObject.itemData);

                    itemObject.gameObject.SetActive(false);
                }
            }
            else
            {
                if (Physics.Raycast(center, center - transform.position, out hit, RayCastData.RayFromCameraDistance))
                {
                    if (hit.collider.TryGetComponent<ItemBox>(out ItemBox itemBox))
                    {
                        itemBox.OpenBox();
                    }

                    if (hit.collider.TryGetComponent<ItemObject>(out ItemObject itemObject))
                    {
                        if (!itemObject.canGet) return;

                        inventory.AddItem(itemObject.itemData);

                        itemObject.gameObject.SetActive(false);
                    }
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (isDeath)
        {
            return;
        }

        stateMachine.PhysicsUpdate();

        //Debug.Log($"Current State : {stateMachine.GetCurState()}");
    }

    private void LateUpdate()
    {
        if (isDeath)
        {
            OnDeath?.Invoke();
            isDeath = false;
        }
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
        // ���� �׸���
        for (int i = 0; i < 3; ++i)
        {
            Vector3 modifier = Vector3.zero;
            modifier.y += i * 0.1f;

            Gizmos.DrawRay(front - modifier, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);
        }

        Gizmos.color = Color.red;

        Vector3 down = Camera.main.transform.position + (Vector3.down * RayCastData.DownPivot);
        Gizmos.DrawRay(down, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        Vector3 up = Camera.main.transform.position + (Vector3.up * RayCastData.UpPivot);
        Gizmos.DrawRay(up, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        Vector3 center = Camera.main.transform.position + (Vector3.down * RayCastData.PlayerCameraPivotPosY);
        Gizmos.DrawRay(center, transform.position - center);

        //Gizmos.DrawRay(front, Camera.main.transform.forward * RayCastData.RayFromCameraDistance);

        //Gizmos.DrawRay(transform.forward, transform.forward * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/Inventory");
            go.GetComponent<InventoryUI>().player = this;
            inventoryUI = Instantiate(go);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Destroy(inventoryUI);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) //OnColliderEnter 안됨
    {
        #region 미끄러움

        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Water"))
        {
            isslipped = true;
        }
        else isslipped = false;

        #endregion


        #region 사라지는 발판

        if (hit.gameObject.TryGetComponent<DisappearBlock>(out DisappearBlock disappearBlock))
        {
            if (Mathf.Approximately(hit.point.y, hit.gameObject.GetComponent<Collider>().bounds.max.y))
                disappearBlock.StartAnim();
        }

        #endregion

        #region 넉백

        if (1 << hit.gameObject.layer == 1 << LayerMask.NameToLayer("Trap"))
        {
            if (!isKnockback)
            {
                Vector3 cameraRightabs = mainCamera.transform.right.Abs();
                isKnockback = true;
                Vector3 knockback = (hit.point - hit.collider.bounds.center).normalized;
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

    #region 워프관련

    public void WarpIn(Vector3 warpTransform)
    {
        warpPos = warpTransform;
        Input.PlayerActions.Interactionportal.started += OnWarpStart;
        Debug.Log("WarpIn");
    }

    public void WarpOut()
    {
        warpPos = Vector3.zero;
        Input.PlayerActions.Interactionportal.started -= OnWarpStart;
        Debug.Log("WarpOut");
    }

    public void OnWarpStart(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isWarp = true;
            ForceReceiver.isStop = true;
        }
    }

    #endregion

    public void ReSpawn()
    {
        Controller.enabled = false;

        gameObject.transform.position = saveTransform.position;
        gameObject.transform.rotation = saveTransform.rotation;
        gameObject.transform.localScale = saveTransform.localScale;

        ForceReceiver.verticalVelocity = 0;
        isDeath = false;

        Animator.SetTrigger(AnimationData.DieParameterHash);
    }

    public void SetIdleState()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}