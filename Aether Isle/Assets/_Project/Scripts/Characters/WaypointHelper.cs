using System.Linq;
using UnityEngine;
using Utilities;

namespace Game
{
    public class WaypointHelper
    {
        // TODO
        Vector2[] waypoints = new Vector2[0];
        public bool loop;
        public bool reverse;
        float nextWaypointRange;

        int currentIndex;
        bool atEnd;

        int EndIndex => reverse ? 0 : waypoints.Length - 1;
        public int Length => waypoints.Length;

        public WaypointHelper(Vector2[] waypoints, bool loop = true, bool reverse = false, float nextWaypointRange = 0.5f)
        {
            SetWaypoints(waypoints);
            this.loop = loop;
            this.reverse = reverse;
            this.nextWaypointRange = nextWaypointRange;
        }

        public Vector2? GetCurrentWaypoint(Vector2 position)
        {
            if (waypoints.Length == 0)
                return null;

            CheckIndex(position);

            if (atEnd)
            {
                float sqrDis = (waypoints[currentIndex] - position).sqrMagnitude;
                float epsilon = 0.05f;

                if (sqrDis < epsilon * epsilon) 
                    return null;
            }

            return waypoints[currentIndex];
        }

        void CheckIndex(Vector2 position)
        {
            int i = currentIndex;
            int offset = reverse ? -1 : 1;
            while (i >= 0 && i < waypoints.Length)
            {
                float sqrDist = (position - waypoints[i]).sqrMagnitude;

                // Sets index to first waypoint farther away than the range
                if (sqrDist > nextWaypointRange * nextWaypointRange)
                {
                    SetIndex(i);
                    return;
                }

                // Advance the loop
                i += offset;
            }

            // If at the last waypoint then set to last or first if loop
            SetIndex(EndIndex + offset);
        }

        public WaypointHelper SetIndex(int index)
        {
            if (waypoints.Length == 0) return this;

            atEnd = false;

            if (loop)
            {
                index = Maths.Mod(index, waypoints.Length);
            }
            else
            {
                index = Mathf.Clamp(index, 0, waypoints.Length - 1);

                if (index == EndIndex)
                    atEnd = true;
            }

            currentIndex = index;

            return this;
        }

        public WaypointHelper Reset()
        {
            SetIndex(0);
            return this;
        }

        public WaypointHelper SetWaypoints(Vector2[] waypoints)
        {
            if (waypoints != null)
                this.waypoints = waypoints;
            else
                waypoints = new Vector2[0];

            return this;
        }

        public WaypointHelper SetCurrentIndexToEnd()
        {
            SetIndex(EndIndex);
            return this;
        }

        public WaypointHelper SetCurrentIndexToClosest(Vector2 position)
        {
            if (waypoints.Length == 0)
                return this;

            // Find the index of the closest waypoint
            int closestIndex = 0;
            float closestSqrDist = (waypoints[0] - position).sqrMagnitude;

            for (int i = 1; i < waypoints.Length; i++)
            {
                float sqrDist = (waypoints[i] - position).sqrMagnitude;
                if (sqrDist < closestSqrDist)
                {
                    closestSqrDist = sqrDist;
                    closestIndex = i;
                }
            }

            SetIndex(closestIndex);
            return this;
        }

        public void DrawWaypoints()
        {
            for (int i = 1; i < waypoints.Length; i++)
            {
                Debug.DrawLine(waypoints[i - 1], waypoints[i], Color.cyan);
            }
        }
    }
}
