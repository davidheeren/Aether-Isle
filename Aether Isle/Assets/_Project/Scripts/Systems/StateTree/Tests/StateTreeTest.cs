using Game;
using UnityEngine;

namespace StateTree.Test
{
    public class StateTreeTest : MonoBehaviour
    {
        [SerializeField] RootState rootState;


        private void Start()
        {
            Node mainBranch = new If(new VirtualCondition(InputCondition), new LockCooldownModifier(1, null, new VirtualState(UpdateMethod: UpdateState)));
            rootState.Init(new Selector(mainBranch, new HolderState().SetName("TEST HOLDER")));


            //print(test.GetFirstSuperNode<State>().name);
            //List<Node> states = test.GetSuperNodes<Node>();
            //print(states.Count);
            //print(test.stateDepth);

            //foreach (var state in states)
            //{
            //    print(state.name);
            //}

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