using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Custom/Draw")]
public class DrawMoveToAltar : PlaceAction
{

    Vector2 randPosition;
    Vector2 targetPosition;
    Vector3 targetPositionV3;
    public override void OnStart()
    {
        base.OnStart();
        pc.target = pc.altar;

        pc.playerAnimator.SetBool("isRun", true);
        pc.user.currentState.detailState = DetailState.DrawMoveToAltar;
        pc.batchInsCount = pc.insQueue.Count;

        GetTargetPoint();

        
    }

    public override TaskStatus OnUpdate()
    {
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);
        

        if (Vector2.Distance(positionA, targetPosition) < 2f)
        {
            return TaskStatus.Success;
        }
        // if (!pc.navMeshAgent.hasPath)
        // {
        //     GetTargetPoint();
        // }
        // else
        // {
        pc.MoveToTarget(targetPositionV3);
        // }
        return TaskStatus.Running;
    }

    void GetTargetPoint()
    {
        randPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        Vector2 positionB = new Vector2(base.pc.target.position.x, base.pc.target.position.z);
        targetPosition = positionB + randPosition;
        targetPositionV3 = new Vector3(targetPosition.x, pc.target.position.y, targetPosition.y);
        targetPositionV3 = NavMesh.SamplePosition(targetPositionV3, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) ? hit.position : targetPositionV3;
        targetPosition = new Vector2(targetPositionV3.x, targetPositionV3.z);
    }
}