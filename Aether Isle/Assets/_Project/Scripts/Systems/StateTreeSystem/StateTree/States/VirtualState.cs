using System;

namespace StateTree
{
    public class VirtualState : State
    {
        Action EnterMethod;
        Action UpdateMethod;
        Action ExitMethod;

        public VirtualState(Action EnterMethod, Action UpdateMethod, Action ExitMethod, Node child) : base(null, child)
        {
            this.EnterMethod = EnterMethod;
            this.UpdateMethod = UpdateMethod;
            this.ExitMethod = ExitMethod;
        }

        protected override void EnterState()
        {
            base.EnterState();
            EnterMethod?.Invoke();
        }

        protected override void UpdateState()
        {
            base.UpdateState();
            UpdateMethod?.Invoke();
        }
        protected override void ExitState()
        {
            base.ExitState();
            ExitMethod?.Invoke();
        }
    }
}
