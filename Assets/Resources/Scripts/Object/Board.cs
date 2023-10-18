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
    int destroyCount;
    int spawnCheck;
    public int Width;
    public int Height;
    public int offset;
    public GameObject[,] allTiles;
    public GameObject[,] allDots;

    bool isSpawn;
    public bool isMakeBomb;

    public Dot currentDot;

    FindMatch findMatch;
    CameraShake camShake;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new GameObject[Width, Height];
        allDots = new GameObject[Width, Height];
        findMatch = FindObjectOfType<FindMatch>();
        camShake = FindObjectOfType<CameraShake>();
        SetUp();
        CheckdestroyDelay();
    }

    public void CheckdestroyDelay()
    {
        StartCoroutine(checkdestroyDelay());
    }

    IEnumerator checkdestroyDelay()
    {
        state = Enum.Enum.State.Destroy;
        yield return new WaitForSeconds(.3f);
        DestroyCheck();
    }

    public void DestroyCheck()
    {
        if (state == Enum.Enum.State.Destroy)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    DestroyCheckOn(i, j);
                }
            }
            if (destroyCount == 0)
            {
                state = Enum.Enum.State.Move;
            }
            else
            {
                destroyCount = 0;
                findMatch.match.Clear();
                StartCoroutine(downRow());
            }
        }
        else
        {
            return;
        }
    }

    void DestroyCheckOn(int col, int row)
    {
        if (allDots[col, row] != null)
        {
            var alldot = allDots[col, row].GetComponent<Dot>();
            BombCheck();
            if (alldot.isMatch)
            {
                if (alldot.isBomb == true)
                {
                    BombExplosion(col, row);
                }
                makeEffect(alldot.value, col, row);
                findMatch.match.Remove(allDots[col, row]);
                alldot.dottPool.Release(allDots[col, row]);
                allDots[col, row] = null;
                destroyCount++;
            }
        }
    }

    public void makeEffect(int value, int col, int row)
    {
        Debug.Log(value);
        var obj = ObjectPoolManager.Instance;
        Vector2 tempVec = new Vector2(col, row);
        switch (value)
        {
            case 0:
                var eff0 = obj.E_RedPool.Get();
                eff0.transform.position = tempVec;
                eff0.GetComponent<EffectsRed>().DestroySelf();
                Debug.Log("이펙트 생성");
                break; 
            case 1:
                var eff1 = obj.E_YellowPool.Get();
                eff1.transform.position = tempVec;
                eff1.GetComponent<EffectsYellow>().DestroySelf();
                break;
            case 2:
                var eff2 = obj.E_GreenPool.Get();
                eff2.transform.position = tempVec;
                eff2.GetComponent<EffectsGreen>().DestroySelf();
                break;
            case 3:
                var eff3 = obj.E_BluePool.Get();
                eff3.transform.position = tempVec;
                eff3.GetComponent<EffectsBlue>().DestroySelf();
                break;
            case 4:
                var eff4 = obj.E_PurplePool.Get();
                eff4.transform.position = tempVec;
                eff4.GetComponent<EffectsPurple>().DestroySelf();
                break;

        }
    }

    void BombCheck()
    {
        if (currentDot != null)
        {
        }
        else if(currentDot == null)
        {
        }

        if(findMatch.match.Count == 4 || findMatch.match.Count == 7)
        {
            isMakeBomb = true;
            findMatch.checkColRowBomb();
            isMakeBomb = false;
        }
        else if(findMatch.match.Count == 5 || findMatch.match.Count == 8 || findMatch.match.Count == 9)
        {
            isMakeBomb = true;
            findMatch.checkAreaBomb();
            isMakeBomb = false;
        }
        findMatch.match.Clear();
        currentDot = null;
    }

    IEnumerator downRow()
    {
        if(state == Enum.Enum.State.Destroy)
        {
            downCount = 0;
            for(int i = 0; i< Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    if (allDots[i, j] == null)
                    {
                        downCount++;
                    }
                    else if (downCount > 0 && allDots[i,j] != null)
                    {
                        allDots[i, j].GetComponent<Dot>().row -= downCount;
                        allDots[i, j] = null;
                    }
                }
                downCount = 0;
            }
        }
        yield return new WaitForSeconds(.4f);
        state = Enum.Enum.State.Down;
        findMatch.MatchDown();
        yield return new WaitForSeconds(.4f);
        state = Enum.Enum.State.Spawn;
        SpawnDot();
    }

    void SpawnDot()
    {
        if(state == Enum.Enum.State.Spawn)
        {
            spawnCheck = 0;
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
                        spawnCheck++;
                    }
                }
            }
            findMatch.MatchDown();
        }
    }


    public void BombExplosion(int col, int row)
    {
        findMatch.bombMatch.Clear();
        var dotBomb = allDots[col, row].GetComponent<Dot>().bombType;
        if (dotBomb == Enum.Enum.Bomb.columnBomb)
        {
            camShake.ShakeCam();
            findMatch.bombAddListColumn(col, row);
        }
        else if (dotBomb == Enum.Enum.Bomb.rowBomb)
        {
            camShake.ShakeCam();
            findMatch.bombAddListRow(col, row);
        }
        else if (dotBomb == Enum.Enum.Bomb.areaBomb)
        {
            camShake.ShakeCam();
            findMatch.bombAddListArea(col, row);
        }
        else
        {
        }
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

    void mathCheck(int col, int row, Vector2 vec)  //다시 스폰할 때 중복 x
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
                    Debug.Log("돌림 : " + dotSetD.value);
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

    
    

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
