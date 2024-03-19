using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Attack")]
public class AttackStealing : PlaceAction
{
    public override void OnStart()
    {
        base.OnStart();

        pc.user.currentState.detailState = DetailState.AttackStealing;
    }

    public override TaskStatus OnUpdate()
    {
        // wait for 1 second
        return TaskStatus.Success;
    }
}