using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Defend")]
public class DefendAtDoorIdle : PlaceAction
{
    
    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.selfDoor;
        // 初始化目的位置
        if (base.pc.target.name == "Door")
        {
            Debug.Log("DefendAtDoorIdle");
        }
        pc.playerAnimator.SetBool("isRun", false);

    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}