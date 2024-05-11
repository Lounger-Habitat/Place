using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Defend")]
public class DefendToDoor : PlaceAction
{
    
    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.selfDoor;
        // 初始化目的位置
        if (base.pc.target.name == "Door")
        {
            Debug.Log("DefendToDoor");
        }

        pc.user.currentState.detailState = DetailState.DefendToDoor;

    }

    public override TaskStatus OnUpdate()
    {
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        Vector2 positionB = new Vector2(base.pc.target.position.x, base.pc.target.position.z);

        if (Vector2.Distance(positionA, positionB) < 1f)
        {
            // 角色到达目的地，变身
            // 播放动画和特效
            // running
            base.pc.TransToDefend();

            if (base.pc.isDefending)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
        // base.pc.MoveToTarget();
        return TaskStatus.Running;
    }
}