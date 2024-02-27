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
    PlayerInput input;

    CinemachineBrain brain;
    [SerializeField] CinemachineVirtualCamera[] vcams;
    int currentCameraIndex;

    bool isBlending = false;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();

        GameObject vcamsPref = Instantiate(Resources.Load<GameObject>("Camera/VCams"));

        vcams = vcamsPref.GetComponentsInChildren<CinemachineVirtualCamera>();
        VCamsInit();

        input = target.parent.gameObject.GetComponent<PlayerInput>();
    }

    private void Start()
    {
        input.PlayerActions.CameraMove.started += CameraMove_Started;
    }

    private void Update()
    {
        // ���� ������ bool���� ���尡 ���� �Ǿ��� �� true �� ����
        // brain.IsBlending �� ���� ������ ������������ ��Ÿ���� ������ 
        // ���� �ϳ��� ���ٸ� ������ ��� �ɷ��� �������� �޼ҵ带 ��� ���Ŷ�� �����ؼ� �̷������� ������ �ɾ���
        if (brain.IsBlending && !isBlending)
        {
            OnBlendStarted();   // ������ ���� �Ǹ� �÷��̾��� �������� �����ϱ� ���ؼ�
        }
        else if(!brain.IsBlending && isBlending)
        {
            OnBlendCompleted(); // �������� ���� ����
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
        int temp = currentCameraIndex; // ī�޶� �켱���� �ѹ��� ���� �ϴ°� ������ ���Ƽ� �ϴ� ���� �ε��� ����
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
        // ������ ���� �Ǿ����� PlayerInput ����
        isBlending = true;
        input.enabled = false;
        Debug.Log("Blending Start");
    }

    private void OnBlendCompleted()
    {
        // ������ �Ϸ� �Ǿ����� PlayerInput �ѱ�
        isBlending = false;
        input.enabled = true;
        Debug.Log("Blending Completed");
    }
}
