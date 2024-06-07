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

        [Header("States")]
        [SerializeField] RootState rootState;
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

        Movement movement;
        PlayerAimDirection aim;

        private void OnDrawGizmosSelected()
        {
            // Just to draw the box where we are checking for water
            swimCondition.DrawBox(transform);
        }

        private void Awake()
        {
            // Components
            movement = GetComponent<Movement>();
            aim = GetComponent<PlayerAimDirection>();

            // Conditions
            swimCondition = new CheckGroundCondition(swimCondition.CopyJson(), transform);

            // State Branches
            Node swimBranch = new PlayerSwimState(swimState.CopyJson(), movement, animator, null);
            Node runBranch = new PlayerRunState(runState.CopyJson(), movement, animator, null);
            Node dashBranch = new NullCooldownModifier(dashCooldown, new LockDurationModifier(dashDuration, 1, new PlayerDashState(dashState.CopyJson(), movement, null)));
            Node attackBranch = new NullCooldownModifier(attackCooldown, new LockDurationModifier(attackDuration, null, new PlayerAttackState(attackState.CopyJson(), transform, aim, animator, movement, null)));
            Node idleBranch = new PlayerIdleState(animator, null);

            // Large Branches
            Node groundedBranch = new HolderState(new Selector(new Node[] {
                            new If(new VirtualCondition(RollCondition), dashBranch),
                            new If(new VirtualCondition(AttackCondition), attackBranch),
                            new If(new VirtualCondition(IdleCondition), idleBranch),
                            runBranch }));

            // State Tree
            rootState = new RootState(rootState.CopyJson(), new Selector(new Node[] {
                            new If(swimCondition, swimBranch),
                            groundedBranch}));
        }

        private void Update()
        {
            rootState.UpdateStateTree();
        }

        bool RollCondition()
        {
            return InputManager.Instance.input.Game.Roll.WasPressedThisFrame() && InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
        }

        bool AttackCondition()
        {
            return InputManager.Instance.input.Game.Attack.WasPressedThisFrame();
        }

        bool IdleCondition()
        {
            return InputManager.Instance.input.Game.Move.ReadValue<Vector2>() == Vector2.zero;
        }
    }
}
