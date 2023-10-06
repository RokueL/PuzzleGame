using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour
{
    int downCount;
    public int Width;
    public int Height;
    public GameObject[,] allTiles;
    public GameObject[,] allDots;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new GameObject[Width, Height];
        allDots = new GameObject[Width, Height];
        SetUp();
        Invoke("DestroyCheck", 0.2f);
    }

    //void invokeUse()
    //{
    //    DestroyCheck();
    //    CancelInvoke();
    //}

    void SetUp()
    {
        for(int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++) 
            {
                Vector2 tempVec2 = new Vector2(i,j);
                var tileSet = ObjectPoolManager.Instance.Pool.Get();
                tileSet.transform.position = tempVec2;
                tileSet.transform.parent = this.transform;
                tileSet.name = "( " + i + " , " + j + " )";
                allTiles[i, j] = tileSet;

                var dotSet = ObjectPoolManager.Instance.dotPool.Get();
                dotSet.transform.position = tempVec2;
                dotSet.transform.parent = this.transform;
                dotSet.name = "( " + i + " , " + j + " )";
                allDots[i, j] = dotSet;
            }
        }
    }

    void SpawnDot()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 tempVec = new Vector2(i, j);
                    var dotSet = ObjectPoolManager.Instance.dotPool.Get();
                    dotSet.GetComponent<Dot>().colorCheck();
                    dotSet.transform.position = tempVec;
                    dotSet.name = "new ( " + i + " , " + j + " )"; ;
                    allDots[i, j] = dotSet;
                    allDots[i, j].GetComponent<Dot>().row = j;
                    allDots[i, j].GetComponent<Dot>().column = i;

                }
            }
        }
    }

    public void DestroyCheck()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatch == true)
                    {
                        allDots[i, j].GetComponent<Dot>().dottPool.Release(allDots[i, j]);
                        allDots[i,j] = null;
                    }
                }
            }
        }
        StartCoroutine(DownCheck());
    }

    IEnumerator DownCheck()
    {
        downCount = 0;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++) // 아래에서 위 한줄 체킹 후 옆에 한줄 체킹
            {
                if (allDots[i, j] == null)
                {
                    downCount++;
                }
                else if (downCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= downCount;
                    allDots[i, j] = null; 
                }
            }
            downCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        SpawnDot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
