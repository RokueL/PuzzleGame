using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Tile : MonoBehaviour
{
    public IObjectPool<GameObject> bgPool { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        var dotSet = ObjectPoolManager.Instance.dotPool.Get();
        dotSet.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
