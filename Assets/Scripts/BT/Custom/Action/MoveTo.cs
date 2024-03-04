using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class MoveTo : Action
{
    // 目的地
    public SharedVector3 targetPosition;
    // 移动速度
    public float speed = 1;
    // 自身位置
    private Transform selfTransform;

    private PlacePlayerController pm;
    
    public override void OnStart()
    {
        pm = GetComponent<PlacePlayerController>();
        // 初始化目的位置
        selfTransform = transform;
        if (targetPosition.Value == null)
        {
            targetPosition.Value = selfTransform.position;
        }
    }

    public override TaskStatus OnUpdate()
    {
        targetPosition.Value = pm.target.position;
        Vector2 positionA = new Vector2(selfTransform.position.x, selfTransform.position.z);
        Vector2 positionB = new Vector2(targetPosition.Value.x, targetPosition.Value.z);

        if (Vector2.Distance(positionA, positionB) < 0.1f)
        {
            return TaskStatus.Success;
        }
        // selfTransform.position = Vector3.MoveTowards(selfTransform.position, targetPosition.Value, speed * Time.deltaTime);
        pm.MoveToTarget();
        return TaskStatus.Running;
    }
}