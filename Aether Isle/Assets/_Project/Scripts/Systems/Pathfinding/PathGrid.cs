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
                return new Vector2Int(Mathf.CeilToInt(initialGridWorldSize.x / nodeDiameter), Mathf.CeilToInt(initialGridWorldSize.y / nodeDiameter));
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

            Vector2 bottomLeft = positionAtGridCreation - new Vector2(gridWorldSize.x / 2, gridWorldSize.y / 2);

            LayerMask allPenaltiesMask = new LayerMask();
            foreach (PathLayer p in pathLayers)
            {
                allPenaltiesMask |= p.mask;
            }

            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    CreateNode(bottomLeft, new Vector2Int(i, j), allPenaltiesMask);
                }
            }

            foreach (Node node in grid)
            {
                node.neighbors = GetNeighbors(node.gridPosition);
            }

            sw.Stop();
            print("Grid created in " + sw.ElapsedMilliseconds + " ms");
        }

        public void UpdateGrid(Bounds bounds)
        {

        }

        void CreateNode(Vector2 bottomLeft, Vector2Int gridPos, LayerMask allPenaltiesMask)
        {
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

        [Serializable]
        struct PathLayer
        {
            public LayerMask mask;
            public int penalty;
        }
    }
}
