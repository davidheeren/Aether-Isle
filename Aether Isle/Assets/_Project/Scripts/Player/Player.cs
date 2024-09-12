using StateTree;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(Movement), typeof(PlayerAimDirection))]
    public class Player : MonoBehaviour
    {
        [Header("General Vars")]
        [SerializeField] CharacterComponents components;

        [Header("States")]
        [SerializeField] RootState playerRoot;
        [SerializeField] CharacterStunState stunState;
        [SerializeField] CharacterDieState dieState;
        [SerializeField] PlayerIdleState idleState;
        [SerializeField] PlayerMoveState runState;
        [SerializeField] PlayerDashState dashState;
        [SerializeField] PlayerAttackState attackState;
        [SerializeField] PlayerSwimState swimState;
        [SerializeField] PlayerIdleState swimIdleState;
        [SerializeField] PlayerMoveState swimMoveState;

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
            components.Init(this);
            PlayerAimDirection aim = GetComponent<PlayerAimDirection>();

            // Conditions
            swimCondition.Init(transform);

            // State Branches
            Node swimBranch = swimState.Init(components, new Selector(
                                new If(new VirtualCondition(MoveCondition), swimMoveState.Init(components)),
                                swimIdleState.Init(components)));

            Node runBranch = runState.Init(components);
            Node dashBranch = new LockNullModifier(dashDuration, 1, dashCooldown, dashState.Init(components));
            Node attackBranch = new LockNullModifier(attackDuration, 2, attackCooldown, attackState.Init(components, aim));
            Node idleBranch = idleState.Init(components);

            // Large Branches
            Node groundedBranch = new HolderState(new Selector(
                                    dashBranch,
                                    attackBranch,
                                    new If(new VirtualCondition(MoveCondition), runBranch),
                                    idleBranch));

            State notHitBranch = new HolderState(new Selector(
                                new If(swimCondition, swimBranch),
                                groundedBranch));

            // State Tree
            playerRoot.Init(new Selector(
                            stunState.Init(false, null, components),
                            dieState.Init(components),
                            notHitBranch));
        }

        private void Update()
        {
            playerRoot.UpdateStateTree();
        }

        [ContextMenu("Test Performance")]
        void TestPerformance()
        {
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            int count = 10000;

            for (int i = 0; i < count; i++)
            {
                playerRoot.UpdateStateTree();
            }

            print("Count: " + count + " ms: " + sw.ElapsedMilliseconds);
            sw.Stop();
        }

        bool MoveCondition()
        {
            return InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
        }
    }
}
