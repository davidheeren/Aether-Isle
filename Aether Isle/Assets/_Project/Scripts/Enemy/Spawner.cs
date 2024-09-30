using CustomInspector;
using Save;
using StateTree;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        // TODO: Refactor. Completely unreadable code
        [Header("Scan")]
        [Button(nameof(Scan))]
        [SerializeField] Vector2Int cellCount = new Vector2Int(20, 20);
        [SerializeField] float cellSize = 2;
        [SerializeField] Vector2 offset;
        [SerializeField] float scanDetectionMargin = 0.9f;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] bool spawnOnAwake = false;

        [Header("Enemy")]
        [Button(nameof(SpawnEnemies))]
        [SerializeField] GameObject prefab;
        [SerializeField] int count = 3;
        [SerializeField] float randomOffsetRange;
        [SerializeField] float spawnDelay = 120;
        [SerializeField] float respawnDelay = 10;

        Vector2[,] positions;
        bool[,] canSpawn;

        List<Vector2> canSpawnPositions;
        List<GameObject> spawnedEnemies = new List<GameObject>();

        Timer enemyDieTimer;
        bool enemyDied;
        bool spawnedOnce;

        UniqueID uniqueID;
        WaypointsContainer waypointsContainer;

        private void OnDrawGizmosSelected()
        {
            if (canSpawn == null)
                return;

            if (cellCount.x > canSpawn.GetLength(0) || cellCount.y > canSpawn.GetLength(1))
                return;

            for (int i = 0; i < cellCount.x; i++)
            {
                for (int j = 0; j < cellCount.y; j++)
                {
                    Gizmos.color = canSpawn[i, j] ? Color.green : Color.red;
                    Gizmos.DrawWireCube(positions[i, j], Vector3.one * cellSize * .75f);
                }
            }
        }

        void Awake()
        {
            uniqueID = GetComponent<UniqueID>();
            waypointsContainer = GetComponent<WaypointsContainer>();
            Scan();
            enemyDieTimer = new Timer(respawnDelay);
            enemyDieTimer.Stop();

            if (spawnOnAwake)
            {
                SpawnEnemies();
            }
        }

        private void Update()
        {
            CheckSpawnedEnemies();

            if (spawnedOnce)
            {
                if (spawnedEnemies.Count < count && !enemyDied)
                {
                    enemyDieTimer.Reset();
                    enemyDied = true;
                }

                if (!enemyDieTimer.isDone)
                    return;
            }


            if (SaveSystem.SaveData.enemySpawnTimes.TryGetValue(uniqueID.ID, out var time))
            {
                if (Time.time + SaveSystem.SaveData.timeAtLastUnload - time >= spawnDelay)
                    SpawnEnemies();
            }
            else
                SpawnEnemies();
        }

        void SpawnEnemies()
        {
            Scan();

            int spawnCount = count - spawnedEnemies.Count;

            for (int i = 0; i < spawnCount; i++)
            {
                if (canSpawnPositions.Count == 0)
                {
                    print("More enemies than open spawn positions");
                    break;
                }

                Invoke(nameof(SpawnEnemy), Random.value); // 0-1 second offset
            }

            SaveSystem.SaveData.enemySpawnTimes[uniqueID.ID] = Time.time + SaveSystem.SaveData.timeAtLastUnload;

            spawnedOnce = true;
            enemyDied = false;
            enemyDieTimer.Stop();
        }

        void SpawnEnemy()
        {
            int rand = Random.Range(0, canSpawnPositions.Count); // Max is exclusive
            Vector2 offset = new Vector2(Random.value, Random.value) * randomOffsetRange;

            GameObject obj = Instantiate(prefab, canSpawnPositions[rand] + offset, Quaternion.identity, transform);
            spawnedEnemies.Add(obj);
            canSpawnPositions.RemoveAt(rand);

            TrySetWaypoints(obj);
        }

        void TrySetWaypoints(GameObject obj)
        {
            if (waypointsContainer == null) return;
            if (obj.TryGetComponent<StateTreeMB>(out var stateTree))
            {
                AgentPatrolState patrol = stateTree.RootState.GetFirstSubNode<AgentPatrolState>();
                if (patrol != null)
                {
                    patrol.TrySetWaypoints(waypointsContainer.GetWaypoints());
                }
            }
        }

        void Scan()
        {
            canSpawn = new bool[cellCount.x, cellCount.y];
            positions = new Vector2[cellCount.x, cellCount.y];
            canSpawnPositions = new List<Vector2>();

            for (int i = 0; i < cellCount.x; i++)
            {
                for (int j = 0; j < cellCount.y; j++)
                {
                    positions[i, j] = transform.position + cellSize * (new Vector3(i, j) + Vector3.one * 0.5f - new Vector3(cellCount.x, cellCount.y) * 0.5f) + (Vector3)offset;

                    canSpawn[i, j] = !Physics2D.OverlapBox(positions[i, j], Vector2.one * cellSize * scanDetectionMargin, 0, detectionMask);

                    if (canSpawn[i, j])
                        canSpawnPositions.Add(positions[i, j]);
                }
            }

# if UNITY_EDITOR
            SceneView.RepaintAll(); // This forces a refresh of the scene to automatically update the gizmos
#endif
        }

        void CheckSpawnedEnemies()
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    spawnedEnemies.Remove(spawnedEnemies[i]);
                    break;
                }

                if (!spawnedEnemies[i].activeInHierarchy)
                {
                    spawnedEnemies.Remove(spawnedEnemies[i]);
                }
            }
        }
    }
}
