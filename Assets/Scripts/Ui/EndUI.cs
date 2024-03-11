using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    public void Init()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
        //获取排行数据，将当前前三名玩家展示出来
    }

    public void OnClickNextBtn()
    {
        SceneManager.LoadScene(0);
    }
}
