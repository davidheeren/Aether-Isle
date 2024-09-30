using StateTree;
using Stats;
using UnityEngine;

namespace Game
{
    public class Slime : StateTreeMB, IAggravate
    {
        [Header("General Vars")]
        [SerializeField] ObstacleAvoidanceData obstacleAvoidanceData;
        [SerializeField] ObstacleAvoidanceData smallObstacleAvoidanceData;
        [SerializeField] ActorComponents components;

        [Header("Data")]
        [SerializeField] FindTargetTaskData findTargetData;
        [SerializeField] AgentWanderState.Data wanderData;
        //[SerializeField] AgentIdleState.Data idleData;
        [SerializeField] AgentChaseState.Data chaseData;
        [SerializeField] AgentRandomAttack.Data randomAttackData;
        [SerializeField] ActorStunState.Data stunData;
        [SerializeField] ActorDieState.Data dieData;

        FindTargetTask findTargetTask;

        TargetInfo targetInfo = new TargetInfo();
        ObjectStats stats;

        private void OnDrawGizmosSelected()
        {
            FindTargetTask.DrawRadius(findTargetData, transform.position);
        }

        void Awake()
        {
            components.Init(this);
            stats = GetComponent<ObjectStats>();
            ObstacleAvoidance obstacleAvoidance = new ObstacleAvoidance(obstacleAvoidanceData, transform);
            ObstacleAvoidance smallObstacleAvoidance = new ObstacleAvoidance(smallObstacleAvoidanceData, transform);
            ActorMoveToPoint moveToPoint = new ActorMoveToPoint(obstacleAvoidance, components);

            findTargetTask = new FindTargetTask(findTargetData, components, targetInfo, 
                                    new VirtualCondition(TargetActiveCondition, 
                                    new Selector(
                                        new AgentRandomAttack(randomAttackData, targetInfo, smallObstacleAvoidance, components),
                                        new AgentChaseState(chaseData, stats, moveToPoint, targetInfo, components))));

            Node moveBranch = new Selector(findTargetTask,
                new AgentWanderState(wanderData, stats, obstacleAvoidance, components));
                //new AgentIdleState(idleData, components.animator));


            rootState = new RootState(rootStateData, new Selector(
                new ActorSpawnState(components),
                new ActorStunState(stunData, false, null, components),
                new ActorDieState(dieData, components),
                moveBranch));

            rootState.UpdateStateTree();
        }

        bool TargetActiveCondition() => targetInfo.isActive;

        public void Aggravate(Target tar) => findTargetTask.Aggravate(tar);
    }
}
