using StateTree;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Movement), typeof(PlayerAimDirection))]
    public class Player : MonoBehaviour
    {
        [Header("General Vars")]
        [SerializeField] CharacterComponents components;
        [SerializeField] SFXLoop runSFXLoop;

        [Header("States")]
        [SerializeField] RootState playerRoot;
        [SerializeField] PlayerSwimState swimState;
        [SerializeField] CharacterStunState stunState;
        [SerializeField] CharacterDieState dieState;
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
            components.Setup(this);
            PlayerAimDirection aim = GetComponent<PlayerAimDirection>();

            // Conditions
            swimCondition.Create(transform);

            // State Branches
            Node swimBranch = swimState.Create(components);
            Node runBranch = runState.Create(components, runSFXLoop);
            Node dashBranch = new LockNullModifier().Create(dashDuration, 1, dashCooldown, dashState.Create(components));
            Node attackBranch = new LockNullModifier().Create(attackDuration, 2, attackCooldown, attackState.Create(components, aim));
            Node idleBranch = new PlayerIdleState().Create(components);

            // Large Branches
            Node groundedBranch = new HolderState().Create(new Selector().Create(new Node[] {
                            new If().Create(new VirtualCondition().Create(DashCondition), dashBranch),
                            new If().Create(new VirtualCondition().Create(AttackCondition), attackBranch),
                            new If().Create(new VirtualCondition().Create(IdleCondition), idleBranch),
                            runBranch }));

            State notHitState = new HolderState().Create(new Selector().Create(new Node[] {
                                new If().Create(swimCondition, swimBranch),
                                groundedBranch}));

            // State Tree
            playerRoot = playerRoot.Create(new Selector().Create(new Node[] {
                            stunState.Create(false, null, components), // Automatically locks and returns null if
                            dieState.Create(components),
                            notHitState }));
        }

        private void Update()
        {
            playerRoot.UpdateStateTree();
        }

        bool DashCondition()
        {
            return InputManager.Instance.input.Game.Dash.WasPressedThisFrame() && InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
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
