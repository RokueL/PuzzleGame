using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public static sceneManager Instance;

    public Button loginButton;

    float time;
    float loadingTime = 2f;

    IEnumerator LoadAsynSceneCoroutine(string sceneName)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        switch (sceneName)
        {
            case "LobbyScene":
                while (!operation.isDone)
                {
                    time += Time.deltaTime;

                    if (operation.progress >= 0.9f && time >= loadingTime)
                    {
                        operation.allowSceneActivation = true;
                    }
                    yield return null;
                }
                break;
            case "GameScene":

                while (!operation.isDone)
                {
                    time += Time.deltaTime;

                    if (operation.progress >= 0.9f && time >= loadingTime)
                    {
                        operation.allowSceneActivation = true;
                    }
                    yield return null;
                }
                break;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    #region BUTTON_SETUP
    public void GameButtonSetup(Button button)
    {
        button.onClick.AddListener(LoadGameScene);
    }
    public void LobbyButtonSetup(Button button)
    {
        button.onClick.AddListener(LoadLobbyScene);
    }

    public void LoadLobbyScene()
    {
        StartCoroutine(LoadAsynSceneCoroutine("LobbyScene"));
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadAsynSceneCoroutine("GameScene"));
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "FirstScene")
        {
            loginButton.onClick.AddListener(LoadLobbyScene);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
