using EventSystem;
using UnityEngine;

namespace Game
{
    public class PlayerSpawnBroadcast : MonoBehaviour
    {
        [SerializeField] GameEvent onPlayerSpawn;

        private void Start()
        {
            onPlayerSpawn.Raise(gameObject);
        }
    }
}
