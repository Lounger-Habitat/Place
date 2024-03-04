using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawSome : PlaceAction
{
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);
    }

    public override TaskStatus OnUpdate()
    {
        if (pc.insList.Count > 0)
        {
            foreach (Instruction ins in pc.insList)
            {
                // Debug.Log(instruction.mode);
                // 这里调用绘制像素的逻辑
                // PixelsContainer.Instance.DrawPixel(instruction.x, instruction.y, instruction.r, instruction.g, instruction.b);
                switch (ins.mode)
                {
                    case "/draw":
                    case "/d":
                        PlaceBoardManager.Instance.DrawCommand(ins.mode, ins.x, ins.y, ins.r, ins.g, ins.b, pc.user.camp);
                        pc.user.carryingInkCount -= ins.needInkCount;
                        pc.user.score += ins.needInkCount;
                        break;
                    case "/line":
                    case "/l":
                        PlaceBoardManager.Instance.LineCommand(ins.mode, ins.x, ins.y, ins.ex, ins.ey, ins.r, ins.g, ins.b, pc.user.camp);
                        pc.user.carryingInkCount -= ins.needInkCount;
                        pc.user.score += ins.needInkCount;
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
            }
            if (pc.user.carryingInkCount != 0)
            {
                Debug.Log("账不对啊");
                pc.user.carryingInkCount = 0;
            }
            pc.insList.Clear();
        }

        // wait for 1 second
        return TaskStatus.Success;
    }
}