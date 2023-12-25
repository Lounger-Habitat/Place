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
    public GameObject nameUI;
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
        GameObject go = Instantiate(teamAreaPrefab, spawnPosition, Quaternion.identity);
        CreateNameTag(go.transform,team.Name);
        TeamAreaManager ta = go.GetComponent<TeamAreaManager>();
        ta.setTeamInfo(team);
        teamAreas.Add(ta);
        return ta;
    }


    public void CreateNameTag(Transform characterTransform,string name)
    {
        GameObject canvasObj = GameObject.Find("WS_Canvas");
        if (canvasObj != null && nameUI != null)
        {
            Canvas canvas = canvasObj.GetComponent<Canvas>();

            // 在Canvas下生成NameTag UI
            GameObject nameTagObj = Instantiate(nameUI, canvasObj.transform);

            // 设置NameTag UI的位置和属性
            // 假设NameTag UI有一个脚本用于定位和显示
            NameTag nameTagScript = nameTagObj.GetComponent<NameTag>();
            if (nameTagScript != null)
            {
                nameTagScript.target = characterTransform;
                nameTagScript.go_name = name;
                // 设置其他必要的属性，如偏移量等
            }
        }
    }

    public void CreateTeam(string username, string teamId, string teamName)
    {
        if (teams.Count < maxTeams && !teams.Exists(t => t.Id == teamId))
        {
            Team newTeam = new Team(teamId, teamName, 5);
            teams.Add(newTeam);
            TeamAreaManager teamAreaManager = CreateTeamArea(newTeam);
            // 在这里创建角色并加入队伍，具体实现取决于你的游戏逻辑
            // 例如: CreateCharacterInTeamArea(newTeam);
            teamAreaManager.CreateCharacterInTeamArea(username);
        }
        else
        {
            Debug.Log("无法创建更多队伍，场景已满");
        }
    }

    public void AddTeam(string username,string teamId)
    {   
        Team team = teams.Find(t => t.Id == teamId);
        if(team==null)
        {
            //没找到这个team 玩蛇皮
            Debug.Log("没有这个队伍,先创建这个队伍");
        }else
        {
            TeamAreaManager teamAreaManager = teamAreas.Find(ta => ta.getTeamInfo().Id == team.Id);
            // 在这里实现加入队伍的逻辑
            teamAreaManager.CreateCharacterInTeamArea(username);
        }
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
