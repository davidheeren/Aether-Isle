using System;
using UnityEngine;

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
            //EnterStateWrapper();
            UpdateStateWrapper();
        }

        // Avoid debugging update
        protected override void UpdateState()
        {

        }
    }
}
