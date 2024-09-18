using SpriteAnimator;
using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class CharacterDieState : State
    {
        Data data;
        CharacterComponents components;

        Timer delayTimer;
        Timer fadeTimer;

        const float despawnTime = 2;
        const float fadeTime = 1;

        public CharacterDieState(Data data, CharacterComponents components, Node child = null) : base(child)
        {
            this.data = data;
            this.components = components;

            delayTimer = new Timer(despawnTime);
        }

        [System.Serializable]
        public class Data
        {
            public SpriteAnimation animation;
            public AudioClip dieSFX;
        }

        protected override bool CanEnterState()
        {
            return components.health.isDead;
        }

        protected override void EnterState()
        {
            base.EnterState();

            LockSuperStates(null, true);

            delayTimer.Reset();
            components.col.enabled = false;
            components.rb.velocity = Vector2.zero;
            components.animator.Play(data.animation);
            SFXManager.Instance.PlaySFXClip(data.dieSFX, components.rb.transform.position);
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
