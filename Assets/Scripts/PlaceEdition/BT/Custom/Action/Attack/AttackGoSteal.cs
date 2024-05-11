using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Attack")]
public class AttackGoSteal : PlaceAction
{
    public bool meetEnemy = false;
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);

        pc.user.currentState.detailState = DetailState.AttackGoSteal;
    }

    public override TaskStatus OnUpdate()
    {
        pc.buildings.TryGetValue("Totem" + pc.user.attckingIns, out pc.target);
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        Vector2 positionB = new Vector2(pc.target.position.x, pc.target.position.z);

        if (pc.enemies.Count > 0)
        {
            return TaskStatus.Failure;
        }
        if (Vector2.Distance(positionA, positionB) < 3f)
        {
                return TaskStatus.Success;
        }
        // pc.MoveToTarget();
        return TaskStatus.Running;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!other.CompareTag("Player")) {};
        PlacePlayerController epc = other.transform.root.gameObject.GetComponent<PlacePlayerController>();
        if (pc.isFriend(epc.user.Camp)) {
            return;
        }
        pc.enemies.Add(epc.user.Name, epc);

    }
}