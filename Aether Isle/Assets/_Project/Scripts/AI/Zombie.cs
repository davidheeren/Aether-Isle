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

            Node chaseBranch = findTarget.Create(components, targetRef, new If().Create(new Not().Create(new NullCondition<Transform>().Create(targetRef)), new LockCooldownModifier().Create(rememberTargetTime, 1, true,chaseState.Create(aiMovement, components.animator))));
            Node moveBranch = new HolderState().Create(new Selector().Create(new Node[] { chaseBranch, new CharacterIdleState().Create(components.animator) }));

            enemyRoot.Create(new Selector().Create(new Node[] {
                stunState.Create(true, null, components), // Automatically locks and returns null if
                dieState.Create(components),
                moveBranch }));
        }

        private void Update()
        {
            enemyRoot.UpdateStateTree();
        }
    }
}
