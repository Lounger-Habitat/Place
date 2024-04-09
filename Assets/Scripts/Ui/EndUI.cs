using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    public void Init()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
        //获取排行数据，将当前前三名玩家展示出来
        var userList = PlaceCenter.Instance.users.Values.ToList();
        userList.Sort((a,b)=>b.score.CompareTo(a.score));//降序排列贡献值，取前三位
        for (int i = 0; i < 3; i++)
        {
            
        }
    }

    public void OnClickNextBtn()
    {
        SceneManager.LoadScene(0);
    }
}
