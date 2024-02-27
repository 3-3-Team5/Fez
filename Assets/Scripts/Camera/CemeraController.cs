using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Default = 10,
    Active = 11
}

public class CemeraController : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] CinemachineVirtualCamera[] vcams;
    private void Awake()
    {
        GameObject vcamsPref = Instantiate(Resources.Load<GameObject>("Camera/VCams"));

        vcams = vcamsPref.GetComponentsInChildren<CinemachineVirtualCamera>();
        VCamsInit();
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
    }
}
