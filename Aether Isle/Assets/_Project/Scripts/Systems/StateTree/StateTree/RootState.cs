using System;

namespace StateTree
{
    [Serializable]
    public class RootState : State
    {
        public bool debugState = false;
        public bool debugGeneral = false;
        public bool createGameObjectTree = false;

        private RootState() : base(null) { }
        public RootState(Node child = null) : base(child)
        {
            InitializeRootState();
        }

        public void Init(Node child = null)
        {
            InitializeState(child);
            InitializeRootState();
        }

        private void InitializeRootState()
        {
            SetupWrapper(this, 0);

            if (createGameObjectTree)
                CreateGameObjectTree(null);
        }

        public void UpdateStateTree()
        {
            Evaluate();
            EnterStateWrapper();
            UpdateStateWrapper();
        }

        public void FixedUpdateStateTree()
        {
            FixedUpdateStateWrapper();
        }
    }
}
