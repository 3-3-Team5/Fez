using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LayerData
{
    public static LayerMask Ground = 1 << LayerMask.NameToLayer("Ground");
    public static LayerMask Wall = 1 << LayerMask.NameToLayer("Wall");
}

public struct RayCastData
{
    public static float RayFromCameraDistance = 60f;

    public static float PlayerCameraPivotPosY = 3f;
    public static float PlayerUnderFoot = 0.6f;
    public static float PlayerOverHead = 0.7f;

    public static float DownPivot => PlayerCameraPivotPosY + PlayerUnderFoot;
    public static float UpPivot => -PlayerCameraPivotPosY + PlayerOverHead;
    public static float CameraToPlayerDis => 20f;
    public static float PlayerFrontPivot => 0.4f;

    public static float ClimbableDistance = .6f; // Lay�浹 ������ �ֻ���� �Ÿ��� Climbable�� �Ǵ� �Ҷ� ����ϴ� ���ذ�
}
