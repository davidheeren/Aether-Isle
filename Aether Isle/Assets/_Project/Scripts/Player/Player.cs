using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Movement), typeof(PlayerAimDirection))]
    public class Player : MonoBehaviour
    {
        [Header("General Vars")]
        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer spriteRenderer;

        [Header("States")]
        [SerializeField] RootState playerRoot;
        [SerializeField] PlayerSwimState swimState;
        [SerializeField] PlayerRunState runState;
        [SerializeField] PlayerDashState dashState;
        [SerializeField] PlayerAttackState attackState;

        [Header("Conditions")]
        [SerializeField] CheckGroundCondition swimCondition;

        [Header("Modifier Vars")]
        [SerializeField] float dashDuration = 0.25f;
        [SerializeField] float dashCooldown = 0.5f;
        [Space(10)]
        [SerializeField] float attackDuration = 0.25f;
        [SerializeField] float attackCooldown = 0.5f;

        private void OnDrawGizmosSelected()
        {
            // Just to draw the box where we are checking for water
            swimCondition.DrawBox(transform.position);
        }

        private void Awake()
        {
            // Components
            Movement movement = GetComponent<Movement>();
            PlayerAimDirection aim = GetComponent<PlayerAimDirection>();
            Collider2D collider = GetComponent<Collider2D>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Health health = GetComponent<Health>();

            // Conditions
            swimCondition = new CheckGroundCondition(swimCondition.CopyJson(), transform);

            // State Branches
            Node swimBranch = new PlayerSwimState(swimState.CopyJson(), movement, animator);
            Node runBranch = new PlayerRunState(runState.CopyJson(), movement, animator);
            Node dashBranch = new LockNullModifier(dashDuration, 1, dashCooldown, new PlayerDashState(dashState.CopyJson(), movement, collider, health));
            Node attackBranch = new LockNullModifier(attackDuration, null, attackCooldown, new PlayerAttackState(attackState.CopyJson(), transform, aim, animator, movement));
            Node idleBranch = new PlayerIdleState(animator);

            // Large Branches
            Node groundedBranch = new HolderState(new Selector(new Node[] {
                            new If(new VirtualCondition(RollCondition), dashBranch),
                            new If(new VirtualCondition(AttackCondition), attackBranch),
                            new If(new VirtualCondition(IdleCondition), idleBranch),
                            runBranch }));

            // State Tree
            playerRoot = new RootState(playerRoot.CopyJson(), new Selector(new Node[] {
                            new CharacterStunState(health, rb, null, animator), // Automatically locks and returns null if
                            new CharacterDieState(health, collider, rb, animator, spriteRenderer),
                            new If(swimCondition, swimBranch),
                            groundedBranch}));
        }

        private void Update()
        {
            playerRoot.UpdateStateTree();
        }

        bool RollCondition()
        {
            return InputManager.Instance.input.Game.Roll.WasPressedThisFrame() && InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
            //return InputManager.Instance.input.Game.Roll.IsPressed() && InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
        }

        bool AttackCondition()
        {
            return InputManager.Instance.input.Game.Attack.WasPressedThisFrame();
            //return InputManager.Instance.input.Game.Attack.IsPressed();
        }

        bool IdleCondition()
        {
            return InputManager.Instance.input.Game.Move.ReadValue<Vector2>() == Vector2.zero;
        }
    }
}
