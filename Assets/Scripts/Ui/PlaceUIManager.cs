using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlaceUIManager : MonoBehaviour
{

    public static PlaceUIManager Instance;
    private RankPanel rankPanel;
    private TeamPanel teamPanel;
    private TipsPanel tipsPanel;

    private CountdownPanel countDown;
    // Start is called before the first frame update
    void Awake(){
        if (Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void UpdateTeam(Team team)
    {
        // 获取 team id
        // string id = team.Id;
        teamPanel.UpdateTeamUI(team);
        // 更新 team 对 id 对应 UI
        // throw new NotImplementedException();
    }

    private void UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    private void UpdateRank(List<User> userList)
    {
        rankPanel.UpdateRankUI(userList);
    }

    public void Init()
    {
        tipsPanel = GetComponentInChildren<TipsPanel>();
        rankPanel  = GetComponentInChildren<RankPanel>();
        teamPanel = GetComponentInChildren<TeamPanel>();
        countDown = GetComponentInChildren<CountdownPanel>();
        tipsPanel.Init();
        rankPanel.Init();
        teamPanel.Init();

        // Event
        UIEvent.OnTeamUpdateEvent += UpdateTeam;//
        UIEvent.OnRankUpdateEvent += UpdateRank;

    }

    public void SetRankData(List<User> data){
        rankPanel.SetRankUI(data);//设置数据
        //做一些事情，比如刷新UI
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    
    public void SetTeamData(List<Team> data){
        teamPanel.SetTeamUI(data);//
        //做一些事情，比如刷新UI
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    // public void SetTeamData(TeamItem data){
    //     teamPanel.SetTeamUI(data);//
    //     //做一些事情，比如刷新UI
    //     LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    // }

    public void AddTips(TipsItem tips){
        tipsPanel.AddTips(tips);
    }

    public void StartGame(Action action)
    {
        countDown.Init(action);
        countDown.StartTimeDown();
    }


    public void Reset() {
        // TODO
        countDown.Reset();
    }
}
