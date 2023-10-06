using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject bgPrefab;
    public GameObject dotPrefab;
    int defaultCapacity = 100;
    int maxPoolSize = 150;

    public GameObject PoolIndex;

    public GameObject[] tiles = new GameObject[50];


    public IObjectPool<GameObject> Pool { get; private set; }
    public IObjectPool<GameObject> dotPool { get; private set; }

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
        //=================< 풀링 선언         >=====================
        Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);

        dotPool = new ObjectPool<GameObject>(CreatePooledItem2, OnTakeFromPool2, OnReturnedToPool2,
        OnDestroyPoolObject2, true, defaultCapacity, maxPoolSize);

        //=================< 초기 미리 생성         >=====================
        for (int i = 0; i < defaultCapacity; i++)
        {
            Tile tile = CreatePooledItem().GetComponent<Tile>();
            tile.bgPool.Release(tile.gameObject);
            Dot dot= CreatePooledItem2().GetComponent<Dot>();
            dot.dottPool.Release(dot.gameObject);

            tile.transform.parent = PoolIndex.transform;
            dot.transform.parent = PoolIndex.transform;
        }
    }

    //=================< 생성         >=====================
    private GameObject CreatePooledItem()
    {
        GameObject poolGO = Instantiate(bgPrefab);
        poolGO.GetComponent<Tile>().bgPool = this.Pool;

        return poolGO;
    }
    private GameObject CreatePooledItem2()
    {
        GameObject poolGO2 = Instantiate(dotPrefab);
        poolGO2.GetComponent<Dot>().dottPool = this.dotPool;

        return poolGO2;
    }
    // =================< 가져오기         >=====================
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnTakeFromPool2(GameObject poolGo2)
    {
        poolGo2.GetComponent<Dot>().value = Random.Range(0, 5);
        poolGo2.GetComponent<Dot>().isMatch = false;
        poolGo2.SetActive(true);
    }

    // =================< 반환         >=====================
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnReturnedToPool2(GameObject poolGo2)
    {
        poolGo2.SetActive(false);
    }

    // =================< 삭제         >=====================
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    private void OnDestroyPoolObject2(GameObject poolGo2)
    {
        Destroy(poolGo2);
    }


}