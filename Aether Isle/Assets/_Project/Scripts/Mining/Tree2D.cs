using Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class Tree2D : MonoBehaviour
    {
        public int health = 3;
        [SerializeField] ItemPickup dropPrefab;
        [SerializeField] ItemData item;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                DropWood();
                Destroy(gameObject);
            }
        }

        private void DropWood()
        {
            dropPrefab.Spawn(transform.position, item);
        }
    }
}
