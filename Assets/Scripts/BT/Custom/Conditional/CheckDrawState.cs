using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class CheckDrawState : Conditional
{
    public SharedPlayerGoal pg;

    public override void OnStart()
    {
        pg = (SharedPlayerGoal)Owner.GetVariable("playerGoal");
        Debug.Log("CheckDrawState" + " PlayerGoal: " + pg.Value);
    }
    public override TaskStatus OnUpdate()
    {
        if (pg.Value == PlayerGoal.Draw)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}