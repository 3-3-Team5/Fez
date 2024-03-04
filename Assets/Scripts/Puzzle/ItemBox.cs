using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemBox : MonoBehaviour
{
    [SerializeField]
    private GameObject dropItem;
    private float rotationSpeed = 100f;
    private float dropHeight = 1f;
    private float dropDuration = 1f;

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                OpenBox();
            }
        }
    }
    */

    public void OpenBox()
    {
        StartCoroutine(ItemDrop(dropItem));
    }

    private IEnumerator ItemDrop(GameObject dropItem)
    {
        GameObject go = Instantiate(dropItem);
        go.transform.position = transform.position;

        Vector3 startPosition = go.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, dropHeight, 0);

        float time = 0;

        while (time < dropDuration)
        {
            go.transform.position = Vector3.Lerp(startPosition, endPosition, time / dropDuration);
            go.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * 10f);
            time += Time.deltaTime;
            yield return null;
        }

        go.transform.position = endPosition;
        go.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        //go.GetComponent<Rigidbody>().useGravity = true;

        time = 0;
        while (time < dropDuration)
        {
            go.transform.position = Vector3.Lerp(endPosition, startPosition, time / dropDuration);
            //go.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * 5f);
            time += Time.deltaTime;
            yield return null;
        }

        go.transform.position = startPosition;

        gameObject.SetActive(false);
    }
}
