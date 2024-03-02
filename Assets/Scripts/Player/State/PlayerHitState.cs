using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private Vector3 startdir;
    private float needTime = 0.7f;
    private float currentTime = 0f;
    
    public PlayerHitState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.moveSpeedModifier = 0f; // 이렇게하면 움직임 제한.
        startdir = player.knockbackDir;
        StartAnimation(animData.HitParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animData.HitParameterHash);
    }

    public override void Update()
    {
        base.Update();
        currentTime += Time.deltaTime;
        player.knockbackDir = Vector3.Lerp(startdir, Vector3.zero,currentTime/needTime);
        

        if (currentTime/needTime>=1f)
        {
            player.isKnockback = false;
            currentTime = 0f;
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