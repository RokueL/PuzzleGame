using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int Width;
    public int Height;
    Tile[,] allTiles;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new Tile[Width, Height];
        SetUp();
    }

    void SetUp()
    {
        for(int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++) 
            {
                Vector2 tempVec2 = new Vector2((float)i/1.5f,(float)j/1.5f);
                var tileSet = ObjectPoolManager.Instance.Pool.Get();
                tileSet.transform.position = tempVec2;
                tileSet.transform.parent = this.transform;
                tileSet.name = "( " + i + " , " + j + " )";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
