using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D)), RequireComponent(typeof(ProjectileDamage))]
    public class ArrowProjectile : MonoBehaviour
    {
        [SerializeField] LayerMask obstacleMask;
        public float speed = 10;

        Collider2D col;
        Rigidbody2D rb;
        ProjectileDamage projectileDamage;

        bool collided;

        void Awake()
        {
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        private void FixedUpdate()
        {
            if (!collided)
                rb.linearVelocity = transform.up * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == col)
                return;

            LayerMask totalMask = projectileDamage.damageMask | obstacleMask;

            if (!totalMask.Compare(collision.gameObject.layer))
                return;

            collided = true;
            rb.linearVelocity = Vector2.zero;
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
