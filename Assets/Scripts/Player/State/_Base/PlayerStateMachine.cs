using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }

    // States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    
    public PlayerWarpState WarpState { get; private set; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float MovementSpeedModifier { get; set; }
    public float JumpForce { get; set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        IdleState = new(this);
        MoveState = new(this);
        JumpState = new(this);
        FallState = new(this);
        HitState = new(this);
        WarpState = new(this);
    }
}
