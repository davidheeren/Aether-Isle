using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class PathfinderLegacy1
    {
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        HashSet<Vector2Int> openSet = new HashSet<Vector2Int>();
        MinHeapLegacy1<Vector2Int, int> minHeap;

        PathGridLegacy1 pathGrid;

        /// <summary>If path grid is null it will be the singleton</summary>
        public PathfinderLegacy1(PathGridLegacy1 pathGrid = null)
        {
            if (pathGrid == null) pathGrid = PathGridLegacy1.Instance;
            this.pathGrid = pathGrid;
            minHeap = new MinHeapLegacy1<Vector2Int, int>(pathGrid.gridSize.x * pathGrid.gridSize.y);
        }

        public Vector2[] FindPath(Vector2 startPos, Vector2 endPos)
        {
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            Vector2Int startIndex = pathGrid.GetClosestWalkableNodeIndexFromWorldPosition(startPos);
            pathGrid.grid[startIndex.x, startIndex.y].gCost = 0;
            Vector2Int targetIndex = pathGrid.GetClosestWalkableNodeIndexFromWorldPosition(endPos);

            openSet.Clear();
            minHeap.Clear();
            openSet.Add(startIndex);
            minHeap.Add(startIndex, 0); // Double check priority

            closedSet.Clear();

            while (openSet.Count > 0)
            {
                Vector2Int currentIndex = minHeap.Pop();
                openSet.Remove(currentIndex);

                closedSet.Add(currentIndex);

                if (currentIndex == targetIndex)
                {
                    sw.Stop();
                    //Debug.Log("Path found in " + sw.ElapsedMilliseconds + " ms");

                    Vector2[] path = RetracePath(startIndex, targetIndex);

                    return path;
                }

                foreach (Vector2Int neighborIndex in pathGrid.GetValidNeighborIndices(currentIndex))
                {
                    if (closedSet.Contains(neighborIndex))
                        continue;

                    int neighborCost = pathGrid.grid[currentIndex.x, currentIndex.y].gCost;
                    neighborCost += GetDistanceBetweenNodes(currentIndex, neighborIndex) + pathGrid.grid[neighborIndex.x, neighborIndex.y].penalty;

                    if (neighborCost < pathGrid.grid[neighborIndex.x, neighborIndex.y].gCost || !openSet.Contains(neighborIndex))
                    {
                        pathGrid.grid[neighborIndex.x, neighborIndex.y].gCost = neighborCost;
                        pathGrid.grid[neighborIndex.x, neighborIndex.y].hCost = GetDistanceBetweenNodes(neighborIndex, targetIndex);
                        pathGrid.grid[neighborIndex.x, neighborIndex.y].parentIndex = currentIndex;

                        if (!openSet.Contains(neighborIndex))
                        {
                            minHeap.Add(neighborIndex, pathGrid.grid[neighborIndex.x, neighborIndex.y].fCost);
                            openSet.Add(neighborIndex);
                        }
                    }
                }
            }

            Debug.LogWarning("No Path Found");
            return null;
        }

        Vector2[] RetracePath(Vector2Int start, Vector2Int end)
        {
            Stack<Vector2> stack = new Stack<Vector2>();
            Vector2Int currentIndex = end;

            while (currentIndex != start)
            {
                stack.Push(pathGrid.CellToWorldPosition(currentIndex));
                currentIndex = pathGrid.grid[currentIndex.x, currentIndex.y].parentIndex;
            }

            return stack.ToArray();
        }

        int GetDistanceBetweenNodes(Vector2Int a, Vector2Int b)
        {
            // Perpendicular distance is 10 while diagonal is 14

            int distX = Mathf.Abs(a.x - b.x);
            int distY = Mathf.Abs(a.y - b.y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);

            return 14 * distX + 10 * (distY - distX);
        }
    }
}
