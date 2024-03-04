using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Defend")]
public class DefendAtDoorAttack : PlaceAction
{
    
    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.selfDoor;
        // 初始化目的位置
        if (base.pc.target.name == "Door")
        {
            Debug.Log("DefendAtDoorAttack");
        }

    }

    public override TaskStatus OnUpdate()
    {
        if (base.pc.hp <= 0)
        {
            return TaskStatus.Failure;
        }
        //发现敌人
        
        return TaskStatus.Running;
    }
}