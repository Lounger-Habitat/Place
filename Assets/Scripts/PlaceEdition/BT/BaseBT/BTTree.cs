using UnityEngine;

namespace BT
{
    public abstract class BTTree:MonoBehaviour
    {
        public BTNode root = null;

        public void Start()
        {
            // this.root = SetupTree();
        }

        public void Update()
        {
            // root 非空
            if (root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract BTNode SetupTree();
    }
}