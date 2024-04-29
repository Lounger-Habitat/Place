
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
    #nullable enable
    // 用户信息
    public Dictionary<string, User> users = new Dictionary<string, User>();
    // 队伍信息
    public Dictionary<string, Team> teams = new Dictionary<string, Team>();

    public List<User> top8 = new List<User>();

    public string[] defaultTeamName = new string[] { "A", "B", "C", "D" };


    public GameObject nameUI = null!;
    public GameObject bubbleUI = null!;

    // 游戏开始标志
    public bool gameRuning = false;

    int[] lastTeamScore = new int[5] {0,0,0,0,0};

    public int recorderTime = 60;

    int baseId = 0;

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
        
        CreateTeam();
        
    }

    void Update() {
        if (gameRuning)
        {
            CalculateTeamScore();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            ResetGame();
        }
    }


    void CreateTeam()
    {
        // List<Team> teamList = new List<Team>();
        for (int i = 0; i < teams.Count; i++)
        {
            teams.Add($"Team{i+1}", PlaceTeamManager.Instance.teamAreas[0].teaminfo);
            // teamList.Add(PlaceTeamManager.Instance.teamAreas[0].teaminfo);
        }

        // var teamList = new List<Team>(){
        //     PlaceTeamManager.Instance.teamAreas[0].teaminfo,
        //     PlaceTeamManager.Instance.teamAreas[1].teaminfo,
        //     PlaceTeamManager.Instance.teamAreas[2].teaminfo,
        //     PlaceTeamManager.Instance.teamAreas[3].teaminfo
        // };
        // 不知道为什么成为空函数了
        // PlaceUIManager.Instance.SetTeamData(teamList);
    }

    // 绘制 ui
    public GameObject CreateNameTag(Transform characterTransform, User u)
    {
        GameObject canvasObj = GameObject.Find("SpaceCanvas");

        // 在Canvas下生成NameTag UI
        GameObject nameTagObj = Instantiate(nameUI, canvasObj.transform);

        // 设置NameTag UI的位置和属性
        // 假设NameTag UI有一个脚本用于定位和显示
        IconNameTag tagScript = nameTagObj.GetComponent<IconNameTag>();
        if (tagScript != null)
        {
            tagScript.target = characterTransform;
            tagScript.user = u;
            // 设置其他必要的属性，如偏移量等
        }
        return nameTagObj;
    }

    // public GameObject CreateIconTag(Transform characterTransform, Sprite icon)
    // {
    //     GameObject canvasObj = GameObject.Find("SpaceCanvas");

    //     // 在Canvas下生成NameTag UI
    //     GameObject nameTagObj = Instantiate(nameUI, canvasObj.transform);

    //     // 设置NameTag UI的位置和属性
    //     // 假设NameTag UI有一个脚本用于定位和显示
    //     NameTag nameTagScript = nameTagObj.GetComponent<NameTag>();
    //     if (nameTagScript != null)
    //     {
    //         nameTagScript.target = characterTransform;
    //         nameTagScript.icon.sprite = icon;
    //         // 设置其他必要的属性，如偏移量等
    //     }
    //     return nameTagObj;
    // }

    // add player
    public void AddPlayer(User user,int t)
    {
        // 检查用户是否已经存在
        // if (users.ContainsKey(user.username))
        // {
        //     Debug.Log("用户已存在");
        //     return;
        // }

        // 创建角色
        // int t = user.Camp;
        User u = PlaceTeamManager.Instance.teamAreas[t - 1].CreateCharacterInTeamArea(user);
        users.Add(u.Name, u);
        
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

    public void SayMessage(User user, string message)
    {
        // // 检查用户是否已经存在
        // if (!users.ContainsKey(username))
        // {
        //     Debug.Log("用户不存在");
        //     return;
        // }

        int teamId = users[user.Name].Camp;


        // 创建气泡
        GameObject bubble = PlaceTeamManager.Instance.teamAreas[teamId - 1].CreateMessageBubbleOnPlayer(user.Name, message);

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

    
    public int GenId() {
        baseId += 1;
        return baseId;
    }

    public int GenPUID() {
        // 多少位？
        // 平台 + 时间 + 主播 + 人数 + 价值 + ？？
        // TODO
        int puid = 0;
        return puid;
    }

    public void JoinGame(User user, string teamId)
    {
        int t = int.Parse(teamId);
        // 检查用户是否已经存在
        if (users.ContainsKey(user.Name))
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
        // 创建 用户

        AddPlayer(user,t);
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
        int inkCount = (int)(PlaceTeamManager.Instance.teamAreas[teamId - 1].teaminfo.ink);
        // Debug.Log("Team " + teamId + " ink count :" + inkCount);
        return inkCount;
    }

    public void SetTeamInkCount(int teamId, int delta)
    {
        PlaceTeamManager.Instance.teamAreas[teamId - 1].teaminfo.ink += (float)delta;
    }

    public void StartGame()
    {
        if (gameRuning)
        {
            Debug.Log("游戏已经开始");
            return;
        }
        PlaceUIManager.Instance.StartGame(() =>
        {
            gameRuning = false;
            //游戏结束，其他逻辑
        });
        gameRuning = true;
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


    // 计算 队伍分数
    public void CalculateTeamScore()
    {
        int[] newTeamScore  = new int[5];
        foreach (int c in PlaceBoardManager.Instance.pixelsInfos)
        {
            newTeamScore[c]++;
        }

        for (int i = 0; i < newTeamScore.Length; i++)
        {
            if (i!=0 && lastTeamScore[i] != newTeamScore[i]  )
            {
                int score = newTeamScore[i];
                // 如果不相等，调用其他函数或执行其他逻辑
                PlaceTeamManager.Instance.teamAreas[i-1].teaminfo.score = score;
                // OnTeamUIUpdate(PlaceTeamManager.Instance.teamAreas[i-1].teaminfo);
            }
        }

        // UI显示 , 不知道为什么成为空函数了
        // PlaceUIManager.Instance.SetTeamData(teams.Values.ToList());

    }
    public void GainPower(string username, float power)
    {
        if (!users.ContainsKey(username))
        {
            Debug.Log("用户不存在");
            return;
        }
        User u = users[username];
        TipsType messageType = TipsType.messagePanel;
        int normalPower = 0;
        string message = "";
        switch (u.currentState.topState)
        {
            case HighLevelState.Draw:
                messageType = u.Camp == 1? TipsType.giftDrawPanel:TipsType.giftDrawPanelRight;
                break;
            case HighLevelState.Attack:
                messageType = TipsType.giftAttackPanel;
                break;
            case HighLevelState.Defend:
                messageType = TipsType.giftDefensePanel;
                break;
        }
        switch (power)
        {
            case 0.1f://这是礼物得人民币价值，那应该在这个里边通知
                normalPower = 10;
                // u.level += normalPower;
                // u.score += normalPower * 10;
                // PlaceTeamManager.Instance.teamAreas[u.camp - 1].teaminfo.ink += normalPower * 10;
                // 特效 动画
                // UI 更新
                message = "急速神行";
                u.character.GetComponent<PlacePlayerController>().ActiveSpeedlUp(10);
                break;
            case 1:
                normalPower = 100;//固定是加颜料
                message = "颜料爆发";
                u.character.GetComponent<PlacePlayerController>().Thunder();
                break;
            case 1.9f:
                normalPower = 199;//固定是攻击
                u.character.GetComponent<PlacePlayerController>().Tornado((int)(power*10));
                message = "风之束缚";
                break;
            case 5.2f:
                normalPower = 520;//固定是防御
                u.character.GetComponent<PlacePlayerController>().Invincible(60);
                message = "绝对防御";
                break;
            case 9.9f:
                normalPower = 999;
                message = "颜料核弹";
                // 随机自动画一个图案
                break;
            case 19.9f:
                normalPower = 1999;
                message = "掌控雷电";
                // 全屏攻击
                break;
            case 29.9f:
                normalPower = 2990;
                break;
            case 52f:
                normalPower = 5200;
                break;
            case 66.6f:
                normalPower = 6666;
                break;
            case 88.8f:
                normalPower = 8888;
                break;
            case 99.9f:
                normalPower = 9999;
                break;
            case 120f:
                normalPower = 12000;
                break;
            case 166.6f:
                normalPower = 16666;
                break;
            case 188.8f:
                normalPower = 18888;
                break;
            case 300f:
                normalPower = 30000;
                break;
        }
        u.character.GetComponent<PlacePlayerController>().InkUp(normalPower);
        u.score += normalPower;
        u.genInkCount += normalPower;
        u.Update();
        PlaceTeamManager.Instance.teamAreas[u.Camp - 1].teaminfo.ink += normalPower;
        //在这通知UI？还得要状态切换啊，先检查状态再通知
        PlaceUIManager.Instance.AddTips(new TipsItem()
        {
            userName = username,
            text = message,
            icon = u.userIcon,//玩家头像
            tipsType = messageType,
            value =$"+{normalPower:D}",
            isLeft = u.Camp == 1,
            level = u.level
        });
    }

    public void GainLikePower(User user, long power)
    {
        // B 站 每人 每天 点赞上限 1000
        int p = (int)power;
        user.score += p;
        user.Update();
        PlaceTeamManager.Instance.teamAreas[user.Camp - 1].teaminfo.ink += p;
        // 颜料增加的特效
        user.character.GetComponent<PlacePlayerController>().InkUp(p);
        var messageType = user.Camp == 1? TipsType.likeTipsPanel:TipsType.likeTipsPanelRight;
        string message = "";
        if (p<5)
        {
            message = $"点赞！颜料x{p}";
        }

        if (p>5)
        {
            message = $"点赞手速突破天际！！颜料x{p}";
        }
        
        
        PlaceUIManager.Instance.AddTips(new TipsItem()
        {
            userName = user.Name,
            text =message,
            icon = user.userIcon,//玩家头像
            tipsType = messageType,
            value =$"+{1}",
            isLeft = user.Camp == 1
        });
        // 限时加速
        // StartCoroutine(user.character.GetComponent<PlacePlayerController>().TimeLimitSpeedUp(p));
    }

    public void GainGiftPower(User user, float power)
    {
        int p = (int)power;
        user.score += p;
        user.Update();
        PlaceTeamManager.Instance.teamAreas[user.Camp - 1].teaminfo.ink += p;
        // 限时加速
        user.character.GetComponent<PlacePlayerController>().Tornado((int)power);
    }


    // 重新开始游戏
    void RestartGame()
    {
        ResetGame();
        StartGame();
    }

    void ResetGame()
    {
        PlaceBoardManager.Instance.Reset();
        PlaceTeamManager.Instance.Reset();
        PlaceUIManager.Instance.Reset();

        // clear 
        users.Clear();
        top8.Clear();

    }

    // 开始记录图像
    public void RecordImage()
    {
        StartCoroutine(SaveImagePreRecorderTime(recorderTime));
    }
    // 结束 生成 gif
    public void GenGif()
    {
        PlaceBoardManager.Instance.ConvertTex2DToGIF();
    }

    // public void ShowGif(string path) {
    //     PlaceUIManager.Instance.endUI.ShowGIF(path);
    // }


    //  ===== 协程 =====


    // IEnumerator CallTornado(User u ,int num)
    // {
    //     u.character.GetComponent<PlacePlayerController>().Tornado(num);
    // }

    IEnumerator SaveImagePreRecorderTime(int time = 60)
    {
        // 持续等待一分钟
        while (gameRuning) {
            yield return new WaitForSeconds(time);
            PlaceBoardManager.Instance.SaveImage();
        }
    }
}