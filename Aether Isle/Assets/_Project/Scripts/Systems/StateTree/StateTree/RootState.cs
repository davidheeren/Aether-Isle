using System;

namespace StateTree
{
    [Serializable]
    public class RootState : State
    {
        public bool debugSetup = false;
        public bool debugState = false;
        public bool debugGeneral = false;

        public bool enabled = true;

        public RootState Create(Node child = null)
        {
            CreateState(child);
            SetupWrapper(this);

            return this;
        }

        public void UpdateStateTree()
        {
            if (!enabled) return;

            Evaluate();
            EnterStateWrapper();
            UpdateStateWrapper();
        }

        public void FixedUpdateStateTree()
        {
            if (!enabled) return;

            FixedUpdateStateWrapper();
        }
    }
}
