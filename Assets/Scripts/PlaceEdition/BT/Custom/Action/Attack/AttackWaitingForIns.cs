using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Attack")]
public class AttackWaitingForIns : PlaceAction
{
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);

        pc.user.currentState.detailState = DetailState.AttackWaitingForIns;
    }

    public override TaskStatus OnUpdate()
    {
        // 获取队伍颜料数量
        if (pc.user.attckingIns == 0) {
            return TaskStatus.Running; 
        }
        if (pc.user.attckingIns == pc.user.camp) {
            Debug.Log("不能偷取自家能量");
            return TaskStatus.Running;
        }
        // 取出目标位置
        return TaskStatus.Success;
    }
}