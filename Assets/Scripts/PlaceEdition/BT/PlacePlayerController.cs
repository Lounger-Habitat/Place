using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using System;
using System.Collections;
using DG.Tweening;

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

    // 指令队列
    public Queue<Instruction> insQueue = new Queue<Instruction>();

    public int batchCount = 0;

    // 指令 Cache ，暂时 没用 
    public List<Instruction> insReadyList = new List<Instruction>();


    // 动画
    public Animator playerAnimator;
    public int waitingDraw = 0;

    // 特效

    public User user;
    // user 
    // public int speed = 5;
    public int hp = 100;

    int lastLevel = 0;

    public bool isDefending = false;
    public bool isAttacking = false;
    public bool isDrawing = false;

    public List<Vector3> totemPath = new List<Vector3>();
    public List<Vector3> consolePath = new List<Vector3>();
    public int pathIndex = 0;  // 当前路径点的索引

    [Header("玩家特效")]
    public GameObject slowEffect;
    public GameObject runEffect_1;
    public GameObject runEffect_2;
    public GameObject shieldsEffect_1;
    public GameObject shieldsEffect_2;
    public GameObject shieldsEffect_3;
    public GameObject tornadoEffect;
    public GameObject electricityEffect;
    public GameObject strikeEffect;
    public GameObject levelUpEffect;

    public void Start()
    {
        // 获取行为树
        playerBT = GetComponent<BehaviorTree>();
        // 设置行为树里的变量
        playerGoal = playerBT.GetVariable("playerGoal");
        playerGoal.SetValue(PlayerGoal.Draw);
        playerBT.SetVariable("playerGoal", playerGoal);

        int campNum = PlaceTeamManager.Instance.teamAreas.Count;

        for (int i = 0; i < campNum; i++)
        {
            buildings.Add($"Totem{i+1}", PlaceTeamManager.Instance.teamAreas[i].totem);
            buildings.Add($"Door{i+1}", PlaceTeamManager.Instance.teamAreas[i].door);
        }


        if (altar == null)
        {
            altar = GameObject.Find("Console").transform;
        }
        if (selfTotem == null)
        {
            selfTotem = GameObject.Find($"TeamArea{user.Camp}").transform;
        }
        if (selfDoor == null)
        {
            selfDoor = GameObject.Find($"TeamArea{user.Camp}Door").transform;
        }
        // if (enemyTotem == null)
        // {
        //     enemyTotem = GameObject.Find("TeamArea2").transform;
        // }
        // if (enemyDoor == null)
        // {
        //     enemyDoor = GameObject.Find("TeamArea2Door").transform;
        // }
        totemPath = GenerateCirclePath(selfTotem.position, 3, 100);
        consolePath = GenerateCirclePath(altar.position, 3, 100);
        navMeshAgent = GetComponent<NavMeshAgent>();
        lastLevel = user.Level;
    }
    public void Update()
    {
        //PlaceCenter.Instance.OnRankUIUpdate(user);
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

        // 计算贡献，自己升级
        if (user.Level > lastLevel) {
            LevelUp();
            lastLevel = user.Level;
        }
    }

    // public void MoveToTarget()
    // {
    //     transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    // }
    public void MoveToTarget()
    {
        var des = target.GetChild(0);
        navMeshAgent.destination=des.position;
        navMeshAgent.speed = user.speed;
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
        return user.Camp == c;
    }

    public void AttackTarget()
    {
        
        Debug.Log("AttackTarget");
    }

    public List<Vector3> GenerateCirclePath(Vector3 center, float radius, int numberOfPoints) {
        List<Vector3> path = new List<Vector3>();
        double angleIncrement = 2 * Math.PI / numberOfPoints;  // 计算角度间隔

        for (int i = 0; i < numberOfPoints; i++) {
            double angle = i * angleIncrement;  // 当前点的角度
            float x = (float)(center.x + radius * Math.Cos(angle));  // 计算X坐标
            float z = (float)(center.z + radius * Math.Sin(angle));  // 计算Y坐标
            path.Add(new Vector3(x, transform.position.y, z)); // 将点添加到路径中
        }

        return path;
    }

    public void SetSpeed(float speed){
        navMeshAgent.speed = speed;
    }


    public void Dance(List<Vector3> path) {
        transform.LookAt(selfTotem);
        navMeshAgent.SetDestination(path[pathIndex]);  // 设置下一个目标点
        pathIndex = (pathIndex + 1) % path.Count;  // 移动到下一个路径点
    }

    public void DrawPoint(Instruction ins) {
        PlaceConsoleAreaManager.Instance.PlayEffect(ins.x, ins.y,user.Camp);
        StartCoroutine(IDrawPoint(ins));
        // yield return new WaitForSeconds(2);
        // PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, user.camp);
        // user.carryingInkCount -= ins.needInkCount;
        // user.score += ins.needInkCount;

    }

    public void DrawLine(Instruction ins) {
        List<(int,int)> points = PlaceBoardManager.Instance.GetLinePoints(ins.x, ins.y, ins.ex, ins.ey);
        points.ForEach(p => {
            PlaceConsoleAreaManager.Instance.PlayEffect(p.Item1, p.Item2,user.Camp);
            StartCoroutine(IDrawLine(p.Item1, p.Item2, ins.r, ins.g, ins.b, user.Camp));
        });
        waitingDraw = waitingDraw + 1;
        // PlaceConsoleAreaManager.Instance.PlayEffect(ins.x, ins.y,user.camp);
        // StartCoroutine(IDrawPoint(ins));

        // yield return new WaitForSeconds(2);
        // PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, user.camp);
        // user.carryingInkCount -= ins.needInkCount;
        // user.score += ins.needInkCount;

    }

    // 协程执行绘画
    private IEnumerator IDrawPoint(Instruction ins) {
        // 等待两秒执行
        yield return new WaitForSeconds(1.5f);
        PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, user.Camp);
        user.carryingInkCount -= ins.needInkCount;
        user.score += ins.needInkCount;
        waitingDraw = waitingDraw + 1;
    }
    private IEnumerator IDrawLine(int x, int y, int r, int g, int b, int camp) {
        // 等待两秒执行
        yield return new WaitForSeconds(1.5f);
        PlaceBoardManager.Instance.DrawCommand(x, y, r, g, b, user.Camp);
        user.carryingInkCount --;
        user.score ++;
        // waitingDraw = waitingDraw + 1;
    }
    

    public void LevelUp()
    {
        //播放特效相关
        var effect = Instantiate(levelUpEffect, transform.position+new Vector3(0f,0.1f,0f),Quaternion.identity,transform.parent);//
        effect.transform.SetParent(this.transform);
        //后续可能有音效、UI提示等效果
    }

    public void PlaySlowEffect()
    {
        slowEffect.SetActive(true);
        Invoke("CloseSlowEffect",2f);
    }

    public void CloseSlowEffect()
    {
        slowEffect.SetActive(false);
    }

    public void PlayRunEffect_1(float time)
    {
        runEffect_1.SetActive(true);
        Invoke("CloseRunEffect_1", time);
    }

    public void CloseRunEffect_1()
    {
        runEffect_1.SetActive(false);
    }

    public void PlayRunEffect_2(float time)
    {
        runEffect_2.SetActive(true);
        Invoke("CloseRunEffect_2", time);
    }

    public void CloseRunEffect_2()
    {
        runEffect_2.SetActive(false);
    }

    public void PlayShieldsEffect_1()
    {
        shieldsEffect_1.SetActive(true);
        Invoke("CloseShieldsEffect_1", 2f);
    }

    public void CloseShieldsEffect_1()
    {
        shieldsEffect_1.SetActive(false);
    }

    public void PlayShieldsEffect_2()
    {
        shieldsEffect_2.SetActive(true);
        Invoke("CloseShieldsEffect_2", 2f);
    }

    public void CloseShieldsEffect_2()
    {
        shieldsEffect_2.SetActive(false);
    }

    public void PlayShieldsEffect_3()
    {
        shieldsEffect_3.SetActive(true);
        Invoke("CloseShieldsEffect_3", 2f);  //测试时自动关闭
    }

    public void CloseShieldsEffect_3()
    {
        shieldsEffect_3.SetActive(false);
    }

    /// ////////////////////////////////////////////////////////////////////////////////////龙卷风效果不是单纯的开启关闭

    public float tornadoRange = 5f;
    public void PlayTornadoEffect()
    {
        //tornadoEffect.SetActive(true);
        //Invoke("CloseTornadoEffect", 2f);  //测试时自动关闭
        
        //首先知道要生成几股龙卷风 随机获得
        int num = 4;
        for (int i = 0; i < num; i++)
        {
            float dur = UnityEngine.Random.Range(3f, 3.8f);//获得持续时间
            float xdir = UnityEngine.Random.Range(-1f, 1f);
            float zdir = UnityEngine.Random.Range(-1f, 1f);//分别获得两个方向的
            Vector3 targetPos = transform.position + new Vector3(xdir, 0, zdir).normalized * tornadoRange; //当前位置加上目标方向乘以距离就是目标位置
            GameObject tornado = Instantiate(tornadoEffect, transform.position, Quaternion.identity);
            tornado.SetActive(true);
            tornado.transform.DOMove(targetPos, dur).OnComplete(() =>
            {
               Destroy(tornado.gameObject); 
            });
        }
    }

    public void CloseTornadoEffect()
    {
        tornadoEffect.SetActive(false);
    }
    /// ////////////////////////////////////////////////////////////////////////////////////
    public void PlayElectricityEffect()
    {
        electricityEffect.SetActive(true);
        Invoke("CloseElectricityEffect", 2f);  //测试时自动关闭
    }

    public void CloseElectricityEffect()
    {
        electricityEffect.SetActive(false);
    }

    public void PlayStrikeEffect()
    {
        strikeEffect.SetActive(true);
        Invoke("CloseStrikeEffect", 2f);  //测试时自动关闭
    }

    public void CloseStrikeEffect()
    {
        strikeEffect.SetActive(false);
    }

}