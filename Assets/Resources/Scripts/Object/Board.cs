using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enum;
using Unity.Mathematics;

public class Board : MonoBehaviour
{
    public Enum.Enum.State state = Enum.Enum.State.Move;

    int downCount;
    int endcheck;
    public int Width;
    public int Height;
    public int offset;
    public GameObject[,] allTiles;
    public GameObject[,] allDots;
    
    bool isSpawn;
    public bool isMakeBomb;

    public Dot currentDot;

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

    #region DEFAULT

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

                mathCheck(i,j,tempVec2);
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

    void mathCheck(int col, int row, Vector2 vec)
    {
        var dotSet = ObjectPoolManager.Instance.dotPool.Get();
        Dot dotSetD = dotSet.GetComponent<Dot>();
        dotSetD.row = row;
        dotSetD.column = col;

        if(row > 1)
        {
            Dot down = allDots[col, row - 1].GetComponent<Dot>();
            Dot ddown = allDots[col, row - 2].GetComponent<Dot>();

            if (down.value == dotSetD.value && ddown.value == dotSetD.value)
            {
                while (reSetCheck(dotSetD.value, down.value))
                {
                    dotSetD.value = UnityEngine.Random.Range(0, 5);
                }
            }
        }

        if(col > 1)
        {
            Dot left = allDots[col - 1, row].GetComponent<Dot>();
            Dot lleft = allDots[col - 2, row].GetComponent<Dot>();

            if (left.value == dotSetD.value && lleft.value == dotSetD.value)
            {
                Debug.Log(left.value + ", " + dotSetD.value);
                while (reSetCheck(dotSetD.value, left.value))
                {
                    dotSetD.value = UnityEngine.Random.Range(0, 5);
                    Debug.Log("µ¹¸² : " + dotSetD.value);
                }
            }
        }
        dotSet.transform.position = vec;
        dotSet.transform.parent = this.transform;
        dotSet.name = "( " + col + " , " + row + " )";
        allDots[col, row] = dotSet;
    }

    bool reSetCheck(int dotvalue, int othervalue)
    {
        while (othervalue == dotvalue )
        {
            return true;
        }
        return false;
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
        state = Enum.Enum.State.Wait;
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

        currentDot = null;
        yield return new WaitForSeconds(.4f);
        state = Enum.Enum.State.Move;
    }

    IEnumerator spawnDelay()
    {
        isSpawn = true;
        yield return new WaitForSeconds(.8f);
        isSpawn = false;
    }

    #endregion

    public void bombCheck(int col, int row)
    {
        findMatch.bombMatch.Clear();
        var dotBomb = allDots[col, row].GetComponent<Dot>().bombType;
        if (dotBomb == Enum.Enum.Bomb.columnBomb)
        {
            Debug.Log("°¡·ÎÆøÅº");
            findMatch.bombAddListColumn(col, row);
        }
        else if (dotBomb == Enum.Enum.Bomb.rowBomb)
        {
            Debug.Log("¼¼·ÎÆøÅº");
            findMatch.bombAddListRow(col, row);
        }
        else if (dotBomb == Enum.Enum.Bomb.areaBomb)
        {
            Debug.Log("¹üÀ§ÆøÅº");
            findMatch.bombAddListArea(col, row);
        }
        else
        {
        }
    }

    void MakeBomb()
    {
        if (findMatch.match.Count == 4 || findMatch.match.Count == 7)
        {
            Debug.Log(findMatch.match.Count);
            Debug.Log("ÆøÅº¸¸µé±â");
            isMakeBomb = true;
            findMatch.checkColRowBomb();
            isMakeBomb = false;
        }
        if (findMatch.match.Count == 5)
        {
            Debug.Log(findMatch.match.Count);
            Debug.Log("ÆøÅº¸¸µé±â");
            isMakeBomb = true;
            findMatch.checkAreaBomb();
            isMakeBomb = false;
        }
        findMatch.match.Clear();
    }

    public void DestroyCheck() // ¸ÅÄª µÇ´Â °Å ºÎ¼ö±â
    {
        MakeBomb();
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    var alldot = allDots[i, j].GetComponent<Dot>();
                    if (alldot.isMatch == true)
                    {
                        if (alldot.isBomb == true)
                        {
                            bombCheck(i, j);
                        }
                        if (allDots[i, j] != null)
                        {
                            findMatch.match.Remove(allDots[i, j]);
                            allDots[i, j].GetComponent<Dot>().dottPool.Release(allDots[i, j]);
                            allDots[i, j] = null;
                        }
                    }
                }
            }
        }
        findMatch.bombMatch.Clear();
        StartCoroutine(DownCheck());
    }

    

    public IEnumerator DownCheck() // ºÎ¼ö°í ³­ ´ÙÀ½ ¶óÀÎ ¹ØÀ¸·Î ³»¸®±â
    {
        state = Enum.Enum.State.Wait;
        downCount = 0;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++) // ¾Æ·¡¿¡¼­ À§ ÇÑÁÙ Ã¼Å· ÈÄ ¿·¿¡ ÇÑÁÙ Ã¼Å·
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
        //findMatch.MatchFinder();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpawnDots());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
