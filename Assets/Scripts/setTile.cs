using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTile : MonoBehaviour
{
    public GameObject[] obj;

    void Start()
    {
        SetTile2();
    }

    void SetTile1()
    {
        //¹Ù´Ú
        MakeBlock(-8, 9, 0, 17, 0, 2,0);
        //±âµÕ¼ÂÆÃ
        for (int x = -3; x < 3; x++)
            for (int z = 5; z < 11; z++)
                for (int y = 0; y < 20; y++)
                {
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

    void SetTile2()
    {
        //¹Ù´Ú
        MakeBlock(0, 21, 0, 15, 0, 2,0);
       
        //±âµÕ¼ÂÆÃ
        for (int x = 6; x < 13; x++)
            for (int z = 5; z < 12; z++)
                for (int y = 0; y < 4; y++)
                {
                    if (z > 5 && z < 11 && x > 6 && x < 12 && y < 3) ;
                    else
                    {
                        if (y == 3)
                        {
                            var front = Instantiate(obj[2], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                            if (z == 5) front.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                        }
                        else Instantiate(obj[1], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                    }
                    
                }
        //±âµÕ¼ÂÆÃ
        for (int x = 7; x < 12; x++)
            for (int z = 6; z < 11; z++)
                for (int y = 4; y < 26; y++)
                {
                    if (z > 6 && z < 10 && x > 7 && x < 11 && y < 19) ;
                    else Instantiate(obj[1], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                }
        //±âµÕ¼ÂÆÃ
        for (int x = 6; x < 13; x++)
            for (int z = 5; z < 12; z++)
                for (int y = 26; y < 28; y++)
                {
                    if (y == 27)
                    {
                        var front = Instantiate(obj[2], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        if (z == 5) front.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                    }
                    else Instantiate(obj[1], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                }
        //ÀÛÀº Áý
        MakeBlock(4, 8, 2, 5, 0, 3, 1);
        MakeBlock(4, 6, 7, 11, 0, 3, 1);
        MakeBlock(11, 15, 3, 7, 4, 8, 1);
        MakeBlock(10, 14, 5, 8, 8, 11, 1);
        MakeBlock(3, 7, 7, 11, 8, 12, 1);
        MakeBlock(10, 14, 10, 13, 8, 11, 1);
        MakeBlock(5, 9, 2, 6, 16, 20, 1);
        MakeBlock(5, 9, 9, 13, 16, 20, 1);
        MakeBlock(10, 14, 4, 7, 20, 23, 1);
        MakeBlock(11, 14, 8, 12, 20, 23, 1);

        //³ª¹«ÆÇ
        MakeBlock(10, 17, 2, 5, 3, 4, 6);
        MakeBlock(13, 17, 5, 10, 3, 4, 6);
        MakeBlock(1, 15, 4, 15, 7, 8, 6);
        MakeBlock(4, 10, 1, 15, 15, 16, 6);
        MakeBlock(9, 15, 3, 13, 19, 20, 6);

        //¹®
        MakeBlock(6, 7, 2, 3, -3, -2, 8,0);
        MakeBlock(13, 14, 3, 4, 1, 2, 8);
        MakeBlock(12, 13, 5, 6, 5, 6, 8);
        MakeBlock(3, 4, 9, 10, 5, 6, 7);
        MakeBlock(6, 7, 2, 3, 13, 14, 8);
        MakeBlock(6, 7, 13, 14, 13, 14, 8);
        MakeBlock(12, 13, 4, 5, 17, 18, 8);
        MakeBlock(14, 15, 10, 11, 17, 18, 7);

        //´Ù¸®
        MakeBlock(14, 21, 6, 7, 10, 11, 6);
        MakeBlock(21, 22, -2, 7, 10, 11, 6);

        //³ª¹«¹Ú½º
        MakeBlock(12, 14, 7, 9, 6, 8, 0);
        MakeBlock(8, 10, 11, 13, 10, 12, 0);
        MakeBlock(21, 22, 6, 7, 12, 13, 0);
        MakeBlock(5, 7, 6, 8, 18, 20, 0);
        MakeBlock(13, 16, 7, 11, 14, 17, 0);


        //µÕµÕ¼¶1
        MakeBlock(21, 28, -9, -2, 9, 13, 0);
        MakeBlock(22, 26, -7, -4, 11, 14, 1);

        //³ª¹«
        MakeBlock(0, 1, 14, 15, 0, 1, 10);
        MakeBlock(-1, 2, 13, 16 , 1, 4, 11);

        //µÕµÕ¼¶1 ³ª¹«
        MakeBlock(24, 25, -7, -5, 14, 15, 10);
        MakeBlock(25, 26, -7, -6, 14, 16, 10);
        MakeBlock(23, 27, -8, -4, 16, 20, 11);

        //µÕµÕ¼¶1 ¹®
        MakeBlock(24, 25, -7, -6, 8, 9, 8);

        //Ãß°¡ ¹ßÆÇ
        MakeBlock(8, 9, 11, 12, 1, 2, 13);
        MakeBlock(10, 11, 11, 12, 8, 9, 13);
        MakeBlock(10, 11, 6, 7, 8, 9, 13);
        MakeBlock(7, 8, 9, 10, 17, 18, 12);

        //µÕµÕ¼¶2 
        MakeBlock(-1, 0, 5, 6, 21, 22, 0);
        MakeBlock(-5, -1, 19, 23, 18, 22, 0);


        //ÆÄÀÌ³Î ¹ßÆÇ
        MakeBlock(2, 3, 1, 2, 24, 25, 15);
        MakeBlock(15, 16, -3, -2, 26, 27, 15);

        // ¾ÆÀÌÅÛ ¹Ú½º
        GameObject go = Instantiate(obj[16], new Vector3(7f, -0.6f, 2f), Quaternion.Euler(new Vector3(0f, 180f, 0f)), transform);
        go.GetComponent<ItemBox>().dropItem = Resources.Load<GameObject>("Prefabs/Puzzle2");

        go = Instantiate(obj[16], new Vector3(13f, 2.4f, 8f), Quaternion.Euler(new Vector3(0f, 90f, 0f)), transform);
        go.GetComponent<ItemBox>().dropItem = Resources.Load<GameObject>("Prefabs/Puzzle3");

        go = Instantiate(obj[16], new Vector3(22f, 10.4f, -7.5f), Quaternion.Euler(new Vector3(0f, 180f, 0f)), transform);
        go.GetComponent<ItemBox>().dropItem = Resources.Load<GameObject>("Prefabs/Puzzle0");

        go = Instantiate(obj[16], new Vector3(5f, 16.4f, 5f), Quaternion.Euler(new Vector3(0f, -90f, 0f)), transform);
        go.GetComponent<ItemBox>().dropItem = Resources.Load<GameObject>("Prefabs/Puzzle1");
    }

    void MakeBlock(int StartX, int MaxX, int StartZ, int MaxZ, int StartY, int MaxY, int blockNum)
    {
        switch (blockNum)
        {
            case 0:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            if (y == 1 || y==12 || y==16 || y == 21)
                            {
                                var land = Instantiate(obj[5], new Vector3(x, y - 5.5f, z), Quaternion.identity, transform);
                                if (z == 0 || z == -9 || z==7 || z ==5 || z == 19 ) land.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                            }
                            else Instantiate(obj[blockNum], new Vector3(x, y - 5.5f, z), Quaternion.identity, transform);
                        }
                break;
            case 1:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            if (y == MaxY -1)
                            {
                                var front = Instantiate(obj[2], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                                if (z == 3 || z ==2 || z==5 || z ==7 || z == -7 || z == 4 || z == 1 || z == -3) front.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                            }
                            else Instantiate(obj[9], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        }
                break;
            case 6: 
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x, y - 3.05f, z), Quaternion.identity, transform);
                        }
                break;

            case 7:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x- 0.5f, y, z), Quaternion.identity, transform);
                        }
                break;

            case 8:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x, y , z - 0.5f), Quaternion.identity, transform);
                        }
                break;
            case 11:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            if(y == MaxY - 1) Instantiate(obj[14], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                            else Instantiate(obj[blockNum], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        }
                break;
            case 12:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x - 0.5f, y - 0.5f, z), Quaternion.identity, transform);
                        }
                break;

            case 13 :
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            var jump = Instantiate(obj[blockNum], new Vector3(x, y - 0.5f, z - 0.5f), Quaternion.identity, transform);
                            if(z ==6 ) jump.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                        }
                break;
            case 15:
                for (int x = StartX; x < MaxX; x++)
                for (int z = StartZ; z < MaxZ; z++)
                for (int y = StartY; y < MaxY; y++)
                {
                    if (y == MaxY -1)
                    {
                        var front = Instantiate(obj[15], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        if (z == 3 || z ==2 || z==5 || z ==7 || z == -7 || z == 4 || z == 1 || z == -3) front.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                    }
                    else Instantiate(obj[9], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                }
                break;
            default:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        }
                break;

        }
       
    }

    void MakeBlock(int StartX, int MaxX, int StartZ, int MaxZ, int StartY, int MaxY, int blockNum, int index)
    {
        switch (blockNum)
        {
            case 0:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            if (y == 1 || y == 12 || y == 16 || y == 21)
                            {
                                var land = Instantiate(obj[5], new Vector3(x, y - 5.5f, z), Quaternion.identity, transform);
                                if (z == 0 || z == -9 || z == 7 || z == 5 || z == 19) land.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                            }
                            else Instantiate(obj[blockNum], new Vector3(x, y - 5.5f, z), Quaternion.identity, transform);
                        }
                break;
            case 1:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            if (y == MaxY - 1)
                            {
                                var front = Instantiate(obj[2], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                                if (z == 3 || z == 2 || z == 5 || z == 7 || z == -7 || z == 4 || z == 1 || z == -3) front.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                            }
                            else Instantiate(obj[9], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        }
                break;
            case 6:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x, y - 3.05f, z), Quaternion.identity, transform);
                        }
                break;

            case 7:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x - 0.5f, y, z), Quaternion.identity, transform);
                        }
                break;

            case 8:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            var objdoor = Instantiate(obj[blockNum], new Vector3(x, y, z - 0.5f), Quaternion.identity, transform);
                            if (index == 0)
                            {
                                objdoor.AddComponent<PortalBlock>();
                                objdoor.GetComponent<PortalBlock>().warpPos = new Vector3(12, 16.6f, 3f);
                                objdoor.GetComponent<BoxCollider>().isTrigger = true;
                                objdoor.GetComponent<BoxCollider>().size = new Vector3(1,1,30);
                                
                            }
                            
                        }
                break;
            case 11:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            if (y == MaxY - 1) Instantiate(obj[14], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                            else Instantiate(obj[blockNum], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        }
                break;
            case 12:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x - 0.5f, y - 0.5f, z), Quaternion.identity, transform);
                        }
                break;

            case 13:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            var jump = Instantiate(obj[blockNum], new Vector3(x, y - 0.5f, z - 0.5f), Quaternion.identity, transform);
                            if (z == 6) jump.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                        }
                break;
            case 15:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            if (y == MaxY - 1)
                            {
                                var front = Instantiate(obj[15], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                                if (z == 3 || z == 2 || z == 5 || z == 7 || z == -7 || z == 4 || z == 1 || z == -3) front.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                            }
                            else Instantiate(obj[9], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        }
                break;
            default:
                for (int x = StartX; x < MaxX; x++)
                    for (int z = StartZ; z < MaxZ; z++)
                        for (int y = StartY; y < MaxY; y++)
                        {
                            Instantiate(obj[blockNum], new Vector3(x, y - 3.5f, z), Quaternion.identity, transform);
                        }
                break;

        }

    }
}
