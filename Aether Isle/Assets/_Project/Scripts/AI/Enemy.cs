using UnityEngine;

namespace Game
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 3;
        [SerializeField] float detectionRadius = 5;
        [SerializeField] LayerMask playerMask;

        Movement movement;

        void Awake()
        {
            movement = GetComponent<Movement>();
        }

        void FixedUpdate()
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask);

            if (player != null)
            {
                movement.MoveVelocity((player.transform.position - transform.position).normalized * moveSpeed);
            }
        }
    }
}
