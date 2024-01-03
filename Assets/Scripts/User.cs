using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class User {
	public string username;
	public int level;

    public GameObject character;

    public Queue<Instruction> instructionQueue = new Queue<Instruction>();


    public User(string username, int level,GameObject character) {
        this.username = username;
        this.level = level;
        this.character = character;
        this.instructionQueue = new Queue<Instruction>();
    }

    public void Update(){
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
