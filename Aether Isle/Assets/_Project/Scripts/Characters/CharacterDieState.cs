using SpriteAnimator;
using StateTree;
using System;
using UnityEngine;
using Utilities;

namespace Game
{
    [Serializable]
    public class CharacterDieState : State
    {
        [SerializeField] SpriteAnimation animation;
        [SerializeField] AudioClip dieSFX;

        CharacterComponents components;

        Timer delayTimer;
        Timer fadeTimer;

        bool canEnter;
        const float despawnTime = 2;
        const float fadeTime = 1;

        private CharacterDieState() : base(null) { }
        public CharacterDieState Init(CharacterComponents components, Node child = null)
        {
            InitializeState(child);

            this.components = components;

            components.health.OnDie += OnDie;
            delayTimer = new Timer(despawnTime);

            return this;
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
            components.col.enabled = false;
            components.rb.velocity = Vector2.zero;
            components.animator.Play(animation);
            SFXManager.Instance.PlaySFXClip(dieSFX, components.rb.transform.position);
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
                components.col.gameObject.SetActive(false);
                fadeTimer = null;
                return;
            }

            Color col = components.spriteRenderer.color;
            col.a = fadeTimer.currentPercent;
            components.spriteRenderer.color = col;
        }
    }
}
