using SpriteAnimator;
using Stats;
using System;
using UnityEngine;

namespace Game
{
    public class ActorComponents : MonoBehaviour
    {
        // Holds references to common components on a character

        public SpriteRenderer spriteRenderer;
        public SpriteAnimatorController animator;
        public SpriteRenderer shadowRenderer;
        public SpriteRenderer fireRenderer;

        [NonSerialized] public Movement movement;
        [NonSerialized] public Collider2D col;
        [NonSerialized] public Rigidbody2D rb;
        [NonSerialized] public Health health;
        [NonSerialized] public ActorStats stats;
        [NonSerialized] public Target target;


        public IAimDirection aimDirection;
        public IDamageMask damageMask;

        public ActorComponents Init()
        {
            movement = GetComponent<Movement>();
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            stats = GetComponent<ActorStats>();
            target = GetComponent<Target>();

            aimDirection = GetComponent<IAimDirection>(); // Do not require for now
            damageMask = GetComponent<IDamageMask>();

            CheckNull(spriteRenderer);
            CheckNull(animator);
            CheckNull(shadowRenderer);
            CheckNull(fireRenderer);

            return this;
        }

        void CheckNull(Component comp)
        {
            if (comp == null)
            {
                Debug.LogError("Component of type: " + comp.GetType() + " is null on: " + gameObject.name);
            }
        }
    }
}
