using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CommonData
{
    public static readonly float DeathVelocity = -15f;
}
public struct LayerData
{
    public static readonly LayerMask Ground = 1 << LayerMask.NameToLayer("Ground");
    public static readonly LayerMask Wall = 1 << LayerMask.NameToLayer("Wall");
}

public struct RayCastData
{
    public static float PlayerCameraPivotPosY = 3f;

    public static readonly float PlayerOverHead = 0.3f;
    public static readonly float PlayerUnderFoot = 0.6f;
    public static readonly float RayFromCameraDistance = 60f;
    public static readonly float CameraToPlayerDis = 20f;
    public static readonly float PlayerFrontPivot = 0.5f;
    public static readonly float ClimbableDistance = .6f; // Lay�浹 ������ �ֻ���� �Ÿ��� Climbable�� �Ǵ� �Ҷ� ����ϴ� ���ذ�

    public static float DownPivot => PlayerCameraPivotPosY + PlayerUnderFoot;
    public static float UpPivot => -PlayerCameraPivotPosY + PlayerOverHead;
}

public struct ModifierCollection
{
    public static readonly float RadiusModifier = -0.25f;
}
