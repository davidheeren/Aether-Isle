using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        // Custom movement solution using forces
        // Runs after all other scripts

        [SerializeField] float defaultAcceleration = 12;
        [SerializeField] float defaultDeceleration = 7;

        float currentAcceleration;
        float currentDeceleration;

        public Vector2 targetVelocity { get; private set; }
        public Rigidbody2D rb { get; private set; }

        bool wasSetLastFrame;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ResetValues();
        }

        public void Move(Vector2 targetVelocity, float? acceleration = null, float? deceleration = null)
        {
            if (acceleration != null)
                currentAcceleration = acceleration.Value;
            if (deceleration != null)
                currentDeceleration = deceleration.Value;

            this.targetVelocity = targetVelocity;
            wasSetLastFrame = true;
        }

        private void FixedUpdate()
        {
            if (!wasSetLastFrame)
                targetVelocity = Vector2.zero;

            Vector2 deltaVelocity = targetVelocity - rb.velocity;

            //float acceleration = targetVelocity != Vector2.zero ? currentAcceleration : currentDeceleration;
            float acceleration = wasSetLastFrame ? currentAcceleration : currentDeceleration;


            rb.AddForce(deltaVelocity * acceleration);

            ResetValues();
        }

        void ResetValues()
        {
            currentAcceleration = defaultAcceleration;
            currentDeceleration = defaultDeceleration;
            wasSetLastFrame = false;
        }
    }
}
