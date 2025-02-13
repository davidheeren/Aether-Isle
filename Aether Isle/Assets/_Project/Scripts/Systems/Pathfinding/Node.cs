using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pathfinding
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable { get; private set; }
        public Vector2 worldPosition { get; private set; }
        public Vector2Int gridPosition { get; private set; }
        public List<Node> validNeighbors; // List of nodes that can be searched from this node

        public int gCost;
        public int hCost;
        public int penalty {  get; private set; }

        public Node parent;
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

    public struct Neighbor
    {
        Vector2Int offset;
        Node node;
    }
}
