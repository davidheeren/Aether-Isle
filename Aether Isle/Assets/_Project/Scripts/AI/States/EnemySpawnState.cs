using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    public class EnemySpawnState : State
    {
        CharacterComponents components;

        float spawnTime = 1f;

        Timer spawnTimer;

        public EnemySpawnState Create(CharacterComponents components, Node child = null)
        {
            CreateState(child);

            this.components = components;

            return this;
        }

        protected override bool CanEnterState()
        {
            if (spawnTimer != null)
                return !spawnTimer.isDone;

            return true;
        }

        protected override void EnterState()
        {
            base.EnterState();

            spawnTimer = new Timer(spawnTime);
            components.col.enabled = false;
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            Color temp = components.spriteRenderer.color;
            temp.a = 1 - spawnTimer.currentPercent;
            components.spriteRenderer.color = temp;
        }

        protected override void ExitState()
        {
            base.ExitState();

            //Color temp = components.spriteRenderer.color;
            //temp.a = 1;
            //components.spriteRenderer.color = temp;

            components.col.enabled = true;
        }
    }
}
