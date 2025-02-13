using UnityEngine;
using Utilities;
using System.Collections.Generic;
using System;
using UnityEditor;
using CustomInspector;
using UnityEngine.UIElements;

namespace Pathfinding
{
    public class PathGridLegacy1 : Singleton<PathGridLegacy1>
    {
        [Button(nameof(Scan))]
        [SerializeField] LayerMask unWalkableMask;
        [SerializeField] Vector2 initialGridWorldSize = Vector2.one * 25;
        [SerializeField] float nodeDiameter = 1;
        [SerializeField] float overlapCircleOffset = -0.02f; // or 0.2f for a 9 square instead of single

        [SerializeField] PathLayer[] pathLayers;

        public NodeLegacy1[,] grid { get; private set; }

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

                    Gizmos.DrawWireCube(CellToWorldPosition(i, j), Vector2.one * nodeDiameter * 0.75f);
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

            # if UNITY_EDITOR
            SceneView.RepaintAll();
            #endif
        }

        private void CreateGrid()
        {
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            grid = new NodeLegacy1[gridSize.x, gridSize.y];
            positionAtGridCreation = transform.position;


            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    CreateNode(new Vector2Int(i, j));
                }
            }

            sw.Stop();
            print("Pathfinding grid created in " + sw.ElapsedMilliseconds + " ms");
        }

        public void UpdateGrid(Bounds bounds)
        {
            Vector2Int bottomLeft = GetNodeIndexFromWorldPosition(bounds.center - bounds.extents);
            Vector2Int topRight = GetNodeIndexFromWorldPosition(bounds.center + bounds.extents);

            for (int i = Mathf.Max(bottomLeft.x - 1, 0); i <= Mathf.Min(topRight.x + 1, gridSize.x - 1); i++)
            {
                for (int j = Mathf.Max(bottomLeft.y - 1, 0); j <= Mathf.Min(topRight.y + 1, gridSize.y - 1); j++)
                {
                    CreateNode(new Vector2Int(i, j));
                }
            }

            #if UNITY_EDITOR
            SceneView.RepaintAll();
            #endif
        }

        void CreateNode(Vector2Int cell)
        {
            LayerMask allPenaltiesMask = new LayerMask();
            foreach (PathLayer p in pathLayers)
            {
                allPenaltiesMask |= p.mask;
            }

            Vector2 worldPos = CellToWorldPosition(cell);

            Collider2D col = Physics2D.OverlapCircle(worldPos, nodeDiameter / 2 + overlapCircleOffset, unWalkableMask | allPenaltiesMask);
            bool walkable = !(col != null && CompareMask(unWalkableMask, col.gameObject.layer));
            int penalty = 0;

            if (walkable && col != null)
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

            bool CompareMask(LayerMask layerMask, int layer)
            {
                return layerMask == -1 || (layerMask & (1 << layer)) != 0; // added -1
            }

            grid[cell.x, cell.y] = new NodeLegacy1(walkable, penalty);
        }

        public Vector2Int GetNodeIndexFromWorldPosition(Vector2 position)
        {
            float percentX = (position.x - positionAtGridCreation.x) / gridWorldSize.x + 0.5f;
            float percentY = (position.y - positionAtGridCreation.y) / gridWorldSize.y + 0.5f;

            int x = Mathf.FloorToInt(Mathf.Clamp(gridSize.x * percentX, 0, gridSize.x - 1));
            int y = Mathf.FloorToInt(Mathf.Clamp(gridSize.y * percentY, 0, gridSize.y - 1));

            return new Vector2Int(x, y);
        }

        public Vector2Int GetClosestWalkableNodeIndexFromWorldPosition(Vector2 position)
        {
            // Works but might be a more intuitive use the grid position and an offset to get the radius
            Vector2Int startIndex = GetNodeIndexFromWorldPosition(position);

            if (grid[startIndex.x, startIndex.y].walkable)
                return startIndex;

            for (int depth = 1; depth <= 10; depth++)
            {
                Vector2Int closestIndex = -Vector2Int.one;
                float closestSqrDist = float.MaxValue;

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
                    Vector2Int currentIndex = new Vector2Int(startIndex.x + offsetX, startIndex.y + offsetY);

                    if (OutsideOfGrid(currentIndex))
                        return;

                    if (!grid[currentIndex.x, currentIndex.y].walkable)
                        return;


                    float currentSqrDist = (CellToWorldPosition(currentIndex) - position).sqrMagnitude;

                    if (currentSqrDist < closestSqrDist)
                    {
                        closestIndex = currentIndex;
                        closestSqrDist = currentSqrDist;
                    }
                }

                if (depth > 1) // It's ok if the depth is one but probably a problem after
                    Debug.LogWarning("Walkable node found at depth of: " + depth);

                if (closestIndex != -Vector2Int.one)
                    return closestIndex;
            }

            Debug.LogError("Walkable node not found");
            return -Vector2Int.one;
        }

        public IEnumerable<Vector2Int> GetValidNeighborIndices(Vector2Int cell)
        {

            for (int i = -1; i <= 1; i++) // i,j is offset
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue; // Ignore center

                    Vector2Int index = -Vector2Int.one;

                    if (i == 0 || j == 0)
                        index = AddSide(new Vector2Int(i, j));
                    else
                        index = AddDiagonal(new Vector2Int(j, i));

                    if (index != -Vector2Int.one)
                        yield return index;
                }
            }

            Vector2Int AddSide(Vector2Int offset)
            {
                Vector2Int neighborPos = cell + offset;

                if (OutsideOfGrid(neighborPos))
                    return -Vector2Int.one;

                if (!grid[neighborPos.x, neighborPos.y].walkable)
                    return -Vector2Int.one;

                return neighborPos;
            }

            Vector2Int AddDiagonal(Vector2Int offset)
            {
                Vector2Int neighborPos = cell + offset;

                if (OutsideOfGrid(neighborPos))
                    return -Vector2Int.one;

                if (!grid[neighborPos.x, neighborPos.y].walkable)
                    return -Vector2Int.one;

                Vector2Int a = neighborPos + new Vector2Int(-offset.x, 0);
                Vector2Int b = neighborPos + new Vector2Int(0, -offset.y);

                if (OutsideOfGrid(a) || OutsideOfGrid(b))
                    return -Vector2Int.one;

                // If a diagonal with edges are unwalkable then it should not be a neighbor
                if (!grid[a.x, a.y].walkable && !grid[b.x, b.y].walkable)
                    return -Vector2Int.one;

                return neighborPos;
            }
        }

        public Vector2 CellToWorldPosition(Vector2Int cell) => CellToWorldPosition(cell.x, cell.y);
        public Vector2 CellToWorldPosition(int x, int y)
        {
            Vector2 bottomLeft = positionAtGridCreation - new Vector2(gridWorldSize.x / 2, gridWorldSize.y / 2);
            return bottomLeft + new Vector2(x * nodeDiameter + nodeDiameter / 2, y * nodeDiameter + nodeDiameter / 2);
        }

        public bool OutsideOfGrid(Vector2Int pos) => OutsideOfGrid(pos.x, pos.y);
        public bool OutsideOfGrid(int x, int y)
        {
            return x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y;
        }

        [Serializable]
        struct PathLayer
        {
            public LayerMask mask;
            public int penalty;
        }
    }
}
