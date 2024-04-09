using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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

    private Dictionary<string,User> currentUserList = new Dictionary<string, User>();//当前在排行榜上的玩家，不在排行榜上第一次上榜可以通知

    public void UpdateTeamUI(PlaceTeamAreaManager teamArae)
    {
        teamItem = transform.GetChild(teamArae.teaminfo.Id-1);
        teamItem.Find("TeamName").GetComponent<TMP_Text>().text = teamArae.teaminfo.Name;//名字起的太随便，先用现有的
        teamItem.Find("Data").GetComponent<TMP_Text>().text = $"{teamArae.userList.Count}/{teamArae.teaminfo.MaxTeamNumber}";
        teamItem.Find("TeamScore").GetChild(0).GetComponent<TMP_Text>().text = $"{teamArae.teaminfo.score}";
        
        //更新排行
        var list = teamArae.userList;
        list.Sort((a, b) => b.score - a.score);
        //list.Sort((a, b) => b.score.CompareTo(a.score));
        for (int i = 0; i < 3; i++)
        {
            var rankItem = teamItem.Find($"RankItem_{i}");
            if (list.Count <= i)
            {
                //没有数据了 需要自动填充
                rankItem.Find("Name").GetComponent<TMP_Text>().text = "虚位以待";
                rankItem.Find("Data").GetComponent<TMP_Text>().text = $"贡献:";
                //rankItem.Find("UserIcon").GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
            }
            else
            {
                var item = list[i];
                rankItem.Find("Name").GetComponent<TMP_Text>().text = item.username;
                rankItem.Find("Data").GetComponent<TMP_Text>().text = $"贡献:{item.score}";
                //rankItem.Find("UserIcon").GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.userIcon;//TODO:需要对接user头像，目前没有，所以我的遮罩就没了
                
                
                //检测当前玩家是否在榜
                if (!currentUserList.Values.Contains(item))
                {
                    //不在榜，上榜的进行通知 TODO：可以通知UI进行通知
                }
                string onlyId = $"{teamArae.teaminfo.Id}-{i}";//id是队伍id与排名的组合
                //检测玩家是否是第一,降序排序，第一个就是排行第一玩家
                if (i.Equals(0))
                {
                    if (currentUserList.ContainsKey(onlyId)&&!currentUserList[onlyId].Equals(item))
                    {
                        //第一不是当前玩家，此玩家夺得第一 TODO:进行UI通知，争榜一
                    }
                }
                //将当前玩家存到缓存中
                currentUserList[onlyId] = item;
            }
            
            
        }

        Debug.Log($"刷新-{teamArae.teaminfo.Id}-队伍");
    }
}
