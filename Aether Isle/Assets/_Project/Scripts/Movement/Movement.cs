using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        // Custom movement solution using forces
        // Runs after all other scripts

        [SerializeField] float targetSpeed = 5;
        [SerializeField] float acceleration = 15;
        [SerializeField] float deceleration = 10;

        Vector2 targetVelocity;

        Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void MoveVelocity(Vector2 targetDir)
        {
            targetVelocity = targetDir * targetSpeed;
        }

        private void FixedUpdate()
        {
            Vector2 deltaVelocity = targetVelocity - rb.velocity;

            Vector2 force = Vector2.zero;

            if (targetVelocity == Vector2.zero)
                force = deltaVelocity * deceleration;
            else
                force = deltaVelocity * acceleration;

            rb.AddForce(force);

            targetVelocity = Vector2.zero;
        }
    }
}
