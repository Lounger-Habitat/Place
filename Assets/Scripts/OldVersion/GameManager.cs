// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public bool isRuning =false;
//     private void Awake()
//     {
//         Instance = this;
//     }

//     //单例 可以获取
//     //public UiManager UiManager;
//     // Start is called before the first frame update
//     void Start()
//     {
//         PlaceUIManager.Instance.Init();
//         createTeam();
//         StartGame();
//     }

//     //简单的队伍创建，自动
//     void createTeam()
//     {
//         // TeamManager.Instance.CreateTeam("sys", "1001", "坤坤之家");
//         // TeamManager.Instance.CreateTeam("sys", "1002", "小鸡脚");
//         // TeamManager.Instance.CreateTeam("sys", "1003", "篮球");
//         // TeamManager.Instance.CreateTeam("sys", "1004", "背带裤");
//     }

//     public void StartGame()
//     {
//         isRuning = true;
//         PlaceUIManager.Instance.StartGame(() =>
//         {
//             isRuning = false;
//             //游戏结束，其他逻辑
//         });
//     }
    
// }
