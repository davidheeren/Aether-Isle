using StateTree;
using Stats;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ObjectStats))]
    [RequireComponent(typeof(Movement), typeof(PlayerAimDirection), typeof(Target))]
    public class Player : StateTreeMB
    {
        [Header("General Vars")]
        [SerializeField] CharacterComponents components;

        [Header("States")]
        [SerializeField] CharacterStunState.Data stunData;
        [SerializeField] CharacterDieState.Data dieData;
        [SerializeField] PlayerIdleState.Data idleData;
        [SerializeField] PlayerMoveState.Data moveData;
        [SerializeField] PlayerDashState.Data dashData;
        [SerializeField] PlayerAttackState.Data attackData;
        [SerializeField] PlayerSwimState.Data swimData;
        [SerializeField] PlayerIdleState.Data swimIdleData;
        [SerializeField] PlayerMoveState.Data swimMoveData;

        [Header("Conditions")]
        [SerializeField] CheckGroundCondition.Data swimConditionData;

        private void OnDrawGizmosSelected()
        {
            // Just to draw the box where we are checking for water
            CheckGroundCondition.DrawBox(swimConditionData, transform.position);
        }

        private void Awake()
        {
            // Components
            components.Init(this);
            PlayerAimDirection aim = GetComponent<PlayerAimDirection>();
            Target target = GetComponent<Target>();
            ObjectStats stats = GetComponent<ObjectStats>();

            // State Branches
            Node swimBranch = new PlayerSwimState(swimData, stats, components, new Selector(
                                new PlayerMoveState(swimMoveData, stats, components),
                                new PlayerIdleState(swimIdleData, components)));

            Node dashBranch = new LockNullModifier(dashData.duration, 1, dashData.duration, new PlayerDashState(dashData, components, target));
            Node attackBranch = new LockNullModifier(attackData.duration, 2, attackData.cooldown, new PlayerAttackState(attackData, components, aim));

            // Large Branches
            Node groundedBranch = new HolderState(new Selector(
                                    dashBranch,
                                    attackBranch,
                                    new PlayerMoveState(moveData, stats, components),
                                    new PlayerIdleState(idleData, components)));

            State notHitBranch = new HolderState(new Selector(
                                new CheckGroundCondition(swimConditionData, transform, swimBranch),
                                groundedBranch));

            // State Tree
            rootState = new RootState(rootStateData, new Selector(
                            new CharacterStunState(stunData, true, null, components),
                            new CharacterDieState(dieData, components),
                            notHitBranch));
        }
    }
}
