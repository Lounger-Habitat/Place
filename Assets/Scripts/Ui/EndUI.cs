using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public Transform[] rankItem;
    
    public void Init()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
        //获取排行数据，将当前前三名玩家展示出来
        var userList = PlaceCenter.Instance.users.Values.ToList();
        userList.Sort((a,b)=>b.score.CompareTo(a.score));//降序排列贡献值，取前三位
        for (int i = 0; i < 3; i++)
        {
            var item = rankItem[i];//获取当前排行物体，分别赋值
            item.Find("Text_Player1").GetComponent<TMP_Text>().text = userList[i].Name;
            item.Find("Text_Value").GetComponent<TMP_Text>().text = $"贡献值: {userList[i].score}";
            item.Find("Picture").Find("InnerFrame").Find("Image").GetComponent<Image>().sprite = userList[i].userIcon;
        }
        userList.Sort((a,b)=>b.carryingInkCount.CompareTo(a.carryingInkCount));//降序排行有效颜料数 TODO:当前carryingInkCount不是颜料参数
        for (int i = 0; i < 3; i++)
        {
            int index = i + 3;
            var item = rankItem[index];//获取当前排行物体，分别赋值
            item.Find("Text_Player1").GetComponent<TMP_Text>().text = userList[i].Name;
            item.Find("Text_Value").GetComponent<TMP_Text>().text = $"贡献值: {userList[i].carryingInkCount}";
            item.Find("Picture").Find("InnerFrame").Find("Image").GetComponent<Image>().sprite = userList[i].userIcon;
        }
    }

    public void OnClickNextBtn()
    {
        SceneManager.LoadScene(0);
    }
}
