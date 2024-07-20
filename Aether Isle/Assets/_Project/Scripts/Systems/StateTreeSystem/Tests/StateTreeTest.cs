using Game;
using System;
using UnityEngine;

namespace StateTree.Test
{
    public class StateTreeTest : MonoBehaviour
    {
        [SerializeField] RootState rootState;

        private void Start()
        {
            rootState = new RootState().Create(new If().Create(new VirtualCondition().Create(InputCondition), new LockCooldownModifier().Create(1, null, true, new VirtualState().Create(UpdateMethod: UpdateState))));
        }

        private void Update()
        {
            rootState.UpdateStateTree();
        }

        bool InputCondition()
        {
            return InputManager.Instance.input.Game.Dash.IsPressed();
        }
        private void UpdateState()
        {
            transform.eulerAngles += Vector3.forward * 200 * Time.deltaTime;
        }

    }
}