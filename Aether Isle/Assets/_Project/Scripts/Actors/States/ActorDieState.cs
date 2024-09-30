using SpriteAnimator;
using StateTree;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Game
{
    public class ActorDieState : State
    {
        Data data;
        ActorComponents components;

        Timer delayTimer;
        Timer fadeTimer;

        const float despawnTime = 2;
        const float fadeTime = 1;

        public ActorDieState(Data data, ActorComponents components, Node child = null) : base(child)
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
            public GameObject[] gameObjectsToDisable;
        }

        protected override bool CanEnterState()
        {
            return components.health.isDead;
        }

        protected override void EnterState()
        {
            base.EnterState();

            LockSuperStates(null, true);

            foreach (GameObject go in data.gameObjectsToDisable) go.SetActive(false);

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

            components.spriteRenderer.color = components.spriteRenderer.color.SetAlpha(fadeTimer.currentPercent);
            components.shadowRenderer.color = components.shadowRenderer.color.SetAlpha(fadeTimer.currentPercent);
        }
    }
}
