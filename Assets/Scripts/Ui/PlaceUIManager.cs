using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class PlaceUIManager : MonoBehaviour
{

    public static PlaceUIManager Instance;
    private RankPanel rankPanel;
    private TeamPanel teamPanel;
    private TipsPanel tipsPanel;
    private BeginUI beginUI;
    private EndUI endUI;

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

    private void UpdateTeamArea(PlaceTeamAreaManager teamArea)
    {
        teamPanel.UpdateTeamUI(teamArea);
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
        beginUI = GetComponentInChildren<BeginUI>();
        endUI = GetComponentInChildren<EndUI>();
        tipsPanel.Init();
        //rankPanel.Init();
        teamPanel.Init();
        beginUI.Init();
        // Event
        UIEvent.OnTeamUpdateEvent += UpdateTeam;//
        UIEvent.OnRankUpdateEvent += UpdateRank;
        //UIEvent.OnTeamAreaUpdateEvent += UpdateTeamArea;

    }

    public void SetRankData(List<User> data){
        rankPanel.SetRankUI(data);//设置数据
        //做一些事情，比如刷新UI
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    
    public void SetTeamData(List<Team> data){
        //teamPanel.SetTeamUI(data);//
        //做一些事情，比如刷新UI
        //LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
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
        countDown.Init(() =>
        {
            PlaceCenter.Instance.GenGif();
            endUI.Init();
        });
        countDown.StartTimeDown();
    }


    public void Reset() {
        // TODO
        countDown.Reset();
    }
    
    [ContextMenu("EndGame")]
    public void EndGameUI(/*调用这个方法应该把最后的画作，排行榜前三玩家传递进来*/)
    {
            PlaceCenter.Instance.GenGif();
            // endUI.Init()
    }
}
