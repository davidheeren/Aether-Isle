using StateTree;
using UnityEngine;

namespace Game
{
    public class Zombie : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer spriteRenderer;

        [SerializeField] FindTargetState findTarget;
        [SerializeField] ZombieChaseState chaseState;
        [SerializeField] RootState enemyRoot;

        Movement movement;
        Ref<Transform> target = new Ref<Transform>();

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

            Node chaseBranch = new SequenceState(new Node[] {
                                    new FindTargetState(findTarget.CopyJson(), transform, target),
                                    new If(new Not(new NullCondition<Transform>(target)), new ZombieChaseState(chaseState.CopyJson(), target, transform, movement, animator)) });

            enemyRoot = new RootState(enemyRoot.CopyJson(), new Selector(new Node[] {
                new CharacterStunState(health, rb, null, animator), // Automatically locks and returns null if
                new CharacterDieState(health, collider, rb, animator, spriteRenderer),
                chaseBranch,
                new CharacterIdleState(animator) }));        
        }

        private void Update()
        {
            enemyRoot.UpdateStateTree();
        }
    }
}
