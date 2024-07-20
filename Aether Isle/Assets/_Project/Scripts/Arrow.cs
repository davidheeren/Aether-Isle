using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        [SerializeField] LayerMask hitMask;
        [SerializeField] float speed = 10;

        Collider2D col;
        Rigidbody2D rb;
        bool collided;

        void Awake()
        {
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!collided)
                rb.velocity = transform.up * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!hitMask.Compare(collision.gameObject.layer))
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
