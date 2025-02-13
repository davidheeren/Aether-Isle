using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartition
{
    public class SpatialHashGrid<T> : MonoBehaviour where T : ISpatialGridEntry
    {
        protected readonly Dictionary<Vector2Int, HashSet<T>> entries = new Dictionary<Vector2Int, HashSet<T>>();
        protected readonly Dictionary<T, Vector2Int> cellPositions = new Dictionary<T, Vector2Int>();

        // Note: there is no guarantee that entry.Position will actually be in the same cell as it should so we need to update it in fixed update

        protected virtual float CellSize => 3;

        protected IEnumerable<T> AllEntries
        {
            get
            {
                foreach (HashSet<T> hashSet in entries.Values)
                {
                    foreach (T entry in hashSet)
                    {
                        yield return entry;
                    }
                }
            }
        }

        public void Add(T value)
        {
            Vector2Int cell = WorldToCellPosition(value.Position);
            GetInitializedValue(cell).Add(value);

            cellPositions[value] = cell;
        }

        public void Remove(T value)
        {
            if (!cellPositions.TryGetValue(value, out Vector2Int cell))
                return;

            HashSet<T> hashSet = entries[cell];

            hashSet.Remove(value);

            if (hashSet.Count == 0)
                entries.Remove(cell);

            cellPositions.Remove(value);
        }

        public List<T> GetEntriesInRadius(Vector2 position, float radius) => GetEntriesInRadius(position, radius, -1);
        public List<T> GetEntriesInRadius(Vector2 position, float radius, LayerMask mask)
        {
            List<T> res = new List<T>();
            Vector2Int center = WorldToCellPosition(position);
            int cellRadius = WorldToCellRadius(radius);

            foreach (Vector2Int cell in GetCellsInRadius(center, cellRadius))
            {
                if (!entries.TryGetValue(cell, out HashSet<T> hashSet))
                    continue;

                foreach (T entry in hashSet)
                {
                    if (InRadius(position, entry.Position, radius) && LayerMaskContains(mask, entry.Layer))
                        res.Add(entry);
                }
            }

            return res;
        }

        /// <summary>First not ordered but in closest ring</summary>
        public T GetEntryInRadius(Vector2 position, float radius) => GetEntryInRadius(position, radius, -1);
        public T GetEntryInRadius(Vector2 position, float radius, LayerMask mask)
        {
            Vector2Int center = WorldToCellPosition(position);
            int cellRadius = WorldToCellRadius(radius);

            for (int i = 0; i <= cellRadius; i++)
            {
                foreach (Vector2Int cell in GetCellsOnRadius(center, i))
                {
                    if (!entries.TryGetValue(cell, out HashSet<T> hashSet))
                        continue;

                    foreach (T entry in hashSet)
                    {
                        if (InRadius(position, entry.Position, radius) && LayerMaskContains(mask, entry.Layer))
                            return entry;
                    }
                }
            }

            return default;
        }

        public bool TryGetEntryInRadius(Vector2 position, float radius, out T closestEntry) => TryGetEntryInRadius(position, radius, -1, out closestEntry);
        public bool TryGetEntryInRadius(Vector2 position, float radius, LayerMask mask, out T entry)
        {
            entry = GetEntryInRadius(position, radius, mask);
            return !EqualityComparer<T>.Default.Equals(entry, default);
        }

        public T GetClosestEntryInRadius(Vector2 position, float radius) => GetClosestEntryInRadius(position, radius, -1);
        public T GetClosestEntryInRadius(Vector2 position, float radius, LayerMask mask)
        {
            Vector2Int center = WorldToCellPosition(position);
            int cellRadius = WorldToCellRadius(radius);

            T closestEntry = default;
            float closestSqrDist = float.MaxValue;

            for (int i = 0; i <= cellRadius; i++)
            {
                foreach (Vector2Int cell in GetCellsOnRadius(center, i))
                {
                    if (!entries.TryGetValue(cell, out HashSet<T> hashSet))
                        continue;

                    foreach (T entry in hashSet)
                    {
                        if (!LayerMaskContains(mask, entry.Layer))
                            continue;

                        float sqrDist = (position - entry.Position).sqrMagnitude;

                        if (sqrDist > radius)
                            continue;

                        if (sqrDist < closestSqrDist)
                        {
                            closestSqrDist = sqrDist;
                            closestEntry = entry;
                        }
                    }
                }
            }

            return closestEntry;
        }

        public bool TryGetClosestEntryInRadius(Vector2 position, float radius, out T closestEntry) => TryGetClosestEntryInRadius(position, radius, -1, out closestEntry);
        public bool TryGetClosestEntryInRadius(Vector2 position, float radius, LayerMask mask, out T closestEntry)
        {
            closestEntry = GetClosestEntryInRadius(position, radius, mask);
            return !EqualityComparer<T>.Default.Equals(closestEntry, default);
        }

        protected Vector2Int WorldToCellPosition(Vector2 worldPosition)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPosition.x / CellSize), Mathf.FloorToInt(worldPosition.y / CellSize));
        }

        protected int WorldToCellRadius(float worldRadius)
        {
            return Mathf.CeilToInt(worldRadius / CellSize + 0.5f); // Run on the safe side
        }

        protected bool InRadius(Vector2 a, Vector2 b, float radius)
        {
            return (a - b).sqrMagnitude <= radius;
        }

        public static bool LayerMaskContains(LayerMask mask, int layer)
        {
            return mask == -1 || (mask & (1 << layer)) != 0;
        }

        protected HashSet<T> GetInitializedValue(Vector2Int cell)
        {
            if (entries.TryGetValue(cell, out HashSet<T> hashSet))
                return hashSet;

            HashSet<T> newHashSet = new HashSet<T>();
            entries[cell] = newHashSet;
            return newHashSet;
        }

        /// <summary>0 radius returns the center, 1 returns the 8 surrounding and so on</summary>
        protected IEnumerable<Vector2Int> GetCellsOnRadius(Vector2Int center, int radius)
        {
            if (radius < 0) yield break;

            if (radius == 0)
            {
                yield return center;
                yield break;
            }

            for (int i = -radius; i <= radius; i++) // Top and bottom rows
            {
                yield return new Vector2Int(center.x + i, center.y + radius);
                yield return new Vector2Int(center.x + i, center.y - radius);
            }

            for (int i = -radius + 1; i <= radius - 1; i++) // Left and Right rows
            {
                yield return new Vector2Int(center.x + radius, center.y + i);
                yield return new Vector2Int(center.x - radius, center.y + i);
            }
        }

        protected IEnumerable<Vector2Int> GetCellsInRadius(Vector2Int center, int radius)
        {
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    yield return new Vector2Int(center.x + i, center.y + j);
                }
            }
        }
    }

    public interface ISpatialGridEntry
    {
        public Vector2 Position { get; }
        public int Layer { get; }
    }
}
