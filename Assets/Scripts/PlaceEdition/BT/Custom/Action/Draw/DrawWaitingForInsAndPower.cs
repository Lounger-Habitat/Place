using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawWaitingForInsAndPower : PlaceAction
{
    private int danceIndex;
    bool inEpoch = false;
    bool free = true;
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

        pc.batchInsCount = 0;
        pc.batchDrawTimes = 0;
        pc.batchNeedInkCount = 0;

        free = true;

        Debug.Log("DrawWaitingForInsAndPower : 准备接受指令 : " + pc.user.instructionQueue.Count);
    }

    public override TaskStatus OnUpdate()
    {
        if (pc.insQueue.Count == 0)
        {
            free = true;
        }
        // 获取队伍颜料数量
        int teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);

        // 有新指令
        if (pc.user.instructionQueue.Count > 0)
        {
            // 优先使用身上携带的颜料
            if (pc.user.currentCarryingInkCount > 0)
            {
                if (inEpoch == false && free == true)
                {
                    StartCoroutine(UseAvailableInk(() =>
                    {
                        inEpoch = false;
                        free = false;
                        Debug.Log("设置 in Epoch 为 false");
                    }));
                }
                if (inEpoch)
                {
                    Debug.Log("身上取指令没完成，等待中");
                    return TaskStatus.Running;
                }
                // for (int i = 0; i < pc.user.instructionQueue.Count; i++)
                // {
                //     // 计算指令 消耗颜料数量
                //     int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(pc.user.instructionQueue.Peek());
                //     if (availableInkCount < needInkCount) // 身上颜料不够 ，跳出循环
                //     {
                //         break;
                //     }
                //     // 身上颜料可以负担这个指令
                //     Instruction instruction = pc.user.instructionQueue.Dequeue();
                //     instruction.needInkCount = needInkCount;
                //     pc.batchNeedInkCount += needInkCount;
                //     availableInkCount -= needInkCount;
                //     pc.insQueue.Enqueue(instruction);
                //     pc.user.currentCarryingInsCount += 1;

                // }
                // 如果颜料富裕，直接走
                if (pc.insQueue.Count > 0 && pc.user.currentCarryingInkCount >= pc.user.maxCarryingInkCount)
                {
                    return TaskStatus.Success;
                }
            }

            // 队伍颜料充足
            if (teamInkCount > 0)
            {
                if (inEpoch == false && free == true )
                {
                    StartCoroutine(UseTeamInk(() =>
                    {
                        inEpoch = false;
                        free = false;
                        Debug.Log("设置 in Epoch 为 false");
                    }));
                }
                if (inEpoch)
                {
                    Debug.Log("队伍取指令没完成，等待中");
                    return TaskStatus.Running;
                }
                // Debug.Log("有新指令添加,队伍有颜料");
                // for (int i = 0; i < pc.user.instructionQueue.Count; i++)
                // {
                //     // 计算指令 消耗颜料数量
                //     int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(pc.user.instructionQueue.Peek());
                //     teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
                //     if (teamInkCount < needInkCount) // 队伍颜料不够 ，跳出循环
                //     {
                //         break;
                //     }
                //     // 队伍颜料可以负担这个指令
                //     Instruction instruction = pc.user.instructionQueue.Dequeue();
                //     instruction.needInkCount = needInkCount;
                //     pc.batchNeedInkCount += needInkCount;
                //     pc.user.currentCarryingInkCount += needInkCount;
                //     PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -needInkCount);
                //     pc.insQueue.Enqueue(instruction);
                //     pc.user.currentCarryingInsCount += 1;
                //     // 自己已经携带的颜料已经达到最大值
                //     if (pc.user.currentCarryingInkCount >= pc.user.maxCarryingInkCount)
                //     {
                //         return TaskStatus.Success;
                //     }
                // }
            }
            if (pc.insQueue.Count > 0)
            {
                return TaskStatus.Success;
            }

        }
        return TaskStatus.Running;

        //     // 遍历instructionQueue ，取出全部 Instruction
        //     // Debug.Log("队列中有命令");
        //     for (int i = 0; i < pc.user.instructionQueue.Count; i++)
        //     {
        //         // 如果当前携带的指令数量已经达到最大值 , 或者 携带颜料数 已经达到最大值
        //         if (pc.insQueue.Count == pc.user.maxCarryingInsCount)
        //         {
        //             // Debug.Log("如果当前携带的指令数量已经达到最大值");
        //             return TaskStatus.Success;
        //         }
        //         // 如果当前已经携带指令，但后续颜料不够了
        //         int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(pc.user.instructionQueue.Peek());
        //         teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
        //         // if (pc.insQueue.Count > 0 && teamInkCount < needInkCount)
        //         // {
        //         //     return TaskStatus.Success;
        //         // }
        //         if (pc.insQueue.Count > 0 && teamInkCount < needInkCount && (pc.user.currentCarryingInkCount - pc.insQueue.Count) < needInkCount)
        //         {
        //             // Debug.Log("如果当前已经携带指令，新的一条指令，队伍颜料不够了，自己身上的颜料也不够了，就不拿了");
        //             return TaskStatus.Success;
        //         }

        //         // 取出指令
        //         // Instruction instruction = pc.user.instructionQueue.Peek();

        //         // 判断指令颜料 和 当前有的数量 是否满足


        //         // instruction.needInkCount = needInkCount;

        //         // Debug.Log("needInkCount: " + needInkCount);



        //         // 取出来 一条 指令，当前 cache 没指令，队伍也没颜料，自己也没颜料，所以颜料不足等待
        //         if (pc.insQueue.Count == 0 && teamInkCount < needInkCount && pc.user.currentCarryingInkCount < needInkCount)
        //         {
        //             // 身上没有指令，队伍也不够颜料，自己也没有颜料
        //             // 颜料不足 , 只能是  0 0 0 吧
        //             // Debug.Log($"{pc.user.Name} : 颜料不足 , 需要: {needInkCount} , 当前队伍: {teamInkCount} , 个人 : {pc.user.currentCarryingInkCount}");
        //             // if (pc.insQueue.Count > 0) {
        //             //     return TaskStatus.Success;
        //             // }

        //             // 持续等待
        //             return TaskStatus.Running;

        //         }

        //         Instruction instruction = pc.user.instructionQueue.Dequeue();
        //         instruction.needInkCount = needInkCount;
        //         // 指令加入准备队列
        //         // pc.insReadyList.Add(instruction);
        //         if (teamInkCount > needInkCount && pc.user.currentCarryingInkCount < pc.insQueue.Count && pc.user.currentCarryingInkCount < pc.user.maxCarryingInkCount - needInkCount ) {
        //             // Debug.Log("队伍颜料充足,自己身上颜料不够指令数,颜料不满，新添加的指令从队伍颜料中取");
        //             pc.user.currentCarryingInkCount += needInkCount;
        //             PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -needInkCount);
        //         }



        //         // 指令加入队列
        //         pc.insQueue.Enqueue(instruction);


        //     }

        //     if (pc.insQueue.Count > pc.user.currentCarryingInkCount)
        //     {
        //         // 身上有 指令 ， 指令数大于 携带颜料数
        //         // Debug.Log("有新指令 添加,身上有 指令 , 指令数大于 携带颜料数");
        //         teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
        //         if (teamInkCount > 0)
        //         {
        //             int diff = pc.insQueue.Count - pc.user.currentCarryingInkCount;
        //             int realTakeInkCount = (int)Math.Clamp(diff, 0, (int)(teamInkCount * pc.user.contributionRate));
        //             pc.user.currentCarryingInkCount += realTakeInkCount;
        //             PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -realTakeInkCount);
        //         }
        //     }

        //     return TaskStatus.Success;
        // }

        // // 只取颜料
        // // 自己有指令 ，只拿颜料
        // if (pc.insQueue.Count > 0 && pc.user.currentCarryingInkCount == 0 && teamInkCount > 0)
        // {
        //     // 携带颜料不足
        //     // Debug.Log("没有新指令添加,自己身上有指令,自己身上没有颜料，队伍有颜料");
        //     int needInkCount = pc.insQueue.Count;
        //     int realTakeInkCount = (int)Math.Clamp(needInkCount, 1, teamInkCount * pc.user.contributionRate);
        //     pc.user.currentCarryingInkCount += realTakeInkCount;

        //     PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -realTakeInkCount);
        //     return TaskStatus.Success;
        // }


        // // 没有指令 等待
        // return TaskStatus.Running;
    }

    // 协程
    IEnumerator UseTeamInk(System.Action callback)
    {
        inEpoch = true;
        int teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
        while (teamInkCount > 0 && pc.user.instructionQueue.Count > 0)
        {
            Debug.Log("队伍有待用的颜料和指令");
            // 计算指令 消耗颜料数量
            int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(pc.user.instructionQueue.Peek());
            teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
            if (teamInkCount < needInkCount) // 身上颜料不够 ，跳出循环
            {
                Debug.Log("队伍颜料不够了，队伍颜料: " + teamInkCount + " 需要颜料: " + needInkCount);
                break;
            }
            // 身上颜料可以负担这个指令
            // 1.取出指令
            Instruction instruction = pc.user.instructionQueue.Dequeue();
            // 2.设置指令消耗颜料数量
            instruction.needInkCount = needInkCount;
            // 3.设置批量消耗颜料数量
            pc.batchNeedInkCount += needInkCount;
            // 4.增加身上当前颜料数量
            pc.user.currentCarryingInkCount += needInkCount;
            // 5.减少队伍颜料数量
            PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -needInkCount);
            // 6.加入指令队列
            pc.insQueue.Enqueue(instruction);
            // 7.增加携带指令数量
            pc.user.currentCarryingInsCount += 1;

            // 自己已经携带的颜料已经达到最大值
            if (pc.user.currentCarryingInkCount >= pc.user.maxCarryingInkCount)
            {
                Debug.Log("携带颜料到达最大值");
                break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("协程结束");
        if (callback != null)
        {
            callback();
        }

    }

    IEnumerator UseAvailableInk(System.Action callback)
    {
        inEpoch = true;
        int availableInkCount = pc.user.currentCarryingInkCount;
        while (availableInkCount > 0 && pc.user.instructionQueue.Count > 0)
        {
            Debug.Log("身上有待用颜料和队伍有待用的指令");
            // 计算指令 消耗颜料数量
            int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(pc.user.instructionQueue.Peek());
            if (availableInkCount < needInkCount) // 身上颜料不够 ，跳出循环
            {
                Debug.Log("身上颜料不够了, 身上颜料: " + availableInkCount + " 需要颜料: " + needInkCount);
                break;
            }
            // 身上颜料可以负担这个指令
            // 1.取出指令
            Instruction instruction = pc.user.instructionQueue.Dequeue();
            // 2.设置指令消耗颜料数量
            instruction.needInkCount = needInkCount;
            // 3.设置批量消耗颜料数量
            pc.batchNeedInkCount += needInkCount;
            // 4.减少身上可用颜料数量
            availableInkCount -= needInkCount;
            // 5.加入指令队列
            pc.insQueue.Enqueue(instruction);
            // 6.增加携带指令数量
            pc.user.currentCarryingInsCount += 1;
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("协程结束");
        if (callback != null)
        {
            callback();
        }

    }
}