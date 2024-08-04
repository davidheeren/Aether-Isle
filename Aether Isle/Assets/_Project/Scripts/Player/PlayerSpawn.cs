using EventSystem;
using UnityEngine;

namespace Game
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] GameEvent onPlayerSpawn;

        private void Start()
        {
            onPlayerSpawn.Raise(new GameEventData(gameObject));
        }
    }
}
