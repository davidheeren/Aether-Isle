using UnityEngine;
using Utilities;
using System.Collections.Generic;
using System;
using UnityEditor;
using CustomInspector;

namespace Pathfinding
{
    public class PathGrid : Singleton<PathGrid>
    {
        [Button(nameof(Scan))]
        [SerializeField] LayerMask unWalkableMask;
        [SerializeField] Vector2 initialGridWorldSize = Vector2.one * 25;
        [SerializeField] float nodeDiameter = 1;
        [SerializeField] float overlapCircleOffset = -0.02f; // or 0.2f for a 9 square instead of single

        [SerializeField] PathLayer[] pathLayers;

        Node[,] grid;
        Vector2 positionAtGridCreation;

        public Vector2Int gridSize
        {
            get
            {
                Vector2Int size = new Vector2Int(Mathf.CeilToInt(initialGridWorldSize.x / nodeDiameter), Mathf.CeilToInt(initialGridWorldSize.y / nodeDiameter));
                // This makes it centered on the grid
                if (size.x % 2 == 1)
                    size.x++;
                if (size.y % 2 == 1)
                    size.y++;
                return size;
            }
        }
        Vector2 gridWorldSize { get { return (Vector2)gridSize * nodeDiameter; } }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(positionAtGridCreation, new Vector3(initialGridWorldSize.x, initialGridWorldSize.y));

            if (grid == null)
                return;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Gizmos.color = grid[i, j].walkable ? Color.green : Color.red;

                    if (grid[i, j].penalty > 0)
                        Gizmos.color = Color.yellow;

                    Gizmos.DrawWireCube(grid[i, j].worldPosition, Vector2.one * nodeDiameter * 0.75f);
                }
            }

        }

        private void Awake()
        {
            CreateGrid();
        }

        void Scan()
        {
            CreateGrid();
            SceneView.RepaintAll();
        }

        private void CreateGrid()
        {
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            grid = new Node[gridSize.x, gridSize.y];
            positionAtGridCreation = transform.position;


            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    CreateNode(new Vector2Int(i, j));
                }
            }

            foreach (Node node in grid)
            {
                node.neighbors = GetNeighbors(node.gridPosition);
            }

            sw.Stop();
            print("Pathfinding grid created in " + sw.ElapsedMilliseconds + " ms");
        }

        public void UpdateGrid(Bounds bounds)
        {
            Node bottomLeft = GetNodeFromWorldPosition(bounds.center - bounds.extents);
            Node topRight = GetNodeFromWorldPosition(bounds.center + bounds.extents);

            for (int i = bottomLeft.gridPosition.x - 1; i <= topRight.gridPosition.x + 1; i++)
            {
                for (int j = bottomLeft.gridPosition.y - 1; j <= topRight.gridPosition.y + 1; j++)
                {
                    if (OutsideOfGrid(i, j))
                        continue;

                    CreateNode(new Vector2Int(i, j));
                }
            }

            SceneView.RepaintAll();
        }

        void CreateNode(Vector2Int gridPos)
        {
            LayerMask allPenaltiesMask = new LayerMask();
            foreach (PathLayer p in pathLayers)
            {
                allPenaltiesMask |= p.mask;
            }

            Vector2 bottomLeft = positionAtGridCreation - new Vector2(gridWorldSize.x / 2, gridWorldSize.y / 2);
            Vector2 worldPos = bottomLeft + new Vector2(gridPos.x * nodeDiameter + nodeDiameter / 2, gridPos.y * nodeDiameter + nodeDiameter / 2);

            bool walkable = !Physics2D.OverlapCircle(worldPos, nodeDiameter / 2 + overlapCircleOffset, unWalkableMask);
            int penalty = 0;

            if (walkable)
            {
                Collider2D col = Physics2D.OverlapCircle(worldPos, nodeDiameter / 2 + overlapCircleOffset, allPenaltiesMask);
                if (col != null)
                {
                    foreach (PathLayer pathLayer in pathLayers)
                    {
                        if (CompareMask(pathLayer.mask, col.gameObject.layer))
                        {
                            penalty = pathLayer.penalty;
                            break;
                        }
                    }
                }
            }

            bool CompareMask(LayerMask layerMask, int layer)
            {
                return (layerMask & (1 << layer)) != 0;
            }

            grid[gridPos.x, gridPos.y] = new Node(walkable, penalty, worldPos, gridPos);
        }

        public Node GetNodeFromWorldPosition(Vector2 position)
        {
            float percentX = (position.x - positionAtGridCreation.x) / gridWorldSize.x + 0.5f;
            float percentY = (position.y - positionAtGridCreation.y) / gridWorldSize.y + 0.5f;

            int x = Mathf.FloorToInt(Mathf.Clamp(gridSize.x * percentX, 0, gridSize.x - 1));
            int y = Mathf.FloorToInt(Mathf.Clamp(gridSize.y * percentY, 0, gridSize.y - 1));

            return grid[x, y];
        }

        public Node GetClosestWalkableNodeFromWorldPosition(Vector2 position)
        {
            // Works but might be a more intuitive use the grid position and an offset to get the radius
            Node startNode = GetNodeFromWorldPosition(position);

            if (startNode.walkable)
                return startNode;

            for (int depth = 1; depth <= 50; depth++)
            {
                List<Node> walkableNodes = new List<Node>();

                // Diagonals
                AddPerimeter(depth, depth);
                AddPerimeter(depth, -depth);
                AddPerimeter(-depth, depth);
                AddPerimeter(-depth, -depth);

                for (int i = -(depth - 1); i <= depth - 1; i++)
                {
                    // Horizontal Lines
                    AddPerimeter(i, depth);
                    AddPerimeter(i, -depth);

                    // Vertical Lines
                    AddPerimeter(depth, i);
                    AddPerimeter(-depth, i);
                }

                void AddPerimeter(int offsetX, int offsetY)
                {
                    int x = startNode.gridPosition.x + offsetX;
                    int y = startNode.gridPosition.y + offsetY;

                    if (OutsideOfGrid(x, y))
                        return;

                    if (grid[x, y].walkable)
                        walkableNodes.Add(grid[x, y]);
                }

                if (walkableNodes.Count == 0)
                    continue;

                int closestIndex = 0;
                float closestSqrDist = (walkableNodes[closestIndex].worldPosition - position).sqrMagnitude;

                for (int i = 1; i < walkableNodes.Count; i++)
                {
                    float currentSqrDist = (walkableNodes[i].worldPosition - position).sqrMagnitude;

                    if (currentSqrDist < closestSqrDist)
                    {
                        closestIndex = i;
                        closestSqrDist = currentSqrDist;
                    }
                }

                if (depth > 1 && depth < 10) // It's ok if the depth is one but probably a problem after
                    Debug.LogWarning("Walkable node found at depth of: " + depth);
                if (depth >= 10)
                    Debug.LogError("Walkable node found at depth of: " + depth);

                return walkableNodes[closestIndex];
            }

            Debug.LogWarning("Walkable node not found");
            return null;
        }

        List<Node> GetNeighbors(Vector2Int gridPos)
        {
            List<Node> neighbors = new List<Node>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int checkX = gridPos.x + i;
                    int checkY = gridPos.y + j;

                    if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                    {
                        neighbors.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbors;
        }

        bool OutsideOfGrid(int x, int y)
        {
            return x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1);
        }

        [Serializable]
        struct PathLayer
        {
            public LayerMask mask;
            public int penalty;
        }
    }
}
