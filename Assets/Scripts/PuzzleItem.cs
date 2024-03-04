using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItem : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                //GetItem( );
            }
        }
    }

    private void GetItem(Player2 player)
    {
       // player.inventory.AddItem(this);
    }
}
