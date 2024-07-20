using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pathfinding
{
    [Serializable]
    public class Node : IHeapItem<Node>
    {
        [field: SerializeField] public bool walkable { get; private set; }
        [field: SerializeField] public Vector2 worldPosition { get; private set; }
        [field: SerializeField] public Vector2Int gridPosition { get; private set; }
        [NonSerialized] public List<Node> neighbors; // Serializing neighbors is a very bad idea

        public int gCost;
        public int hCost;
        [field: SerializeField] public int penalty {  get; private set; }

        [NonSerialized] public Node parent;
        int _heapIndex;

        public int fCost { get { return gCost + hCost; } }

        public Node(bool walkable, int penalty, Vector2 worldPosition, Vector2Int gridPosition)
        {
            this.walkable = walkable;
            this.penalty = penalty;
            this.worldPosition = worldPosition;
            this.gridPosition = gridPosition;
        }

        public int HeapIndex
        {
            get { return _heapIndex; }
            set { _heapIndex = value; }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}
