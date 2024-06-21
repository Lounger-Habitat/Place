using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawSomePattern : PlaceAction
{
 
    RefWrapper<bool> drawing;
    RefWrapper<bool> drawFinish;
    public override void OnStart()
    {
        base.OnStart();
        pc.playerAnimator.SetBool("isRun", false);
        pc.user.currentState.detailState = DetailState.DrawSome;
        pc.batchInsCount = pc.insQueue.Count;
        drawing = new RefWrapper<bool>(false);
        drawFinish = new RefWrapper<bool>(false);
    }

    public override TaskStatus OnUpdate()
    {
        if (!drawing.Value)
        {
            drawing.Value = true;
            StartCoroutine(pc.DrawPattern(drawing,drawFinish));
        }
        if (drawFinish.Value)
        {
            return TaskStatus.Success;
        }
        Debug.Log($"[BT] : drawFinish: {drawFinish.Value} drawing: {drawing.Value}");

        // wait for 1 second
        return TaskStatus.Running;
    }

}