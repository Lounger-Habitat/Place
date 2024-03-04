using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Attack")]
public class AttackResetToTotem : PlaceAction
{
    
    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.selfTotem;
        // 初始化目的位置
        if (base.pc.target.name == "Totem")
        {
            Debug.Log("DefendAtTotem");
        }

    }

    public override TaskStatus OnUpdate()
    {
        if (base.pc.isDefending && base.pc.hp == 0)
        {
            base.pc.Resurgence();
            return TaskStatus.Running;

        }
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        Vector2 positionB = new Vector2(base.pc.target.position.x, base.pc.target.position.z);

        if (Vector2.Distance(positionA, positionB) < 0.1f)
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
        base.pc.MoveToTarget();
        return TaskStatus.Running;
    }
}