using System.Collections.Generic;
using UnityEngine;

namespace StateTree
{
    public abstract class Node
    {
        // Edit vars
        public string name;
        public Node SetName(string name) { this.name = name; return this; }

        // Read only vars
        public RootState rootState { get; private set; }
        public Node parent { get; private set; }
        public List<Node> children { get; private set; } = new List<Node>();
        public int nodeDepth { get; private set; }

        // Constructor
        public Node()
        {
            name = GetType().Name;
        }

        /// <summary>
        /// Finds the best first State. Can be null
        /// </summary>
        /// <returns></returns>
        public abstract State Evaluate();

        /// <summary>
        /// Setups each child. Use the AddChild() method in this method
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
        protected void SetupWrapper(RootState rootState, int nodeDepth)
        {
            this.rootState = rootState;
            this.nodeDepth = nodeDepth;

            SetChildrenParentRelationships();

            foreach (Node child in children)
            {
                child.SetupWrapper(rootState, nodeDepth + 1);
            }

            Setup();
        }

        /// <summary>
        /// Runs once when the State Tree is finished constructing. This runs after all child parent relationships are set
        /// </summary>
        protected virtual void Setup() { }

        protected void DestroyWrapper()
        {
            Destroy();
            foreach (Node child in children)
            {
                child.DestroyWrapper();
            }
        }
        /// <summary>
        /// Mainly used to unsubscribe to events outside of the state tree
        /// </summary>
        protected virtual void Destroy() { }

        protected void CreateGameObjectTree(GameObject parentGO)
        {
            string goName = name;
            goName += " nd: " + nodeDepth;

            State s = this as State;
            if (s != null)
                goName += " sd: " + s.stateDepth;

            GameObject debugGO = new GameObject(goName);

            if (parentGO != null)
                debugGO.transform.parent = parentGO.transform;

            foreach (Node child in children)
            {
                child.CreateGameObjectTree(debugGO);
            }
        }

        protected void Log(string msg) { if (rootState.data.debugGeneral) Debug.Log(msg); }

        #region Tree Helper Methods
        // Terms:
        // Child/Parent are only the node directly above or below node in the tree
        // Sub/Super are nodes which are above or below but not necessarily directly above or below

        public T GetFirstSuperNode<T>() where T : Node
        {
            Node currentNode = parent;

            while (true)
            {
                if (currentNode == null)
                    return null;

                if (TryCast<T>(currentNode, out T cast))
                    return cast;

                currentNode = currentNode.parent;
            }
        }

        public List<T> GetSuperNodes<T>() where T : Node
        {
            List<T> output = new List<T>();

            Node currentNode = this;

            while (true)
            {
                T node = currentNode.GetFirstSuperNode<T>();

                if (node == null)
                    break;

                output.Add(node);
                currentNode = node;
            }

            return output;
        }

        // TODO: Test all search functions below
        // Seems to be fine for now but need more testing probably

        /// <summary>
        /// First Breadth Search
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetFirstSubNodes<T>() where T : Node
        {
            List<Node> nodesToSearchChildren = new List<Node>();
            List<Node> temp = new List<Node>(); // Creates temp array so we can add and remove items in the original while iterating
            nodesToSearchChildren.Add(this);

            List<T> output = new List<T>();

            // All children searched in this loops should be the same depth
            while (true)
            {
                // Copies the nodes to temp list
                temp.Clear();
                foreach (Node child in nodesToSearchChildren)
                    temp.Add(child);

                // Iterates over all nodes's children
                foreach (Node search in temp)
                {
                    foreach (Node child in search.children)
                    {
                        // We do not need to check if the child is null because will never be

                        if (TryCast<T>(child, out T cast))
                            output.Add(cast);
                        else
                            nodesToSearchChildren.Add(child);
                    }
                    nodesToSearchChildren.Remove(search);
                }

                if (output.Count > 0 || nodesToSearchChildren.Count == 0)
                    return output;
            }
        }

        public T GetFirstSubNode<T>() where T : Node
        {
            List<T> output = GetFirstSubNodes<T>();

            if (output.Count == 0) return null;

            return output[0];
        }

        public List<T> GetSubNodes<T>() where T : Node
        {
            List<T> output = new List<T>();

            SearchChildren(this);

            void SearchChildren(Node nodeToSearchChildren)
            {
                foreach (Node child in nodeToSearchChildren.children)
                {
                    if (TryCast<T>(child, out T cast))
                        output.Add(cast);

                    SearchChildren(child);
                }
            }

            return output;
        }

        private static bool TryCast<T>(Node input, out T cast) where T : Node
        {
            cast = input as T;

            return cast != null;
        }

        #endregion
    }
}
