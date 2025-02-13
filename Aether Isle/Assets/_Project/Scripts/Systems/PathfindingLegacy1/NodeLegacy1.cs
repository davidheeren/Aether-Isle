using UnityEngine;

namespace Pathfinding
{
    public struct NodeLegacy1
    {
        public bool walkable { get; private set; }
        public int penalty { get; private set; }

        public Vector2Int parentIndex;

        public int gCost;
        public int hCost;
        public int fCost { get { return gCost + hCost; } }

        public NodeLegacy1(bool walkable, int penalty)
        {
            this.walkable = walkable;
            this.penalty = penalty;

            parentIndex = -Vector2Int.one;

            gCost = 0;
            hCost = 0;
        }
    }

    public struct SerializedNode
    {
        public bool walkable;
        public int penalty;

        public SerializedNode(bool walkable, int penalty)
        {
            this.walkable = walkable;
            this.penalty = penalty;
        }

        public NodeLegacy1 ToNode()
        {
            return new NodeLegacy1(walkable, penalty);
        }
    }
}
