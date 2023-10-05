using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour
{
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
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
