using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawWaitingForInsAndPower : PlaceAction
{
    private int danceIndex;
    private string[] danceName = new[] { "TwistDance", "BreakDacne", "SillyDance", "HipHopDance" };
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);
        pc.user.currentState.detailState = DetailState.DrawWaitingForInsAndPower;
        pc.pathIndex = UnityEngine.Random.Range(0, pc.totemPath.Count);
        pc.SetSpeed(pc.user.waitingSpeed);
        //播放随机舞蹈动画
        // Debug.Log("开始Dance");
        danceIndex = UnityEngine.Random.Range(0, 4);
        pc.playerAnimator.SetBool(danceName[danceIndex], true);
    }

    public override TaskStatus OnUpdate()
    {
        // 获取队伍颜料数量
        int teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);

        // 取指令，带颜料
        // 人物有指令，队伍有颜料, 或自己有颜料
        if (pc.user.instructionQueue.Count > 0 && teamInkCount > 0)
        {
            // 遍历instructionQueue ，取出全部 Instruction
            // Debug.Log("队列中有命令");
            for (int i = 0; i < pc.user.instructionQueue.Count; i++)
            {
                // 如果当前携带的指令数量已经达到最大值 , 或者 携带颜料数 已经达到最大值
                if (pc.insQueue.Count == pc.user.maxCarryingInsCount)
                {
                    // Debug.Log("如果当前携带的指令数量已经达到最大值");
                    return TaskStatus.Success;
                }
                // 如果当前已经携带指令，但后续颜料不够了
                int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(pc.user.instructionQueue.Peek());
                teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
                // if (pc.insQueue.Count > 0 && teamInkCount < needInkCount)
                // {
                //     return TaskStatus.Success;
                // }
                if (pc.insQueue.Count > 0 && teamInkCount < needInkCount && (pc.user.currentCarryingInkCount - pc.insQueue.Count) < needInkCount)
                {
                    // Debug.Log("如果当前已经携带指令，新的一条指令，队伍颜料不够了，自己身上的颜料也不够了，就不拿了");
                    return TaskStatus.Success;
                }

                // 取出指令
                // Instruction instruction = pc.user.instructionQueue.Peek();

                // 判断指令颜料 和 当前有的数量 是否满足


                // instruction.needInkCount = needInkCount;

                // Debug.Log("needInkCount: " + needInkCount);



                // 取出来 一条 指令，当前 cache 没指令，队伍也没颜料，自己也没颜料，所以颜料不足等待
                if (pc.insQueue.Count == 0 && teamInkCount < needInkCount && pc.user.currentCarryingInkCount < needInkCount)
                {
                    // 身上没有指令，队伍也不够颜料，自己也没有颜料
                    // 颜料不足 , 只能是  0 0 0 吧
                    // Debug.Log($"{pc.user.Name} : 颜料不足 , 需要: {needInkCount} , 当前队伍: {teamInkCount} , 个人 : {pc.user.currentCarryingInkCount}");
                    // if (pc.insQueue.Count > 0) {
                    //     return TaskStatus.Success;
                    // }

                    // 持续等待
                    return TaskStatus.Running;

                }

                Instruction instruction = pc.user.instructionQueue.Dequeue();
                instruction.needInkCount = needInkCount;
                // 指令加入准备队列
                // pc.insReadyList.Add(instruction);
                if (teamInkCount > needInkCount && pc.user.currentCarryingInkCount < pc.insQueue.Count && pc.user.currentCarryingInkCount < pc.user.maxCarryingInkCount - needInkCount ) {
                    // Debug.Log("队伍颜料充足,自己身上颜料不够指令数,颜料不满，新添加的指令从队伍颜料中取");
                    pc.user.currentCarryingInkCount += needInkCount;
                    PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -needInkCount);
                }



                // 指令加入队列
                pc.insQueue.Enqueue(instruction);


            }

            if (pc.insQueue.Count > pc.user.currentCarryingInkCount)
            {
                // 身上有 指令 ， 指令数大于 携带颜料数
                // Debug.Log("有新指令 添加,身上有 指令 , 指令数大于 携带颜料数");
                teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
                if (teamInkCount > 0)
                {
                    int diff = pc.insQueue.Count - pc.user.currentCarryingInkCount;
                    int realTakeInkCount = (int)Math.Clamp(diff, 0, (int)(teamInkCount * pc.user.contributionRate));
                    pc.user.currentCarryingInkCount += realTakeInkCount;
                    PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -realTakeInkCount);
                }
            }

            return TaskStatus.Success;
        }

        // 只取颜料
        // 自己有指令 ，只拿颜料
        if (pc.insQueue.Count > 0 && pc.user.currentCarryingInkCount == 0 && teamInkCount > 0)
        {
            // 携带颜料不足
            // Debug.Log("没有新指令添加,自己身上有指令,自己身上没有颜料，队伍有颜料");
            int needInkCount = pc.insQueue.Count;
            int realTakeInkCount = (int)Math.Clamp(needInkCount, 1, teamInkCount * pc.user.contributionRate);
            pc.user.currentCarryingInkCount += realTakeInkCount;

            PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -realTakeInkCount);
            return TaskStatus.Success;
        }


        // 没有指令 等待
        return TaskStatus.Running;
    }
}