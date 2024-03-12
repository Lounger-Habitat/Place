using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Attack")]
public class AttackGoHome : PlaceAction
{
    public override void OnStart()
    {
        base.OnStart();

        pc.user.currentState.detailState = DetailState.AttackGoHome;
    }

    public override TaskStatus OnUpdate()
    {
        // wait for 1 second
        return TaskStatus.Success;
    }
}