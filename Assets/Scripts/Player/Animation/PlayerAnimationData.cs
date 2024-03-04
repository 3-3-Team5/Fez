using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string moveParameterName = "Move";
    [SerializeField] private string ClimbParameterName = "Climb";

    [SerializeField] private string airParameterName = "@Air";
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string fallParameterName = "Fall";

    [SerializeField] private string HitParameterName = "Hit";
    [SerializeField] private string DoorEnterParameterName = "Enter";
    [SerializeField] private string DoorExitParameterName = "Exit";

    [SerializeField] private string DieParameterName = "Die";

    //TODO : 넉백이랑 포탈이동 관련한 애니메이션 데이터 추가해야함. (애니메이션 파라미터 + 애니메이션 등등)
    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int MoveParameterHash { get; private set; }
    public int ClimbParameterHash { get; private set; }

    public int AirParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }
    public int fallParameterHash { get; private set; }

    public int HitParameterHash { get; private set; }
    public int DoorEnterParameterHash { get; private set; }
    public int DoorExitParameterHash { get; private set; }

    public int DieParameterHash { get; private set; }


    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        MoveParameterHash = Animator.StringToHash(moveParameterName);
        ClimbParameterHash = Animator.StringToHash(ClimbParameterName);

        AirParameterHash = Animator.StringToHash(airParameterName);
        JumpParameterHash = Animator.StringToHash(jumpParameterName);
        fallParameterHash = Animator.StringToHash(fallParameterName);

        HitParameterHash = Animator.StringToHash(HitParameterName);
        DoorEnterParameterHash = Animator.StringToHash(DoorEnterParameterName);
        DoorExitParameterHash = Animator.StringToHash(DoorExitParameterName);

        DieParameterHash = Animator.StringToHash(DieParameterName);
    }
}