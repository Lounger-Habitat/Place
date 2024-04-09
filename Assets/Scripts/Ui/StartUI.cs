using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public void StartGame()
    {
        //跳转到游戏场景
        SceneManager.LoadSceneAsync(1);
    }
}
