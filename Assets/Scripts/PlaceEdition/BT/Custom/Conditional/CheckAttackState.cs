using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class CheckAttackState : Conditional
{
    public SharedPlayerGoal pg;

    public override void OnStart()
    {
        pg = (SharedPlayerGoal)Owner.GetVariable("playerGoal");
        Debug.Log("CheckAttackState" + " PlayerGoal: " + pg.Value);
    }
    public override TaskStatus OnUpdate()
    {
        if (pg.Value == PlayerGoal.Attack)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}