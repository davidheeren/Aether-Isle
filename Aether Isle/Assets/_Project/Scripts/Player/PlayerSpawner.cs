using Save;
using UnityEngine;

namespace Game
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] GameObject playerPrefab;

        void Awake()
        {
            Instantiate(playerPrefab, (Vector3)SaveSystem.SaveData.PlayerSpawnPos, Quaternion.identity);
        }
    }
}
