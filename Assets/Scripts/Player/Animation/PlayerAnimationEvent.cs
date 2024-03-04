using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAnimationEvent : MonoBehaviour
{
    Player player;
    CharacterController controller;
    Animator animator;
    PlayerAnimationData animData;

    private void Awake()
    {
        player = transform.parent.parent.GetComponent<Player>();
        controller = transform.parent.parent.GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
        animData = player.AnimationData;
    }

    public void OnClimbEnd()
    {
        animator.SetBool(animData.ClimbParameterHash, false);
    }

    public void OnDieExit()
    {
        controller.enabled = true;
        player.SetIdleState();
    }
}
