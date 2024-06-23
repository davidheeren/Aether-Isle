using StateTree;
using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [Serializable]
    public class CharacterDieState : State
    {
        [SerializeField] AudioClip dieSFX;

        Collider2D collider;
        Rigidbody2D rb;
        Animator animator;
        SpriteRenderer spriteRenderer;

        Timer delayTimer;
        Timer fadeTimer;

        bool canEnter;
        const float despawnTime = 2;
        const float fadeTime = 1;

        public CharacterDieState(string copyJson, Health health, Collider2D collider, Rigidbody2D rb, Animator animator, SpriteRenderer spriteRenderer, Node child = null) : base(copyJson, child)
        {
            this.collider = collider;
            this.rb = rb;
            this.animator = animator;
            this.spriteRenderer = spriteRenderer;

            health.OnDie += OnDie;
            delayTimer = new Timer(despawnTime);
        }

        void OnDie()
        {
            canEnter = true;
        }

        protected override bool CanEnterState()
        {
            return canEnter;
        }

        protected override void EnterState()
        {
            base.EnterState();

            LockSuperStates(null, true);

            delayTimer.Reset();
            collider.enabled = false;
            rb.velocity = Vector2.zero;
            animator.Play("Die");
            SFXManager.Instance.PlaySFXClip(dieSFX, rb.transform.position);
        }

        protected override void UpdateState()
        {
            if (delayTimer.isDone)
            {
                fadeTimer = new Timer(fadeTime);
                delayTimer.Stop();
            }

            if (fadeTimer == null) return;

            if (fadeTimer.isDone)
            {
                collider.gameObject.SetActive(false);
                fadeTimer = null;
                return;
            }

            Color col = spriteRenderer.color;
            col.a = fadeTimer.currentPercent;
            spriteRenderer.color = col;
        }
    }
}
