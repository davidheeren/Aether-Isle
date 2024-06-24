using StateTree;
using UnityEngine;

namespace Game
{
    public class Zombie : MonoBehaviour
    {
        [Header("General Vars")]
        [SerializeField] CharacterComponents components;

        [Header("States")]
        [SerializeField] AIMovement aiMovement;
        [SerializeField] RootState enemyRoot;
        [SerializeField] FindTargetTask findTarget;
        [SerializeField] ZombieChaseState chaseState;
        [SerializeField] CharacterStunState stunState;
        [SerializeField] CharacterDieState dieState;

        [Header("Modifiers")]
        [SerializeField] float rememberTargetTime = 2;

        Ref<Transform> targetRef = new Ref<Transform>();

        private void OnDrawGizmosSelected()
        {
            findTarget.DrawRadius(transform.position);
        }

        void Awake()
        {
            components.Setup(this);
            aiMovement.Setup(targetRef, components);

            Node chaseBranch = new FindTargetTask(findTarget.CopyJson(), transform, targetRef, new If(new Not(new NullCondition<Transform>(targetRef)), new LockCooldownModifier(rememberTargetTime, 1, true, new ZombieChaseState(chaseState.CopyJson(), aiMovement, components.animator))));
            Node moveBranch = new HolderState(new TwoSelector(chaseBranch, new CharacterIdleState(components.animator)));

            enemyRoot = new RootState(enemyRoot.CopyJson(), new Selector(new Node[] {
                new CharacterStunState(stunState.CopyJson(), true, null, components), // Automatically locks and returns null if
                new CharacterDieState(dieState.CopyJson(), components),
                moveBranch }));
        }

        private void Update()
        {
            enemyRoot.UpdateStateTree();
        }
    }
}
