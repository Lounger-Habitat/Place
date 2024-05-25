using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawMoveToTotem : PlaceAction
{
    Vector2 randPosition;
    Vector2 targetPosition;
    Vector3 targetPositionV3;
    
    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.selfTotem;
        // 初始化目的位置
        // if (base.pc.target.name == "Totem")
        // {
        //     Debug.Log("MoveToTotem");
        // }
        pc.playerAnimator.SetBool("isRun", true);

        pc.user.currentState.topState = HighLevelState.Draw;
        pc.user.currentState.detailState = DetailState.DrawMoveToTotem;
                
        randPosition = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f));
        Vector2 positionB = new Vector2(base.pc.target.position.x, base.pc.target.position.z);
        targetPosition = positionB + randPosition;
        targetPositionV3 = new Vector3(targetPosition.x, pc.target.position.y, targetPosition.y);  


    }

    public override TaskStatus OnUpdate()
    {
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        

        if (Vector2.Distance(positionA, targetPosition) < 2f)
        {
            return TaskStatus.Success;
        }
        pc.MoveToTarget(targetPositionV3);
        return TaskStatus.Running;
    }
}