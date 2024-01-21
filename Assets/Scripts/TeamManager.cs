using System.Collections.Generic;
using System.Linq;
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
    public  List<TeamAreaManager> teamAreas = new List<TeamAreaManager>();

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
            if (username.Equals("sys"))
            {

            }
            else
            {
                // 在这里创建角色并加入队伍，具体实现取决于你的游戏逻辑
                // 例如: CreateCharacterInTeamArea(newTeam);
                teamAreaManager.CreateCharacterInTeamArea(username);
            }

            //刷新两个UI列表，不应该写在这里，应该写在数据变化后，TODO: 改改改
            SetTeamUi();
            SetUserUi();
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
            //TODO:刷新两个UI列表，不应该写在这里，应该写在数据变化后，TODO: 改改改
            SetTeamUi();
            SetUserUi();
        }
    }

    // 检查命令是否合法
    public bool CheckUser(string username) {
        foreach (TeamAreaManager teamAreaManager in teamAreas)
        {
            if (teamAreaManager.IsUserInGame(username))
            {
                return true;
            }
        }
        return false;
    }

    public User FindUser(string username) {
        foreach (TeamAreaManager teamAreaManager in teamAreas)
        {
            User user = teamAreaManager.FindUser(username);
            if (user != null)
            {
                return user;
            }
        }
        return null;
    }

    ///UI相关，暂时这么整、后边改
    private void SetTeamUi(){
        //设定队伍列表相关UI，目前只有队伍列表、后续可以根据队伍排行之类的更改
        List<TeamItem> teamRank = new List<TeamItem>();
        int index = 0;
        foreach (var item in teamAreas)
        {
            teamRank.Add(new TeamItem(){
                teamName=item.getTeamInfo().Name,
                teamNumber = item.currentTeamNumberCount.ToString(),
                iconTexture = null,
                index = index
            });
            index++;
        }
        UiManager.Instance.SetTeamData(teamRank);
    }

    private void SetUserUi(){
        //设置用户排行列表相关
        List<User> userList = new List<User>();
        foreach (var item in teamAreas)
        {
            userList.AddRange(item.userList);
        }
        userList.OrderByDescending(u =>u.level);
        List<RankItem> ranks = new List<RankItem>();
        foreach (var item in userList)
        {
            ranks.Add(new RankItem(){
                userName = item.username,
                rankData = item.level.ToString(),
                iconTexture = null
            });
        }
        UiManager.Instance.SetRankData(ranks);
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
