using System;
using UnityEngine;

namespace StateTree
{
    public class StateTreeTest : MonoBehaviour
    {
        [SerializeField] RootState rootState;

        private void Start()
        {
            /*
            rootState = new RootState(rootState.CopyJson(), 
                            new HolderState(
                                new HolderState(
                                    new FillerNodeTest(
                                        new FillerNodeTest(
                                            new LockTimerModifier(1, 
                                                new HolderState(null)))))));
            */

            rootState = new RootState(rootState.CopyJson(), new HolderState(null));
        }

        private void Update()
        {
            rootState.UpdateStateTree();
        }
    }
}