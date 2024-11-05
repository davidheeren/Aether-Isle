using UnityEngine;

namespace Game
{
    public class Tree2D : MonoBehaviour
    {
        public int health = 3;
        public GameObject woodDropPrefab;

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
            if (woodDropPrefab != null) 
            { 
             Instantiate(woodDropPrefab, transform.position, Quaternion.identity);
            
            }

        }
    }
}
