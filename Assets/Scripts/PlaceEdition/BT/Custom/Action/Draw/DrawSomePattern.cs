using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawSomePattern : PlaceAction
{
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);
        pc.user.currentState.detailState = DetailState.DrawSome;
        pc.batchCount = pc.insQueue.Count;
    }

    public override TaskStatus OnUpdate()
    {
        if (pc.insQueue.Count > 0)
        {
            Instruction ins = pc.insQueue.Dequeue();
                // Debug.Log(instruction.mode);
                // 这里调用绘制像素的逻辑
                // PixelsContainer.Instance.DrawPixel(instruction.x, instruction.y, instruction.r, instruction.g, instruction.b);
            switch (ins.mode)
            {
                case "/draw":
                case "/d":
                    pc.DrawPoint(ins);
                        // PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, pc.user.camp);
                        // pc.user.carryingInkCount -= ins.needInkCount;
                        // pc.user.score += ins.needInkCount;
                    break;
                case "/line":
                case "/l":
                        // pc.DrawPoint(ins);
                    pc.DrawLine(ins);
                    // PlaceBoardManager.Instance.LineCommand(ins.x, ins.y, ins.ex, ins.ey, ins.r, ins.g, ins.b, pc.user.camp);
                    // pc.user.carryingInkCount -= ins.needInkCount;
                    // pc.user.score += ins.needInkCount;
                    break;
                case "/paint":
                case "/p":
                    PlaceBoardManager.Instance.PaintCommand(ins.mode, ins.x, ins.y, ins.dx, ins.dy, ins.r, ins.g, ins.b, pc.user.camp);
                    pc.user.carryingInkCount -= ins.needInkCount;
                    pc.user.score += ins.needInkCount;
                    break;
                default:
                    break;
            }
            return TaskStatus.Running;

        }



        if (pc.batchCount == pc.waitingDraw)
        {
            if (pc.user.carryingInkCount != 0)
            {
                Debug.Log("账不对啊");
                pc.user.carryingInkCount = 0;
            }
            pc.batchCount = 0;
            pc.waitingDraw = 0;
            return TaskStatus.Success;
        }

        // wait for 1 second
        return TaskStatus.Running;
    }
}