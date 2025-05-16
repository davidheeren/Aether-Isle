using Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class Ore2D : MonoBehaviour
    {
        public int health = 3;
        [SerializeField] ItemPickup dropPrefab;
        [SerializeField] ItemData item;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                DropOre();
                Destroy(gameObject);
            }
        }

        private void DropOre()
        {
            dropPrefab.Spawn(transform.position, item);
        }
    }
}
