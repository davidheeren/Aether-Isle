using StateTree;
using UnityEngine;

namespace Game
{
    public class Zombie : MonoBehaviour, IAggravate
    {
        [Header("General Vars")]
        [SerializeField] CharacterComponents components;
        [SerializeField] AIMovement aiMovement;
        [SerializeField] ObstacleAvoidance obstacleAvoidance;

        [Header("States")]
        [SerializeField] RootState enemyRoot;
        [SerializeField] FindTargetTask findTarget;
        [SerializeField] CharacterIdleState idleState;
        [SerializeField] ZombieChaseState chaseState;
        [SerializeField] CharacterStunState stunState;
        [SerializeField] CharacterDieState dieState;

        [Header("Modifiers")]

        TargetInfo targetInfo = new TargetInfo();

        private void OnDrawGizmosSelected()
        {
            findTarget.DrawRadius(transform.position);
        }

        void Awake()
        {
            components.Init(this);
            aiMovement.Init(targetInfo, obstacleAvoidance, components);

            Node chaseBranch = findTarget.Init(components, targetInfo, new If(new VirtualCondition(TargetActiveCondition), chaseState.Init(aiMovement, components.animator))); 
            Node moveBranch = new HolderState(new Selector(chaseBranch, idleState.Init(components.animator)));

            enemyRoot.Init(new Selector(
                new CharacterSpawnState(components),
                stunState.Init(true, null, components), // Automatically locks and returns null if
                dieState.Init(components),
                moveBranch));

            enemyRoot.UpdateStateTree();
        }

        private void Update()
        {
            enemyRoot.UpdateStateTree();
        }

        bool TargetActiveCondition()
        {
            return targetInfo.isActive;
        }

        public void Aggravate(Collider2D col)
        {
            findTarget.Aggravate(col);
        }
    }
}
