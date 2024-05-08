using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawMoveToAltar : PlaceAction
{

    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.altar;

        pc.playerAnimator.SetBool("isRun", true);
        pc.user.currentState.detailState = DetailState.DrawMoveToAltar;
        pc.batchInsCount = pc.insQueue.Count;
    }

    public override TaskStatus OnUpdate()
    {
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        Vector2 positionB = new Vector2(base.pc.target.position.x, base.pc.target.position.z);

        if (Vector2.Distance(positionA, positionB) < 1f)
        {
            return TaskStatus.Success;
        }
        pc.MoveToTarget();
        return TaskStatus.Running;
    }
}