using System.Collections.Generic;
using UnityEngine;

public class TeamAreaManager : MonoBehaviour
{
    // 队伍区域当前容纳的人数
    public int currentTeamNumberCount = 0;

    public List<User> userList = new List<User>();

    private Team teaminfo;

    // prefab of character
    public GameObject characterPrefab;

    // 在队伍区域里创建角色
    public void CreateCharacterInTeamArea(string username)
    {
        // 检查队伍区域是否已满
        if (currentTeamNumberCount < teaminfo.MaxTeamNumber)
        {
            // 创建角色
            Vector3 spawnPosition = GetRandomPositionInArea();
            GameObject go = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
            PlayerController PlayerControllerScript = go.GetComponent<PlayerController>();
            TeamManager.Instance.CreateNameTag(go.transform, username);
            User user = new User(username, go, teaminfo.Id);
            if (PlayerControllerScript != null)
            {
                PlayerControllerScript.homePosition = transform.position;
                PlayerControllerScript.user = user;
            }
            userList.Add(user);
            currentTeamNumberCount += 1;
            //通知UIxxx加入xxx队伍
            // UiManager.Instance.AddTips(new TipsItem(){
            //     userName=username,
            //     text =$"加入{teaminfo.Name}队伍！"
            // });
            // 可以在这里设置角色的其他属性，比如所属队伍等
        }
        else
        {
            Debug.Log("队伍区域已满");
        }

    }
    private Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-5f, 5f); // 10x10区域内的随机x坐标
        float z = Random.Range(-5f, 5f); // 10x10区域内的随机z坐标
        return transform.position + new Vector3(x, 0, z);
    }

    public void setTeamInfo(Team team)
    {
        teaminfo = team;
    }

    public Team getTeamInfo()
    {
        return teaminfo;
    }

    // 用户在游戏中
    public bool IsUserInGame(string username)
    {
        User user = FindUser(username);
        if (user != null)
        {
            return true;
        }
        return false;
    }
    // 查找用户
    public User FindUser(string username)
    {
        foreach (User user in userList)
        {
            if (user.username == username)
            {
                return user;
            }
        }
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 确保触发器有一个特定的标签
        {
            PlayerController script = other.transform.root.gameObject.GetComponent<PlayerController>();
            // 角色刚刚进入触发器
            Debug.Log("角色进入触发器");
            string name = script.user.username;
            // 如果角色在此队伍区域内
            if (IsUserInGame(name))
            {
                // 检查角色是否在队伍区域内
                User user = FindUser(name);
                if (user != null)
                {
                    // 角色在队伍区域内
                    Debug.Log("角色在队伍区域内");
                    PlayerFSM stateMachine = user.character.GetComponent<PlayerFSM>();
                    stateMachine.ChangeState(CharacterState.WaitingForCommandInTeamArea);
                }
                else
                {
                    // 角色不在队伍区域内
                    Debug.Log("角色不在队伍区域内");
                }
            }
            else
            {
                // 角色不在队伍区域内
                Debug.Log("角色不在队伍区域内");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 角色刚刚离开触发器
            Debug.Log("角色离开触发器");
        }
    }
}