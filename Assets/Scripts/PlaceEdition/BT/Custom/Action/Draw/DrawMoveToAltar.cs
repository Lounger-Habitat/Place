using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Draw")]
public class DrawMoveToAltar : PlaceAction
{

    Vector2 randPosition;
    Vector2 targetPosition;
    Vector3 targetPositionV3;
    public override void OnStart()
    {
        base.OnStart();
        base.pc.target = base.pc.altar;

        pc.playerAnimator.SetBool("isRun", true);
        pc.user.currentState.detailState = DetailState.DrawMoveToAltar;
        pc.batchInsCount = pc.insQueue.Count;

        randPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
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