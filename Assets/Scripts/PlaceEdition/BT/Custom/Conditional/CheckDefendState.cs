using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class CheckDefendState : Conditional
{
    public SharedPlayerGoal pg;

    public override void OnStart()
    {
        pg = (SharedPlayerGoal)Owner.GetVariable("playerGoal");
        // Debug.Log("CheckDefendState" + " PlayerGoal: " + pg.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (pg.Value == PlayerGoal.Defend)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}