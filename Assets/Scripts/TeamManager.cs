using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance { get; private set; }

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
    public TeamAreaManager teamAreaManager;
    public int maxTeams; // 场景可容纳的最大队伍数
    private List<Team> teams = new List<Team>();



    public void CreateTeam(string teamId, string teamName)
    {
        if (teams.Count < maxTeams)
        {
            Team newTeam = new Team(teamId, teamName, 5);
            teams.Add(newTeam);
            // 在这里创建角色并加入队伍，具体实现取决于你的游戏逻辑
            // 例如: CreateCharacterInTeamArea(newTeam);
            teamAreaManager.CreateCharacterInTeamArea(newTeam);
        }
        else
        {
            Debug.Log("无法创建更多队伍，场景已满");
        }
    }

    public void AddTeam(string teamId)
    {
        Team team = teams.Find(t => t.Id == teamId);
        // 在这里实现加入队伍的逻辑
        teamAreaManager.CreateCharacterInTeamArea(team);
    }
}

// 简单的Team类，用于表示队伍
public class Team
{
    public string Id;
    public string Name;
    // 这里可以添加更多队伍相关的属性和方法
    public int MaxTeamNumber;


    public Team(string id, string name, int max)
    {
        Id = id;
        Name = name;
        MaxTeamNumber = max;
    }
}
