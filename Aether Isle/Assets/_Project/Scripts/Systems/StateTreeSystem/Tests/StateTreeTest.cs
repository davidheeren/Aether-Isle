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
            rootState = new RootState(new If(new VirtualCondition(InputCondition), new LockCooldownModifier(1, null, true, new VirtualState(null, UpdateState, null, null))));
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