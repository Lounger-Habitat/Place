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
        // 初始化目的位置
        if (base.pc.target.name == "Console")
        {
            Debug.Log("MoveToAltar");
        }
        pc.playerAnimator.SetBool("isRun", true);
        pc.user.currentState.detailState = DetailState.DrawMoveToAltar;

    }

    public override TaskStatus OnUpdate()
    {
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        Vector2 positionB = new Vector2(base.pc.target.position.x, base.pc.target.position.z);

        if (Vector2.Distance(positionA, positionB) < 3f)
        {
            return TaskStatus.Success;
        }
        base.pc.MoveToTarget();
        return TaskStatus.Running;
    }
}