using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

// 简单的Team类，用于表示队伍
[System.Serializable]
public class Team
{
    public string Id;
    public string Name;
    // 这里可以添加更多队伍相关的属性和方法
    public int MaxTeamNumber;

    public int score;

    public Team(string id, string name, int max)
    {
        Id = id;
        Name = name;
        MaxTeamNumber = max;
        score = 0;
    }
}