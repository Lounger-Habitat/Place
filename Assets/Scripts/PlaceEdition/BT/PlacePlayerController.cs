using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using System;
using System.Collections;
using DG.Tweening;

using System.Linq;

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
    public Dictionary<string, PlacePlayerController> enemies = new Dictionary<string, PlacePlayerController>();


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
    public GameObject runMagicEffect;
    public GameObject runSmokeEffect;
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
            buildings.Add($"Totem{i + 1}", PlaceTeamManager.Instance.teamAreas[i].totem);
            buildings.Add($"Door{i + 1}", PlaceTeamManager.Instance.teamAreas[i].door);
        }


        if (altar == null)
        {
            altar = GameObject.Find("ConsoleTarget").transform;
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
        if (user.Level > lastLevel)
        {
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
        navMeshAgent.destination = target.position;
        navMeshAgent.speed = user.speed + user.exSpeed;
    }

    public void TransToDefend()
    {
        isDefending = true;
        Debug.Log("TransToDefend");
    }

    public void TransToAttack()
    {
        isAttacking = true;
        Debug.Log("TransToAttack");
    }
    public void Resurgence()
    {
        hp = 100;
        // 重新生成 任务 ，特效 动画
        transform.position = selfTotem.position;
        Debug.Log("Resurgence");
    }

    public bool isFriend(int c)
    {
        return user.Camp == c;
    }

    public void AttackTarget()
    {

        Debug.Log("AttackTarget");
    }

    public List<Vector3> GenerateCirclePath(Vector3 center, float radius, int numberOfPoints)
    {
        List<Vector3> path = new List<Vector3>();
        double angleIncrement = 2 * Math.PI / numberOfPoints;  // 计算角度间隔

        for (int i = 0; i < numberOfPoints; i++)
        {
            double angle = i * angleIncrement;  // 当前点的角度
            float x = (float)(center.x + radius * Math.Cos(angle));  // 计算X坐标
            float z = (float)(center.z + radius * Math.Sin(angle));  // 计算Y坐标
            path.Add(new Vector3(x, transform.position.y, z)); // 将点添加到路径中
        }

        return path;
    }

    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }


    public void Dance(List<Vector3> path)
    {
        transform.LookAt(selfTotem);
        navMeshAgent.SetDestination(path[pathIndex]);  // 设置下一个目标点
        pathIndex = (pathIndex + 1) % path.Count;  // 移动到下一个路径点
    }

    public void DrawPoint(Instruction ins)
    {
        PlaceConsoleAreaManager.Instance.PlayEffect(ins.x, ins.y, user.Camp);
        StartCoroutine(IDrawPoint(ins));
        // yield return new WaitForSeconds(2);
        // PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, user.camp);
        // user.carryingInkCount -= ins.needInkCount;
        // user.score += ins.needInkCount;

    }

    public void DrawLine(Instruction ins)
    {
        List<(int, int)> points = PlaceBoardManager.Instance.GetLinePoints(ins.x, ins.y, ins.ex, ins.ey);
        points.ForEach(p =>
        {
            PlaceConsoleAreaManager.Instance.PlayEffect(p.Item1, p.Item2, user.Camp);
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
    private IEnumerator IDrawPoint(Instruction ins)
    {
        // 等待两秒执行
        yield return new WaitForSeconds(1.5f);
        PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, user.Camp);
        user.carryingInkCount -= ins.needInkCount;
        user.score += ins.needInkCount;
        waitingDraw = waitingDraw + 1;
    }
    private IEnumerator IDrawLine(int x, int y, int r, int g, int b, int camp)
    {
        // 等待两秒执行
        yield return new WaitForSeconds(1.5f);
        PlaceBoardManager.Instance.DrawCommand(x, y, r, g, b, user.Camp);
        user.carryingInkCount--;
        user.score++;
        // waitingDraw = waitingDraw + 1;
    }


    public void LevelUp()
    {
        //播放特效相关
        var effect = Instantiate(levelUpEffect, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform.parent);//
        effect.transform.SetParent(this.transform);
        //后续可能有音效、UI提示等效果
    }

    public void SpeedlUp(float time)
    {
        //播放特效相关
        var effect = Instantiate(runSmokeEffect, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform.parent);//
        effect.transform.SetParent(this.transform);
        effect.GetComponent<EffectAutoDelete>().destroyTime = time;
        //后续可能有音效、UI提示等效果
    }

    public void Invincible(float time = 30)
    {
        var effect = Instantiate(shieldsEffect_1, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform.parent);//
        effect.transform.SetParent(this.transform);
        var auto = effect.GetComponent<EffectAutoDelete>();
        auto.scale = 1.0f;
        auto.destroyTime = time;
    }

    public void BallThunder(float time=10)
    {
        var effect = Instantiate(electricityEffect, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity, transform.parent);//
        effect.transform.SetParent(this.transform);
        var auto = effect.GetComponent<EffectAutoDelete>();
        auto.scale = 1.0f;
        auto.destroyTime = time;
    }



    public void SpeedlUpMagic(float time)
    {
        //播放特效相关
        var effect = Instantiate(runMagicEffect, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform.parent);//
        effect.transform.SetParent(this.transform);
        effect.GetComponent<EffectAutoDelete>().destroyTime = time;
        //后续可能有音效、UI提示等效果
    }

    public void Tornado(int num)
    {
        PlayTornadoEffect(num);
    }

    public void Stuck()
    {
        var effect = Instantiate(slowEffect, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform.parent);//
        effect.transform.SetParent(this.transform);
    }

    ///////////////////////////////////////////////////////////////////////////////////////龙卷风效果不是单纯的开启关闭

    public float tornadoRange = 5f;
    public void PlayTornadoEffect(int num)
    {
        //tornadoEffect.SetActive(true);
        //Invoke("CloseTornadoEffect", 2f);  //测试时自动关闭
        float range = tornadoRange + num * 0.5f;
        //首先知道要生成几股龙卷风 随机获得
        for (int i = 0; i < num; i++)
        {
            float dur = UnityEngine.Random.Range(3f, 4f);//获得持续时间
            float xdir = UnityEngine.Random.Range(-1f, 1f);
            float zdir = UnityEngine.Random.Range(-1f, 1f);//分别获得两个方向的
            Vector3 targetPos = transform.position + new Vector3(xdir, 0, zdir).normalized * range; //当前位置加上目标方向乘以距离就是目标位置
            GameObject tornado = Instantiate(tornadoEffect, transform.position, Quaternion.identity);
            tornado.transform.SetParent(this.transform);
            tornado.name = $"Tornado - {user.Camp}";
            // tornado.SetActive(true);
            tornado.transform.DOMove(targetPos, dur).OnComplete(() =>
            {
                Destroy(tornado.gameObject);
            });
        }
    }

    public void PlayThunder(Transform t=null)
    {
        // float range = 10;
        float xdir = UnityEngine.Random.Range(-5f, 5f);
        float zdir = UnityEngine.Random.Range(-5f, 5f);//分别获得两个方向的
        Vector3 targetPos = t!=null ? t.position : transform.position + new Vector3(xdir, 0, zdir); //当前位置加上目标方向乘以距离就是目标位置
        GameObject thunder = Instantiate(strikeEffect, targetPos, Quaternion.identity);
        var auto = thunder.GetComponent<EffectAutoDelete>();
        auto.scale = 2.0f;
        auto.destroyTime = 5;
        // thunder.transform.SetParent(this.transform);
        thunder.name = $"Thunder - {user.Camp}";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tornado")
        {
            if (other.name.Contains(user.Camp.ToString()))
            {
                StartCoroutine(SpeedUpCoroutine());
            }
            else
            {
                if (user.invincible == false)
                {
                    StartCoroutine(StuckCoroutine());
                }
            }

        }
        if (other.tag == "Thunder")
        {
            if (!other.name.Contains(user.Camp.ToString()))
            {
                if (user.invincible == false)
                {
                    StartCoroutine(StuckCoroutine());
                }
            }

        }
    }
    // === 协程 ===

    // 困住 5秒
    IEnumerator StuckCoroutine()
    {
        user.exSpeed -= 5;
        Stuck();
        yield return new WaitForSeconds(5f);
        user.exSpeed += 5;
    }

    // 魔法跑 加速 3秒
    IEnumerator SpeedUpCoroutine()
    {
        user.exSpeed += 5;
        SpeedlUpMagic(3);
        yield return new WaitForSeconds(3f);
        user.exSpeed -= 5;
    }
    // 加速跑 加速 自定义时间
    public IEnumerator TimeLimitSpeedUp(float time)
    {
        user.speed += 10.0f;
        SpeedlUp(time);
        yield return new WaitForSeconds(time);
        user.speed -= 10.0f;
    }

    // 无敌 自定义时间
    public IEnumerator TimeLimitInvincible(float time = 30)
    {
        user.exSpeed += 10.0f;
        user.invincible = true;
        Invincible(time);
        yield return new WaitForSeconds(time);
        user.exSpeed -= 10.0f;
        user.invincible = false;
    }

    // ⚡️雷电
    public IEnumerator Thunder(float time = 10)
    {
        float cd = time;
        BallThunder(time);
        yield return new WaitForSeconds(1f);
        cd -= 1;
        while (cd > 1) {
            yield return new WaitForSeconds(0.2f);
            Collider[] cs = Physics.OverlapSphere(transform.position, 5f).ToList().Where(c => c.CompareTag("Player") && c.gameObject.GetComponent<PlacePlayerController>().user.Camp != user.Camp).ToArray();
            

            if (cs.Length > 0)
            {
                // 随机数
                int index = UnityEngine.Random.Range(0, cs.Length);
                PlayThunder(cs[index].transform);
            }
            else
            {
                PlayThunder();
            }
            cd -= 0.2f;
        }
    }
}