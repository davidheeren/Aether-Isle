using StateTree;
using UnityEngine;

namespace Game
{
    public class Zombie : MonoBehaviour, IAggravate
    {
        [Header("General Vars")]
        [SerializeField] CharacterComponents components;
        [SerializeField] ObstacleAvoidance obstacleAvoidance;
        [SerializeField] AIMovement aiMovement;
        [SerializeField] FindTargetTask findTarget;

        [Header("States")]
        [SerializeField] RootState enemyRoot;
        [SerializeField] EnemyChaseState chaseState;
        [SerializeField] CharacterStunState stunState;
        [SerializeField] CharacterDieState dieState;

        //[Header("Modifiers")]

        TargetInfo targetInfo = new TargetInfo();

        private void OnDrawGizmosSelected()
        {
            findTarget.DrawRadius(transform.position);
        }

        void Awake()
        {
            components.Setup(this);
            aiMovement.Setup(targetInfo, obstacleAvoidance, components);
            obstacleAvoidance.Setup(transform);

<<<<<<< Updated upstream
            Node chaseBranch = new FindTargetTask(findTarget.CopyJson(), transform, targetRef, new If(new Not(new NullCondition<Transform>(targetRef)), new LockCooldownModifier(rememberTargetTime, 1, true, new ZombieChaseState(chaseState.CopyJson(), aiMovement, components.animator))));
            Node moveBranch = new HolderState(new TwoSelector(chaseBranch, new CharacterIdleState(components.animator)));

            enemyRoot = new RootState(enemyRoot.CopyJson(), new Selector(new Node[] {
                new CharacterStunState(stunState.CopyJson(), true, null, components), // Automatically locks and returns null if
                new CharacterDieState(dieState.CopyJson(), components),
=======
            Node chaseBranch = findTarget.Create(components, targetInfo, new If().Create(new VirtualCondition().Create(TargetActiveCondition), chaseState.Create(aiMovement, components.animator)));
            Node moveBranch = new HolderState().Create(new Selector().Create(new Node[] { chaseBranch, new CharacterIdleState().Create(components.animator) }));

            enemyRoot.Create(new Selector().Create(new Node[] {
                new EnemySpawnState().Create(components),
                stunState.Create(true, null, components), // Automatically locks and returns null if
                dieState.Create(components),
>>>>>>> Stashed changes
                moveBranch }));
        }
        
        private void Update()
        {
            enemyRoot.UpdateStateTree();
        }

        public void Aggravate(Collider2D col)
        {
            findTarget.Aggravate(col);
        }

        bool TargetActiveCondition()
        {
            return targetInfo.isActive;
        }
    }
}
