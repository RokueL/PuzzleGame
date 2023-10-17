using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Effects : MonoBehaviour
{

    public IObjectPool<GameObject> EfRedPool { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroySelf());
    }

    IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(1f);
        EfRedPool.Release(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
