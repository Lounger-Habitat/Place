using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class User {

    /*
    ===============
        元信息
    ===============
    */
	public string username; // 玩家名字
    public int camp;        // 玩家 队伍 阵营

    /*
    ===============
        属性
    ===============
    */
    // 考虑 是否 能与 玩家状态合并？？ FIXME！
    public bool defendingIns = false; // bool 判断是否进入防守
    public int attckingIns = 0;       // 0 表示没有攻击目标 ，1234，分别代表队伍

    /*
    ===============
        变量
    ===============
    */
    public Color lastColor { get; set; } // 玩家 绘画 最后一次 使用的 颜色
	public int level;               // 玩家等级 ， 用来升级
    public PlayerState currentState; // 玩家当前状态
    public int score { get; set; }  // 玩家当前分数
    public int carryingInkCount { get; set; } // 玩家 身上携带的 颜料数量
    public int maxCarryingInsCount; // 玩家 身上携带的 最大指令数量
    public float speed;
    public float waitingSpeed = 1.0f; // 等待速度
    public Queue<Instruction> instructionQueue = new Queue<Instruction>(); // 玩家 指令队列， 用于保存 玩家弹幕输入的指令，等待被执行

    /*
        资源
    */
    public GameObject character;    // 玩家角色 
    public GameObject nameTag;      // 玩家名字标签
    public Sprite userIcon{get;set;}     // 玩家 头像
    // 资源


    public PlaceTeamAreaManager selfTeam;
    
    public User(string username,GameObject character,int camp,PlaceTeamAreaManager teamArea) {
        this.username = username;
        this.level = 1;
        this.camp = camp;
        this.character = character;
        this.selfTeam = teamArea;
        this.instructionQueue = new Queue<Instruction>();
        this.lastColor = Color.white;
        this.score = 0;
        this.carryingInkCount = 0;
        this.maxCarryingInsCount = 1;
        this.userIcon = null;
        this.speed = 2.0f;
        this.currentState = new PlayerState(HighLevelState.Draw, DetailState.DrawMoveToTotem);
    }

    public string getUsername() {
        return username;
    }

    public int getLevel() {
        return level;
    }

    public void setUsername(string username) {
        this.username = username;
    }

    public void setLevel(int level) {
        this.level = level;
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
        carryingInkCount = 1;
        speed = 2.0f;

    }

    public void Update(int score) {
        this.level = CalLevel(score);
        this.speed = 2 + (level-1) * 0.1f;
        this.maxCarryingInsCount = (int)(1 + (level-1) * 0.2f);
    }

    public int CalLevel(int score) {
        /* 
            n 是 level 默认是1, 
            d = 20,
            a1 = 100
            a_n = a_1+(n−1)d
            S_n = n/2(a_1+a_n) = n/2(a_1+a_1+(n−1)d) = n/2(2a_1+(n−1)d)
            给定 S_n = score, 求 n
            20n^2+180n−2S=0
            n = (-180+sqrt(32400+160score))/40
        */ 
        int n = (int)(-180 + Mathf.Sqrt(32400 + 160 * score)) / 40;
        return n;
    }
}


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

public enum HighLevelState {
    Draw,
    Attack,
    Defend,
}

public enum DetailState {
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