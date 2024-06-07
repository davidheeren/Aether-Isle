using UnityEngine;

namespace StateTree.Test
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
                                            new LockDurationModifier(1, 
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