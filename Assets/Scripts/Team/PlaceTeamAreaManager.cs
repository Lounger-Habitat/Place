using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Purchasing;

public class PlaceTeamAreaManager : MonoBehaviour
{
    // 墨水能量数
    public float ink;
    // 队伍区域当前容纳的人数
    public int currentTeamNumberCount = 0;
    private float defaultInkTime = 10f;
    public List<User> userList = new List<User>();
    public Team teaminfo;
    // prefab of character
    public GameObject characterPrefab;
    private float timer = 0f;
    private float updateInterval = 1f; // 更新间隔为1秒

    public GameObject teamAreaName;



    void Update(){
        // 每秒更新
        // 累加计时器
        timer += Time.deltaTime;
        // 检查是否达到更新间隔
        if (timer >= updateInterval)
        {
            // 执行每秒更新的操作
            InkUpdate();
            // 重置计时器
            timer = 0f;

        }
    }

    void InkUpdate()
    {
        // 墨水数随着时间增加，默认初始每10s增加一点，随着人数的增加，增加速度增加
        // 当前人数/10 = 每秒产生颜料数
        // if vip ，
        float exInk = 0f;

        foreach (User u in userList)
        {
            //  额外 墨水
            exInk += (u.getLevel() - 1) * 0.1f;
            // 用户积分
            u.score += (u.getLevel()-1) * 10;
        }

        float inkRate = (float)(currentTeamNumberCount / defaultInkTime) + exInk;

        ink += inkRate;
        //Debug.Log("ink " + ink);

        UpdateTeamAreaName();
        teaminfo.score = PlaceCredits.CalculateScore(ink);
        PlaceCenter.Instance.OnTeamUIUpdate(teaminfo);
        
    }

    void UpdateTeamAreaName()
    {
        string nameTemplate = "{0} - {1}";
        string formattedString = string.Format(nameTemplate, teaminfo.Name, (int)System.Math.Round(ink));
        teamAreaName.GetComponent<NameTag>().go_name = formattedString;
    }

    // 在队伍区域里创建角色
    public User CreateCharacterInTeamArea(string username)
    {
        GameObject go = null;
        User user = null;
        // 检查队伍区域是否已满
        if (currentTeamNumberCount < teaminfo.MaxTeamNumber)
        {
            // 创建角色
            Vector3 spawnPosition = GetRandomPositionInArea();
            go = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
            PlayerController PlayerControllerScript = go.GetComponent<PlayerController>();
            PlaceCenter.Instance.CreateNameTag(go.transform, username);
            user = new User(username,go,teaminfo.Id);
            if (PlayerControllerScript != null)
            {
                PlayerControllerScript.homePosition = transform.position;
                PlayerControllerScript.user = user;
            }
            userList.Add(user);
            currentTeamNumberCount += 1;
            // 可以在这里设置角色的其他属性，比如所属队伍等
            PlaceUIManager.Instance.AddTips(new TipsItem(){
                userName=username,
                text =$"加入{teaminfo.Name}队伍！"
            });
        }
        else
        {
            Debug.Log("队伍区域已满");
        }
        return user;

    }

    public GameObject CreateMessageBubbleOnPlayer(string username,string message)
    {
        GameObject go = null;

        // 找到角色位置
        User user = FindUser(username);
        go = user.character;
        if (go == null)
        {
            Debug.Log("角色不存在");
            return null;
        }
        // PlayerController PlayerControllerScript = go.GetComponent<PlayerController>();
        PlaceCenter.Instance.CreateMessageBubble(go.transform, message);
        // 对Player 元信息 更新
        // if (PlayerControllerScript != null)
        // {
        //     PlayerControllerScript.homePosition = transform.position;
        //     PlayerControllerScript.user = user;
        // }
        // 可以在这里设置角色的其他属性，比如所属队伍等
        return go;

    }
    private Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-5f, 5f); // 10x10区域内的随机x坐标
        float z = Random.Range(-5f, 5f); // 10x10区域内的随机z坐标
        return transform.position + new Vector3(x, 0.85f, z);
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
    public bool IsUserInTeam(string username)
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
            PlayerController pc = other.transform.root.gameObject.GetComponent<PlayerController>();
            // 角色刚刚进入触发器
            Debug.Log("角色进入触发器");
            string name = pc.user.username;
            // 如果角色在此队伍区域内
            /*
                1. 检查角色是否在队伍区域内
                2. 敌对角色进入队伍区域
                3. 友方角色进入队伍区域
            */
            if (IsUserInTeam(name)) // 本队队员进入队伍区域
            {
                // 检查角色是否在队伍区域内
                Debug.Log(name + " 进入队伍区域内,state : " + pc.user.currentState);
                // 判断成员状态
                PlayerFSM stateMachine = pc.user.character.GetComponent<PlayerFSM>();
                if (pc.user.currentState == CharacterState.MoveToTeamArea){
                    // Debug.Log("ReturningFromConsoleToGetCommand -> WaitingForCommandInTeamArea");
                    stateMachine.ChangeState(CharacterState.Trance);
                }
                else if (pc.user.currentState == CharacterState.WaitingForCommandInTeamArea){
                    // 角色在队伍区域内
                    Debug.Log("等待指令");
                }
                else
                {
                    // 角色不在队伍区域内
                    Debug.Log("非法状态");
                }
            }
            // TODO ： 友方队伍
            else // 敌对队员进入队伍区域
            {
                Debug.Log("敌对队员进入队伍区域");
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