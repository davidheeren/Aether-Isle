using StateTree;
using UnityEngine;

namespace Game
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 3;
        [SerializeField] float detectionRadius = 5;
        [SerializeField] LayerMask playerMask;

        [SerializeField] RootState enemyRoot;

        Movement movement;

        void Awake()
        {
            movement = GetComponent<Movement>();
            Health health = GetComponent<Health>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            enemyRoot = new RootState(enemyRoot.CopyJson(), new TwoSelector(
                            new CharacterStunState(health, rb, null, null), // Automatically locks and returns null if
                            new VirtualState(null, ChaseUpdateMethod, null, null, null)));
        }

        private void Update()
        {
            enemyRoot.UpdateStateTree();
        }

        void ChaseUpdateMethod()
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask);

            if (player != null)
            {
                movement.Move((player.transform.position - transform.position).normalized * moveSpeed);
            }
        }
    }
}
