using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary> 카메라에 넣어주세요 </summary>
public class CameraShake : MonoBehaviour
{
    /// <summary> 카메라 포지션 </summary>
    Vector3 myVec;

    /// <summary> 카메라 흔들리는 정도 </summary>
    float shakeAmount = 0.3f;
    /// <summary> 카메라 쉐이크 시간 </summary>
    float shakeTime = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        myVec = transform.position;
    }
    /// <summary> 카메라 흔들기 코루틴 호출 함수 </summary>
    public void ShakeCam()
    {
        StartCoroutine(cameraShake(shakeAmount, shakeTime));
    }
    /// <summary> [코루틴] 카메라 흔들기 </summary>
    IEnumerator cameraShake(float power, float time)
    {
        // 타이머
        float timer = 0;
        while(timer < time) 
        {
            // 카메라 포지션에 원형으로 랜덤한 위치로 이동
            Camera.main.transform.position = (Vector3)UnityEngine.Random.insideUnitSphere * power + myVec;
            // 타이머 시간을 모든 사양에서도 동일하게 작동하도록
            timer += Time.deltaTime;
            // 프레임 마다 작동
            yield return null;
        }
        // 원래 위치로 이동
        Camera.main.transform.position = myVec;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
