using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enum;


public class Board : MonoBehaviour
{
    public Enum.Enum.State state = Enum.Enum.State.Move;

    int downCount;
    public int Width;
    public int Height;
    public int offset;
    public GameObject[,] allTiles;
    public GameObject[,] allDots;

    bool isSpawn;

    FindMatch findMatch;
    // Start is called before the first frame update
    void Start()
    {
        allTiles = new GameObject[Width, Height];
        allDots = new GameObject[Width, Height];
        findMatch = FindObjectOfType<FindMatch>();
        SetUp();
        Invoke("DestroyCheck", 0.2f);
    }

    //void invokeUse()
    //{
    //    DestroyCheck();
    //    CancelInvoke();
    //}
    //

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
                dotSet.GetComponent<Dot>().row = j;
                dotSet.GetComponent<Dot>().column = i;
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
                    Vector2 tempVec = new Vector2(i, j + offset);
                    var dotSet = ObjectPoolManager.Instance.dotPool.Get();
                    dotSet.transform.position = tempVec;
                    dotSet.name = "new ( " + i + " , " + j + " )";
                    allDots[i, j] = dotSet;
                    allDots[i, j].GetComponent<Dot>().row = j;
                    allDots[i, j].GetComponent<Dot>().column = i;
                }
            }
        }
        StartCoroutine(spawnDelay());
    }

    bool reSpawnCheck()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatch)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    IEnumerator SpawnDots()
    {
        if (!isSpawn)
        {
            SpawnDot();
            yield return new WaitForSeconds(.4f);

            while (reSpawnCheck())
            {
                yield return new WaitForSeconds(.4f);
                DestroyCheck();
            }
        }
        state = Enum.Enum.State.Move;
    }

    IEnumerator spawnDelay()
    {
        isSpawn = true;
        yield return new WaitForSeconds(.8f);
        isSpawn = false;
    }

    public void DestroyCheck() // 매칭 되는 거 부수기
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
                        allDots[i, j] = null;
                    }
                }
            }
        }
        StartCoroutine(DownCheck());
    }

    IEnumerator DownCheck() // 부수고 난 다음 라인 밑으로 내리기
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
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpawnDots());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
