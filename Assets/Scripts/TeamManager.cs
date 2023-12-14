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
    public GameObject teamAreaPrefab;
    public int maxTeams; // 场景可容纳的最大队伍数
    private List<Team> teams = new List<Team>();
    private List<TeamAreaManager> teamAreas = new List<TeamAreaManager>();

    // 创建队伍区域
    public TeamAreaManager CreateTeamArea(Team team)
    {
        Vector3 spawnPosition = Vector3.zero;;
        switch (team.Id) {
            case "1":
                spawnPosition = new Vector3(-40, 1, -40);
                break;
            case "2":
                spawnPosition = new Vector3(-20, 1, -40);
                break;
            case "3":
                spawnPosition = new Vector3(0, 1, -40);
                break;
            case "4":
                spawnPosition = new Vector3(20, 1, -40);
                break;
            case "5":
                spawnPosition = new Vector3(40, 1, -40);
                break;
        }
        TeamAreaManager ta = Instantiate(teamAreaPrefab, spawnPosition, Quaternion.identity).GetComponent<TeamAreaManager>();
        ta.setTeamInfo(team);
        teamAreas.Add(ta);
        return ta;
    }




    public void CreateTeam(string teamId, string teamName)
    {
        if (teams.Count < maxTeams && !teams.Exists(t => t.Id == teamId))
        {
            Team newTeam = new Team(teamId, teamName, 5);
            teams.Add(newTeam);
            TeamAreaManager teamAreaManager = CreateTeamArea(newTeam);
            // 在这里创建角色并加入队伍，具体实现取决于你的游戏逻辑
            // 例如: CreateCharacterInTeamArea(newTeam);
            teamAreaManager.CreateCharacterInTeamArea();
        }
        else
        {
            Debug.Log("无法创建更多队伍，场景已满");
        }
    }

    public void AddTeam(string teamId)
    {
        Team team = teams.Find(t => t.Id == teamId);
        TeamAreaManager teamAreaManager = teamAreas.Find(ta => ta.getTeamInfo().Id == team.Id);
        // 在这里实现加入队伍的逻辑
        teamAreaManager.CreateCharacterInTeamArea();
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
