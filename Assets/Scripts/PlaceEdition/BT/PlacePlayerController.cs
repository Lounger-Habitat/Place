using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using System;
using System.Collections;
using DG.Tweening;
using System.Linq;
using System.Net.NetworkInformation;

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

    public NavMeshAgent navMeshAgent;

    // 遇到的敌人
    public Dictionary<string, PlacePlayerController> enemies = new Dictionary<string, PlacePlayerController>();


    // 身上指令
    public List<Instruction> insList = new List<Instruction>();

    // 身上颜料
    public int inkCount = 0;

    // 指令队列 ，cache
    public Queue<Instruction> insQueue = new Queue<Instruction>();



    // 指令 Cache ，暂时 没用 
    public List<Instruction> insReadyList = new List<Instruction>();


    // 动画
    public Animator playerAnimator;

    public int batchInsCount = 0;
    public int batchDrawTimes = 0;
    public int batchNeedInkCount = 0;

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
    public GameObject blueShieldsEffect;
    public GameObject greenShieldsEffect;
    public GameObject blessingEffect;
    public GameObject tornadoEffect;
    public GameObject electricityEffect;
    public GameObject strikeEffect;
    public GameObject levelUpEffect;
    public GameObject inkUpIcon;
    public GameObject blueArea;
    public GameObject PurpleArea;

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
            selfTotem = GameObject.Find($"TeamArea{user.Camp}Totem").transform;
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
        StartLevelUp();
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
        if (Input.GetKeyDown(KeyCode.F2))
        {
            // 清空队列
            insQueue.Clear();
            user.instructionQueue.Clear();
        }
    }

    // public void MoveToTarget()
    // {
    //     transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    // }
    public void MoveToTarget(Vector3 targetPosition)
    {
        navMeshAgent.destination = targetPosition;
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

    // 协程执行绘画
    private IEnumerator IDrawPoint(Instruction ins)
    {
        // 等待两秒执行
        yield return new WaitForSeconds(1.5f);
        PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, user.Camp, user.Id);
        user.currentCarryingInkCount -= ins.needInkCount;
        user.score += ins.needInkCount;
        user.useTotalInkCount += ins.needInkCount;
        batchDrawTimes = batchDrawTimes + 1;
        user.currentCarryingInsCount -= 1;
    }

    public void DrawLine(Instruction ins)
    {
        List<(int, int)> points = PlaceBoardManager.Instance.GetLinePoints(ins.x, ins.y, ins.ex, ins.ey);
        // 一笔画
        points.ForEach(p =>
        {
            StartCoroutine(IDrawLine(p.Item1, p.Item2, ins.r, ins.g, ins.b, user.Camp));
        });

        batchDrawTimes = batchDrawTimes + 1;
        user.currentCarryingInsCount -= 1;
        // PlaceConsoleAreaManager.Instance.PlayEffect(ins.x, ins.y,user.camp);
        // StartCoroutine(IDrawPoint(ins));

        // yield return new WaitForSeconds(2);
        // PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, user.camp);
        // user.carryingInkCount -= ins.needInkCount;
        // user.score += ins.needInkCount;

    }


    private IEnumerator IDrawLine(int x, int y, int r, int g, int b, int camp)
    {
        PlaceConsoleAreaManager.Instance.PlayEffect(x, y, camp);
        // 等待两秒执行
        yield return new WaitForSeconds(1.5f);
        PlaceBoardManager.Instance.DrawCommand(x, y, r, g, b, camp, user.Id);
        user.currentCarryingInkCount--;
        user.score++;
        // waitingDraw = waitingDraw + 1;
    }



    // ============= Effect API =============
    public void StartLevelUp()
    {
        StartCoroutine(LevelUpCoroutine());
    }
    public void PlaylevelUpEffect()
    {
        var effect = Instantiate(levelUpEffect, transform.position, Quaternion.identity, transform.parent);
        effect.transform.SetParent(this.transform);
        effect.GetComponent<EffectAutoDelete>().DoDestroy(2);
    }

    public void ActiveSpeedlUp(float time)
    {
        if (superSpeeding)
        {
            superSpeedUpTime += time;
        }
        else
        {
            superSpeedUpTime = time;
            StartCoroutine(SmokeSpeedUpCoroutine());
        }
    }

    public void PassiveSpeedUp(float time = 5)
    {
        if (speeding)
        {
            speedUpTime += time;
        }
        else
        {
            speedUpTime = time;
            StartCoroutine(MagicSpeedUpCoroutine());
        }
    }

    public void PlaySmokeSpeedlUp()
    {
        if (reSmoke != null)
        {
            reSmoke.SetActive(true);
            var auto = reSmoke.GetComponent<EffectAutoDelete>();
            auto.ReStart();
        }
        else
        {
            reSmoke = Instantiate(runSmokeEffect, transform.position, Quaternion.identity, transform.parent);//
            reSmoke.transform.SetParent(this.transform);
        }
    }

    public void PlayMagicSpeedlUp()
    {
        if (reMagic != null)
        {
            reMagic.SetActive(true);
            var auto = reMagic.GetComponent<EffectAutoDelete>();
            auto.ReStart();
        }
        else
        {
            reMagic = Instantiate(runMagicEffect, transform.position, Quaternion.identity, transform.parent);
            reMagic.transform.SetParent(transform);
        }
    }
    public void PlayBlessingEffect()
    {
        if (reBlessing != null)
        {
            reBlessing.SetActive(true);
            var auto = reBlessing.GetComponent<EffectAutoDelete>();
            auto.ReBlessing();
        }
        else
        {
            reBlessing = Instantiate(blessingEffect, transform.position, Quaternion.identity, transform.parent);
            reBlessing.transform.SetParent(transform);
        }
    }

    public void Invincible(float time = 60)
    {
        if (invincible)
        {
            invincibleTime += time;
        }
        else
        {
            invincibleTime = time;
            StartCoroutine(InvincibleCoroutine(user.Camp));
        }
    }

    public void Blessing(float time = 300)
    {
        if (blessing)
        {
            blessingTime += time;
        }
        else
        {
            blessingTime = time;
            StartCoroutine(BlessingCoroutine());
        }
    }


    public void PlayInvincibleEffect(int camp)
    {
        if (camp == 1)
        {
            if (reBlueInvincible != null)
            {
                reBlueInvincible.SetActive(true);
                reBlueInvincible.transform.position = transform.position;
                var auto = reBlueInvincible.GetComponent<EffectAutoDelete>();
                auto.scale = 1.0f;
                auto.ReStart();
            }
            else
            {
                reBlueInvincible = Instantiate(blueShieldsEffect, transform.position, Quaternion.identity, transform.parent);//
                reBlueInvincible.transform.SetParent(this.transform);
                var auto = reBlueInvincible.GetComponent<EffectAutoDelete>();
                auto.scale = 1.0f;
            }
        }
        else if (camp == 2)
        {
            if (reGreenInvincible != null)
            {
                reGreenInvincible.SetActive(true);
                reGreenInvincible.transform.position = transform.position;
                var auto = reGreenInvincible.GetComponent<EffectAutoDelete>();
                auto.scale = 1.0f;
                auto.ReStart();
            }
            else
            {
                reGreenInvincible = Instantiate(blueShieldsEffect, transform.position, Quaternion.identity, transform.parent);//
                reGreenInvincible.transform.SetParent(this.transform);
                var auto = reGreenInvincible.GetComponent<EffectAutoDelete>();
                auto.scale = 1.0f;
            }
        }
    }

    public void BallThunder()
    {
        reThunderBall = Instantiate(electricityEffect, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity, transform.parent);//
        reThunderBall.transform.SetParent(this.transform);
        var auto = reThunderBall.GetComponent<EffectAutoDelete>();
        auto.scale = 1.0f;
    }

    public void Tornado(int num)
    {
        PlayTornadoEffect(num);
    }

    public void Stuck(int c = 0)
    {
        if (stucking)
        {
            stuckTime += 5;
        }
        else
        {
            stuckTime = 5;
            StartCoroutine(StuckCoroutine(c));
        }
    }

    public void PlayStuckEffect(int camp = 0)
    {
        if (camp == 1)
        {
            if (reGreen != null)
            {
                reGreen.SetActive(true);
                var auto = reGreen.GetComponent<EffectAutoDelete>();
                auto.ReStart();
            }
            else
            {
                reGreen = Instantiate(PurpleArea, transform.position, Quaternion.identity, transform.parent);//
                reGreen.transform.SetParent(this.transform);
            }
        }
        else if (camp == 2)
        {
            if (reBlue != null)
            {
                reBlue.SetActive(true);
                var auto = reBlue.GetComponent<EffectAutoDelete>();
                auto.ReStart();
            }
            else
            {
                reBlue = Instantiate(blueArea, transform.position, Quaternion.identity, transform.parent);//
                reBlue.transform.SetParent(this.transform);
            }
        }
        else
        {
            if (reStuck != null)
            {
                reStuck.SetActive(true);
                var auto = reStuck.GetComponent<EffectAutoDelete>();
                auto.ReStart();
            }
            else
            {
                reStuck = Instantiate(slowEffect, transform.position, Quaternion.identity, transform.parent);//
                reStuck.transform.SetParent(this.transform);
            }
        }
    }
    public void InkUp(int p)
    {
        PlayInkUpEffect(p);
    }

    public void PlayInkUpEffect(int p)
    {
        // var effect = Instantiate(levelUpEffect, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform.parent);//
        // effect.transform.SetParent(this.transform);
        var icon = Instantiate(inkUpIcon, user.nameTag.transform.position + new Vector3(0f, 2, 0f), Quaternion.identity, transform.parent);//
        icon.GetComponent<InkTag>().countText.text = $"X {p}";
        icon.transform.SetParent(user.nameTag.transform);
        icon.transform.DOMoveY(icon.transform.position.y + 2, 1).OnComplete(() =>
        {
            Destroy(icon.gameObject);
        });
    }

    public void PlayInkUpEffect()
    {
        //播放特效相关
        // var effect = Instantiate(runMagicEffect, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform.parent);//
        // effect.transform.SetParent(this.transform);
        // effect.GetComponent<EffectAutoDelete>().destroyTime = time;
        //后续可能有音效、UI提示等效果
    }

    ///////////////////////////////////////////////////////////////////////////////////////龙卷风效果不是单纯的开启关闭

    public float tornadoRange = 5f;
    public void PlayTornadoEffect(int num)
    {
        float powerScale = 1 + user.level * 0.03f;
        //tornadoEffect.SetActive(true);
        //Invoke("CloseTornadoEffect", 2f);  //测试时自动关闭
        float range = tornadoRange + num * 0.5f * powerScale;
        //首先知道要生成几股龙卷风 随机获得
        for (int i = 0; i < num; i++)
        {
            float dur = UnityEngine.Random.Range(3f, 4f);//获得持续时间
            float xdir = UnityEngine.Random.Range(-1f, 1f);
            float zdir = UnityEngine.Random.Range(-1f, 1f);//分别获得两个方向的
            Vector3 targetPos = transform.position + new Vector3(xdir, 0, zdir).normalized * range * powerScale; //当前位置加上目标方向乘以距离就是目标位置
            GameObject tornado = Instantiate(tornadoEffect, transform.position, Quaternion.identity);
            tornado.transform.localScale = new Vector3(powerScale, powerScale, powerScale);
            // tornado.transform.SetParent(this.transform);
            tornado.name = $"Tornado - {user.Camp}";
            // tornado.SetActive(true);
            tornado.transform.DOMove(targetPos, dur).OnComplete(() =>
            {
                Destroy(tornado.gameObject);
            });
        }
    }

    public void Thunder(float time = 10)
    {
        if (thundering)
        {
            thunderTime += time;
        }
        else
        {
            thunderTime = time;
            StartCoroutine(ThunderCoroutine(time));
        }

    }

    public void PlayThunder(Transform t = null)
    {
        // float range = 10;
        float powerScale = 1 + user.level * 0.03f;
        float xdir = UnityEngine.Random.Range(-5f, 5f);
        float zdir = UnityEngine.Random.Range(-5f, 5f);//分别获得两个方向的
        Vector3 targetPos = t != null ? t.position : transform.position + new Vector3(xdir * powerScale, 0, zdir * powerScale); //当前位置加上目标方向乘以距离就是目标位置
        GameObject thunder = Instantiate(strikeEffect, targetPos, Quaternion.identity);
        thunder.transform.localScale = new Vector3(powerScale, powerScale, powerScale);
        var auto = thunder.GetComponent<EffectAutoDelete>();
        auto.scale = 2.0f;
        auto.DoDestroy(3);
        // thunder.transform.SetParent(this.transform);
        thunder.name = $"Thunder - {user.Camp}";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tornado")
        {
            if (other.name.Contains(user.Camp.ToString()))
            {
                PassiveSpeedUp(5);
            }
            else
            {
                if (user.invincible == false)
                {
                    Stuck();
                }
            }

        }
        if (other.tag == "Thunder")
        {
            int camp = user.Camp;
            if (!other.name.Contains(camp.ToString()))
            {
                if (user.invincible == false)
                {
                    if (camp == 2)
                    {
                        Stuck(2);
                    }
                    else
                    {
                        Stuck(1);
                    }

                }
            }

        }
    }
    // === 协程 ===

    float speedUpTime = 0.0f;
    float stuckTime = 0.0f;
    float invincibleTime = 0.0f;
    float blessingTime = 0.0f;
    float thunderTime = 0.0f;
    float superSpeedUpTime = 0.0f;

    // for 特效状态 ， 与 逻辑 状态 区分开，逻辑状态 在 user里
    bool stucking = false;
    bool invincible = false;
    bool thundering = false;
    bool speeding = false;
    bool superSpeeding = false;
    bool blessing = false;

    GameObject reStuck;
    GameObject reSmoke;
    GameObject reMagic;
    GameObject reBlueInvincible;
    GameObject reGreenInvincible;
    GameObject reBlue;
    GameObject reGreen;
    GameObject reThunderBall;
    GameObject reBlessing;




    // 升级
    IEnumerator LevelUpCoroutine()
    {
        while (true)
        {
            if (user.Level > lastLevel)
            {
                PlaylevelUpEffect();
                var messageType = user.Camp == 1 ? TipsType.levelUpPanel : TipsType.levelUpPanelRight;
                PlaceUIManager.Instance.AddTips(new TipsItem()
                {
                    userName = user.Name,
                    text = $"Lv. {lastLevel}  ->  Lv. {user.level}",
                    icon = user.userIcon,//玩家头像
                    tipsType = messageType,
                    value = user.Level.ToString(),
                    isLeft = user.Camp == 1
                });
                lastLevel = user.Level;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // 困住 5秒
    IEnumerator StuckCoroutine(int camp)
    {
        stucking = true;
        user.exSpeed -= 5;
        PlayStuckEffect(camp);
        while (stuckTime > 0)
        {
            yield return new WaitForSeconds(5f);
            stuckTime -= 5;
        }
        user.exSpeed += 5;
        stucking = false;
        OffStuck();
    }

    private void OffStuck()
    {
        if (reStuck != null)
        {
            reStuck.SetActive(false);
        }
        if (reBlue != null)
        {
            reBlue.SetActive(false);
        }
        if (reGreen != null)
        {
            reGreen.SetActive(false);
        }

    }

    // 魔法跑 加速 5秒
    IEnumerator MagicSpeedUpCoroutine()
    {
        speeding = true;
        user.exSpeed += 5;
        PlayMagicSpeedlUp();
        while (speedUpTime > 0)
        {
            yield return new WaitForSeconds(5f);
            speedUpTime -= 5;
        }
        user.exSpeed -= 5;
        speeding = false;
        reMagic.SetActive(false);
    }
    // 加速跑 加速 自定义时间
    public IEnumerator SmokeSpeedUpCoroutine()
    {
        superSpeeding = true;
        user.exSpeed += 10.0f;
        PlaySmokeSpeedlUp();
        while (superSpeedUpTime > 0)
        {
            yield return new WaitForSeconds(5f);
            superSpeedUpTime -= 5f;
        }
        user.exSpeed -= 10.0f;
        superSpeeding = false;
        reSmoke.SetActive(false);
    }

    // 无敌 自定义时间
    public IEnumerator InvincibleCoroutine(int camp)
    {
        invincible = true; // 特效 
        user.exSpeed += 10.0f;
        user.invincible = true; // 逻辑 ，注意区分，为了方便 就设置了两个
        PlayInvincibleEffect(camp);
        while (invincibleTime > 1)
        {
            yield return new WaitForSeconds(60f);
            invincibleTime -= 60;
        }
        if (camp == 1)
        {
            reBlueInvincible.transform.DOScale(0, 1).OnComplete(() =>
            {
                reBlueInvincible.SetActive(false);
            });
        }
        else if (camp == 2)
        {
            reGreenInvincible.transform.DOScale(0, 1).OnComplete(() =>
            {
                reGreenInvincible.SetActive(false);
            });
        }
        yield return new WaitForSeconds(1f);
        user.exSpeed -= 10.0f;
        user.invincible = false;
        invincible = false;

    }

    IEnumerator BlessingCoroutine()
    {
        blessing = true; // 特效 
        PlayBlessingEffect();

        Collider[] cs = Physics.OverlapSphere(transform.position, 10f * 1).ToList().Where(c => c.CompareTag("Player")).ToArray();

        foreach (var c in cs)
        {
            Debug.Log(c.name + "被击退");
            c.GetComponent<Rigidbody>().AddExplosionForce(1000, transform.position, 10f);
        }

        while (blessingTime > 1)
        {
            yield return new WaitForSeconds(300f);
            blessingTime -= 300;
        }

        reBlessing.transform.DOScale(0, 1).OnComplete(() =>
        {
            reBlessing.SetActive(false);
        });

        yield return new WaitForSeconds(1f);
        blessing = false;
    }


    // ⚡️雷电
    public IEnumerator ThunderCoroutine(float time = 10)
    {
        float powerScale = 1 + user.level * 0.03f;
        thundering = true;
        BallThunder();
        yield return new WaitForSeconds(1f);
        thunderTime -= 1;
        while (thunderTime > 1)
        {
            yield return new WaitForSeconds(0.2f);
            Collider[] cs = Physics.OverlapSphere(transform.position, 5f * powerScale).ToList().Where(c => c.CompareTag("Player") && c.gameObject.GetComponent<PlacePlayerController>().user.Camp != user.Camp).ToArray();

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
            thunderTime -= 0.2f;
        }
        reThunderBall.transform.DOScale(0, 1).OnComplete(() =>
        {
            Destroy(reThunderBall.gameObject);
        });
        thundering = false;
    }
}