using System.Collections.Generic;

namespace BT
{
    public class BTNode
    {
        protected NodeState nodeState;
        public BTNode parent;
        protected List<BTNode> children = new List<BTNode>();

        public BTNode()
        {
            parent = null;
        }

        public BTNode(List<BTNode> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        // add a child to the node
        public void AddChild(BTNode child)
        {
            child.parent = this;
            children.Add(child);
        }

        public virtual NodeState Evaluate()
        {
            return NodeState.FAILURE;
        }

        // custom data for the node
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public void SetData(string key, object value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.Add(key, value);
            }
        }

        public object GetData(string key)
        {
            
            object value = null;

            if (_data.TryGetValue(key, out value))
            {
                return value;
            }

            // 如果 本 node 没有 ，找 parent

            BTNode node = parent;
            
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null) return value;

                node = node.parent;
            }

            return null;


            
        }

        public bool RemoveData(string key)
        {
            if (_data.ContainsKey(key))
            {
                _data.Remove(key);
                return true;
            }

            BTNode node = parent;
            while (node != null)
            {
                bool cleared = node.RemoveData(key);
                if (cleared) return true;

                node = node.parent;
            }
            return false;
        }

    }
}