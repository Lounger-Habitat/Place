using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawWaitingForInsAndPower : PlaceAction
{
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);
        pc.user.currentState.detailState = DetailState.DrawWaitingForInsAndPower;
    }

    public override TaskStatus OnUpdate()
    {
        // 获取队伍颜料数量
        int teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.camp);
        // if (pc.insReadyList.Count > 0 && teamInkCount > 0)
        // {
        //     Instruction instruction = pc.insReadyList[0];
        //     if (teamInkCount < instruction.needInkCount)
        //     {
        //         return TaskStatus.Running;
        //     }

        //     pc.user.carryingInkCount += instruction.needInkCount;
        //     pc.insReadyList.RemoveAt(0);
        //     PlaceCenter.Instance.SetTeamInkCount(pc.user.camp, -instruction.needInkCount);
        //     pc.insList.Add(instruction);
        // }
        // 有指令有颜料
        if (pc.user.instructionQueue.Count > 0 && teamInkCount > 0)
        {
            // 遍历instructionQueue ，取出全部 Instruction
            // Debug.Log("队列中有命令");
            for (int i = 0; i < pc.user.instructionQueue.Count; i++)
            {
                if (pc.insList.Count == pc.user.maxCarryingInsCount)
                {
                    return TaskStatus.Success;
                }
                
                // 取出指令
                Instruction instruction = pc.user.instructionQueue.Peek();

                // 判断指令颜料 和 当前有的数量 是否满足
                int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(instruction);
                

                // instruction.needInkCount = needInkCount;

                Debug.Log("needInkCount" + needInkCount);



                if (needInkCount > teamInkCount)
                {
                    // 颜料不足
                    Debug.Log("颜料不足");
                    if (pc.insList.Count > 0) {
                        return TaskStatus.Success;
                    }

                    // 持续等待
                    return TaskStatus.Running;

                }

                instruction = pc.user.instructionQueue.Dequeue();
                instruction.needInkCount = needInkCount;
                // 指令加入准备队列
                // pc.insReadyList.Add(instruction);
                pc.user.carryingInkCount += needInkCount;

                PlaceCenter.Instance.SetTeamInkCount(pc.user.camp, -needInkCount);


                pc.insList.Add(instruction);


            }

            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}