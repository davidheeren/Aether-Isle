using StateTree;
using Stats;
using UnityEngine;

namespace Game
{
    public class Zombie : StateTreeMB, IAggravate
    {
        [Header("General Vars")]
        [SerializeField] ObstacleAvoidanceData obstacleAvoidanceData;
        [SerializeField] ObstacleAvoidanceData smallObstacleAvoidanceData;
        [SerializeField] CharacterComponents components;

        [Header("Data")]
        [SerializeField] FindTargetTaskData findTargetData;
        [SerializeField] CharacterIdleState.Data idleData;
        [SerializeField] ActorChaseState.Data chaseData;
        [SerializeField] CharacterRandomAttack.Data randomAttackData;
        [SerializeField] CharacterStunState.Data stunData;
        [SerializeField] CharacterDieState.Data dieData;

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
                                        new CharacterRandomAttack(randomAttackData, targetInfo, smallObstacleAvoidance, components),
                                        new ActorChaseState(chaseData, stats, moveToPoint, targetInfo, components)))); 

            Node moveBranch = new Selector(findTargetTask, new CharacterIdleState(idleData, components.animator));


            rootState = new RootState(rootStateData, new Selector(
                new CharacterSpawnState(components),
                new CharacterStunState(stunData, false, null, components),
                new CharacterDieState(dieData, components),
                moveBranch));

            rootState.UpdateStateTree();
        }

        bool TargetActiveCondition() => targetInfo.isActive;

        public void Aggravate(Target tar) => findTargetTask.Aggravate(tar);
    }
}
