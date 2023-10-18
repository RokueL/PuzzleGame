using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 myVec;

    float shakeAmount = 0.3f;
    float shakeTime = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        myVec = transform.position;
    }

    public void ShakeCam()
    {
        StartCoroutine(cameraShake(shakeAmount, shakeTime));
    }

    IEnumerator cameraShake(float power, float time)
    {
        float timer = 0;
        while(timer < time) 
        {
            Camera.main.transform.position = (Vector3)UnityEngine.Random.insideUnitSphere * power + myVec;
            timer += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = myVec;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
