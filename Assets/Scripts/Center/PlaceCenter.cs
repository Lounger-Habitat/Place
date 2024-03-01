using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlaceCenter : MonoBehaviour
{
    /*
        1. 负责分发消息
        2. 负责管理用户信息
        3. 负责管理队伍信息
        4. 负责管理场地信息
        5. 负责界面更新
        6. 负责调度模块
    */
    public static PlaceCenter Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 用户信息
#nullable enable

    public Dictionary<string, User> users = new Dictionary<string, User>();
    // 队伍信息
    public Dictionary<string, Team> teams = new Dictionary<string, Team>();

    public List<User> top8 = new List<User>();

    public string[] defaultTeamName = new string[] { "A", "B", "C", "D" };


    public GameObject nameUI;
    public GameObject bubbleUI;

    // 游戏开始标志
    bool gameRuning;

    public void Start()
    {
        // 初始化用户信息
        // 初始化队伍信息
        // 初始化场地信息
        // 初始化界面
        // 初始化调度模块

        // 界面初始化
        PlaceUIManager.Instance.Init();
        // 游戏计时
        StartGame();
        //CreateTeam();
    }

    public void Reset()
    {
        // 重置用户信息
        // 重置队伍信息
        // 重置场地信息
        // 重置界面
        // 重置调度模块
    }


    void CreateTeam()
    {
        teams.Add("A", PlaceTeamManager.Instance.teamAreas[0].teaminfo);
        teams.Add("B", PlaceTeamManager.Instance.teamAreas[1].teaminfo);
        teams.Add("C", PlaceTeamManager.Instance.teamAreas[2].teaminfo);
        teams.Add("D", PlaceTeamManager.Instance.teamAreas[3].teaminfo);
        var teamList = new List<Team>(){
            PlaceTeamManager.Instance.teamAreas[0].teaminfo,
            PlaceTeamManager.Instance.teamAreas[1].teaminfo,
            PlaceTeamManager.Instance.teamAreas[2].teaminfo,
            PlaceTeamManager.Instance.teamAreas[3].teaminfo
        };
        PlaceUIManager.Instance.SetTeamData(teamList);
    }

    // 绘制 ui
    public GameObject CreateNameTag(Transform characterTransform, string name)
    {
        GameObject canvasObj = GameObject.Find("SpaceCanvas");

        // 在Canvas下生成NameTag UI
        GameObject nameTagObj = Instantiate(nameUI, canvasObj.transform);

        // 设置NameTag UI的位置和属性
        // 假设NameTag UI有一个脚本用于定位和显示
        NameTag nameTagScript = nameTagObj.GetComponent<NameTag>();
        if (nameTagScript != null)
        {
            nameTagScript.target = characterTransform;
            nameTagScript.go_name = name;
            // 设置其他必要的属性，如偏移量等
        }
        return nameTagObj;
    }

    // add player
    public void AddPlayer(string username, int t)
    {
        // 检查用户是否已经存在
        if (users.ContainsKey(username))
        {
            Debug.Log("用户已存在");
            return;
        }

        // 创建角色
        User character = PlaceTeamManager.Instance.teamAreas[t - 1].CreateCharacterInTeamArea(username);
        if (character == null)
        {
            Debug.Log("创建角色失败");
            return;
        }
        users.Add(username, character);
    }

    public GameObject CreateMessageBubble(Transform characterTransform, string message)
    {
        GameObject canvasObj = GameObject.Find("SpaceCanvas");
        // AssetBundle assetBundle = AssetBundle.LoadFromFile("Assets/Prefabs");
        // GameObject messageBubbleInstance = assetBundle.LoadAsset<GameObject>("MessageBubble");

        // 在Canvas下生成NameTag UI
        GameObject bubblTagObj = Instantiate(bubbleUI, canvasObj.transform);

        // 设置NameTag UI的位置和属性
        // 假设NameTag UI有一个脚本用于定位和显示
        MessageBubbleTag messageTagScript = bubblTagObj.GetComponent<MessageBubbleTag>();
        if (messageTagScript != null)
        {
            messageTagScript.target = characterTransform;
            messageTagScript.message = message;
            // 设置其他必要的属性，如偏移量等
        }
        return bubblTagObj;
    }

    public void SayMessage(string username, string message)
    {
        // 检查用户是否已经存在
        if (!users.ContainsKey(username))
        {
            Debug.Log("用户不存在");
            return;
        }

        int teamId = int.Parse(users[username].camp);


        // 创建气泡
        GameObject bubble = PlaceTeamManager.Instance.teamAreas[teamId - 1].CreateMessageBubbleOnPlayer(username, message);

    }

    public bool CheckUser(string username)
    {
        if (!users.ContainsKey(username))
        {
            Debug.Log("用户不存在");
            return false;
        }
        return true;
    }

    public User? FindUser(string username)
    {
        if (!CheckUser(username))
        {
            Debug.Log("用户不存在");
            return null;
        }
        User u = users[username];
        return u;
    }

    public void JoinTeam(string username, string teamId)
    {
        int t = int.Parse(teamId);
        // 检查用户是否已经存在
        if (users.ContainsKey(username))
        {
            Debug.Log("用户已存在");
            return;
        }
        // 检查队伍是否已经存在
        // if (!teams.ContainsKey(teamId))
        // {
        //     Debug.Log("队伍不存在");
        //     return;
        // }
        // 检查队伍是否已经满员
        if (PlaceTeamManager.Instance.teamAreas[t - 1].userList.Count >= PlaceTeamManager.Instance.teamAreas[t - 1].teaminfo.MaxTeamNumber)
        {
            Debug.Log("队伍已满");
            return;
        }
        AddPlayer(username, t);
        // 将用户加入队伍区域
    }

    // 计算指令所需颜料数
    public int ComputeInstructionColorCount(Instruction ins)
    {
        int colorNumber = 0;
        switch (ins.mode)
        {
            case "/draw":
            case "/d":
                colorNumber = 1;
                break;
            case "/line":
            case "/l":
                colorNumber = PlaceBoardManager.Instance.GetLineCount(ins.x, ins.y, ins.ex, ins.ey);
                break;
            case "/paint":
            case "/p":
                colorNumber = PlaceBoardManager.Instance.GetPaintCount(ins.dx, ins.dy);
                break;

        }
        return colorNumber;
    }
    // 获取队伍颜料数
    public int GetTeamInkCount(int teamId)
    {
        int inkCount = (int)(PlaceTeamManager.Instance.teamAreas[teamId - 1].ink);
        return inkCount;
    }

    public void SetTeamInkCount(int teamId, int delta)
    {
        PlaceTeamManager.Instance.teamAreas[teamId - 1].ink += (float)delta;
    }

    public void StartGame()
    {
        gameRuning = true;
        PlaceUIManager.Instance.StartGame(() =>
        {
            gameRuning = false;
            //游戏结束，其他逻辑
        });
    }

    public void OnTeamUIUpdate(Team team)
    {
        UIEvent.OnTeamUIUpdate(team);
    }

    public void OnRankUIUpdate(User user)
    {


        if (user.score > 0)
        {
            // 如果 top 8 数量为 小于8 ,且用户不在 top 里
            if (top8.Count < 8 && !top8.Contains(user))
            {
                top8.Add(user);
            }
            else if (top8.Count == 8 && !top8.Contains(user))
            {
                // top8 数量已经达到8个
                // 比较当前用户的分数和top8中最小的分数
                int minScore = top8.Min(u => u.score);
                if (user.score > minScore)
                {
                    // 替换最小分数的用户
                    User minUser = top8.Find(u => u.score == minScore);
                    top8.Remove(minUser);
                    top8.Add(user);
                }
            }
        }


        // top8 里的元素按照 u.score 由大到小排序
        top8.Sort((user1, user2) => user2.score.CompareTo(user1.score));


        UIEvent.OnRankUIUpdate(top8);
    }

    // public void OnUserUIUpdate(User user)
    // {

    //     // 如果 top 8 数量为 小于8
    //     if (top8.Count < 8 && user.score > 0)
    //     {
    //         top8.Add(user);
    //     }
    //     else
    //     {
    //         // top8 数量已经达到8个
    //         // 比较当前用户的分数和top8中最小的分数
    //         int minScore = top8.Min(u => u.score);
    //         if (user.score > minScore)
    //         {
    //             // 替换最小分数的用户
    //             User minUser = top8.Find(u => u.score == minScore);
    //             top8.Remove(minUser);
    //             top8.Add(user);
    //         }
    //     }

    //     // top8 里的元素按照 u.score 由大到小排序

    //     top8.Sort((u1, u2) => u2.score.CompareTo(u1.score));

    //     // --------------------  stop ----------------

    //     UIEvent.OnUserUIUpdate(user);
    // }
}