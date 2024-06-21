using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class PlaceAction : Action
{
    protected PlacePlayerController pc;

    public override void OnStart()
    {
        pc = GetComponent<PlacePlayerController>();
    }
}
public class RefWrapper<T>
{
    public T Value;
    public RefWrapper(T value) { Value = value; }
}