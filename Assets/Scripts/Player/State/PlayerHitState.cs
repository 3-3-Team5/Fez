using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private float needtime = 1f;
    private float currenttime = 0f;
    private Vector3 startdir;
    

    public PlayerHitState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.moveSpeedModifier = 0f; // 이렇게하면 움직임 제한.
        startdir = player.knockbackDir;
        //StartAnimation(animData.HitParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(animData.HitParameterHash);
    }

    public override void Update()
    {
        base.Update();
        player.knockbackDir = Vector3.Lerp(startdir, Vector3.zero, currenttime/needtime);
        currenttime += Time.deltaTime;

        if (currenttime >= needtime)
        {
            currenttime = 0f;
            player.isKnockback = false;
            player.moveSpeedModifier = 1f;
            if (controller.velocity.y < 0)
            {
                stateMachine.ChangeState(stateMachine.FallState);
                return;
            }
            
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }
}