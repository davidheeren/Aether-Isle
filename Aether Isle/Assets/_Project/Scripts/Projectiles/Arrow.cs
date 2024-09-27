using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D)), RequireComponent(typeof(CollisionDamage))]
    public class Arrow : MonoBehaviour
    {
        [SerializeField] LayerMask obstacleMask;
        [SerializeField] float speed = 10;

        Collider2D col;
        Rigidbody2D rb;
        CollisionDamage collisionDamage;

        bool collided;

        void Awake()
        {
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            collisionDamage = GetComponent<CollisionDamage>();
        }

        private void FixedUpdate()
        {
            if (!collided)
                rb.velocity = transform.up * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == col)
                return;

            LayerMask totalMask = collisionDamage.damageMask | obstacleMask;

            if (!totalMask.Compare(collision.gameObject.layer))
                return;

            collided = true;
            rb.velocity = Vector2.zero;
            col.enabled = false;

            transform.parent = collision.transform;

            Invoke(nameof(Die), 2);
        }

        void Die()
        {
            gameObject.SetActive(false);
        }
    }
}
