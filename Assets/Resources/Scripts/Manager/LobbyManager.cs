using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button StageButton;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager.Instance.GameButtonSetup(StageButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
