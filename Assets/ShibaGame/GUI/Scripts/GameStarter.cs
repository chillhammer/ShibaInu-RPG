using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField]
    private string mainSceneName = null;

    public void ChangeScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
