using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class User {
	public string username;
	public int level;

    public CharacterState currentState { get; set; }

    public GameObject character;

    public string camp;

    public Color lastColor { get; set; }

    public Queue<Instruction> instructionQueue = new Queue<Instruction>();

    public int score { get; set; }

    public int carryingInkCount { get; set; }
    public Sprite userIcon{get;set;}

    public User(string username,GameObject character,string camp,int level=1) {
        this.username = username;
        this.level = level;
        this.camp = camp;
        this.character = character;
        this.instructionQueue = new Queue<Instruction>();
        this.currentState = CharacterState.WaitingForCommandInTeamArea;
        this.lastColor = Color.white;
        this.score = 0;
        this.carryingInkCount = 0;
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
