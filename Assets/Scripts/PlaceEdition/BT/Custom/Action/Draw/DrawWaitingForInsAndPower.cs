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
        pc.pathIndex = Random.Range(0, pc.totemPath.Count);
        pc.SetSpeed(pc.user.waitingSpeed);
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
                // 如果当前携带的指令数量已经达到最大值
                if (pc.insQueue.Count == pc.user.maxCarryingInsCount)
                {
                    return TaskStatus.Success;
                }
                // 如果当前已经携带指令，但后续颜料不够了
                int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(pc.user.instructionQueue.Peek());
                teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.camp);
                if (pc.insQueue.Count > 0 && teamInkCount < needInkCount)
                {
                    return TaskStatus.Success;
                }
                
                // 取出指令
                // Instruction instruction = pc.user.instructionQueue.Peek();

                // 判断指令颜料 和 当前有的数量 是否满足
                
                

                // instruction.needInkCount = needInkCount;

                // Debug.Log("needInkCount: " + needInkCount);



                if (pc.insQueue.Count == 0 && teamInkCount < needInkCount)
                {
                    // 颜料不足
                    Debug.Log($"{pc.user.username} : 颜料不足 , 需要: {needInkCount} , 当前: {teamInkCount}");
                    if (pc.insQueue.Count > 0) {
                        return TaskStatus.Success;
                    }

                    // 持续等待
                    return TaskStatus.Running;

                }

                Instruction instruction = pc.user.instructionQueue.Dequeue();
                instruction.needInkCount = needInkCount;
                // 指令加入准备队列
                // pc.insReadyList.Add(instruction);
                pc.user.carryingInkCount += needInkCount;

                PlaceCenter.Instance.SetTeamInkCount(pc.user.camp, -needInkCount);


                pc.insQueue.Enqueue(instruction);


            }

            return TaskStatus.Success;
        }
        pc.Dance(pc.totemPath);
        return TaskStatus.Running;
    }
}