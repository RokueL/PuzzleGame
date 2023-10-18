using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectsAreaBomb : MonoBehaviour
{

    public IObjectPool<GameObject> EfAreaBombPool { get; set; }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void DestroySelf()
    {
        StartCoroutine(destroySelf());
    }

    IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(1f);
        EfAreaBombPool.Release(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
