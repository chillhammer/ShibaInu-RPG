using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField]
    private string mainSceneName = null;
    [SerializeField]
    private GameObject[] dontDestroyOnLoadList = null;

    public void ChangeScene()
    {
        foreach (GameObject go in dontDestroyOnLoadList)
            DontDestroyOnLoad(go);
        SceneManager.LoadScene(mainSceneName);
    }
}
