using UnityEngine;

namespace Game
{
    public class SpawnOverride : MonoBehaviour
    {
        // TODODODODODOD

        public CollisionDamage collisionDamage { get; private set; }

        void Awake()
        {
            collisionDamage = GetComponent<CollisionDamage>();
        }
    }
}
