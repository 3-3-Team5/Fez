using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    Player player;
    CharacterController controller;

    public float verticalVelocity;

    public bool isStop;

    public Vector3 Movement => Vector3.up * verticalVelocity;

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
        isStop = false;
    }

    private void Update()
    {
        if (isStop)
            return;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        if(verticalVelocity < CommonData.DeathVelocity)
        {
            player.isDeath = true;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
