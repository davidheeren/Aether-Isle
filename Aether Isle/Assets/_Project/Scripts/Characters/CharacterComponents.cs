using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterComponents
    {
        // Holds references to common components on a character

        public Animator animator;
        public SpriteRenderer spriteRenderer;

        [NonSerialized] public Movement movement;
        [NonSerialized] public Collider2D col;
        [NonSerialized] public Rigidbody2D rb;
        [NonSerialized] public Health health;

        [NonSerialized] public GameObject gameObject;
        [NonSerialized] public Transform transform;

        public void Setup(MonoBehaviour mb)
        {
            movement = mb.GetComponent<Movement>();
            col = mb.GetComponent<Collider2D>();
            rb = mb.GetComponent<Rigidbody2D>();
            health = mb.GetComponent<Health>();

            gameObject = mb.gameObject;
            transform = mb.transform;
        }
    }
}
