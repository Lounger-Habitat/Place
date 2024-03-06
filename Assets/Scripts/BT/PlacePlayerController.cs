using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;

public class PlacePlayerController : MonoBehaviour
{
    public SharedVariable playerGoal;
    public BehaviorTree playerBT;
    public Transform target;

    // 祭坛
    public Transform altar;
    // 自家图腾
    public Transform selfTotem;

    public Transform selfDoor;

    public Transform enemyTotem;

    public Transform enemyDoor;

    public Dictionary<string, Transform> buildings = new Dictionary<string, Transform>();

    private NavMeshAgent navMeshAgent;

    // 遇到的敌人
    public Dictionary<string,PlacePlayerController> enemies = new Dictionary<string,PlacePlayerController>();
    
    
    // 身上指令

    public List<Instruction> insList = new List<Instruction>();

    // 指令 Cache ，暂时 没用 
    public List<Instruction> insReadyList = new List<Instruction>();


    // 动画
    public Animator playerAnimator;

    // 特效

    public User user;
    // user 
    public int speed = 5;
    public int hp = 100;

    public bool isDefending = false;
    public bool isAttacking = false;
    public bool isDrawing = false;

    public void Start()
    {
        // 获取行为树
        playerBT = GetComponent<BehaviorTree>();
        // 设置行为树里的变量
        playerGoal = playerBT.GetVariable("playerGoal");
        playerGoal.SetValue(PlayerGoal.Draw);
        playerBT.SetVariable("playerGoal", playerGoal);

        for (int i = 0; i < 4; i++)
        {
            buildings.Add($"Totem{i+1}", PlaceTeamManager.Instance.teamAreas[i].totem);
            buildings.Add($"Door{i+1}", PlaceTeamManager.Instance.teamAreas[i].door);
        }


        if (altar == null)
        {
            altar = GameObject.Find("Console").transform;
        }
        if (target == null)
        {
            target = selfTotem;
        }
        // if (enemyTotem == null)
        // {
        //     enemyTotem = GameObject.Find("TeamArea2").transform;
        // }
        // if (enemyDoor == null)
        // {
        //     enemyDoor = GameObject.Find("TeamArea2Door").transform;
        // }
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void Update()
    {
        PlaceCenter.Instance.OnRankUIUpdate(user);
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("trans to Draw");
            playerGoal.SetValue(PlayerGoal.Draw);
            playerBT.SetVariable("playerGoal", playerGoal);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("trans to Defend");
            playerGoal.SetValue(PlayerGoal.Defend);
            playerBT.SetVariable("playerGoal", playerGoal);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("trans to Attack");
            playerGoal.SetValue(PlayerGoal.Attack);
            playerBT.SetVariable("playerGoal", playerGoal);
        }
    }

    // public void MoveToTarget()
    // {
    //     transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    // }
    public void MoveToTarget()
    {
        navMeshAgent.destination=target.position;
    }

    public void TransToDefend() {
        isDefending = true;
        Debug.Log("TransToDefend");
    }

    public void TransToAttack() {
        isAttacking = true;
        Debug.Log("TransToAttack");
    }
    public void Resurgence(){
        hp = 100;
        // 重新生成 任务 ，特效 动画
        transform.position = selfTotem.position;
        Debug.Log("Resurgence");
    }

    public bool isFriend(int c){
        return user.camp == c;
    }

    public void AttackTarget()
    {
        
        Debug.Log("AttackTarget");
    }
    
}