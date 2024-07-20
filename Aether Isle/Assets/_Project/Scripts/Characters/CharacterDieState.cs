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

        CharacterComponents components;

        Timer delayTimer;
        Timer fadeTimer;

        bool canEnter;
        const float despawnTime = 2;
        const float fadeTime = 1;

        public CharacterDieState Create(CharacterComponents components, Node child = null)
        {
            CreateState(child);

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
            components.animator.Play("Die");
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
