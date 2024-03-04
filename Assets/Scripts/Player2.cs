using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public Inventory inventory = new Inventory(4);


    public void RemoveItem(int index)
    {
        inventory.RemoveItem(index);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puzzle"))
        {
            inventory.AddItem(other.GetComponent<ItemObject>().itemData);

            other.gameObject.SetActive(false);
        }
    }
}
