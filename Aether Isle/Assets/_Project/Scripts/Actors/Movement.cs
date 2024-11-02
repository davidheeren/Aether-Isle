using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        // Custom movement solution using forces
        // Runs after all other scripts

        [SerializeField] float defaultAccelerationForce = 12;
        [SerializeField] float defaultDecelerationForce = 7;

        float currentAccelerationForce;
        float currentDecelerationForce;

        public Vector2 targetVelocity { get; private set; }
        public Rigidbody2D rb { get; private set; }

        bool wasSetLastFrame;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ResetValues();
        }

        public void Move(Vector2 targetVelocity, float? accelerationForce = null, float? decelerationForce = null)
        {
            if (accelerationForce != null)
                currentAccelerationForce = accelerationForce.Value;
            if (decelerationForce != null)
                currentDecelerationForce = decelerationForce.Value;

            this.targetVelocity = targetVelocity;
            wasSetLastFrame = true;
        }

        private void FixedUpdate()
        {
            if (!wasSetLastFrame)
                targetVelocity = Vector2.zero;

            float acceleration = wasSetLastFrame ? currentAccelerationForce : currentDecelerationForce;


            Vector2 velocity = Smoothing.ExpDecay(rb.linearVelocity, targetVelocity, acceleration / rb.mass, Time.fixedDeltaTime);


            rb.linearVelocity = velocity;

            ResetValues();

            // Old way of doing using force. It's framerate dependent but FixedUpdate is fixed. However, it is less elegent
            //Vector2 deltaVelocity = targetVelocity - rb.velocity;
            //rb.AddForce(deltaVelocity * acceleration);
        }

        void ResetValues()
        {
            currentAccelerationForce = defaultAccelerationForce;
            currentDecelerationForce = defaultDecelerationForce;
            wasSetLastFrame = false;
        }
    }
}
