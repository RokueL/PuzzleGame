using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject tilePrefab;
    int defaultCapacity = 50;
    int maxPoolSize = 100;

    public GameObject[] tiles = new GameObject[50];


    public IObjectPool<GameObject> Pool { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Init();
    }

    void Init()
    {
        Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);

        //=================< �ʱ� �̸� ����         >=====================
        for (int i = 0; i < defaultCapacity; i++)
        {
            Tile tile = CreatePooledItem().GetComponent<Tile>();
            tile.Pool.Release(tile.gameObject);
        }
    }

    //=================< ����         >=====================
    private GameObject CreatePooledItem()
    {
        GameObject poolGO = Instantiate(tilePrefab);
        poolGO.GetComponent<Tile>().Pool = this.Pool;
        return poolGO;
    }
    // =================< ��������         >=====================
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // =================< ��ȯ         >=====================
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // =================< ����         >=====================
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }


}