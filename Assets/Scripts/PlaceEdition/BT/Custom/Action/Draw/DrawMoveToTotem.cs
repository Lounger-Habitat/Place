using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

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

        GetTargetPoint();

    }

    public override TaskStatus OnUpdate()
    {
        Vector2 positionA = new Vector2(transform.position.x, transform.position.z);


        if (Vector2.Distance(positionA, targetPosition) < 3f)
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
        randPosition = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        targetPosition = new Vector2(base.pc.target.position.x, base.pc.target.position.z) + randPosition;
        targetPositionV3 = new Vector3(targetPosition.x, pc.target.position.y, targetPosition.y);
        targetPositionV3 = NavMesh.SamplePosition(targetPositionV3, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) ? hit.position : targetPositionV3;
        targetPosition = new Vector2(targetPositionV3.x, targetPositionV3.z);
    }
}