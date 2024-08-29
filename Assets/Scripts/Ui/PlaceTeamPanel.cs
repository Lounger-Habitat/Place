using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceTeamPanel : MonoBehaviour
{
    private Transform teamItem;//排行物体。
    private Dictionary<string,User> currentUserList = new Dictionary<string, User>();//当前在排行榜上的玩家，不在排行榜上第一次上榜可以通知
    private int numMax;

    public PlaceTeamSlider leftTeamScore, rightTeamScore;
    public void Init()
    {
        teamItem = transform.GetChild(0);
        numMax = GameSettingManager.Instance.Mode == GameMode.Competition ? 3 : 5;
        numMax = teamItem.Find("RankList").Find("RankContent").childCount;
        UIEvent.OnTeamUpdateEvent += UpdateTeamUI;//
    }

    private string teamPanel, ranklist,rankItemname;
    public void UpdateTeamUI(Team team){
        teamItem = transform.GetChild(team.Id-1);//1是左边UI，2是右边UI
        teamPanel = team.Id == 1 ? "TeamPanel" : "TeamPanelRight";
        ranklist = team.Id == 1 ? "RankList" : "RankListRight";
        rankItemname = team.Id == 1 ? "RankItem" : "RankItemRight";
        var teamScore = team.Id == 1 ? leftTeamScore : rightTeamScore;
        //teamItem.Find("Data").GetComponent<TMP_Text>().text = $"{team.score}";
        //数据全部通过team获取
        var list = PlaceTeamManager.Instance.teamAreas[team.Id - 1].userList;
        teamItem.Find(teamPanel).Find("TeamName").GetComponent<TMP_Text>().text = team.Name;//名字起的太随便，先用现有的
        teamItem.Find(teamPanel).Find("PalyerNum").GetComponent<TMP_Text>().text = $"{list.Count}/{team.MaxTeamNumber}";
        // TODO this
        if (GameSettingManager.Instance.Mode == GameMode.Competition && PlaceCenter.Instance.gameRuning)//不切换场景会报错
        {
            teamScore.valueText.text = $"{team.score}/{PlaceTeamBoardManager.Instance.width * PlaceTeamBoardManager.Instance.height}";//队伍分数更新
        }
        else if(GameSettingManager.Instance.Mode != GameMode.Competition)
        {
            teamScore.valueText.text = $"{team.score}/{PlaceBoardManager.Instance.pixelsCampInfos.Length}";//队伍分数更新
        }
        
        teamScore.UpdateSlider(team.score);
        //teamItem.Find("TeamScore").GetChild(0).GetComponent<TMP_Text>().text = $"{team.score}";//队伍分数被拆分出去了

        var rankContent = teamItem.Find(ranklist).Find("RankContent");
        //更新排行
        list.Sort((a, b) => b.score - a.score);
        //list.Sort((a, b) => b.score.CompareTo(a.score));
        for (int i = 0; i < numMax; i++)
        {
            var rankItem = rankContent.Find($"{rankItemname}_{i+1}");
            if (list.Count <= i)
            {
                //直接隐藏
                rankItem.gameObject.SetActive(false);
                //没有数据了 需要自动填充
                // rankItem.Find("Name").GetComponent<TMP_Text>().text = "虚位以待";
                // rankItem.Find("Data").GetComponent<TMP_Text>().text = $"贡献:";
                // rankItem.Find("UserIcon").GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
            }
            else
            {
                rankItem.gameObject.SetActive(true);
                var item = list[i];
                rankItem.Find("Name").GetComponent<TMP_Text>().text = item.Name;
                rankItem.Find("Score").Find("Data").GetComponent<TMP_Text>().text = $"{item.score}";
                rankItem.Find("UserIcon").GetChild(1).GetChild(0).GetComponent<Image>().sprite = item.userIcon;//TODO:需要对接user头像，目前没有，所以我的遮罩就没了
                
                
                //检测当前玩家是否在榜
                if (!currentUserList.Values.Contains(item))
                {
                    //不在榜，上榜的进行通知 TODO：可以通知UI进行通知
                }
                string onlyId = $"{team.Id}-{i}";//id是队伍id与排名的组合
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
    }

    private void OnDestroy()
    {
        UIEvent.OnTeamUpdateEvent -= UpdateTeamUI;//
    }
}
