using System.Collections.Generic;

namespace BT {
    public class Selector : BTNode {
        public Selector() : base() {
        }

        public Selector(List<BTNode> children) : base(children) {
        }

        public override NodeState Evaluate() {
            foreach (var child in children) {
                switch (child.Evaluate()) {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;
                    default:
                        continue;
                }
            }
            nodeState = NodeState.FAILURE;
            return nodeState;
        }
    }
}