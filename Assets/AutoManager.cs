using System.Data.Common;
using UnityEngine;

public class AutoManager : MonoBehaviour
{
    [System.Serializable]
    public class AutoPlayer
    {
        public AutoPlayerController controller;
        public Transform tr;

        public int number = -1;
        public bool current = false;
    }

    public AutoPlayer[] players;
    public int playersCount = 20;
    public GameObject playerPrefab;

    private float rotation;
    private float attackTimer;


    // Start is called before the first frame update
    void Start()
    {
        // 获取目标身体的HitController
        // targetHitController = targetBody.GetComponent<AutoHitController>();

        // 初始化玩家数组
        players = new AutoPlayer[playersCount];

        // 遍历玩家数组
        for (int i = 0; i < playersCount; i++)
        {
            // 随机生成一个角度
            float angle = Random.Range(0f, 360f);
            // 根据角度生成一个旋转
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            // 初始化玩家
            players[i] = new AutoPlayer();
            // 实例化玩家
            GameObject playergo = Instantiate(playerPrefab, transform.position + rot * Vector3.forward * 4, rot);
            // 获取玩家的控制器
            AutoContainer con = playergo.GetComponent<AutoContainer>();
            players[i].controller = con.playerController;
            // 设置玩家的Transform
            players[i].tr = players[i].controller.transform;
        }

        // 随机生成一个角度
        // float angle = Random.Range(0f, 360f);
        // // 根据角度生成一个旋转
        // Quaternion rot = Quaternion.Euler(0, angle, 0);

        // player = new AutoPlayer();
        // GameObject playergo = Instantiate(playerPrefab, targetBody.position + rot * Vector3.forward * 4, rot);
        // player.controller = playergo.GetComponent<AutoRedirector>().enemyController;
        // // player.controller.SetEnemyCollectiveController(this);
        // player.controller.targetHead = targetHead;
        // player.controller.targetBody = targetBody;
        // player.tr = player.controller.transform;
    }


    // Update is called once per frame
    void Update()
    {
        // 如果没有目标 找目标
        foreach (AutoPlayer player in players)
        {

            if (player.controller.targetHead == null)
            {
                player.controller.FindTarget();
                continue;
            }
            else
            {
                if (player.controller.attackTimer > 0)
                {
                    // 计时器减去时间增量
                    player.controller.attackTimer -= Time.deltaTime;
                }
                else
                {
                    if (!player.controller.targetHitController.IsDead)
                    {
                        // 设置攻击状态为真
                        player.controller.isAttackState = true;
                        // 重置攻击计时器
                        attackTimer = Random.Range(1f, 3f);
                    }
                }

                // 设置 目标的位置
                // Vector3 target = Vector3.zero;
                // 如果玩家的控制器是攻击状态
                if (player.controller.isAttackState)
                {
                    // 设置目标为目标身体的位置
                    // target = targetBody.position;
                    // 设置玩家的移动目标
                    player.controller.SetMoveTarget();
                }
                else
                {
                    // 如果玩家不是 攻击状态，设置目标为目标身体的位置 + 旋转后的前向向量 * 4 ，这里 TODO
                    // target = targetBody.position + Quaternion.Euler(0, rotation, 0) * Vector3.forward * 4;
                    player.controller.FindTarget();
                    // 设置玩家的移动目标
                    player.controller.SetMoveTarget();
                }

            }

        }

    }
}
