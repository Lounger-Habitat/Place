
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text.RegularExpressions;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using Unity.VisualScripting;

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

    int[] lastTeamScore = new int[3] { 0, 0, 0 };

    public int recorderTime = 6;

    int baseId = 0;

    private int maxSocre = 120000;
    // List<Texture2D>? demoTextures = null;
    Dictionary<string, Texture2D> freeDemoTexturesDic = new Dictionary<string, Texture2D>();
    Dictionary<string, Texture2D> giftDemoTexturesDic = new Dictionary<string, Texture2D>();
    Dictionary<string, List<Texture2D>> cartoonTexturesDic = new Dictionary<string, List<Texture2D>>();

    public string platform = "bilibili";
    public string anchorName = "anchor";

    public bool Low = true;
    public bool High = false;

#if UNITY_EDITOR
    string demoPath = "Assets/Images/Demo";
#else
    string demoPath = Application.streamingAssetsPath + "/Demo";
#endif

    public void Start()
    {
#if !UNITY_EDITOR
            Debug.unityLogger.logEnabled = false;
#endif
        // 初始化用户信息
        // 初始化队伍信息
        // 初始化场地信息
        // 初始化界面
        // 初始化调度模块

        // 界面初始化
        PlaceUIManager.Instance.Init();
        // 游戏计时

        CreateTeam();
        // demoTextures = LoadDemoResources();

        freeDemoTexturesDic = LoadDemoDicResources("free");
        giftDemoTexturesDic = LoadDemoDicResources("gift");
        // DemoTexturesDic = LoadDemoDicResources("free");
        if (GameSettingManager.Instance.Mode == GameMode.Competition)
        {
            maxSocre = PlaceTeamBoardManager.Instance.width *
                       PlaceTeamBoardManager.Instance.height;
        }
        else
        {
            maxSocre = PlaceBoardManager.Instance.width *
                       PlaceBoardManager.Instance.height;
        }
        //初始化平台信息，与主播信息

    }

    void Update()
    {
        if (gameRuning)
        {
            CalculateTeamScore();
        }
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     StartGame();
        // }
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     ResetGame();
        // }
    }


    void CreateTeam()
    {
        // List<Team> teamList = new List<Team>();
        for (int i = 0; i < teams.Count; i++)
        {
            teams.Add($"Team{i + 1}", PlaceTeamManager.Instance.teamAreas[0].teaminfo);
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
    public void AddPlayer(User user, int t)
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

    // public void SayMessage(User user, string message)
    // {
    //     // // 检查用户是否已经存在
    //     // if (!users.ContainsKey(username))
    //     // {
    //     //     Debug.Log("用户不存在");
    //     //     return;
    //     // }

    //     int teamId = users[user.Name].Camp;


    //     // 创建气泡
    //     GameObject bubble = PlaceTeamManager.Instance.teamAreas[teamId - 1].CreateMessageBubbleOnPlayer(user.Name, message);

    // }

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


    public int GenId()
    {
        baseId += 1;
        return baseId;
    }

    public void GenPUID()
    {
        // 多少位？
        // 平台 + 时间 + 主播 + 人数 + 价值 + ？？
        // TODO
        if (GameSettingManager.Instance.Mode == GameMode.Competition)//不切换场景会报错
        {
            PlaceTeamBoardManager.GenerateUniqueId();
        }
        else
        {
            PlaceBoardManager.GenerateUniqueId();
            string puid = PlaceBoardManager.UniqueId;
            var text = GameObject.Find("DrawID").transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = $"PUID : {puid}";
        }
        
        
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

        AddPlayer(user, t);
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

    public Action satrtGameAction;

    public void StartGame()
    {
        if (gameRuning)
        {
            Debug.Log("游戏已经开始");
            return;
        }
        satrtGameAction?.Invoke();
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
        // 队伍长度
        int[] newTeamScore = new int[3];
        if (GameSettingManager.Instance.Mode == GameMode.Competition)
        {
            // foreach (int c in PlaceTeamBoardManager.Instance.team1Board.pixelsUserInfos)
            // {
            //     if (c != 0)
            //     {
            //         newTeamScore[1]++;
            //     }
            // }
            // foreach (int c in PlaceTeamBoardManager.Instance.team2Board.pixelsUserInfos)
            // {
            //     if (c != 0)
            //     {
            //         newTeamScore[2]++;
            //     }
            // }
            newTeamScore[1] = PlaceTeamBoardManager.Instance.team1Board.socre;
            newTeamScore[2] = PlaceTeamBoardManager.Instance.team2Board.socre;

        }
        else
        {
            foreach (int c in PlaceBoardManager.Instance.pixelsCampInfos)
            {
                newTeamScore[c]++;
            }

        }

        for (int i = 1; i < newTeamScore.Length; i++)
        {
            
            if (lastTeamScore[i] != newTeamScore[i])
            {
                int score = newTeamScore[i];
                // 如果不相等，调用其他函数或执行其他逻辑
                PlaceTeamManager.Instance.teamAreas[i - 1].teaminfo.score = score;
                lastTeamScore[i] = score;
            }
            OnTeamUIUpdate(PlaceTeamManager.Instance.teamAreas[i-1].teaminfo);
            
        }
        
        if (GameSettingManager.Instance.Mode!=GameMode.Competition) return;
        
        if (newTeamScore[1]>=maxSocre-10)
        {
            //一队获胜
            GameEnd();
            //PlaceUIManager.Instance.SetWinInfo(1);
            Debug.Log("Game end ,win team1");
            return;
        }
        if (newTeamScore[2]>=maxSocre-10)
        {
            //二队获胜
            GameEnd();
            //PlaceUIManager.Instance.SetWinInfo(2);
            Debug.Log("Game end ,win team1");
            return;
        }

        // UI显示 , 不知道为什么成为空函数了
        //PlaceUIManager.Instance.SetTeamData(teams.Values.ToList());

    }

    //游戏结束
    public void GameEnd()
    {
        gameRuning=false;
        GenPUID();
        GenGif();
        Reset();
        CheckWinTeam();
        PlaceUIManager.Instance.EndGameUI();
    }

    public void CheckWinTeam()
    {
        if (GameSettingManager.Instance.Mode != GameMode.Competition)
        {
            return;
        }
        if (PlaceTeamBoardManager.Instance.team1Board.socre>PlaceTeamBoardManager.Instance.team2Board.socre)
        {
            //一队获胜
            PlaceUIManager.Instance.SetWinInfo(1);
        }
        else
        {
            PlaceUIManager.Instance.SetWinInfo(2);
        }
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
        SkillIcon skill = SkillIcon.Pencil;
        switch (u.currentState.topState)
        {
            case HighLevelState.Draw:
                messageType = u.Camp == 1 ? TipsType.giftDrawPanel : TipsType.giftDrawPanelRight;
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
            case 1f://这是礼物得人民币价值，那应该在这个里边通知
                normalPower = 300; // 颜料数（一毛相当于优惠）给300他要是冲52次也行
                message = "急速神行";
                skill = SkillIcon.Speed;
                u.character.GetComponent<PlacePlayerController>().ActiveSpeedlUp(10);
                break;
            case 10f:
                normalPower = 900;//固定是加颜料
                message = "风之束缚";
                skill = SkillIcon.Tornado;
                u.character.GetComponent<PlacePlayerController>().Tornado((int)power);
                break;
            case 19f:
            case 20f:
                normalPower = 1800;//固定是攻击
                u.character.GetComponent<PlacePlayerController>().Thunder();
                skill = SkillIcon.Thunder;
                message = "雷霆万钧";
                break;
            case 52f:
                normalPower = 4800;//固定是防御
                u.character.GetComponent<PlacePlayerController>().Invincible(180);
                message = "绝对防御";
                skill = SkillIcon.Defense;
                break;
            case 99f:
                // normalPower = 3600;
                // message = "天官赐福";
                // // 随机自动画一个图案
                // skill = SkillIcon.Pencil;
                // u.character.GetComponent<PlacePlayerController>().Blessing(180);
                // int x = Random.Range(0, PlaceBoardManager.Instance.width - 64);
                // int y = Random.Range(0, PlaceBoardManager.Instance.height - 64);
                // List<Instruction> IL = GenerateRandomImage(x, y, 64);
                // if (IL.Count != 0)
                // {
                //     IL.ForEach(i => u.instructionQueue.Enqueue(i));
                // }
                break;
            case 199f:
                if (GameSettingManager.Instance.Mode == GameMode.Graffiti)
                {
                    u.character.GetComponent<PlacePlayerController>().Invincible(20);
                    var Iname = iconNames[Random.Range(0, 5)];
                    DrawUserIconImage(u, Iname);
                    int max = PlaceBoardManager.Instance.height / 2;
                    normalPower = max * max + 10;
                    switch (Iname)
                    {
                        case "comeOn-150-115-40":
                            messageType = TipsType.giftImageBaiTuo;
                            break;
                        case "guang-608-452-141":
                            messageType = TipsType.gittImagePanel;
                            break;
                        case "lailou-250-256-76":
                            messageType = TipsType.giftImageComeOn;
                            break;
                    }
                }
                else if(GameSettingManager.Instance.Mode == GameMode.Competition)//会将所有技能释放一边
                {
                    normalPower = 20000;
                    message = "天官赐福";
                    skill = SkillIcon.Pencil;
                    u.character.GetComponent<PlacePlayerController>().ActiveSpeedlUp(30);
                    u.character.GetComponent<PlacePlayerController>().Tornado(50);
                    u.character.GetComponent<PlacePlayerController>().Thunder();
                    u.character.GetComponent<PlacePlayerController>().Invincible(300);
                    foreach (var item in u.selfTeam.userList)
                    {
                        item.character.GetComponent<PlacePlayerController>().ActiveSpeedlUp(5);
                        item.character.GetComponent<PlacePlayerController>().Invincible(5);//解控，只是解控
                    }

                   
                }
                else
                {
                    normalPower = 20000;
                    message = "天官赐福";
                    // 随机自动画一个图案
                    skill = SkillIcon.Pencil;
                    u.character.GetComponent<PlacePlayerController>().Blessing(180);
                    int x128 = Random.Range(0, PlaceBoardManager.Instance.width - 128);
                    int y128 = Random.Range(0, PlaceBoardManager.Instance.height - 128);
                    List<Instruction> IL128 = GenerateRandomImage(x128, y128, 128);
                    if (IL128.Count != 0)
                    {
                        IL128.ForEach(i => u.instructionQueue.Enqueue(i));
                    }
                }

                break;
            case 299f:
                // normalPower = 62500;
                // message = "天官赐福";
                // // 随机自动画一个图案
                // skill = SkillIcon.Pencil;
                // u.character.GetComponent<PlacePlayerController>().Blessing(180);
                // int x256 = Random.Range(0, PlaceBoardManager.Instance.width - 256);
                // int y256 = Random.Range(0, PlaceBoardManager.Instance.height - 256);
                // List<Instruction> IL256 = GenerateRandomImage(x256, y256, 256);
                // if (IL256.Count != 0)
                // {
                //     IL256.ForEach(i => u.instructionQueue.Enqueue(i));
                // }
                break;
                // case 19.9f:
                //     normalPower = 1999;
                //     break;
                // case 29.9f:
                //     message = "";
                //     normalPower = 2990;
                //     break;
                // case 52f:
                //     message = "";
                //     normalPower = 5200;
                //     break;
                // case 66.6f:
                //     message = "";
                //     normalPower = 6666;
                //     break;
                // case 88.8f:
                //     message = "";
                //     normalPower = 8888;
                //     break;
                // case 99.9f:
                //     message = "";
                //     normalPower = 9999;
                //     break;
                // case 120f:
                //     message = "";
                //     normalPower = 12000;
                //     break;
                // case 166.6f:
                //     message = "";
                //     normalPower = 16666;
                //     break;
                // case 188.8f:
                //     message = "";
                //     normalPower = 18888;
                //     break;
                // case 300f:
                //     message = "";
                //     normalPower = 30000;
                //     break;
        }
        u.character.GetComponent<PlacePlayerController>().InkUp(normalPower);
        u.score += normalPower;
        u.genInkCount += normalPower;
        u.usePowerCount += power * 10;
        u.Update();
        u.currentCarryingInkCount += normalPower;
        PlaceTeamManager.Instance.teamAreas[u.Camp - 1].teaminfo.ink += (int)(0.1 * normalPower);
        PlaceTeamManager.Instance.teamAreas[u.Camp - 1].teaminfo.hisInk += (int)(0.1 * normalPower);
        PlaceTeamManager.Instance.teamAreas[u.Camp - 1].teaminfo.hisExInk += (int)(0.1 * normalPower);
        //在这通知UI？还得要状态切换啊，先检查状态再通知
        PlaceUIManager.Instance.AddTips(new TipsItem()
        {
            userName = username,
            text = message,
            icon = u.userIcon,//玩家头像
            tipsType = messageType,
            value = $"+{normalPower:D}",
            isLeft = u.Camp == 1,
            level = u.level,
            skillIcon = skill
        });
    }

    public void GainLikePower(User user, long power)
    {
        // B 站 每人 每天 点赞上限 1000
        // Dy 每人 每天 不限
        int lc = (int)power;
        int p = lc * 10;//TODO:记得修改哦！根据点赞数量获取颜料，修改为自动倍率。
        user.score += p;
        user.likeCount +=lc;
        user.Update();
        PlaceTeamManager.Instance.teamAreas[user.Camp - 1].teaminfo.ink += p;
        // 颜料增加的特效
        user.character.GetComponent<PlacePlayerController>().InkUp(p);
        var messageType = user.Camp == 1 ? TipsType.likeTipsPanel : TipsType.likeTipsPanelRight;
        string message = "";
        // if (lc < 5)
        // {
        //     message = $"点赞! 颜料 x {p}";
        // }
        //
        // if (lc > 5)
        // {
        //     message = $"点赞手速突破天际!! 颜料 x {p}";
        // }

        if (lc >= 10)
        {
            int speedTime = lc / 10;
            user.character.GetComponent<PlacePlayerController>().ActiveSpeedlUp(speedTime);//一个赞给0.1s加速。
        }
        
        if ((user.likeCount/1000)>user.likeTimes)//判断本局是否点赞数超过1000//每次过一千才提醒，你这相当于过了一千 然后每次点赞都触发这个
        {
            user.likeTimes++;
            lc = user.likeCount;//大于一千拿总数，小于1000拿这次点赞数，
            message = $"手速突破天际!! {lc}次！";
            
        }
        PlaceUIManager.Instance.AddTips(new TipsItem()
        {
            userName = user.Name,
            text = message,
            icon = user.userIcon,//玩家头像
            tipsType = messageType,
            value = $"+{1}",
            likeCount = lc,
            isLeft = user.Camp == 1
        });
        // 限时加速
        // StartCoroutine(user.character.GetComponent<PlacePlayerController>().TimeLimitSpeedUp(p));
    }

    // public void GainGiftPower(User user, float power)
    // {
    //     int p = (int)power;
    //     user.score += p;
    //     user.Update();
    //     PlaceTeamManager.Instance.teamAreas[user.Camp - 1].teaminfo.ink += p;
    //     // 限时加速
    //     user.character.GetComponent<PlacePlayerController>().Tornado((int)power);
    // }


    private string[] iconNames = new[]
    {
        "comeOn-150-115-40",
        "comeOn-150-115-40",
        //"flower",
        "guang-608-452-141",
        "lailou-250-256-76",
        "lailou-250-256-76"
    };

    public Texture2D defTex2D;
    //这是画指定的，上一级随机的名字
    private void DrawUserIconImage(User user, string IName)
    {
        int x, y, max;
        max = PlaceBoardManager.Instance.height / 2;
        x = Random.Range(1, PlaceBoardManager.Instance.width - max);
        y = Random.Range(1, PlaceBoardManager.Instance.height - max);
        Texture2D userTex;
        if (user.userIcon == null)
        {
             userTex = defTex2D;
        }
        else
        {
            userTex= user.userIcon.texture;
        }
        
        List<Instruction> IL = Instance.GiftGenerateImage(x, y, max, userTex, IName);
        if (IL.Count != 0)
        {
            IL.ForEach(i => user.instructionQueue.Enqueue(i));
        }
        else
        {
            Debug.Log("roll 失败,或许名字key 不对");
        }

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
        if (GameSettingManager.Instance.Mode != GameMode.Competition)
        {
            PlaceBoardManager.Instance.ConvertTex2DToGIF();
        }
        else
        {
            PlaceTeamBoardManager.Instance.ConvertTex2DToGIFTeamFun();
        }

    }

    // public void ShowGif(string path) {
    //     PlaceUIManager.Instance.endUI.ShowGIF(path);
    // }


    public List<Instruction> FreeGenerateImage(int ox, int oy, int max, string name)
    {
        // 图库
        // 获取index 
        if (freeDemoTexturesDic != null && freeDemoTexturesDic.Count > 0)
        {
            // Texture2D tex;
            // demoTexturesDic.TryGetValue(name,out tex);
            // if (tex == null)
            // {
            //     return new List<Instruction>();
            // }
            if (!freeDemoTexturesDic.ContainsKey(name))
            {
                return new List<Instruction>();
            }
            Texture2D tex = freeDemoTexturesDic[name];
            // texture 2d
            // Texture2D tex = Resources.Load<Texture2D>($"Images/{index}");
            Texture2D retex = PlaceBoardManager.Instance.ScaleTextureProportionally(tex, max, max);

            // 转换成 instruction
            return Image2Instruction(retex, ox, oy);
        }

        return new List<Instruction>();
    }
    public List<Instruction> GiftGenerateImage(int ox, int oy, int max, Texture2D userTex, string name)
    {
        // 图库
        // 获取index 
        if (giftDemoTexturesDic != null && giftDemoTexturesDic.Count > 0)
        {
            // Texture2D tex;
            // demoTexturesDic.TryGetValue(name,out tex);
            // if (tex == null)
            // {
            //     return new List<Instruction>();
            // }
            if (!giftDemoTexturesDic.ContainsKey(name))
            {
                return new List<Instruction>();
            }
            Texture2D tex = giftDemoTexturesDic[name];
            tex.name = name;
            // texture 2d
            // Texture2D tex = Resources.Load<Texture2D>($"Images/{index}");

            // 合并 头像 与 图片模版
            Texture2D mergeTex = PlaceBoardManager.Instance.MergeTexture(userTex, tex);

            // 裁剪大小
            Texture2D retex = PlaceBoardManager.Instance.ScaleTextureProportionally(mergeTex, max, max);

            // 转换成 instruction
            return Image2Instruction(retex, ox, oy);
        }

        return new List<Instruction>();
    }
    // public List<Instruction> CartoonGenerateImage(int ox, int oy, int max, string name)
    // {
    //     // 图库
    //     // 获取index 
    //     if (demoTextures != null && demoTextures.Count > 0)
    //     {
    //         // Texture2D tex;
    //         // demoTexturesDic.TryGetValue(name,out tex);
    //         // if (tex == null)
    //         // {
    //         //     return new List<Instruction>();
    //         // }
    //         if (!freeDemoTexturesDic.ContainsKey(name))
    //         {
    //             return new List<Instruction>();
    //         }
    //         Texture2D tex = freeDemoTexturesDic[name];
    //         // texture 2d
    //         // Texture2D tex = Resources.Load<Texture2D>($"Images/{index}");
    //         Texture2D retex = PlaceBoardManager.Instance.ScaleTextureProportionally(tex, max, max);

    //         // 转换成 instruction
    //         return Image2Instruction(retex, ox, oy);
    //     }

    //     return new List<Instruction>();
    // }
    public List<Instruction> GenerateRandomImage(int ox, int oy, int max)
    {
        // 图库
        // 获取index 
        List<Texture2D> demoTextures = freeDemoTexturesDic.Values.ToList();
        if (demoTextures != null && demoTextures.Count > 0)
        {
            int index = Random.Range(0, demoTextures.Count);
            Texture2D tex = demoTextures[index];
            // texture 2d
            // Texture2D tex = Resources.Load<Texture2D>($"Images/{index}");
            Texture2D retex = PlaceBoardManager.Instance.ScaleTextureProportionally(tex, max, max);

            // 转换成 instruction
            return Image2Instruction(retex, ox, oy);
        }

        return new List<Instruction>();
    }

    public List<Instruction> UserIcon2Instruction(int ox, int oy, int max, Texture2D userTex)
    {
        Texture2D retex = PlaceBoardManager.Instance.ScaleTextureProportionally(userTex, max, max);

        // 转换成 instruction
        return Image2Instruction(retex, ox, oy);
    }
    List<Instruction> Image2Instruction(Texture2D tex, int ox, int oy)
    {
        // 读取 颜色
        Color32[] imageData = tex.GetPixels32();


        // 宽高
        int width = tex.width;
        int height = tex.height;
        int boardwidth = PlaceBoardManager.Instance.width;
        int boardheight = PlaceBoardManager.Instance.height;

        List<Instruction> insList = new List<Instruction>();
        // 确认没有出界
        if (ox + width > boardwidth || oy + height > boardheight)
        {
            Debug.Log("图片超出边界");
            // 裁剪
            for (int i = 0; i < imageData.Length; i++)
            {
                if (ox + i % width > width) continue;
                if (oy + i / width > height) continue;
                // c, x, y, r: r, g: g, b: b
                Color32 c = imageData[i];
                Instruction ins = new Instruction("/draw", ox + i % width, oy + i / width, r: c.r, g: c.g, b: c.b);
                insList.Add(ins);
            }
        }
        else // 高频
        {
            for (int i = 0; i < imageData.Length; i++)
            {
                // c, x, y, r: r, g: g, b: b

                Color32 c = imageData[i];
                if (c.a == 0) continue;
                Instruction ins = new Instruction("/draw", ox + i % width, oy + i / width, r: c.r, g: c.g, b: c.b);
                insList.Add(ins);
            }
        }
        return insList;
    }




    // public Texture2D ImageFitBoardProcessor(List<Texture2D> texlist, int texindex)
    // {
    //     Texture2D inputTexture = texlist[texindex];
    //     // 外部引用 
    //     // TODO 之后修改
    //     Texture2D resizeTexture = PlaceBoardManager.Instance.ScaleTextureProportionally(inputTexture, PlaceBoardManager.Instance.width, PlaceBoardManager.Instance.height);
    //     // pixelsImage = resizeTexture.GetPixels();
    //     return resizeTexture;
    // }

    public List<Texture2D> LoadDemoResources()
    {
        return LoadResources(demoPath);
    }
    public Dictionary<string, Texture2D> LoadDemoDicResources(string sbuClass)
    {
        string classFormat = @"free|gift";

        if (Regex.IsMatch(sbuClass, classFormat))
        {
            return LoadDicResources(Path.Combine(demoPath, sbuClass));
        }
        Debug.LogError("Class not found: " + sbuClass);
        return new Dictionary<string, Texture2D>();
    }
    public Dictionary<string, List<Texture2D>> LoadCartoonDemoDicResources(string sbuClass)
    {
        string classFormat = @"cartoon";

        if (Regex.IsMatch(sbuClass, classFormat))
        {
            return LoadCartoonDicResources(Path.Combine(demoPath, sbuClass));
        }
        Debug.LogError("Class not found: " + sbuClass);
        return new Dictionary<string, List<Texture2D>>();
    }

    public Dictionary<string, List<Texture2D>> LoadCartoonDicResources(string imagePath)
    {
        Dictionary<string, List<Texture2D>> texCartoonDic = new Dictionary<string, List<Texture2D>>();
        // List<Texture2D> texlist = new List<Texture2D>();
        // 检查目录是否存在
        if (Directory.Exists(imagePath))
        {
            // 获取目录中的所有子目录
            string[] subDirs = Directory.GetDirectories(imagePath);
            foreach (string subDirectory in subDirs)
            {

                // 获取目录中的所有文件
                List<Texture2D> texlist = LoadResources(subDirectory);

                texCartoonDic.Add(Path.GetFileName(subDirectory), texlist);
            }



        }
        else
        {
            Debug.LogError("Directory not found: " + imagePath);
        }

        return texCartoonDic;
    }


    public Dictionary<string, Texture2D> LoadDicResources(string imagePath)
    {
        Dictionary<string, Texture2D> texDic = new Dictionary<string, Texture2D>();
        // List<Texture2D> texlist = new List<Texture2D>();
        // 检查目录是否存在
        if (Directory.Exists(imagePath))
        {
            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(imagePath);

            foreach (string filePath in files)
            {
                // 检查文件是否是图片
                if (IsImageFile(filePath))
                {
                    // 加载图片资源并添加到List
                    Texture2D texture = LoadTexture(filePath);
                    if (texture != null)
                    {
                        //Debug.Log("Load Texture: " + filePath + " " + texture.width + " " + texture.height);
                        string fname = Path.GetFileNameWithoutExtension(filePath);
                        texDic.Add(fname, texture);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Directory not found: " + imagePath);
        }

        return texDic;
    }

    public List<Texture2D> LoadResources(string imagePath)
    {
        List<Texture2D> texlist = new List<Texture2D>();
        // 检查目录是否存在
        if (Directory.Exists(imagePath))
        {
            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(imagePath);

            foreach (string filePath in files)
            {
                // 检查文件是否是图片
                if (IsImageFile(filePath))
                {
                    // 加载图片资源并添加到List
                    Texture2D texture = LoadTexture(filePath);
                    if (texture != null)
                    {
                        texlist.Add(texture);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Directory not found: " + imagePath);
        }

        return texlist;
    }
    bool IsImageFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
    }

    Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(1, 1); // 创建一个临时Texture2D，稍后会被替换为实际的图像数据
        if (!texture.LoadImage(fileData)) // 加载图像数据
        {
            Debug.LogError("Failed to load texture: " + filePath);
        }
        // Debug.Log("Load Texture: " + filePath + " " + texture.width + " " + texture.height);
        return texture;
    }

    public string AllMember()
    {
        return users.Count.ToString();
    }
    public List<string> AllMemberName()
    {
        return users.Values.Select(user => user.Name).ToList();
    }
    public string Price()
    {
        return users.Values.Sum(u => u.usePowerCount).ToString();
    }


    //  ===== 协程 =====


    // IEnumerator CallTornado(User u ,int num)
    // {
    //     u.character.GetComponent<PlacePlayerController>().Tornado(num);
    // }

    IEnumerator SaveImagePreRecorderTime(int time = 6)
    {
        // 持续等待一分钟
        while (gameRuning)
        {
            yield return new WaitForSeconds(time);
            if (GameSettingManager.Instance.Mode == GameMode.Competition)
            {
                PlaceTeamBoardManager.Instance.SaveTeamImage();
            }
            else
            {
                PlaceBoardManager.Instance.SaveImage();
            }
        }
        // PlaceBoardManager.Instance.SaveImage(lastone: true);
    }


    public void Reset()
    {
        foreach (var user in users.Values)
        {
            user.character.GetComponent<PlacePlayerController>().Reset();
        }
    }
}