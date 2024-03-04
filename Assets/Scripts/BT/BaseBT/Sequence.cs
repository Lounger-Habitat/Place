using System.Collections.Generic;

namespace BT {
    public class Sequence : BTNode {
        public Sequence() : base() {
        }

        public Sequence(List<BTNode> children) : base(children) {
        }

        public override NodeState Evaluate() {
            bool anyChildRunning = false;

            foreach (var child in children) {
                switch (child.Evaluate()) {
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        continue;
                    default:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                }
            }
            nodeState = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return nodeState;
        }
    }
}