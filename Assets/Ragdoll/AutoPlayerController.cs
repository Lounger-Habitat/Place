
using System.Linq;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using UnityEngine;
public class AutoPlayerController : AutoCharacterController
{
    // 目标 头部
    public Transform targetHead;
    // 目标 身体
    public Transform targetBody;
    public AutoHitController targetHitController;
    // 朝向
    public bool lookAt;

    // 目标对象数组
    public GameObject[] targets;

    // 目标位置
    private Vector3 moveTargetPosition;
    // 输入方向 ？
    private Vector3 inputDirection;
    // 输入速度 ？
    private float inputVelocity;
    // 刚体
    private Rigidbody rb;
    // 动画
    private Animator animator;
    // 攻击控制器
    private AutoHitController hitController;
    // 敌人集合控制器 ？
    // private AutoEnemyCollectiveController collectiveController;

    // 是否奔跑
    private bool isRun;
    // 是否攻击状态
    public bool isAttackState;

    public float attackTimer = 0;

    // 是否死亡
    public bool IsDead
    {
        get
        {
            if (hitController == null)
                hitController = GetComponent<AutoHitController>();
            return hitController.IsDead;
        }
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hitController = GetComponent<AutoHitController>();
    }

    void FixedUpdate()
    {
    }

    public void SetMoveTarget()
    {
        moveTargetPosition = targetBody.position;
        // rb.MovePosition(moveTargetPosition);
    }

    // public void SetEnemyCollectiveController(EnemyCollectiveController collectiveController)
    // {
    //     this.collectiveController = collectiveController;
    // }

    public override void Die()
    {
        isAttackState = false;
        // collectiveController.NewEnemiesCircle();
    }

    // Update is called once per frame
    void Update()
    {
        // 如果死亡，返回
        if (hitController.IsDead) return;

        // 如果目标头部不为空
        if (targetHead != null)
        {
            // 输入方向 = 目标位置 - 刚体位置
            inputDirection = moveTargetPosition - rb.position;
        }
        else
        {
            // 输入方向 = 零向量
            inputDirection = Vector3.zero;
        }

        // 输入速度 = 输入方向的长度
        float vel = Mathf.Clamp(inputDirection.magnitude * 2, -1, 1);
        inputVelocity = Mathf.Abs(vel);

        // 角度 = 向量夹角
        float angle = Vector3.SignedAngle(transform.forward, (inputDirection * vel).normalized, Vector3.up);
        animator.SetFloat("direction", angle / 180);
        animator.SetFloat("velocity", inputVelocity);

        // 判断是否在攻击状态
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");

        // float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (targetBody == null) return;
        // 计算目标位置与自身位置的距离
        float dist = Vector3.Distance(targetBody.position, rb.position);
        // 如果距离小于 0.6 并且不在奔跑状态
        if (dist <= 0.6f && !isRun)
        {
            // 判断是不是在默认状态
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
            {
                animator.SetBool("side", !animator.GetBool("side"));
                animator.SetInteger("number", Random.Range(0, 3));
                animator.SetTrigger("attack");
                hitController.Attack();
            }

            /*if (Input.GetKeyDown(KeyCode.K)) {
                animator.SetBool("side", !animator.GetBool("side"));
                animator.SetInteger("number", 3);
                animator.SetTrigger("attack");
            }*/
        }

        // 如果距离大于 6 并且不在奔跑状态
        if (dist > 6 && !isRun)
        {
            // 设置为奔跑状态
            isRun = true;
            animator.SetBool("run", true);
            //Time.timeScale = 1;
        }

        // 如果距离小于等于 6 并且在奔跑状态
        if (dist <= 6 && isRun)
        {
            animator.SetBool("run", false);
            isRun = false;
        }

        if (targetHitController != null && targetHitController.IsDead)
        {
            isAttackState = false;
        }

    }

    void LateUpdate()
    {
        // 如果目标身体为空 , 并且在攻击状态，返回
        if (targetBody == null && isAttacking) return;
        // 目标方向是 目标位置 - 刚体位置 或 transform.forward
        Vector3 directionToTarget = targetBody != null ? targetBody.position - rb.position : transform.forward;
        // 旋转 = 朝向目标方向
        Quaternion rotation = Quaternion.LookRotation(directionToTarget.normalized, Vector3.up);
        // 刚体的旋转 = 旋转的插值
        rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), Time.deltaTime * 10);
    }

    public void FindTarget()
    {
        // 感知5米内的敌人
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100).ToList().Where(c => c.CompareTag("Player") && c.gameObject != gameObject).ToArray();
        // 选取最近的一个敌人
        float minDist = float.MaxValue;
        Transform nearest = null;
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<AutoHitController>().IsDead) continue;
            float dist = Vector3.Distance(transform.position, collider.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = collider.transform;
            }
        }
        // 设置目标头部
        targetBody = nearest;
        targetHitController = nearest.GetComponent<AutoHitController>();
        targetHead = targetHitController.head;
        // 设置目标攻击控制器

    }

    void OnAnimatorIK()
    {
        // 如果看向
        if (lookAt)
        {
            // 设置看向权重 ？
            animator.SetLookAtWeight(1, 0.5f);
            // 设置看向位置 ？
            if (targetHead != null) {
                animator.SetLookAtPosition(targetHead.position);
            }else {
                animator.SetLookAtPosition(transform.forward);
            }
        }
    }
}