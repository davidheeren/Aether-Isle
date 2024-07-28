using System;

namespace StateTree
{
    [Serializable]
    public class RootState : State
    {
        public bool debugSetup = false;
        public bool debugState = false;
        public bool debugGeneral = false;

        private RootState() : base(null, null) { }
        public RootState(Node child) : this(null, child) { }
        public RootState(string copyJson, Node child) : base(copyJson, child)
        {
            SetupWrapper(this);
        }

        public void UpdateStateTree()
        {
            Evaluate();
<<<<<<< Updated upstream:Aether Isle/Assets/_Project/Scripts/Systems/StateTreeSystem/StateTree/RootState.cs
            //EnterStateWrapper();
=======
>>>>>>> Stashed changes:Aether Isle/Assets/_Project/Scripts/Systems/StateTreeSystem/StateTree/States/RootState.cs
            UpdateStateWrapper();
        }

        public void FixedUpdateStateTree()
        {
            FixedUpdateStateWrapper();
        }
    }
}
