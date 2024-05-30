using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        // Custom movement solution using forces
        // Runs after all other scripts

        [SerializeField] float defaultSpeed = 5;
        [SerializeField] float defaultAcceleration = 15;
        [SerializeField] float defaultDeceleration = 10;

        float currentSpeed;
        float currentAcceleration;
        float currentDeceleration;

        public Vector2 targetVelocity {  get; private set; }

        bool wasSetLastFrame;

        Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ResetValues();
        }

        public void MoveVelocity(Vector2 targetDir, float ?speed = null, float ?acceleration = null, float ?deceleration = null)
        {
            if (speed != null)
                currentSpeed = speed.Value;
            if (acceleration != null)
                currentAcceleration = acceleration.Value;
            if (deceleration != null)
                currentDeceleration = deceleration.Value;

            targetVelocity = targetDir * currentSpeed;
            wasSetLastFrame = true;
        }

        private void FixedUpdate()
        {
            
            Vector2 deltaVelocity = targetVelocity - rb.velocity;
            Vector2 force = Vector2.zero;
            if (targetVelocity == Vector2.zero)
                force = deltaVelocity * currentDeceleration;
            else
                force = deltaVelocity * currentAcceleration;
            


            /*
            Vector2 force = Vector2.zero;
            if (targetVelocity == Vector2.zero)
                force = Smoothing.ExpDecay(rb.velocity, targetVelocity, decay, Time.fixedDeltaTime) * currentDeceleration;
            else
                force = Smoothing.ExpDecay(rb.velocity, targetVelocity, decay, Time.fixedDeltaTime) * currentAcceleration;
            */


            rb.AddForce(force);

            if (!wasSetLastFrame)
                targetVelocity = Vector2.zero;

            ResetValues();
            wasSetLastFrame = false;
        }

        void ResetValues()
        {
            currentSpeed = defaultSpeed;
            currentAcceleration = defaultAcceleration;
            currentDeceleration = defaultDeceleration;
        }
    }
}
