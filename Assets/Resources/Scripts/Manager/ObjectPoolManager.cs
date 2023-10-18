using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject bgPrefab;
    public GameObject dotPrefab;
    public GameObject EfRedPrefab;
    public GameObject EfYellowPrefab;
    public GameObject EfGreenPrefab;
    public GameObject EfBluePrefab;
    public GameObject EfPurplePrefab;

    public GameObject EfRowBombPrefab;
    public GameObject EfColBombPrefab;
    public GameObject EfAreaBombPrefab;

    int defaultCapacity = 100;
    int maxPoolSize = 150;

    public GameObject PoolIndex;

    public GameObject[] tiles = new GameObject[100];


    public IObjectPool<GameObject> Pool { get; private set; }
    public IObjectPool<GameObject> dotPool { get; private set; }

    public IObjectPool<GameObject> E_RedPool { get; private set; }
    public IObjectPool<GameObject> E_YellowPool { get; private set; }
    public IObjectPool<GameObject> E_GreenPool { get; private set; }
    public IObjectPool<GameObject> E_BluePool { get; private set; }
    public IObjectPool<GameObject> E_PurplePool { get; private set; }

    public IObjectPool<GameObject> E_RowBombPool { get; private set; }
    public IObjectPool<GameObject> E_ColBombPool { get; private set; }
    public IObjectPool<GameObject> E_AreaBombPool { get; private set; }

    ParticleSystem a;
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

        //매치 이펙트
        E_RedPool = new ObjectPool<GameObject>(CreatePooledEfRed, OnTakeFromPoolRed, OnReturnedToPoolRed,
       OnDestroyPoolObjectRed, true, defaultCapacity, maxPoolSize);

        E_YellowPool = new ObjectPool<GameObject>(CreatePooledEfYellow, OnTakeFromPoolYellow, OnReturnedToPoolYellow,
       OnDestroyPoolObjectYellow, true, defaultCapacity, maxPoolSize);

        E_GreenPool = new ObjectPool<GameObject>(CreatePooledEfGreen, OnTakeFromPoolGreen, OnReturnedToPoolGreen,
       OnDestroyPoolObjectGreen, true, defaultCapacity, maxPoolSize);

        E_BluePool = new ObjectPool<GameObject>(CreatePooledEfBlue, OnTakeFromPoolBlue, OnReturnedToPoolBlue,
       OnDestroyPoolObjectBlue, true, defaultCapacity, maxPoolSize);

        E_PurplePool = new ObjectPool<GameObject>(CreatePooledEfPurple, OnTakeFromPoolPurple, OnReturnedToPoolPurple,
       OnDestroyPoolObjectPurple, true, defaultCapacity, maxPoolSize);

        //폭탄 이펙트
        E_ColBombPool = new ObjectPool<GameObject>(CreatePooledEfCol, OnTakeFromPoolCol, OnReturnedToPoolCol,
OnDestroyPoolObjectCol, true, defaultCapacity, maxPoolSize);
        E_RowBombPool = new ObjectPool<GameObject>(CreatePooledEfRow, OnTakeFromPoolRow, OnReturnedToPoolRow,
OnDestroyPoolObjectRow, true, defaultCapacity, maxPoolSize);
        E_AreaBombPool = new ObjectPool<GameObject>(CreatePooledEfArea, OnTakeFromPoolArea, OnReturnedToPoolArea,
OnDestroyPoolObjectArea, true, defaultCapacity, maxPoolSize);

        //=================< 초기 미리 생성         >=====================
        for (int i = 0; i < defaultCapacity; i++)
        {
            Tile tile = CreatePooledItem().GetComponent<Tile>();
            tile.bgPool.Release(tile.gameObject);
            Dot dot= CreatePooledItem2().GetComponent<Dot>();
            dot.dottPool.Release(dot.gameObject);

            EffectsRed effR = CreatePooledEfRed().GetComponent<EffectsRed>();
            effR.EfRedPool.Release(effR.gameObject);

            EffectsYellow effY = CreatePooledEfYellow().GetComponent<EffectsYellow>();
            effY.EfYellowPool.Release(effY.gameObject);

            EffectsGreen effG = CreatePooledEfGreen().GetComponent<EffectsGreen>();
            effG.EfGreenPool.Release(effG.gameObject);

            EffectsBlue effB = CreatePooledEfBlue().GetComponent<EffectsBlue>();
            effB.EfBluePool.Release(effB.gameObject);

            EffectsPurple effP = CreatePooledEfPurple().GetComponent<EffectsPurple>();
            effP.EfPurplePool.Release(effP.gameObject);

            EffectsRowBomb effRow = CreatePooledEfRow().GetComponent<EffectsRowBomb>();
            effRow.EfRowBombPool.Release(effRow.gameObject);

            EffectsColBomb effCol = CreatePooledEfCol().GetComponent<EffectsColBomb>();
            effCol.EfColBombPool.Release(effCol.gameObject);

            EffectsAreaBomb effArea = CreatePooledEfArea().GetComponent<EffectsAreaBomb>();
            effArea.EfAreaBombPool.Release(effArea.gameObject);

            tile.transform.parent = PoolIndex.transform;
            dot.transform.parent = PoolIndex.transform;
            effR.transform.parent = PoolIndex.transform;
            effY.transform.parent = PoolIndex.transform;
            effG.transform.parent = PoolIndex.transform;
            effB.transform.parent = PoolIndex.transform;
            effP.transform.parent = PoolIndex.transform;
            effRow.transform.parent = PoolIndex.transform;
            effCol.transform.parent = PoolIndex.transform;
            effArea.transform.parent = PoolIndex.transform;
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
        var poolgo2 = poolGo2.GetComponent<Dot>();

        poolgo2.value = Random.Range(0, 5);
        poolgo2.isMatch = false;
        poolgo2.isBomb = false;
        poolgo2.isArea = false;
        poolgo2.isRow = false;
        poolgo2.isCol = false;
        poolgo2.bombType = Enum.Enum.Bomb.None;

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


    #region EFFECTS
    //=================< 생성         >=====================
    private GameObject CreatePooledEfRed()
    {
        GameObject poolGO = Instantiate(EfRedPrefab);
        poolGO.GetComponent<EffectsRed>().EfRedPool = this.E_RedPool;

        return poolGO;
    }
    private GameObject CreatePooledEfYellow()
    {
        GameObject poolGO = Instantiate(EfYellowPrefab);
        poolGO.GetComponent<EffectsYellow>().EfYellowPool = this.E_YellowPool;

        return poolGO;
    }
    private GameObject CreatePooledEfGreen()
    {
        GameObject poolGO = Instantiate(EfGreenPrefab);
        poolGO.GetComponent<EffectsGreen>().EfGreenPool = this.E_GreenPool;

        return poolGO;

    }
    private GameObject CreatePooledEfBlue()
    {
        GameObject poolGO = Instantiate(EfBluePrefab);
        poolGO.GetComponent<EffectsBlue>().EfBluePool = this.E_BluePool;

        return poolGO;
    }
    private GameObject CreatePooledEfPurple()
    {
        GameObject poolGO = Instantiate(EfPurplePrefab);
        poolGO.GetComponent<EffectsPurple>().EfPurplePool = this.E_PurplePool;

        return poolGO;
    }
    /// <summary>
    /// /Bomb
    /// </summary>
    /// <returns></returns>
    private GameObject CreatePooledEfRow()
    {
        GameObject poolGO = Instantiate(EfRowBombPrefab);
        poolGO.GetComponent<EffectsRowBomb>().EfRowBombPool = this.E_RowBombPool;

        return poolGO;
    }
    private GameObject CreatePooledEfCol()
    {
        GameObject poolGO = Instantiate(EfColBombPrefab);
        poolGO.GetComponent<EffectsColBomb>().EfColBombPool = this.E_ColBombPool;

        return poolGO;
    }
    private GameObject CreatePooledEfArea()
    {
        GameObject poolGO = Instantiate(EfAreaBombPrefab);
        poolGO.GetComponent<EffectsAreaBomb>().EfAreaBombPool = this.E_AreaBombPool;

        return poolGO;
    }
    // =================< 가져오기         >=====================
    private void OnTakeFromPoolRed(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnTakeFromPoolYellow(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnTakeFromPoolGreen(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnTakeFromPoolBlue(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnTakeFromPoolPurple(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    /// <summary>
    /// Bomb
    /// </summary>
    /// <param name="poolGo"></param>
    /// 
    private void OnTakeFromPoolRow(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnTakeFromPoolCol(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    private void OnTakeFromPoolArea(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }
    // =================< 반환         >=====================
    private void OnReturnedToPoolRed(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnReturnedToPoolYellow(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnReturnedToPoolGreen(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnReturnedToPoolBlue(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnReturnedToPoolPurple(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    private void OnReturnedToPoolRow(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnReturnedToPoolCol(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    private void OnReturnedToPoolArea(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }
    // =================< 삭제         >=====================
    private void OnDestroyPoolObjectRed(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    private void OnDestroyPoolObjectYellow(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    private void OnDestroyPoolObjectGreen(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    private void OnDestroyPoolObjectBlue(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    private void OnDestroyPoolObjectPurple(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    private void OnDestroyPoolObjectRow(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    private void OnDestroyPoolObjectCol(GameObject poolGo)
    {
        Destroy(poolGo);
    }
    private void OnDestroyPoolObjectArea(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    #endregion
}