using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] float acceleration = 15;
        [SerializeField] float deceleration = 10;

        Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void MoveVelocity(Vector2 targetVelocity)
        {
            Vector2 deltaVelocity = targetVelocity - rb.velocity;

            Vector2 force = Vector2.zero;

            if (targetVelocity == Vector2.zero)
                force = deltaVelocity * deceleration;
            else
                force = deltaVelocity * acceleration;

            rb.AddForce(force);
        }
    }
}
