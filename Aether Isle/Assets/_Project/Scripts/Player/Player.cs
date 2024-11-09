using Inventory;
using StateTree;
using Stats;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ActorStats))]
    [RequireComponent(typeof(Movement), typeof(PlayerAimDirection), typeof(Target))]
    public class Player : StateTreeMB, IDamageMask
    {
        [Header("States")]
        [SerializeField] ActorStunState.Data stunData;
        [SerializeField] ActorDieState.Data dieData;
        [SerializeField] PlayerIdleState.Data idleData;
        [SerializeField] PlayerMoveState.Data moveData;
        [SerializeField] PlayerDashState.Data dashData;
        [SerializeField] PlayerSwimState.Data swimData;
        [SerializeField] PlayerIdleState.Data swimIdleData;
        [SerializeField] PlayerMoveState.Data swimMoveData;
        

        [Header("Conditions")]
        [SerializeField] CheckGroundCondition.Data swimConditionData;

        [Header("Vars")]
        [SerializeField] LayerMask damageMask;
        public LayerMask DamageMask => damageMask;

        private void OnDrawGizmosSelected()
        {
            // Just to draw the box where we are checking for water
            CheckGroundCondition.DrawBox(swimConditionData, transform.position);
        }

        private void Awake()
        {
            InputManager.Instance.input.UI.Disable();

            // Components
            ActorComponents components = GetComponent<ActorComponents>().Init();
            PlayerInventoryController inventoryController = GetComponent<PlayerInventoryController>();

            // State Branches
            Node swimBranch = new PlayerSwimState(swimData, components, new Selector(
                                new PlayerMoveState(swimMoveData, components),
                                new PlayerIdleState(swimIdleData, components)));

            Node dashBranch = new LockNullModifier(dashData.duration, 1, dashData.duration, new PlayerDashState(dashData, components));

            // Large Branches
            Node groundedBranch = new HolderState(new Selector(
                                    new PlayerUseableState(components, inventoryController),
                                    dashBranch,
                                    //attackBranch,
                                    new PlayerMoveState(moveData, components),
                                    new PlayerIdleState(idleData, components)));

            State notHitBranch = new HolderState(new Selector(
                                new CheckGroundCondition(swimConditionData, transform, swimBranch),
                                groundedBranch));

            // State Tree
            rootState = new RootState(rootStateData, new Selector(
                            new ActorStunState(stunData, null, components),
                            new ActorDieState(dieData, components),
                            notHitBranch));                   
        }
    }
}
