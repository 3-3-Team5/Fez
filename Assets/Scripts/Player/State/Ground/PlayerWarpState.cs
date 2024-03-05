using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWarpState : PlayerGroundState
{
   
    private bool warp;
    public PlayerWarpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.audioManager.PlayDoorFX(0);
        StartAnimation(player.AnimationData.DoorEnterParameterHash);
        warp = true;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(player.AnimationData.DoorExitParameterHash);
        player.isWarp = false;
        forceReceiver.isStop = false;
    }

    public override void Update()
    {
        if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Enter") &&player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (warp)
            {
                player.gameObject.transform.position = player.warpPos;
                warp = false;
                StopAnimation(player.AnimationData.DoorEnterParameterHash);
                player.audioManager.PlayDoorFX(1);
                StartAnimation(player.AnimationData.DoorExitParameterHash);
            }
        }
        
        if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Exit")  && player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }
}

