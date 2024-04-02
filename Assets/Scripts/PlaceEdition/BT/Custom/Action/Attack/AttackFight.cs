using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;
using UnityEngine;

[TaskCategory("Custom/Attack")]
public class AttackFight : PlaceAction
{
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);

        pc.user.currentState.detailState = DetailState.AttackFight;
    }

    public override TaskStatus OnUpdate()
    {
        if (pc.hp == 0)
        {
            return TaskStatus.Failure;

        }
        if (pc.enemies.Count == 0)
        {
            return TaskStatus.Success;
        }
        foreach (KeyValuePair<string, PlacePlayerController> enemy in pc.enemies)
        {
            if (enemy.Value.hp == 0)
            {
                pc.enemies.Remove(enemy.Key);
                continue;
            }
            if (enemy.Value.hp > 0)
            {
                pc.target = enemy.Value.transform;
                pc.AttackTarget();
                return TaskStatus.Running;
            }
        }
        return TaskStatus.Running;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!other.CompareTag("Player")) { };
        PlacePlayerController epc = other.transform.root.gameObject.GetComponent<PlacePlayerController>();
        if (pc.isFriend(epc.user.camp))
        {
            return;
        }
        if (pc.enemies.ContainsKey(epc.user.username))
        {
            return;
        }
        pc.enemies.Add(epc.user.username,epc);



    }
}