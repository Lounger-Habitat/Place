namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedPlayerGoal : SharedVariable<PlayerGoal>
    {
        public static implicit operator SharedPlayerGoal(PlayerGoal value)
        {
            return new SharedPlayerGoal { mValue = value };
        }
    }
}