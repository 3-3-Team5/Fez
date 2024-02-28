using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private float needtime = 1f;
    private float currenttime = 0f;
    private Vector3 enddir;

    public PlayerHitState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enddir = new Vector3(player.knockbackDir.x, 0, player.knockbackDir.z);
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
        //enddir = Vector3.Lerp(enddir, Vector3.zero, currenttime/needtime);
        player.knockbackDir = Vector3.Lerp(player.knockbackDir, Vector3.zero, currenttime / needtime);
        Debug.Log($"{currenttime} || {needtime} = {currenttime/needtime} / KB : {player.knockbackDir}");
        currenttime += Time.deltaTime;

        if (currenttime >= needtime)
        {
            currenttime = 0f;
            player.isKnockback = false;
            if (controller.velocity.y < 0)
            {
                //player.knockbackDir = Vector3.zero;
                stateMachine.ChangeState(stateMachine.FallState);
                return;
            }

            //player.knockbackDir = Vector3.zero;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void HandleInput()
    {
        if (!player.isKnockback)
            base.HandleInput();
    }
}