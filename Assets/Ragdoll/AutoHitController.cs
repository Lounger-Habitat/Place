using System.Collections.Generic;
using UnityEngine;

public class AutoHitController : MonoBehaviour
{
    public GameObject[] hitBodyParts;
    public Transform head;
    public GameObject hitParticle;

    public bool canDie = false;

    private AutoCharacterController charController;
    private Animator animator;
    private Rigidbody rb;
    private Collider col;
    private RagdollMecanimMixer.RamecanMixer ramecanMixer;

    private List<AutoHitController> hitTargets = new List<AutoHitController>();

    [SerializeField]
    private bool dead;
    [SerializeField]
    private float deadTimer;
    [SerializeField]
    private float deadTime = 2;

    public void Die()
    {
        dead = true;
        deadTimer = deadTime;
        ramecanMixer.BeginStateTransition("dead");
        animator.SetBool("dead", true);
        rb.isKinematic = true;
        col.enabled = false;
        charController.Die();
        charController.life--;

        // CameraOrbit co = Camera.main.GetComponent<CameraOrbit>();
        // co.secondTarget = this;
        // co.targetTimeScale = 0.05f;

    }

    public bool IsDead
    {
        get
        {
            return dead;
        }
    }

    public bool TakeHit(GameObject go, Vector3 point, Vector3 impulse)
    {
        // 如果死了，不可击打
        if (dead) return false;

        bool hit = false;
        // 遍历 可击打部分, 如果击打到了，hit为true
        foreach (GameObject bone in hitBodyParts)
        {
            if (go == bone)
            {
                if (charController.hp > 0) charController.hp -= 10;
                hit = true;
                break;
            }
        }
        // 如果没有击打到，返回
        if (!hit) return false;
        // 如果可以死亡，死亡
        if (canDie) {
            if(charController.hp <= 0) Die();
        }
            

        // 获取go的rg
        Rigidbody boneRb = go.GetComponent<Rigidbody>();
        boneRb.AddForceAtPosition(impulse.normalized * 400, point, ForceMode.Impulse);
        Vector3 dir = new Vector3(impulse.x, 0, impulse.z);
        rb.AddForce(dir.normalized * 400, ForceMode.Impulse);
        Debug.Log("Hit");
        // Instantiate(hitParticle, point, Quaternion.identity);
        return true;
    }

    public bool CanHitThisTarget(AutoHitController targetHitController)
    {
        if (charController == null) return false;
        if (!charController.isAttacking || dead) return false;
        return !hitTargets.Contains(targetHitController);
    }

    public void GiveHit(AutoHitController targetHitController)
    {
        hitTargets.Add(targetHitController);
    }

    public void Attack()
    {
        if (hitTargets.Count > 0) hitTargets.Clear();
    }

    void Start()
    {
        charController = GetComponent<AutoCharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        ramecanMixer = GetComponent<RagdollMecanimMixer.RamecanMixer>();
        ramecanMixer.BeginStateTransition("default");
    }

    void LateUpdate()
    {
        if (dead)
        {
            deadTimer -= Time.deltaTime;
            if (ramecanMixer.RootBoneRb.velocity.magnitude > 0.4f
                || ramecanMixer.RootBoneRb.angularVelocity.magnitude > 2f) deadTimer = deadTime;
            Vector3 revivePos = ramecanMixer.RootBoneTr.position;
            rb.position = new Vector3(revivePos.x, rb.position.y, revivePos.z);
            if (deadTimer <= 0 && charController.life > 0)
            {
                Revive();
            }
        }
    }

    public void Revive()
    {
        if (ramecanMixer.RootBoneRb.velocity.magnitude > 0.4f || ramecanMixer.RootBoneRb.angularVelocity.magnitude > 2f) return;
        Vector3 reviveDir = ramecanMixer.RootBoneTr.forward;
        Quaternion reviveRot = Quaternion.LookRotation(-reviveDir, Vector3.up);
        rb.rotation = Quaternion.Euler(0, reviveRot.eulerAngles.y, 0);
        //Time.timeScale = 1;
        animator.SetTrigger("reviveUp");

        animator.SetBool("dead", false);
        dead = false;
        rb.isKinematic = false;
        col.enabled = true;
        ramecanMixer.BeginStateTransition("default");
    }
}
