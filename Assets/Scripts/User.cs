using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class User {
    // 玩家名字
	public string username;

    // 玩家等级 ， 用来升级
	public int level;

    // 玩家当前状态，状态机使用的变量，升级到行为树后，暂时没用，正在考虑留不留
    public PlayerState currentState;

    // 玩家角色 模型
    public GameObject character;
    // 玩家名字
    public GameObject nameTag;

    // 玩家 队伍 阵营
    public int camp;

    // 玩家 绘画 最后一次 使用的 颜色
    public Color lastColor { get; set; }

    // 玩家 指令队列， 用于保存 玩家弹幕输入的指令，等待被执行
    public Queue<Instruction> instructionQueue = new Queue<Instruction>();

    public bool defendingIns = false;

    public int attckingIns = 0;

    // 玩家当前分数
    public int score { get; set; }

    // 玩家 身上携带的 颜料数量
    public int carryingInkCount { get; set; }

    // 玩家 身上携带的 最大指令数量
    public int maxCarryingInsCount { get; set; }

    // 玩家 头像
    public Sprite userIcon{get;set;}

    public PlaceTeamAreaManager selfTeam;
    
    public User(string username,GameObject character,int camp,PlaceTeamAreaManager teamArea,int level=1) {
        this.username = username;
        this.level = level;
        this.camp = camp;
        this.character = character;
        this.selfTeam = teamArea;
        this.instructionQueue = new Queue<Instruction>();
        this.lastColor = Color.white;
        this.score = 0;
        this.carryingInkCount = 0;
        this.maxCarryingInsCount = 1;
        this.userIcon = null;
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
        carryingInkCount = 0;
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