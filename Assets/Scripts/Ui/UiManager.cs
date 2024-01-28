// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class UiManager : MonoBehaviour
// {

//     public static UiManager Instance;
//     private RankListPanel rankList;
//     private TeamListPanel teamList;
//     private TipsPanel tipspanel;

//     private CountdownCoroutine countDown;
//     // Start is called before the first frame update
//     void Awake(){
//         if (Instance!=null)
//         {
//             Destroy(gameObject);
//             return;
//         }
//         Instance = this;
//     }
//     public void Init()
//     {
//         tipspanel = GetComponentInChildren<TipsPanel>();
//         rankList  = GetComponentInChildren<RankListPanel>();
//         teamList = GetComponentInChildren<TeamListPanel>();
//         countDown = GetComponentInChildren<CountdownCoroutine>();
//         tipspanel.Init();
//         rankList.Init();
//         teamList.Init();
//     }

//     public void SetRankData(List<RankItem> data){
//         rankList.SetRankUI(data);//设置数据
//         //做一些事情，比如刷新UI
//         LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
//     }
    
//     public void SetTeamData(List<TeamItem> data){
//         teamList.SetTeamUI(data);//
//         //做一些事情，比如刷新UI
//         LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
//     }
//     public void SetTeamData(TeamItem data){
//         teamList.SetTeamUI(data);//
//         //做一些事情，比如刷新UI
//         LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
//     }

//     public void AddTips(TipsItem tips){
//         tipspanel.AddTips(tips);
//     }

//     public void StartGame(Action action)
//     {
//         countDown.Init(action);
//         countDown.StartTimeDown();
//     }
// }
