using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    Animator animator;
    PlayerAnimationData animData;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animData = transform.parent.parent.GetComponent<Player>().AnimationData;
    }

    public void OnClimbEnd()
    {
        animator.SetBool(animData.ClimbParameterHash, false);
    }
}
