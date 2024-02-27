using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTile : MonoBehaviour
{
    public GameObject[] obj;

    void Start()
    {
        //¹Ù´Ú
        for(int x = -8; x < 9; x++)
            for(int z = 0; z < 17; z++)
                for (int y = 0; y < 2; y++)
                    Instantiate(obj[0], new Vector3(x, - 4.5f - y, z), Quaternion.identity, transform);
        //±âµÕ¼ÂÆÃ
        for (int x = -3; x < 3; x++)
            for (int z = 5; z < 11; z++)
                for (int y = 0; y < 20; y++){
                    if (z > 5 && z < 10 && x > -3 && x < 2 && y < 19) ;
                    else Instantiate(obj[1], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                }
        //¹ßÆÇ¼ÂÆÃ
        Instantiate(obj[2], new Vector3(-1, -2.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(0, -2.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(1, -0.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(2, -0.5f, 4), Quaternion.identity, transform);
        //Instantiate(obj[2], new Vector3(3, -0.5f, 4), Quaternion.identity, transform);
        //Instantiate(obj[2], new Vector3(3, -0.5f, 5), Quaternion.identity, transform);
        //Instantiate(obj[2], new Vector3(3, -0.5f, 6), Quaternion.identity, transform);
        //Instantiate(obj[2], new Vector3(3, 1.5f, 7), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(3, 1.5f, 8), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(3, 1.5f, 9), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(3, 1.5f, 10), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(3, 1.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(2, 1.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(1, 3.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(0, 5.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-1, 5.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-2, 7.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-3, 7.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 7.5f, 11), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 7.5f, 10), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 7.5f, 9), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 9.5f, 8), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 9.5f, 7), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 11.5f, 6), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 11.5f, 5), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-4, 11.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-3, 11.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-2, 11.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(-1, 13.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(0, 13.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(1, 15.5f, 4), Quaternion.identity, transform);
        Instantiate(obj[2], new Vector3(2, 15.5f, 4), Quaternion.identity, transform);
    }

}
