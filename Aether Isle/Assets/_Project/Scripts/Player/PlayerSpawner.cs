using Save;
using UnityEngine;
using UnityEngine.Search;

namespace Game
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SearchContext("p: t:prefab dir:Actors")]
        [SerializeField] GameObject playerPrefab;

        void Awake()
        {
            Instantiate(playerPrefab, (Vector3)SaveSystem.Data.PlayerSpawnPos, Quaternion.identity);
        }
    }
}
