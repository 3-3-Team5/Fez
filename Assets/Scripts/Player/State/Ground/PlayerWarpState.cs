using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWarpState : PlayerGroundState
{

    private float time;
    private bool warp;
    public PlayerWarpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //StartAnimation();
        warp = true;
        time = 0;
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation();
        player.isWarp = false;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        //if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        if (time >=1f)
        {
            Debug.Log("Start");
            if (warp)
            {
                player.gameObject.transform.position = player.warpPos.position;
                warp = false;
            }
        }
        
        if (time >= 2f)
        {
            Debug.Log("End");
            stateMachine.ChangeState(stateMachine.IdleState);
        }
            
    }
}

