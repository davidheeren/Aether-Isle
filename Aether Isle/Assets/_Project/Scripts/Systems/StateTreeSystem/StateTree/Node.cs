using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public abstract class Node : Copyable
    {
        protected RootState rootState;

        public Node parent;
        public List<Node> children = new List<Node>();

        [NonSerialized] public string name;

        public Node(string copyJson) : base(copyJson) // I don't need an overflow constructor because this is an abstract class
        {
            // Sets default name to the type
            if (String.IsNullOrEmpty(name))
                name = this.GetType().Name;
        }

        /// <summary>
        /// Finds the best first State. Can be null
        /// </summary>
        /// <returns></returns>
        public abstract State Evaluate();

        /// <summary>
        /// Setups each child
        /// </summary>
        protected abstract void SetChildrenParentRelationships();

        /// <summary>
        /// Adds a child to the children list and sets its parent to this
        /// </summary>
        /// <param name="child"></param>
        protected void AddChild(Node child)
        {
            if (child == null)
                return;

            children.Add(child);
            child.parent = this;
        }

        /// <summary>
        /// Recursively sets relationships through the node tree. Then calls the Setup function. Only should be called in the State Tree constructor
        /// </summary>
        public void SetupWrapper(RootState rootState)
        {
            this.rootState = rootState;
            SetChildrenParentRelationships();

            if (children != null)
            {
                foreach (Node child in children)
                {
                    child.SetupWrapper(rootState);
                }
            }

            Setup();
        }

        /// <summary>
        /// Runs once when the State Tree is finished constructing. This runs after all child parent relationships are set
        /// </summary>
        protected virtual void Setup()
        {
            if (!rootState.debugSetup)
                return;

            string log = "This: " + name;

            if (parent == null)
                log += "\nParent: " + "null";
            else
                log += "\nParent: " + parent.name;

            for (int i = 0; i < children.Count; i++)
            {
                log += "\nChild " + i + ": " + children[i].name;
            }

            if (children.Count == 0)
                log += "\nNo children";

            Debug.Log(log);
        }
    }
}
