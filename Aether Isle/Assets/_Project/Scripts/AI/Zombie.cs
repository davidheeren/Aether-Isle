using StateTree;
using UnityEngine;

namespace Game
{
    public class Zombie : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer spriteRenderer;

        [SerializeField] AIMovement aiMovement;
        [SerializeField] RootState enemyRoot;
        [SerializeField] FindTargetTask findTarget;
        [SerializeField] ZombieChaseState chaseState;
        [SerializeField] CharacterStunState stunState;
        [SerializeField] CharacterDieState dieState;

        [Header("Modifiers")]
        [SerializeField] float rememberTargetTime = 2;

        Movement movement;
        Ref<Transform> targetRef = new Ref<Transform>();

        private void OnDrawGizmosSelected()
        {
            findTarget.DrawRadius(transform.position);
        }

        void Awake()
        {
            movement = GetComponent<Movement>();
            Health health = GetComponent<Health>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Collider2D collider = GetComponent<Collider2D>();

            aiMovement.Setup(targetRef, transform, movement);

            Node chaseBranch = new FindTargetTask(findTarget.CopyJson(), transform, targetRef, new If(new Not(new NullCondition<Transform>(targetRef)), new LockCooldownModifier(rememberTargetTime, 1, true, new ZombieChaseState(chaseState.CopyJson(), aiMovement, animator))));
            Node moveBranch = new HolderState(new TwoSelector(chaseBranch, new CharacterIdleState(animator)));

            enemyRoot = new RootState(enemyRoot.CopyJson(), new Selector(new Node[] {
                new CharacterStunState(stunState.CopyJson(), true, health, rb, null, animator), // Automatically locks and returns null if
                new CharacterDieState(dieState.CopyJson(), health, collider, rb, animator, spriteRenderer),
                moveBranch }));
        }

        private void Update()
        {
            enemyRoot.UpdateStateTree();
        }
    }
}
