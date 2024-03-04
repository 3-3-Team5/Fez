using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CameraState
{
    Default = 10,
    Active = 11
}

public class CemeraController : MonoBehaviour
{
    [SerializeField] Transform target;
    Player player;

    CinemachineBrain brain;
    CinemachineVirtualCamera[] vcams;
    int currentCameraIndex;

    bool isBlending = false;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();

        GameObject vcamsPref = Instantiate(Resources.Load<GameObject>("Camera/VCams"));

        vcams = vcamsPref.GetComponentsInChildren<CinemachineVirtualCamera>();
        VCamsInit();

        player = target.parent.gameObject.GetComponent<Player>();

        RayCastData.PlayerCameraPivotPosY = target.localPosition.y;
    }

    private void Start()
    {
        player.Input.PlayerActions.CameraMove.started += CameraMove_Started;
    }

    private void Update()
    {
        // 내가 정의한 bool값은 블렌드가 시작 되었을 때 true 로 변함
        // brain.IsBlending 은 현재 블렌딩이 진행중인지를 나타내기 때문에 
        // 둘중 하나만 쓴다면 조건이 계속 걸려서 쓸데없이 메소드를 계속 들어갈거라고 생각해서 이런식으로 조건을 걸었음
        if (brain.IsBlending && !isBlending)
        {
            OnBlendStarted();   // 블렌딩이 시작 되면 플레이어의 움직임을 제한하기 위해서
        }
        else if(!brain.IsBlending && isBlending)
        {
            OnBlendCompleted(); // 끝났으니 제한 해제
        }
    }

    void VCamsInit()
    {
        foreach (var vcam in vcams)
        {
            vcam.Follow = target;
            vcam.LookAt = target;
            vcam.Priority = (int)CameraState.Default;
        }

        vcams[0].Priority = (int)CameraState.Active;
        currentCameraIndex = 0;
    }

    public void CameraMove(int idx)
    {
        int temp = currentCameraIndex; // 카메라 우선도는 한번에 변경 하는게 나을것 같아서 일단 현재 인덱스 저장
        currentCameraIndex += idx;

        if (currentCameraIndex < 0)
            currentCameraIndex = vcams.Length - 1;
        else if (currentCameraIndex >= vcams.Length)
            currentCameraIndex = 0;

        VCamReset(vcams[temp]);
        vcams[currentCameraIndex].Priority = (int)CameraState.Active;
    }

    void VCamReset(CinemachineVirtualCamera vcam)
    {
        vcam.Priority = (int)CameraState.Default;
    }

    private void CameraMove_Started(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        CameraMove((int)value.x);
    }

    private void OnBlendStarted()
    {
        // 블렌딩이 시작 되었으니 PlayerInput 끄기
        isBlending = true;
        player.SetPlayerControlEnabled(false);
        //Debug.Log("Blending Start");
    }

    private void OnBlendCompleted()
    {
        // 블렌딩이 완료 되었으니 PlayerInput 켜기
        isBlending = false;
        player.SetPlayerControlEnabled(true);
        //Debug.Log("Blending Completed");
    }
}
