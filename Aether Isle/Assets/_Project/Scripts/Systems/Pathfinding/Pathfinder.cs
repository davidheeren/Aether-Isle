using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class Pathfinder
    {
        HashSet<Node> closedSet = new HashSet<Node>();
        Heap<Node> openSet;

        PathGrid pathGrid;

        Vector2[] pathForDraw = new Vector2[0];

        public Pathfinder(PathGrid pathGrid)
        {
            this.pathGrid = pathGrid;
            openSet = new Heap<Node>(pathGrid.gridSize.x * pathGrid.gridSize.y);
        }

        public Vector2[] FindPath(Vector2 startPos, Vector2 endPos)
        {
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            Node startNode = pathGrid.GetClosestWalkableNodeFromWorldPosition(startPos);
            startNode.gCost = 0; // ?
            Node targetNode = pathGrid.GetClosestWalkableNodeFromWorldPosition(endPos);

            //#region Check start and end nodes
            //if (!targetNode.walkable)
            //    Debug.LogWarning("Target Node not walkable");

            //int neighborUnwalkbableCount = 0;
            //for (int i = 0; i < startNode.neighbors.Count; i++)
            //{
            //    if (!startNode.neighbors[i].walkable)
            //        neighborUnwalkbableCount++;
            //}

            //if (neighborUnwalkbableCount == startNode.neighbors.Count && !startNode.walkable)
            //{
            //    Debug.LogWarning("Start node and neighbors are unwalkable");
            //}
            //#endregion

            openSet.Clear();
            openSet.Add(startNode);

            closedSet.Clear();

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    //Debug.Log("Path found in " + sw.ElapsedMilliseconds + " ms");

                    Vector2[] path = RetracePath(startNode, targetNode);
                    pathForDraw = path;

                    return path;
                }

                foreach (Node neighbor in currentNode.neighbors)
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;

                    int neighborCost = currentNode.gCost + GetDistanceBetweenNodes(currentNode, neighbor) + neighbor.penalty;

                    if (neighborCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = neighborCost;
                        neighbor.hCost = GetDistanceBetweenNodes(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            Debug.LogWarning("No Path Found");
            return null;
        }

        Vector2[] RetracePath(Node start, Node end)
        {
            List<Vector2> path = new List<Vector2>();
            Node currentNode = end;

            while(currentNode != start)
            {
                path.Add(currentNode.worldPosition);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path.ToArray();
        }

        int GetDistanceBetweenNodes(Node a, Node b)
        {
            // Perpendicular distance is 10 while diagonal is 14

            int distX = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
            int distY = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);

            return 14 * distX + 10 * (distY - distX);
        }

        public void DrawPath()
        {
            for (int i = 1; i < pathForDraw.Length; i++)
            {
                Debug.DrawLine(pathForDraw[i - 1], pathForDraw[i], Color.cyan);
            }
        }
    }
}
