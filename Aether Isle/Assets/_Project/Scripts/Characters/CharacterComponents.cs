using SpriteAnimator;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterComponents
    {
        // Holds references to common components on a character

        public SpriteRenderer spriteRenderer;
        public SpriteAnimatorController animator;
        public SpriteRenderer shadowRenderer;

        [NonSerialized] public Movement movement;
        [NonSerialized] public Collider2D col;
        [NonSerialized] public Rigidbody2D rb;
        [NonSerialized] public Health health;

        [NonSerialized] public GameObject gameObject;
        [NonSerialized] public Transform transform;

        public void Init(MonoBehaviour mb)
        {
            movement = mb.GetComponent<Movement>();
            col = mb.GetComponent<Collider2D>();
            rb = mb.GetComponent<Rigidbody2D>();
            health = mb.GetComponent<Health>();

            gameObject = mb.gameObject;
            transform = mb.transform;

            CheckNull(spriteRenderer);
            CheckNull(animator);
            CheckNull(shadowRenderer);
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
