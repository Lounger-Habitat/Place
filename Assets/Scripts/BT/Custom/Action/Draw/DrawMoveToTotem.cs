using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawMoveToTotem : PlaceAction
{
    
    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.selfTotem;
        // 初始化目的位置
        if (base.pc.target.name == "Totem")
        {
            Debug.Log("MoveToTotem");
        }
        pc.playerAnimator.SetBool("isRun", true);

    }

    public override TaskStatus OnUpdate()
    {
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        Vector2 positionB = new Vector2(base.pc.target.position.x, base.pc.target.position.z);

        if (Vector2.Distance(positionA, positionB) < 1f)
        {
            return TaskStatus.Success;
        }
        base.pc.MoveToTarget();
        return TaskStatus.Running;
    }
}