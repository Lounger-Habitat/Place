using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Defend")]
public class DefendResetToTotem : PlaceAction
{
    
    public override void OnStart()
    {
        base.OnStart();
        pc.target = pc.selfTotem;
        // 初始化目的位置
        if (pc.target.name == "Totem")
        {
            Debug.Log("DefendAtTotem");
        }

        if (pc.isDefending)
        {
            pc.playerAnimator.SetBool("isRun", false);
        }
        else{
            pc.playerAnimator.SetBool("isRun", true);
        
        }

        pc.user.currentState.topState = HighLevelState.Defend;
        pc.user.currentState.detailState = DetailState.DefendResetToTotem;

    }

    public override TaskStatus OnUpdate()
    {
        if (pc.isDefending && pc.hp == 0)
        {
            // 复活
            pc.Resurgence();
            return TaskStatus.Running;

        }
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        Vector2 positionB = new Vector2(pc.target.position.x, pc.target.position.z);

        if (Vector2.Distance(positionA, positionB) < 3f)
        {
            // 角色到达目的地，变身
            // 播放动画和特效
            // running
            pc.TransToDefend();

            if (pc.isDefending)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
        pc.MoveToTarget();
        return TaskStatus.Running;
    }
}