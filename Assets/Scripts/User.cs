using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class User {
    // 玩家名字
	public string username;

    // 玩家等级 ， 用来升级
	public int level;

    // 玩家当前状态，状态机使用的变量，升级到行为树后，暂时没用，正在考虑留不留
    public CharacterState currentState { get; set; }

    // 玩家角色 模型
    public GameObject character;

    // 玩家 队伍 阵营
    public int camp;

    // 玩家 绘画 最后一次 使用的 颜色
    public Color lastColor { get; set; }

    // 玩家 指令队列， 用于保存 玩家弹幕输入的指令，等待被执行
    public Queue<Instruction> instructionQueue = new Queue<Instruction>();

    // 玩家当前分数
    public int score { get; set; }

    // 玩家 身上携带的 颜料数量
    public int carryingInkCount { get; set; }

    // 玩家 身上携带的 最大指令数量
    public int maxCarryingInsCount { get; set; }

    // 玩家 头像
    public Sprite userIcon{get;set;}

    public User(string username,GameObject character,int camp,int level=1) {
        this.username = username;
        this.level = level;
        this.camp = camp;
        this.character = character;
        this.instructionQueue = new Queue<Instruction>();
        this.currentState = CharacterState.WaitingForCommandInTeamArea;
        this.lastColor = Color.white;
        this.score = 0;
        this.carryingInkCount = 0;
        this.maxCarryingInsCount = 1;
        this.userIcon = null;
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
}
