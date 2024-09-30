using StateTree;
using Utilities;
using UnityEngine;

namespace Game
{
    public class ActorSpawnState : State
    {
        ActorComponents components;

        Timer timer;
        float spawnTime = 1;

        public ActorSpawnState(ActorComponents components, State child = null) : base(child)
        {
            this.components = components;
            timer = new Timer(spawnTime).Stop();
        }

        protected override bool CanEnterState()
        {
            return !timer.isDone;
        }

        protected override void EnterState()
        {
            base.EnterState();

            timer.Reset();
            components.col.enabled = false;
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            float a = EasingFunction.EaseOutCubic(0, 1, 1 - timer.currentPercent);
            components.spriteRenderer.color = components.spriteRenderer.color.SetAlpha(a);
            components.shadowRenderer.color = components.shadowRenderer.color.SetAlpha(a);
        }

        protected override void ExitState()
        {
            base.ExitState();

            components.col.enabled = true;
            components.spriteRenderer.color = components.spriteRenderer.color.SetAlpha(1);
            components.shadowRenderer.color = components.shadowRenderer.color.SetAlpha(1);
        }
    }
}
