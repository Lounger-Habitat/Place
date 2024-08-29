using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class User
{

    /*
    ===============
        元信息
    ===============
    */
    public string name; // 玩家名字
    public string Name { get { return name; } set { name = value; } }
    private int camp;
    public int Camp { get { return camp; } set { camp = value; } }       // 玩家 队伍 阵营
    private int id;          // 玩家 id
    public int Id { get { return id; } set { id = value; } }

    private string open_id;          // 玩家 openid
    public string Open_ID { get { return open_id; } set { open_id = value; } }


    /*
    ===============
        状态
    ===============
    */
    // 考虑 是否 能与 玩家状态合并？？ FIXME！
    public bool defendingIns = false; // bool 判断是否进入防守
    public int attckingIns = 0;       // 0 表示没有攻击目标 ，1234，分别代表队伍
    public bool invincible = false; // 无敌状态
    /*
    ===============
        变量
    ===============
    */
    public Color lastColor { get; set; } // 玩家 绘画 最后一次 使用的 颜色
    public (int x, int y) lastPoint { get; set; } // 玩家 绘画 最后一次 使用的 颜色
    public int level;               // 玩家等级 ， 用来升级
    public int Level { get { return level; } set { level = value; } }
    public PlayerState currentState; // 玩家当前状态
    public int score;  // 玩家当前分数
    public int currentCarryingInkCount = 300;  // 玩家 身上携带的 颜料数量
    public int currentCarryingInsCount = 0; // 玩家 身上携带的 最大指令数量
    public int maxCarryingInkCount = 100; // 玩家 身上携带的 颜料数量
    public int maxCarryingInsCount = 100; // 玩家 身上携带的 最大指令数量
    // base 速度
    public float speed;
    // 额外速度
    public float exSpeed = 0;
    public float waitingSpeed = 1.0f; // 等待速度

    public float contributionRate = 0.1f; // 贡献率

    public int likeCount=0; // 玩家本局点赞数量
    public int likeTimes=0; // 点赞到1000的参数
    
    public Queue<Instruction> instructionQueue;// 玩家 指令队列， 用于保存 玩家弹幕输入的指令，等待被执行

    /*
        资源
    */
    public GameObject character;    // 玩家角色 
    public GameObject nameTag;      // 玩家名字标签
    public Sprite userIcon { get; set; }     // 玩家 头像
    // 资源

    // 统计信息
    // 用户绘画次数
    public int drawTimes = 0;

    // 使用颜料总数
    public int useTotalInkCount = 0;
    // 画面上 有效点
    public int effectiveInkCount = 0;
    // 使用了多少超能力
    public float usePowerCount = 0;
    // 生成了多少颜料
    public int genInkCount = 0;



    public PlaceTeamAreaManager selfTeam;

    public User(string username)
    {
        this.name = username;
        this.level = 1;
        this.camp = 0;
        this.character = null;
        this.selfTeam = null;
        this.lastColor = Color.white;
        this.score = 0;
        this.maxCarryingInkCount = 500;
        this.maxCarryingInsCount = 500;
        this.userIcon = null;
        this.speed = 2.0f;
        this.currentState = new PlayerState(HighLevelState.Draw, DetailState.DrawMoveToTotem);
        this.lastPoint = (20, 20);
        this.instructionQueue = new Queue<Instruction>();
    }

    public void Reset()
    {
        // destroy the character
        GameObject.Destroy(character);
        GameObject.Destroy(nameTag);
        // 清空指令，释放内存
        instructionQueue.Clear();

        // 重置玩家状态
        score = 0;
        maxCarryingInkCount = 500;
        maxCarryingInsCount = 500;
        currentCarryingInkCount = 0;
        currentCarryingInsCount = 0;
        speed = 2.0f;

    }

    public void Update()
    {
        // 等级
        this.level = CalLevel(score);
        // 速度
        this.speed = 2 + (level - 1) * 0.03f;
        // 承载量
        this.maxCarryingInsCount = (int)(500 + (level - 1) * 20f);
        this.maxCarryingInkCount = (int)(500 + (level - 1) * 20f);

        // 体现角色能力
        if (nameTag != null && character != null)
        {
            float delta = level * 0.03f;
            nameTag.GetComponent<IconNameTag>().UpdateIconRect(delta);
            character.transform.localScale = new Vector3(1 + delta, 1 + delta, 1 + delta);
            character.GetComponent<NavMeshAgent>().avoidancePriority = Mathf.Clamp(99 - level, 0, 99);
        }

    }

    public int CalLevel(int score)
    {
        /* 
            n 是 level 默认是1, 
            d = 20, # 2024-05-24:太大了，升级太慢，改成 1,d=1
            a1 = 100
            a_2 = a1+(n−1)d

            1). S_n = n/2(a_1+a_n) = n/2(a_1+a_1+(n−1)d) = n/2(2a_1+(n−1)d)
            fix: S_n = n * a_1 + ( (n*(n-1) / 2) * d )
                     = n * 100 + ( (n*(n-1) / 2) * 20 )
                     = 100n + 10n^2 - 10n
                     = 10n^2 + 90n
            2). S_n = n * a_1 + ( (n*(n-1) / 2) * d )
            fix: S_n = n * a_1 + ( (n*(n-1) / 2) * d )
                     = n * 100 + ( (n*(n-1) / 2) * 20 )
                     = 100n + 10n^2 - 10n
                     = 10n^2 + 90n
                     当 S_n = 0 时，n = 0 不对
                改写成: S_n = 10n^2 + 90n - 100
                所以 n = (-90 + sqrt(8100+4000S_n))/20
                    
            3). 求根公式: n = (-b + sqrt(b^2 - 4ac)) / 2a
                             
                             a = 10 , b = 90, c = -100-Sn
                        n = (-90 + sqrt(8100+4000S_n))/20

            给定 S_n = score, 求 n
            

            n = (-180+sqrt(32400+160score))/40
            n = [-(2a-d) + sqrt((2a-d)^2+8dS_n)] / 2d
            n = [-199 + sqrt(39961+8*score)] / 2    when d = 1
            n = [-(2*100 - d) + sqrt((2*100 - d)^2 + 8*d*score)] / 2d
            n = [-(200 - 10) + sqrt((200 - 10)^2 + 80*score)] / 20
            n = [-(190) + sqrt(36100 + 80*score)] / 20
        */
        // int n = (int)(-180 + Mathf.Sqrt(32400 + 160 * score)) / 40;
        // int n = (int)(-199 + Mathf.Sqrt(39601 + 8 * score)) / 2;
        // int d = 20;
        // int n = (int)(-(200 - d) + Mathf.Sqrt( Mathf.Pow(200 - d,2) + 8 * d * score)) / 2 * d;
        int n = (int)(-190 + Mathf.Sqrt(36100 + 80 * score)) / 20;
        n = Mathf.Clamp(n, 1, 999);
        return n;
    }
}

[System.Serializable]
public struct PlayerState
{
    public HighLevelState topState;
    public DetailState detailState;

    public PlayerState(HighLevelState topState, DetailState detailState)
    {
        this.topState = topState;
        this.detailState = detailState;
    }
}
[System.Serializable]
public enum HighLevelState
{
    Draw,
    Attack,
    Defend,
}

[System.Serializable]
public enum DetailState
{
    DrawMoveToTotem,
    DrawWaitingForInsAndPower,
    DrawSome,
    DrawMoveToAltar,
    DefendResetToTotem,
    DefendToDoor,
    DefendAtDoorIdle,
    DefendAtDoorHelp,
    DefendAtDoorAttack,
    AttackResetToTotem,
    AttackWaitingForIns,
    AttackGoSteal,
    AttackStealing,
    AttackFight,
    AttackCharge,
    AttackGoHome

}