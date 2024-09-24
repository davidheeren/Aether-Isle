using StateTree;
using Utilities;
using UnityEngine;

namespace Game
{
    public class CharacterSpawnState : State
    {
        CharacterComponents components;

        Timer timer;
        float spawnTime = 1;

        public CharacterSpawnState(CharacterComponents components, State child = null) : base(child)
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
            components.spriteRenderer.color = ChangeAlpha(components.spriteRenderer.color, a);
        }

        protected override void ExitState()
        {
            base.ExitState();

            components.col.enabled = true;
            components.spriteRenderer.color = ChangeAlpha(components.spriteRenderer.color, 1);
        }


        private Color ChangeAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
