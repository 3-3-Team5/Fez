using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBlock : MonoBehaviour
{
    [HideInInspector] public Vector3 warpPos;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.WarpIn(warpPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.WarpOut();
        }
    }
}
