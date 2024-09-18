using StateTree;
using UnityEngine;

namespace Game
{
    public class Zombie : StateTreeMB, IAggravate
    {
        [Header("General Vars")]
        [SerializeField] CharacterComponents components;
        [SerializeField] CharacterMovement.Data aiMovementData;
        [SerializeField] ObstacleAvoidanceData obstacleAvoidanceData;

        [Header("Data")]
        [SerializeField] FindTargetTaskData findTargetData;
        [SerializeField] CharacterIdleState.Data idleData;
        [SerializeField] CharacterChaseState.Data chaseData;
        [SerializeField] CharacterRandomAttack.Data randomAttackData;
        [SerializeField] CharacterStunState.Data stunData;
        [SerializeField] CharacterDieState.Data dieData;

        FindTargetTask findTargetTask;

        TargetInfo targetInfo = new TargetInfo();
        CharacterMovement aiMovement;
        ObstacleAvoidance obstacleAvoidance;

        private void OnDrawGizmosSelected()
        {
            FindTargetTask.DrawRadius(findTargetData, transform.position);
        }

        void Awake()
        {
            components.Init(this);
            obstacleAvoidance = new ObstacleAvoidance(obstacleAvoidanceData, transform);
            aiMovement = new CharacterMovement(aiMovementData, targetInfo, obstacleAvoidance, components);

            findTargetTask = new FindTargetTask(findTargetData, components, targetInfo, 
                                    new If(new VirtualCondition(TargetActiveCondition), 
                                    new Selector(
                                        new CharacterRandomAttack(randomAttackData, targetInfo, obstacleAvoidance, components),
                                        new CharacterChaseState(chaseData, aiMovement, components.animator)))); 

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
