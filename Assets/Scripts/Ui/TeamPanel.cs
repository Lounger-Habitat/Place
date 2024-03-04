using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamPanel : MonoBehaviour
{
    private Transform teamItem;
    private List<Transform> useList = new List<Transform>();
    private List<Transform> poolList = new List<Transform>();
    public void Init()
    {
        teamItem = transform.GetChild(0);
        //teamItem.gameObject.SetActive(false);
    }

    //必须传递四个队伍，如果不传递四个队伍会将不存在队伍清除（目前是报错）
    public void SetTeamUI(List<Team> teamList)
    {

        //四个队伍
        for (int i = 0; i < 4; i++)
        {   //根据位置设置
            teamItem = transform.GetChild(i);
            if (teamList.Count > i)
            {
                var item = teamList[i];
                teamItem.Find("TeamName").GetComponent<TMP_Text>().text = item.Name;
                teamItem.Find("Data").GetComponent<TMP_Text>().text = $"{item.score}";
                // if (item.iconTexture != null)
                // {
                //     Sprite sp = Sprite.Create(item.iconTexture, new Rect(0, 0, item.iconTexture.width, item.iconTexture.height), new Vector2(.5f, .5f));
                //     //teamItem.Find("Icon").GetComponent<Image>().sprite = sp;
                // }
            }
            else
            {
                teamItem.Find("TeamName").GetComponent<TMP_Text>().text = "等待创建";
                teamItem.Find("Data").GetComponent<TMP_Text>().text = $"{0}";
                //teamItem.Find("Icon").GetComponent<Image>().sprite = null;
            }
        }
    }

    // public void SetTeamUI(Team teamData)
    // {

    //     teamItem = transform.GetChild(teamData.index);
    //     var item = teamData;
    //     teamItem.Find("TeamName").GetComponent<TMP_Text>().text = item.teamName;
    //     teamItem.Find("Data").GetComponent<TMP_Text>().text = $"当前人数:\n{item.teamNumber}";
    //     if (item.iconTexture != null)
    //     {
    //         Sprite sp = Sprite.Create(item.iconTexture, new Rect(0, 0, item.iconTexture.width, item.iconTexture.height), new Vector2(.5f, .5f));
    //         //teamItem.Find("Icon").GetComponent<Image>().sprite = sp;
    //     }
    // }

    public void UpdateTeamUI(Team team){
        teamItem = transform.GetChild(team.Id-1);
        teamItem.Find("Data").GetComponent<TMP_Text>().text = $"{team.score}";
    }
}
