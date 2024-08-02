using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawWaitingForInsAndPower : PlaceAction
{
    private int danceIndex;
    bool inEpoch = false;
    bool free = true;
    private string[] danceName = new[] { "TwistDance", //"BreakDacne",
        "SillyDance","SillyDance", "HipHopDance" };
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
        //Debug.Log("DrawWaitingForInsAndPower OnUpdate");
        if (pc.insQueue.Count == 0 && free == false)
        {
            Debug.Log("【wait for ins and power】：玩家身上携带指令数量为0");
            free = true;
        }
        // 获取队伍颜料数量
        int teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);

        // 有新指令，player接收的弹幕指令
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
                        Debug.Log("UseAvailableInk 设置 in Epoch 为 false");
                    }));
                }
                if (inEpoch)
                {
                    Debug.Log("身上取指令没完成，等待中");
                    return TaskStatus.Running;
                }
                if (pc.insQueue.Count > 0 && pc.user.currentCarryingInkCount >= pc.user.maxCarryingInkCount)
                {
                    Debug.Log("身上颜料富裕，直接走");
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
            }


        }
        if (free == false)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;

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
        // 有多少颜料 拿 多少指令，拿到没指令位置
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
        yield return new WaitForSeconds(0.2f);
        Debug.Log("协程结束");
        if (callback != null)
        {
            callback();
        }

    }
}