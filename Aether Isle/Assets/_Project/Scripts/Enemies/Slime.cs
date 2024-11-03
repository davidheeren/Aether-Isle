using StateTree;
using Stats;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ActorStats))]
    public class Slime : StateTreeMB, IAggravate
    {
        [Header("General Vars")]
        [SerializeField] ObstacleAvoidanceData obstacleAvoidanceData;

        [Header("Data")]
        [SerializeField] FindTargetTaskData findTargetData;
        [SerializeField] AgentWanderState.Data wanderData;
        [SerializeField] AgentChaseState.Data chaseData;
        [SerializeField] AgentLungeState.Data lungeData;
        [SerializeField] ActorStunState.Data stunData;
        [SerializeField] ActorDieState.Data dieData;

        FindTargetTask findTargetTask;

        TargetInfo targetInfo = new TargetInfo();
        ActorComponents components;

        private void OnDrawGizmosSelected()
        {
            FindTargetTask.DrawRadius(findTargetData, transform.position);
        }

        void Awake()
        {
            components = GetComponent<ActorComponents>().Init();
            ObstacleAvoidance obstacleAvoidance = new ObstacleAvoidance(obstacleAvoidanceData, transform);
            ActorMoveToPoint moveToPoint = new ActorMoveToPoint(obstacleAvoidance, components);

            findTargetTask = new FindTargetTask(findTargetData, components, targetInfo,
                                    new VirtualCondition(TargetActiveCondition,
                                    new HolderState("TargetActiveState",
                                    new Selector(
                                        new NullCooldownModifier(lungeData.cooldownTime, new AgentLungeState(lungeData, targetInfo, components, 1)),
                                        new AgentChaseState(chaseData, moveToPoint, targetInfo, components)))));

            Node moveBranch = new Selector(findTargetTask,
                new AgentWanderState(wanderData, obstacleAvoidance, components));


            rootState = new RootState(rootStateData, name + "RootState", new Selector(
                new ActorSpawnState(components),
                new ActorStunState(stunData, null, components),
                new ActorDieState(dieData, components),
                moveBranch));

            rootState.UpdateStateTree();
        }

        bool TargetActiveCondition() => targetInfo.isActive;

        public void Aggravate(Target tar) => findTargetTask.Aggravate(tar);
    }
}
