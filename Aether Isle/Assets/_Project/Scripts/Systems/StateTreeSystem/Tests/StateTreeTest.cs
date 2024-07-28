using Game;
using System;
using UnityEngine;

namespace StateTree.Test
{
    public class StateTreeTest : MonoBehaviour
    {
        RootState rootState;

        private void Start()
        {
<<<<<<< Updated upstream
            rootState = new RootState(new If(new VirtualCondition(InputCondition), new LockCooldownModifier(1, null, true, new VirtualState(null, UpdateState, null, null))));
=======
            Node sequence = new Sequence().Create(new Node[]
            {
                new If().Create(new VirtualCondition().Create(InputCondition), new LockDurationModifier().Create(1, null, new VirtualState().Create(UpdateMethod: UpdateState1).SetName("State 1"))),
                new LockDurationModifier().Create(2, null, new VirtualState().Create(UpdateMethod: UpdateState2).SetName("State 2")),
                new LockDurationModifier().Create(2, null, new VirtualState().Create(UpdateMethod: UpdateState3).SetName("State 3"))
            });
            rootState = rootState.Create(sequence);
>>>>>>> Stashed changes
        }

        private void Update()
        {
            rootState.UpdateStateTree();
        }

        bool InputCondition()
        {
            return InputManager.Instance.input.Game.Dash.IsPressed();
        }
        private void UpdateState1()
        {
            transform.eulerAngles += Vector3.forward * 100 * Time.deltaTime;
        }
        private void UpdateState2()
        {
            transform.eulerAngles -= Vector3.forward * 100 * Time.deltaTime;
        }
        private void UpdateState3()
        {
            transform.eulerAngles += Vector3.forward * 500 * Time.deltaTime;
        }
    }
}